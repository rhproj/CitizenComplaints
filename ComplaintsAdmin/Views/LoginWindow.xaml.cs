using ComplaintsAdmin.ViewModels;
using System.Windows;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            btnClose.Click += (s, e) => Close();
        }
    }
}