using Crematorium.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Crematorium.UI
{
    public class App : System.Windows.Application
    {
        readonly MainWindow startWindow;

        // через систему внедрения зависимостей получаем объект главного окна
        public App(MainWindow mainWindow)
        {
            this.startWindow = mainWindow;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            startWindow.Show();  // отображаем главное окно на экране
            base.OnStartup(e);
        }
    }
}
