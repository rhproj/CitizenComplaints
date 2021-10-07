using System.Windows;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            btnClose.Click += (s, e) => Close();
        }
    }
}
