using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class Chief : INotifyPropertyChanged
    {
        private int _chiefId;
        public int ChiefId
        {
            get { return _chiefId; }
            set { _chiefId = value; OnPropertyChanged("ChiefId"); }
        }

        private string _chiefName;
        public string ChiefName
        {
            get { return _chiefName; }
            set { _chiefName = value; OnPropertyChanged("ChiefName"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
