using ComplaintsAdmin.Model;
using ComplaintsAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _adminList;
        public ObservableCollection<string> AdminList
        {
            get { return _adminList; }
            set { _adminList = value; OnPropertyChanged("AdminList"); }
        }

        AccessServiceADO accessService = new AccessServiceADO();

        private void LoadData()
        {
            AdminList = new ObservableCollection<string>(accessService.GetAdmins());
        }


        public LoginViewModel()
        {
            LoadData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
