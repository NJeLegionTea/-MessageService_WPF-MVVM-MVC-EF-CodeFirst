using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MessageClient.View;
using MessageClient.ViewModel;

namespace MessageClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var autorizationWindow = new AutorizationWindow();
            
            autorizationWindow.DataContext = new AutorizationViewModel(autorizationWindow);

            autorizationWindow.Show();
        }
    }
}
