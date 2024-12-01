using Crematorium.Domain.Entities;
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
    /// Логика взаимодействия для OrderInformationPage.xaml
    /// </summary>
    public partial class OrderInformationPage : Window
    {
        OrderInformationVM _informationVM;
        public OrderInformationPage(OrderInformationVM VM)
        {
            _informationVM = VM;
            InitializeComponent();
            DataContext = _informationVM;
        }

        public void InitializeOrder(Order order)
        {
            _informationVM.InitializeOrder(order);
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
    }
}
