using Complaints_WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.ViewModels
{
    public class SummOZhViewModel : INotifyPropertyChanged
    {
        ComplaintServiceADO complaintService;

        private OZhClassification _oZhClassif;
        public OZhClassification OZhClassif
        {
            get { return _oZhClassif; }
            set { _oZhClassif = value; OnPropertyChanged("OZhClassif"); }
        }

        private ObservableCollection<OZhClassification> _oZhClassificationList;
        public ObservableCollection<OZhClassification> OZhClassificationList
        {
            get { return _oZhClassificationList; }
            set { _oZhClassificationList = value; OnPropertyChanged("OZhClassificationList"); }
        }

        public SummOZhViewModel()
        {
            OZhClassif = new OZhClassification();
            complaintService = new ComplaintServiceADO();

            //LoadOzhClassification();
            OZhClassificationList = new ObservableCollection<OZhClassification>(complaintService.LoadOZhWithSumm(ComplaintsViewModel.YearToFilter)); //LoadOZhClassif()); //
        }

        //private void LoadOzhClassification()
        //{
        //    OZhClassificationList = new ObservableCollection<OZhClassification>(complaintService.LoadOZhClassif()); //LoadOZhClassification());
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
