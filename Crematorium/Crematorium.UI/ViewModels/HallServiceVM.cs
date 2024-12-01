using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Crematorium.UI.ViewModels
{
    public partial class HallServiceVM : ObservableValidator
    {
        private IHallService _hallService;

        public ObservableCollection<Hall> Halls { get; set; }

        public HallServiceVM(IHallService baseService)
        {
            _hallService = baseService;
            Halls = new ObservableCollection<Hall>(_hallService.GetAllAsync().Result);
        }

        [ObservableProperty]
        [MaxLength(20)]
        private string inputFindName = string.Empty;

        [RelayCommand]
        public void FindHalls()
        {
            Halls.Clear();
            if (string.IsNullOrEmpty(InputFindName) || string.IsNullOrWhiteSpace(InputFindName))
            {
                UpdateHallsCollection();
                return;
            }

            foreach (Hall hall in _hallService.FindByNameAsync(InputFindName).Result)
            {
                Halls.Add(hall);
            }
        }

        [RelayCommand]
        public void AddHall()
        {
            var userChange = (ChangeHallPage)ServicesFabric.GetPage(typeof(ChangeHallPage));
            userChange.InitializeHall(-1);
            userChange.OpBtnName.Text = "Registration";
            userChange.ShowDialog();
            UpdateHallsCollection();
        }

        [ObservableProperty]
        private Hall? selectedHall;

        [RelayCommand]
        public void UpdateHall()
        {
            if (SelectedHall is null)
                return;

            var userChange = (ChangeHallPage)ServicesFabric.GetPage(typeof(ChangeHallPage));
            userChange.InitializeHall(SelectedHall.Id);
            userChange.OpBtnName.Text = "Update";
            userChange.ShowDialog();
            UpdateHallsCollection();
        }

        [RelayCommand]
        public void DeleteHall()
        {
            if (SelectedHall is null)
                return;

            _hallService.DeleteAsync(SelectedHall.Id);
            UpdateHallsCollection();
        }

        private void UpdateHallsCollection()
        {
            Halls.Clear();
            foreach (Hall user in _hallService.GetAllAsync().Result)
            {
                Halls.Add(user);
            }
        }
    }
}
