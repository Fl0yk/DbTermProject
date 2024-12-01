using Crematorium.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        private HomeVM _homeVM;
        public HomePage(HomeVM VM)
        {
            _homeVM = VM;
            InitializeComponent();
            DataContext = _homeVM;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _homeVM.UpdateCollections();
        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {
            _homeVM.UpdateDates();
        }
    }
}
