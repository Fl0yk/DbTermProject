using Crematorium.UI.ViewModels;
using System.Windows.Controls;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для AllOrdersPage.xaml
    /// </summary>
    public partial class AllOrdersPage : UserControl
    {
        private AllOrdersVM _orderVM;
        public AllOrdersPage(AllOrdersVM VM)
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
