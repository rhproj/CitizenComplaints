using Complaints_WPF.Models;
using Complaints_WPF.ViewModels;
using Complaints_WPF.Views;
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
            var dbService = new ComplaintServiceADO();
            var viewModel = new ComplaintsViewModel(dbService);
            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
