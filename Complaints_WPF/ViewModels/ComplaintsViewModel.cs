using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Complaints_WPF.ViewModels
{
    public class ComplaintsViewModel : INotifyPropertyChanged
    {
        #region Prop
        ComplaintServiceADO complaintService; // = new ComplaintService(); принято это делать в CTOR-е

        //private Prosecutor _prosecutorU;
        //public Prosecutor ProsecutorU
        //{
        //    get { return _prosecutorU; }
        //    set { _prosecutorU = value; OnPropertyChanged("ProsecutorU"); } //see if i need OnPropertyChanged() here 
        //}

        private Complaint _currentComplaint;
        public Complaint CurrentComplaint
        {
            get { return _currentComplaint; }
            set { _currentComplaint = value; OnPropertyChanged("CurrentComplaint"); }
        }

        private Complaint _selectedComplaint;
        public Complaint SelectedComplaint
        {
            get { return _selectedComplaint; }
            set { _selectedComplaint = value; OnPropertyChanged("SelectedComplaint"); }
        }

        private ObservableCollection<Complaint> _complaintsList;
        public ObservableCollection<Complaint> ComplaintsList
        {
            get { return _complaintsList; }
            set { _complaintsList = value; OnPropertyChanged("ComplaintsList"); }
        }

        private ObservableCollection<string> _resultsList; //25.11.20
        public ObservableCollection<string> ResultsList
        {
            get { return _resultsList; }
            set { _resultsList = value; OnPropertyChanged("ResultsList"); }
        }

        private ObservableCollection<string> _prosecutorsList; //25.11.20
        public ObservableCollection<string> ProsecutorsList
        {
            get { return _prosecutorsList; }
            set { _prosecutorsList = value; OnPropertyChanged("ProsecutorsList"); }
        }

        private ObservableCollection<string> _chiefsList; //29.11.20
        public ObservableCollection<string> ChiefsList
        {
            get { return _chiefsList; }
            set { _chiefsList = value; OnPropertyChanged("ChiefsList"); }
        }

        private string _message; //Don't forget to implement it later
        public string Message  //will be bind to tBlock in UI
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private string _dateToFilter;
        public string DateToFilter
        {
            get { return _dateToFilter; }
            set { _dateToFilter = value; OnPropertyChanged("DateToFilter"); }
        }

        private string _nameToFilter;
        public string NameToFilter
        {
            get { return _nameToFilter; }
            set { _nameToFilter = value; OnPropertyChanged("NameToFilter"); }
        }

        private string _contentToFilter;
        public string ContentToFilter
        {
            get { return _contentToFilter; }
            set { _contentToFilter = value; OnPropertyChanged("ContentToFilter"); }
        }

        private string _prosecutorToFilter;
        public string ProsecutorToFilter
        {
            get { return _prosecutorToFilter; }
            set { _prosecutorToFilter = value; OnPropertyChanged("ProsecutorToFilter"); }
        }

        private string _chiefToFilter;
        public string ChiefToFilter
        {
            get { return _chiefToFilter; }
            set { _chiefToFilter = value; OnPropertyChanged("ChiefToFilter"); }
        }
        #region AUTHORIZATION
        public static string ProsecutorLogin { get; set; } //workaround for ProsName to be passed
        //public static string СhiefProsecutor { get; set; } //when Chiefs were upfront
        //private string _loginName;
        //public string LoginName
        //{
        //    get { return _loginName; }
        //    set { _loginName = ProsecutorLogin; OnPropertyChanged("LoginName"); }
        //}
        #endregion

        //07-10.Commands:
        #region COMMAND props

        //private RelayCommand _searchCommand;
        //public RelayCommand SearchCommand
        //{
        //    get { return _searchCommand; } //btnz probably don't need {set} most of the time
        //}

        private RelayCommand _newEntryCommand;
        public RelayCommand NewEntryCommand
        {
            get { return _newEntryCommand; }
        }

        private RelayCommand _registerCommand;
        public RelayCommand RegisterCommand  //right now it's null, in order to create its instance we need to pass a meth that matches Action delegate signature
        {
            get { return _registerCommand; } //no set, cuz binds only one way
        }

        private RelayCommand _findCitizenCommand;
        public RelayCommand FindCitizenCommand
        {
            get { return _findCitizenCommand; }
        }

        public RelayCommand UnFilterCommand { get; set; }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get { return _editCommand; }
        }

        private RelayCommand _deleteComplaintCommand;
        public RelayCommand DeleteComplaintCommand //{ get; }
        {
            get { return _deleteComplaintCommand; }
        }

        public RelayCommand FilterCommand { get; }
        #endregion
        #endregion

        #region CTOR
        public ComplaintsViewModel()
        {
            complaintService = new ComplaintServiceADO(); //using ADO.Net
            CurrentComplaint = new Complaint(); //? why do we need this? see if it can be omitted //NO cuz its thru this all fields are filled and passed to methods           

            _newEntryCommand = new RelayCommand(NewEntry, null);
            _registerCommand = new RelayCommand(RegisterComplaint, RegisterComplaint_CanExecute); //b/f not prop, cuz it's read-only (no set) And that's why no set! - we set it here!

            //                                                                                      //its  prop will {get} its value from it and we'll bind it with UI Command={Binding RegisterCommand}
            _findCitizenCommand = new RelayCommand(FindCitizen, null);

            _editCommand = new RelayCommand(EditComplaint, null);
            _deleteComplaintCommand = new RelayCommand(DeleteComplaint, DeleteComplaint_CanExecute);

            FilterCommand = new RelayCommand(FilterComplaints, null);   //_filterCommand = new RelayCommand(FilterComplaints, null);           
            UnFilterCommand = new RelayCommand(UnFilteromplaints, null);

            ResultsList = new ObservableCollection<string>(complaintService.LoadResults());     //moved this 2 from Load() so they don't have to reload every entry         
            ProsecutorsList = new ObservableCollection<string>(complaintService.LoadProsecutors()); //for login window only
            ChiefsList = new ObservableCollection<string>(complaintService.LoadChiefs()); 

            LoadData();
        }
        #endregion

        #region METHODS (Mirrors whats in Services, but there we can use differend data base techniques)

        private void LoadData() //we repeating our GetAll method, why not have it somewhere once and use it? NO, cuz it's easier to feed it to ObsColl this way
        {
            ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaints()); //GetAllComplaints());
        }

        private void ClearEntryFields(bool withName, bool withChief, bool withMessage)
        {
            if (withName)
            {
                CurrentComplaint.Citizen.CitizenName = null;//string.Empty;
            }
            if (withChief)
            {
                CurrentComplaint.Chief.ChiefName = null;
            }
            if (withMessage)
            {
                Message = null;
            }

            CurrentComplaint.ComplaintText = null;//string.Empty;
            CurrentComplaint.Citizen.BirthDate = null;//string.Empty;
            CurrentComplaint.Citizen.CitizenAdress = null;//string.Empty;
            CurrentComplaint.Comments = null;//string.Empty;
            CurrentComplaint.PageNum = null;//string.Empty;
            CurrentComplaint.AppendNum = null;//string.Empty;
            CurrentComplaint.Result.Rezolution = null;//string.Empty;
            CurrentComplaint.Citizen.Occupation = null;//string.Empty;
            CurrentComplaint.Citizen.PhoneNumber = null;//string.Empty;
            CurrentComplaint.Citizen.Email = null;//string.Empty;

            CurrentComplaint.ComplaintID = 0;
            CurrentComplaint.Citizen.CitizenID = 0;
        }

        public void NewEntry()
        {
            ClearEntryFields(true, true, true);
        }

        //07.Commands: now we create method = Action signature, and inside meth we call our EmpServise to actually save data
        public void RegisterComplaint()
        {
            try
            { //calling EmpService(which already has data in it's ctor) by passing current Employee
                bool isSaved = false;

                //isSaved = complaintService.AddToComplaintList(CurrentComplaint);
                if (CurrentComplaint.Citizen.CitizenID == 0)
                {
                    isSaved = complaintService.AddToComplaintList(CurrentComplaint, ProsecutorLogin);
                }
                else if (CurrentComplaint.ComplaintID == 0) //Гражданин сущ-ет в базе
                {
                    isSaved = complaintService.AddToComplaintListUpd(CurrentComplaint, ProsecutorLogin); //
                }
                else
                {
                    isSaved = complaintService.UpdateComplaint(CurrentComplaint); //корректировка всего Заявления с инфой по гр-ну
                }

                LoadData(); //refreshes

                if (isSaved)
                    Message = "Заявление сохранено";
                else
                    Message = "Не удалось сохранить заявление";
                //try later: //Message = isSaved? "Employee saved": "RegisterCommand failed";

                ClearEntryFields(true, false, false);
            }
            catch (Exception ex) ///########///
            {
                Message = ex.Message;
            }
        }
        private bool RegisterComplaint_CanExecute()
        {
            if (string.IsNullOrWhiteSpace(CurrentComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(CurrentComplaint.ComplaintText))
                return false;
            else
                return true;
        }

        public void FindCitizen()
        {
            //CurrentComplaint.CitizenID = 0; //better:
            ClearEntryFields(false, false, true);
            try
            {
                Complaint citizen = complaintService.SearchCitizen(CurrentComplaint.Citizen.CitizenName);
                if (citizen != null)
                {
                    CurrentComplaint.Citizen.CitizenID = citizen.Citizen.CitizenID;
                    CurrentComplaint.Citizen.CitizenAdress = citizen.Citizen.CitizenAdress;
                    CurrentComplaint.Citizen.Occupation = citizen.Citizen.Occupation;
                    CurrentComplaint.Citizen.PhoneNumber = citizen.Citizen.PhoneNumber;
                    CurrentComplaint.Citizen.Email = citizen.Citizen.Email;
                    CurrentComplaint.Citizen.BirthDate = citizen.Citizen.BirthDate;
                }
                else
                {
                    Message = "Заявитель не найден";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void EditComplaint()
        {
            try
            {
                //im thinking Clear Fields not req-ed since we asigning every field anyway
                CurrentComplaint = complaintService.SelectComplaint(SelectedComplaint.Citizen.CitizenName, SelectedComplaint.ReceiptDate);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void DeleteComplaint()
        {
            try
            {
                bool isDeleted = complaintService.DeleteComplaint(CurrentComplaint.ComplaintID); //, CurrentComplaint.ReceiptDate);
                if (isDeleted)
                {
                    LoadData();
                    Message = "Жалоба удалена";
                }
                else
                {
                    Message = "Не удалось удалить жалобу";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            ClearEntryFields(true, false, false);
        }
        private bool DeleteComplaint_CanExecute()
        {
            if (CurrentComplaint.ComplaintID == 0)
                return false;
            else
                return true;
        }

        //private bool CanEvaluatorTrue()
        //{
        //    return true;
        //}

        private void FilterComplaints()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(DateToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaints("sp_FilterComplaintsByDate", "@receiptDate", DateToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(NameToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaints("sp_FilterComplaintsByName", "@fullName", NameToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ContentToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaints("sp_FilterComplaintsByСontent", "@content", ContentToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ProsecutorToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaints("sp_FilterComplaintsByProsecutor", "@prosecutor", ProsecutorToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ChiefToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaints("sp_FilterComplaintsByChief", "@chiefName", ChiefToFilter));
                }
                else
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaints()); //GetAllComplaints());
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void UnFilteromplaints()
        {
            LoadData();

            DateToFilter = NameToFilter = ContentToFilter = ProsecutorToFilter = ChiefToFilter = null;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
