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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = ViewModelServiceSetter.SetUpViewModel();

            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }
    }
}
