using Crematorium.UI.ViewModels;
using System.Windows.Controls;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserOrdersPage.xaml
    /// </summary>
    public partial class UserOrdersPage : UserControl
    {
        private UserOrdersVM _orderVM;
        public UserOrdersPage(UserOrdersVM VM)
        {
            _orderVM = VM;
            InitializeComponent();
            DataContext = _orderVM;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _orderVM.UpdateOrdersCollection();
        }
    }
}
