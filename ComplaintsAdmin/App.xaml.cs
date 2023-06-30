using ComplaintsAdmin.Services;
using ComplaintsAdmin.ViewModels;
using ComplaintsAdmin.Views;
using System.Windows;

namespace ComplaintsAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //internal static AccessService _dbService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbService = new AccessServiceADO();
            var loginService = new LoginService();

            LoginViewModel viewModel = new LoginViewModel(dbService, loginService);

            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
