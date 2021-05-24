using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class Prosecutor : INotifyPropertyChanged
    {
        private int _prosecutorID;
        public int ProsecutorID
        {
            get { return _prosecutorID; }
            set { _prosecutorID = value; OnPropertyChanged("ProsecutorID"); }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged("Login"); }
        }

        private string _prosecutorName;
        public string ProsecutorName
        {
            get { return _prosecutorName; }
            set { _prosecutorName = value; OnPropertyChanged("ProsecutorName"); }
        }

        public string Password { get; set; } //not used yet

        public override string ToString()
        {
            return this.ProsecutorName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
