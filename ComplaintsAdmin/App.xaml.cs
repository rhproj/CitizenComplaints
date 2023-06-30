using ComplaintsAdmin.Services;
using ComplaintsAdmin.Views;
using System.Windows;

namespace ComplaintsAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static AccessService _dbService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _dbService = new AccessServiceADO();
            Application.Current.MainWindow = new LoginWindow();
            Application.Current.MainWindow.Show();
        }
    }
}
