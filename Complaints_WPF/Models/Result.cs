using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class Result : INotifyPropertyChanged
    {
        private int _resultID;
        public int ResultID
        {
            get { return _resultID; }
            set { _resultID = value; OnPropertyChanged("ResultID"); }
        }

        private string _rezolution;
        public string Rezolution
        {
            get { return _rezolution; }
            set { _rezolution = value; OnPropertyChanged("Rezolution"); }
        }

        //private string _note;
        //public string Note
        //{
        //    get { return _note; }
        //    set { _note = value; OnPropertyChanged("Note"); }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
