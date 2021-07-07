using ComplaintsAdmin.Commands;
using ComplaintsAdmin.Model;
using ComplaintsAdmin.Services;
using ComplaintsAdmin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComplaintsAdmin.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
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
            if (!accessService.Authenticate(AdminUser.Login, AdminUser.Password))
            {
                MessageBox.Show("Неверное имя пользователя\n или пароль!");
                return;
            }
            else
            {
                var window = new EditUsersView(); //owner?
                window.Show(); //not ShowDialog or mother W won't be closed!

                (p as System.Windows.Window).Close();
            }
        }

        AccessServiceADO accessService;

        public LoginViewModel()
        {
            this.AdminUser = new AdminUser();
            accessService = new AccessServiceADO();

            LoginCommand = new RelayCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
