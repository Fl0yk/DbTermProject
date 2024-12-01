using CommunityToolkit.Mvvm.ComponentModel;
using Crematorium.Domain.Entities;
using System;

namespace Crematorium.UI.ViewModels
{
    public partial class OrderInformationVM : ObservableValidator
    {
        [ObservableProperty]
        private Order selectedOrder;
        public void InitializeOrder(Order order)
        {
            SelectedOrder = order;
            SelectedCustomer = order.Customer;
            SelectedCorpose = order.CorposeId;
            SelectedUrn = order.RitualUrnId;
            SelectedHall = order.HallId;
            DateOfStart = order.DateOfStart;
            SelectedState = order.State;
        }

        [ObservableProperty]
        private User selectedCustomer;

        [ObservableProperty]
        private Corpose selectedCorpose;

        [ObservableProperty]
        private RitualUrn selectedUrn;

        [ObservableProperty]
        private DateTime dateOfStart;

        [ObservableProperty]
        private Hall selectedHall;

        [ObservableProperty]
        private StateOrder selectedState;
    }
}
