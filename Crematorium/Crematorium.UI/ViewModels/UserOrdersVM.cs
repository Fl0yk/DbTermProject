using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crematorium.Application.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using System.Collections.ObjectModel;

namespace Crematorium.UI.ViewModels
{
    public partial class UserOrdersVM : ObservableValidator
    {
        private IOrderService _orderService;
        private User curUser = null!;

        public UserOrdersVM(IOrderService orderService)
        {
            _orderService = orderService;
            curUser = ServicesFabric.CurrentUser!;
            //Orders = new ObservableCollection<Order>(curUser.Orders);
            Orders = new ObservableCollection<Order>(_orderService.GetUserOrdersAsync(curUser).Result);
        }

        public ObservableCollection<Order> Orders { get; set; }

        [ObservableProperty]
        private Order? selectedOrder;

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

            await _orderService.CancelOrder(selectedOrder);
            UpdateOrdersCollection();
        }

        public void UpdateOrdersCollection()
        {
            Orders.Clear();
            foreach (var order in _orderService.GetUserOrdersAsync(curUser).Result)
            {
                Orders.Add(order);
            }
        }
    }
}
