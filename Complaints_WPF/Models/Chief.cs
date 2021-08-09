using System.ComponentModel;

namespace Complaints_WPF.Models
{
    public class Chief : INotifyPropertyChanged
    {
        private int _chiefId;
        public int ChiefID
        {
            get { return _chiefId; }
            set { _chiefId = value; OnPropertyChanged("ChiefID"); }
        }

        private string _chiefName;
        public string ChiefName
        {
            get { return _chiefName; }
            set { _chiefName = value; OnPropertyChanged("ChiefName"); }
        }

        public override string ToString()
        {
            return this.ChiefName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
