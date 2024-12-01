using Crematorium.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangeHallPage.xaml
    /// </summary>
    public partial class ChangeHallPage : Window
    {
        private ChangeHallVM _hallVM;
        public ChangeHallPage(ChangeHallVM hallVM)
        {
            InitializeComponent();
            _hallVM = hallVM;
            DataContext = _hallVM;
        }
        public void InitializeHall(int Id)
        {
            _hallVM.SetHall(Id);
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
            //Close();
            Hide();
        }
    }
}
