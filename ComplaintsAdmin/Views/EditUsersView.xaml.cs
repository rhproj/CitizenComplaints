using ComplaintsAdmin.Services;
using ComplaintsAdmin.ViewModels;
using System.Windows;

namespace ComplaintsAdmin.Views
{
    /// <summary>
    /// Interaction logic for EditUsersView.xaml
    /// </summary>
    public partial class EditUsersView : Window
    {
        public EditUsersView(IAccessService accessService)
        {
            InitializeComponent();
            DataContext = new EditUsersViewModel(accessService);

            btnClose.Click += (s, e) => Close();
        }
    }
}
