using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions.Services;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using System;
using System.Collections.ObjectModel;

namespace Crematorium.UI.ViewModels
{
    public partial class HomeVM : ObservableValidator
    {
        private IRitualUrnService _urnService;
        private IHallService _hallService;
        private IOrderService _orderService;

        public ObservableCollection<RitualUrn> RitualUrns { get; set; }

        public ObservableCollection<Hall> Halls { get; set; }

        [ObservableProperty]
        private User? curUser;

        public HomeVM(IRitualUrnService urnService,
                        IHallService hallService,
                        IOrderService orderService)
        {
            CurUser = ServicesFabric.CurrentUser;
            _urnService = urnService;
            _hallService = hallService;
            _orderService = orderService;

            Halls = new ObservableCollection<Hall>(_hallService.GetAllAsync().Result);
            RitualUrns = new ObservableCollection<RitualUrn>(_urnService.GetAllAsync().Result);

            HallDates = new ObservableCollection<Date>();
        }

        [ObservableProperty]
        private Hall? selectedHall;

        public ObservableCollection<Date> HallDates { get; set; }

        [RelayCommand]
        public void UpdateDates()
        {
            HallDates.Clear();
            if (SelectedHall is null)
                return;

            foreach (var date in SelectedHall.FreeDates) 
            {
                HallDates.Add(date);
            }
        }

        [ObservableProperty]
        private Date? selectedDate;

        [ObservableProperty]
        private RitualUrn? selectedUrn;

        //[ObservableProperty]
        private Corpose? selectedCorpose;

        [RelayCommand]
        public async void CreateOrder()
        {
            if (SelectedHall is null || SelectedDate is null || SelectedUrn is null || selectedCorpose is null)
            {
                var er = ServicesFabric.GetErrorPage("Что-то не заполнили/выбрали");
                er.ShowDialog();
                return;
            }

            User curUser = ServicesFabric.CurrentUser!;
            await _orderService.AddAsync( new Order() {HallId = SelectedHall,
                                                        Customer = curUser!,
                                                        CorposeId = selectedCorpose,
                                                        DateOfStart = DateTime.Parse(SelectedDate.Data),
                                                        RitualUrnId = SelectedUrn });
            //SelectedHall.FreeDates.Remove(SelectedDate);
            //await _hallService.UpdateAsync(SelectedHall);
            SelectedHall = null;
            SelectedDate = null;
            SelectedUrn = null;
            selectedCorpose = null;
        }

        [RelayCommand]
        public void RegCorpose()
        {
            var userChange = (ChangeCorposePage)ServicesFabric.GetPage(typeof(ChangeCorposePage));
            userChange.InitializeCorpose(ref selectedCorpose);
            userChange.OpBtnName.Text = "Registration";
            userChange.ShowDialog();
        }

        public void UpdateCollections()
        {
            RitualUrns.Clear();
            foreach (var urn in _urnService.GetAllAsync().Result)
            {
                RitualUrns.Add(urn);
            }

            Halls.Clear();
            foreach (var hall in _hallService.GetAllAsync().Result)
            {
                Halls.Add(hall);
            }
        }
    }
}
