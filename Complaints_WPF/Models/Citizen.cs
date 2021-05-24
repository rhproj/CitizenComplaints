using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class Citizen : INotifyPropertyChanged
    {
        private int _citizenID;
        public int CitizenID
        {
            get { return _citizenID; }
            set { _citizenID = value; OnPropertyChanged("CitizenID"); }
        }

        private string _citizenName;
        public string CitizenName
        {
            get { return _citizenName; }
            set
            {
                _citizenName = value;
                //Validate();
                OnPropertyChanged("CitizenName");
            }
        }

        private string _birthDate;
        public string BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged("BirthDate"); }
        }

        private string _citizenAdress;
        public string CitizenAdress
        {
            get { return _citizenAdress; }
            set { _citizenAdress = value; OnPropertyChanged("CitizenAdress"); }
        }

        //added later
        private string _occupation;
        public string Occupation
        {
            get { return _occupation; }
            set { _occupation = value; OnPropertyChanged("Occupation"); }
        }

        private string _phonNumber;
        public string PhoneNumber
        {
            get { return _phonNumber; }
            set { _phonNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public override string ToString()
        {
            return this.CitizenName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
