using Complaints_WPF.ViewModels;
using Complaints_WPF.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Complaints_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            var viewModel = new ComplaintsViewModel();
            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
