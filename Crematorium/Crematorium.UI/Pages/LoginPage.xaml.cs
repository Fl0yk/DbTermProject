using Crematorium.UI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogAndRegPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private LoginVM loginVM;
        public LoginPage(LoginVM VM)
        {
            loginVM = VM;
            InitializeComponent();
            DataContext = loginVM;
        }

        private void Window_MousDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            //this.Close();
            this.Hide();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            //this.Close();
            if (loginVM.LoginUser())
            {
                loginVM.ClearFields();
                this.Hide();
            }
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            loginVM.ClearFields();
        }
    }
}
