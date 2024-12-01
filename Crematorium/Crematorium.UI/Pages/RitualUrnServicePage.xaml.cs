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
    /// Логика взаимодействия для RitualUrnServicePage.xaml
    /// </summary>
    public partial class RitualUrnServicePage : UserControl
    {
        private RitualUrnsVM ritualUrnsVM;
        public RitualUrnServicePage(RitualUrnsVM VM)
        {
            ritualUrnsVM = VM;
            InitializeComponent();
            DataContext = ritualUrnsVM;
        }
    }
}
