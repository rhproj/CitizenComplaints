using Complaints_WPF.ViewModels;
using System.Windows;

namespace Complaints_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ComplaintsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            btnMin.Click += (s, e) => WindowState = WindowState.Minimized;
            btnMax.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            btnClose.Click += (s, e) => Close();
        }
    }
}
