using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.Pages;
using Crematorium.UI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Crematorium.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM _mainWindowVM;
        public MainWindow(MainWindowVM VM)
        {
            _mainWindowVM = VM;
            InitializeComponent();
            MainListView.DataContext = _mainWindowVM;
            BtnAccount.DataContext = _mainWindowVM;
        }

        private void ShowMenu(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 0.5;
        }

        private void HideMenu(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 1;
        }

        private void GridContent_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BtnShowHide.IsChecked = false;
        }

        private void ClosePr(object sender, RoutedEventArgs e)
        {
            foreach (Window w in App.Current.Windows)
            {
                w.Close();
            }
            this.Close();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void UsersContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(UsersPage));
        }

        private void UrnsContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(RitualUrnServicePage));
        }

        private void OrdersContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(UserOrdersPage));
        }

        private void AllOrdersContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(AllOrdersPage));
        }

        private void HomeContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(HomePage));
        }

        private void HallsContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(HallServicePage));
        }

        private void AccountContent(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(UserAccountPage));
        }

        private void Window_MousDown(object sender, MouseButtonEventArgs e) 
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.LoginUser();
            DataContext = ServicesFabric.GetPage(typeof(HomePage));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ServicesFabric.GetPage(typeof(HomePage));
        }
    }
}
