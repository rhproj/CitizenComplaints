using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using ComplaintsAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.ViewModels
{
    class EditUsersViewModel : INotifyPropertyChanged
    {
        public Prosecutor NewUser { get; set; }


        private ObservableCollection<Prosecutor> _prosecutorsList;
        public ObservableCollection<Prosecutor> ProsecutorsList
        {
            get { return _prosecutorsList; }
            set { _prosecutorsList = value; OnPropertyChanged(); }
        }

        public RelayCommand AddUserCommand { get;}

        AccessServiceADO accessService;

        private void LoadData()
        {
            ProsecutorsList = new ObservableCollection<Prosecutor>(accessService.LoadProsecutorsInfo());

        }

        public EditUsersViewModel()
        {
            accessService = new AccessServiceADO();
            NewUser = new Prosecutor();

            LoadData();

            AddUserCommand = new RelayCommand(AddUser,null);
        }

        private void AddUser()
        {
            accessService.AddToUsersList(NewUser);
            LoadData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
