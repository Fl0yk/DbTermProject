using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using System.Collections.ObjectModel;

namespace Crematorium.UI.ViewModels
{
    //Сделать кнопку для перевода в следующую стадию. Если не хватает прав, то открывать окно с ошибкой
    public partial class AllOrdersVM : ObservableValidator
    {
        private IOrderService _orderService;
        private User curUser = null!;

        public AllOrdersVM(IOrderService orderService)
        {
            _orderService = orderService;
            Orders = new ObservableCollection<Order>(_orderService.GetAllAsync().Result);
            curUser = ServicesFabric.CurrentUser!;
        }

        public ObservableCollection<Order> Orders { get; set; }


        [RelayCommand]
        public async void NextStateOrder()
        {
            if (SelectedOrder is null)
                return;

            if (SelectedOrder.State == StateOrder.Decorated
                                 && curUser.UserRole == Role.Employee)
            {
                var er = ServicesFabric.GetErrorPage("Заказ подтвердить должен админ");
                er.ShowDialog();
                return;
            }

            await _orderService.NextState(selectedOrder);
            UpdateOrdersCollection();
        }

        [ObservableProperty]
        private Order selectedOrder;

        [RelayCommand]
        public void ViewOrder()
        {
            if (SelectedOrder is null)
                return;

            var orderInfo = (OrderInformationPage)ServicesFabric.GetPage(typeof(OrderInformationPage));
            orderInfo.InitializeOrder(SelectedOrder);
            orderInfo.ShowDialog();
        }

        [RelayCommand]
        public async void CancelOrder()
        {
            if (SelectedOrder is null)
                return;

            if (curUser.UserRole == Role.Employee)
            {
                var er = ServicesFabric.GetErrorPage("Только админ может отменить заказ");
                er.ShowDialog();
                return;
            }

            await _orderService.CancelOrder(selectedOrder);
            UpdateOrdersCollection();
        }

        public void UpdateOrdersCollection()
        {
            Orders.Clear();
            foreach (var order in _orderService.GetAllAsync().Result)
            {
                Orders.Add(order);
            }
        }
    }
}
