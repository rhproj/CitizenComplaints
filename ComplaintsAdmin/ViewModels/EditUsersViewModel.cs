﻿using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using ComplaintsAdmin.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComplaintsAdmin.ViewModels
{
    public class EditUsersViewModel : INotifyPropertyChanged
    {
        private IAccessService _accessService;

        private Prosecutor _newUser;
        public Prosecutor NewUser 
        {
            get { return _newUser; }
            set { _newUser = value; OnPropertyChanged(); } 
        }

        public Prosecutor SelectedUser { get; set; }

        private ObservableCollection<Prosecutor> _prosecutorsList;
        public ObservableCollection<Prosecutor> ProsecutorsList
        {
            get { return _prosecutorsList; }
            set { _prosecutorsList = value; OnPropertyChanged(); }
        }

        public RelayCommand AddUserCommand { get;}
        public RelayCommand DeleteUserCommand { get; }


        private void LoadData()
        {
            ProsecutorsList = new ObservableCollection<Prosecutor>(_accessService.LoadProsecutorsInfo());
        }

        public EditUsersViewModel(IAccessService accessService)
        {
            _accessService = accessService;
            NewUser = new Prosecutor();

            LoadData();

            AddUserCommand = new RelayCommand(AddUser, CanAddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
        }

        private bool CanDeleteUser()
        {
            if (SelectedUser != null)
                return true;
            else
                return false;
        }
        private void DeleteUser()
        {
            _accessService.DeleteFromUsersList(SelectedUser);
            LoadData();
        }

        private bool CanAddUser()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Login) || string.IsNullOrWhiteSpace(NewUser.ProsecutorName))
                return false;
            else
                return true;
        }
        private void AddUser()
        {
            _accessService.AddToUsersList(NewUser);
            LoadData();
            NewUser = new Prosecutor();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
