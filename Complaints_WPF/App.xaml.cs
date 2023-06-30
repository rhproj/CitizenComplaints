using Complaints_WPF.Services;
using Complaints_WPF.Services.Interfaces;
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
            var viewModel = ViewModelServiceSetter.SetUpViewModel();

            Application.Current.MainWindow = new LoginWindow(viewModel);
            Application.Current.MainWindow.Show();
        }

        //private ComplaintsViewModel SetUpViewModel()
        //{
        //   // var dbService = new ComplaintServiceADO();
        //    var categoryReadService = new CategoryReadService();

        //    var viewModel = new ComplaintsViewModel(dbService, categoryReadService);
        //    return viewModel;
        //}
    }
}
