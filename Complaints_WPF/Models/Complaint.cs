using OfficeOpenXml.Attributes;
using System;
using System.ComponentModel;

namespace Complaints_WPF.Models
{
    [EpplusTable]
    public class Complaint : INotifyPropertyChanged
    {
        private int _complaintID;
        [EpplusIgnore]
        public int ComplaintID
        {
            get { return _complaintID; }
            set { _complaintID = value; OnPropertyChanged("ComplaintID"); }
        }

        private int _enumerator;
        [EpplusTableColumn(Order = 0)]
        public int Enumerator
        {
            get { return _enumerator; }
            set { _enumerator = value; OnPropertyChanged("Enumerator"); }
        }

        private DateTime _receiptDate;
        [EpplusTableColumn(Order = 1)]
        public DateTime ReceiptDate
        {
            get { return _receiptDate; }
            set { _receiptDate = value; OnPropertyChanged("ReceiptDate"); }
        }

        private Citizen _citizen;
        [EpplusNestedTableColumn(Order = 2)]
        public Citizen Citizen
        {
            get { return _citizen; }
            set { _citizen = value; OnPropertyChanged("Citizen"); }
        }

        private OZhClassification _oZhComplaintText;
        [EpplusTableColumn(Order = 3)]
        public OZhClassification OZhComplaintText
        {
            get { return _oZhComplaintText; }
            set { _oZhComplaintText = value; OnPropertyChanged("OZhComplaintText"); }
        }

        private string _pageNum;
        [EpplusIgnore]
        public string PageNum
        {
            get { return _pageNum; }
            set { _pageNum = value; OnPropertyChanged("PageNum"); }
        }

        private string _appendNum;
        [EpplusIgnore]
        public string AppendNum
        {
            get { return _appendNum; }
            set { _appendNum = value; OnPropertyChanged("AppendNum"); }
        }

        private string _digitalStorage;
        [EpplusIgnore]
        public string DigitalStorage
        {
            get { return _digitalStorage; }
            set { _digitalStorage = value; OnPropertyChanged("DigitalStorage"); }
        }

        private string _comments;
        [EpplusTableColumn(Order = 4)]
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; OnPropertyChanged("Comments"); }
        }

        private Prosecutor _prosecutor;
        [EpplusTableColumn(Order = 5)]
        public Prosecutor Prosecutor
        {
            get { return _prosecutor; }
            set { _prosecutor = value; OnPropertyChanged("Prosecutor"); }
        }
        private Result _result;
        [EpplusTableColumn(Order = 6)]
        public Result Result
        {
            get { return _result; }
            set { _result = value; OnPropertyChanged("Result"); }
        }

        private Chief _chief;
        [EpplusTableColumn(Order = 7)]
        public Chief Chief
        {
            get { return _chief; }
            set { _chief = value; OnPropertyChanged("Chief"); }
        }

        #region CTOR
        public Complaint()
        {
            Citizen = new Citizen();
            OZhComplaintText = new OZhClassification();
            Result = new Result();
            Prosecutor = new Prosecutor();
            Chief = new Chief();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
