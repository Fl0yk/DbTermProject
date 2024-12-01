using Crematorium.Domain.Entities;
using Crematorium.UI.Fabrics;
using Crematorium.UI.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangeCorposePage.xaml
    /// </summary>
    public partial class ChangeCorposePage : Window
    {
        private ChangeCorposeVM _corposeVM;
        public ChangeCorposePage(ChangeCorposeVM VM)
        {
            _corposeVM = VM;
            InitializeComponent();
            DataContext = _corposeVM;
        }

        public void InitializeCorpose(int Id)
        {
            _corposeVM.SetCorpose(Id);
        }

        public void InitializeCorpose(ref Corpose corpose)
        {
            _corposeVM.SetCorpose(ref corpose);
        }

        private void Window_MousDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void RegCorpose(object sender, RoutedEventArgs e)
        {
            try
            {
                _corposeVM.AddCorpose();
                this.Hide();
            }
            catch(Exception ex)
            {
                var er = ServicesFabric.GetErrorPage(ex.Message.ToString());
                er.ShowDialog();
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
