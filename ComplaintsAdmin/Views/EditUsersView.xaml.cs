using System.Windows;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for EditUsersView.xaml
    /// </summary>
    public partial class EditUsersView : Window
    {
        public EditUsersView()
        {
            InitializeComponent();
            btnClose.Click += (s, e) => Close();
        }
    }
}
