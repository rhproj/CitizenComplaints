using ComplaintsAdmin.Commands;
using ComplaintsAdmin.Model;
using ComplaintsAdmin.Services;
using ComplaintsAdmin.Startup;
using ComplaintsAdmin.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComplaintsAdmin.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private IAccessService _accessService;
        private ILoginService _loginService;
        
        private AdminUser _adminUser;
        public AdminUser AdminUser 
        {
            get { return _adminUser; }
            set { _adminUser = value; OnPropertyChanged(); } 
        }

        public ICommand LoginCommand { get; }


        private bool CanLoginCommandExecute(object p) 
        {
            if (string.IsNullOrWhiteSpace(AdminUser.Login))
                return false;
            else
                return true;
        }

        private void OnLoginCommandExecuted(object p)
        {
            if (!_loginService.Authenticate(AdminUser.Login, AdminUser.Password))
            {
                MessageBox.Show("Неверное имя пользователя\n или пароль!");
                return;
            }
            else
            {
                var window = new EditUsersView(_accessService);
                window.Show();

                (p as System.Windows.Window).Close();
            }
        }

        public LoginViewModel(IAccessService accessService, ILoginService loginService)
        {
            ServerAccess.TestServerAccess();

            _accessService = accessService;
            _loginService = loginService;

            AdminUser = new AdminUser();

            LoginCommand = new RelayCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
