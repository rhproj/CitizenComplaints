using System.ComponentModel;

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

        public override string ToString()
        {
            return this.Rezolution;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
