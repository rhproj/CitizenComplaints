using ComplaintsAdmin.Services;
using ComplaintsAdmin.ViewModels;
using ComplaintsAdmin.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ComplaintsAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbService = new AccessServiceADO();
            var viewModel = new LoginViewModel(dbService);
            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
