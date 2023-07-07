using Autofac;
using Complaints_WPF.Startup;
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

            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();

            Application.Current.MainWindow = container.Resolve<LoginWindow>();
            Application.Current.MainWindow.Show();
        }
    }
}
