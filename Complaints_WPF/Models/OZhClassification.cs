using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class OZhClassification : INotifyPropertyChanged
    {
        private string _oZhComplaint;
        public string OZhComplaint
        {
            get { return  _oZhComplaint; }
            set {  _oZhComplaint = value; OnPropertyChanged("OZhComplaint"); } //could be bcz of this it didn't clear the Ozh Combobox
        }

        private int _summOZh;
        public int SummOzh
        {
            get { return _summOZh; }
            set { _summOZh = value; OnPropertyChanged("SummOzh"); }
        }

        public override string ToString()
        {
            return this.OZhComplaint;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
