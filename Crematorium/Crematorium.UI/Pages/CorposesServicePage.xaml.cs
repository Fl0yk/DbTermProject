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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crematorium.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для CorposesServicePage.xaml
    /// </summary>
    public partial class CorposesServicePage : UserControl
    {
        private CorposesVM corposesVM;
        public CorposesServicePage(CorposesVM VM)
        {
            corposesVM = VM;
            InitializeComponent();
            DataContext = corposesVM;
        }
    }
}
