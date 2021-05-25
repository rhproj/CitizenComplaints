using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using Complaints_WPF.Services;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

        private ObservableCollection<string> _oZhClassificationList;
        public ObservableCollection<string> OZhClassificationList
        {
            get { return _oZhClassificationList; }
            set { _oZhClassificationList = value; OnPropertyChanged("OZhClassificationList"); }
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

        private int _currentNum; //13.01 Used to summ up all complaints and show the current one
        public int CurrentNum
        {
            get { return _currentNum; }
            set { _currentNum = value; OnPropertyChanged("CurrentNum"); }
        }

        private string _message; 
        public string Message 
        {
            get { return _message; }
            set{_message = value; OnPropertyChanged("Message");}
        }

        #region Props to Filter
        private string _numberToFilter;
        public string NumberToFilter
        {
            get { return _numberToFilter; }
            set { _numberToFilter = value; OnPropertyChanged("NumberToFilter"); }
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

        //private string _contentToFilter; //before it became Combobox
        //public string ContentToFilter
        //{
        //    get { return _contentToFilter; }
        //    set { _contentToFilter = value; OnPropertyChanged("ContentToFilter"); }
        //}
        
        private string _oZhComplaintToFilter;
        public string OZhComplaintToFilter
        {
            get { return _oZhComplaintToFilter; }
            set { _oZhComplaintToFilter = value; OnPropertyChanged("OZhComplaintToFilter"); }
        }

        private string _commentsToFilter;
        public string CommentsToFilter
        {
            get { return _commentsToFilter; }
            set { _commentsToFilter = value; OnPropertyChanged("CommentsToFilter"); }
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

        private string _addValueToCombobox;  // K !
        public string AddValueToCombobox
        {
            get { return _addValueToCombobox; }
            set { _addValueToCombobox = value; OnPropertyChanged("AddValueToCombobox"); }
        }

        #endregion
        #region AUTHORIZATION
        public static string ProsecutorLogin { get; set; } //workaround for ProsName to be passed

        //public static string CurrentYear { get; set; }
        //private string _currentYear;
        //public string CurrentYear
        //{
        //    get { return _currentYear; }
        //    set { _currentYear = value; OnPropertyChanged("CurrentYear"); }
        //}

        public static string YearToFilter { get; set; }

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

        public RelayCommand SaveSpreadSheetsCommand { get; }  // SaveToCsvCommand

        public RelayCommand FilterCommand { get; }
        public RelayCommand UnFilterCommand { get; }

        #region ComboConstruct // K !
        public RelayCommand AddOzhCommand { get; }
        public RelayCommand AddChiefCommand { get; }
        public RelayCommand DeleteChiefCommand { get; }
        //public RelayCommand DeleteOzhCommand { get; set; }
        #endregion

        #endregion
        #endregion

        #region CTOR
        public ComplaintsViewModel()
        {
            complaintService = new ComplaintServiceADO(); //using ADO.Net
            CurrentComplaint = new Complaint(); //? why do we need this? see if it can be omitted //NO cuz its thru this all fields are filled and passed to methods           

            YearToFilter = DateTime.Now.Year.ToString();
            
            _newEntryCommand = new RelayCommand(NewEntry, null);
            _registerCommand = new RelayCommand(RegisterComplaint, RegisterComplaint_CanExecute); //b/f not prop, cuz it's read-only (no set) And that's why no set! - we set it here!

            //                                                                                      //its  prop will {get} its value from it and we'll bind it with UI Command={Binding RegisterCommand}
            _findCitizenCommand = new RelayCommand(FindCitizen, null);

            _editCommand = new RelayCommand(EditComplaint, null);
            _deleteComplaintCommand = new RelayCommand(DeleteComplaint, DeleteComplaint_CanExecute);

            FilterCommand = new RelayCommand(FilterComplaints, null);   //_filterCommand = new RelayCommand(FilterComplaints, null);           
            UnFilterCommand = new RelayCommand(UnFilteromplaints, null);

            AddOzhCommand = new RelayCommand(AddToOzhCombobox, AddToCombobox_CanExecute); //ComboConstructor
            AddChiefCommand = new RelayCommand(AddToChiefsCombobox, AddToCombobox_CanExecute);
            DeleteChiefCommand = new RelayCommand(DeleteChief, DeleteChief_CanExecute);
            //DeleteOzhCommand = new RelayCommand(DeleteOzh, DeleteOzh_CanExecute);

            OZhClassificationList = new ObservableCollection<string>(complaintService.LoadOZhClassification());
            ResultsList = new ObservableCollection<string>(complaintService.LoadResults());     //moved this 2 from Load() so they don't have to reload every entry         
            ProsecutorsList = new ObservableCollection<string>(complaintService.LoadProsecutors()); //for login window only
            ChiefsList = new ObservableCollection<string>(complaintService.LoadChiefs()); 

            LoadData(YearToFilter);

            SaveSpreadSheetsCommand = new RelayCommand(SaveSpreadSheets, null);  //used to be: SaveToCsv
        }
        #endregion

        #region METHODS (Mirrors whats in Services, but there we can use differend data base techniques)

        private void LoadData(string year) //we repeating our GetAll method, why not have it somewhere once and use it? NO, cuz it's easier to feed it to ObsColl this way
        {
            ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaintsByYear(year)); //GetAllComplaints()); //
            CurrentNum = ComplaintsList.Count;
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

            CurrentComplaint.Enumerator = CurrentNum + 1;//activate later with increment !
            CurrentComplaint.Citizen.BirthDate = null;//string.Empty;
            CurrentComplaint.Citizen.CitizenAdress = null;
            CurrentComplaint.Comments = null;
            CurrentComplaint.PageNum = null;
            CurrentComplaint.AppendNum = null;

            CurrentComplaint.DigitalStorage = null; //21.01

            CurrentComplaint.OZhComplaintText.OZhComplaint = null; //used to be this: //CurrentComplaint.ComplaintText = null;//string.Empty;
            CurrentComplaint.Result.Rezolution = null;
            CurrentComplaint.Citizen.Occupation = null;
            CurrentComplaint.Citizen.PhoneNumber = null;
            CurrentComplaint.Citizen.Email = null;

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

                if (isSaved)
                    Message = "Заявление сохранено";
                else
                    Message = "Не удалось сохранить заявление";

                LoadData(YearToFilter); //refreshes
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
            if (string.IsNullOrWhiteSpace(CurrentComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(CurrentComplaint.OZhComplaintText.OZhComplaint)) //b4: //string.IsNullOrWhiteSpace(CurrentComplaint.ComplaintText))
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
                    CurrentComplaint.Citizen.CitizenName = citizen.Citizen.CitizenName;    //added
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
                CurrentComplaint = complaintService.SelectComplaint(YearToFilter, SelectedComplaint.Citizen.CitizenName, SelectedComplaint.ReceiptDate);  //SelectComplaintFun("2021", SelectedComplaint.Citizen.CitizenName, SelectedComplaint.ReceiptDate);    //
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
                if (CurrentComplaint.Enumerator == CurrentNum)
                {
                    bool isDeleted = complaintService.DeleteComplaint(CurrentComplaint.ComplaintID); //, CurrentComplaint.ReceiptDate);
                    if (isDeleted)
                    {
                        LoadData(YearToFilter);
                        Message = "Жалоба удалена";
                        ClearEntryFields(true,true,true);
                    }
                    else
                    {
                        Message = "Не удалось удалить жалобу";
                    }
                }
                else
                {
                    Message = "Вы можете удалить только последнюю запись";
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


        private void FilterComplaints()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NumberToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[N]", NumberToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(DateToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterByDate, "[ReceiptDate]", DateToFilter, YearToFilter)); //FilterComplaints("sp_FilterComplaintsByDate", "@receiptDate", DateToFilter)); //
                }
                else if (!string.IsNullOrWhiteSpace(NameToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterLike, "[FullName]", NameToFilter, YearToFilter));  //FilterComplaints("sp_FilterComplaintsByName", "@fullName", NameToFilter));  //
                }
                else if (!string.IsNullOrWhiteSpace(OZhComplaintToFilter))    //b4: //ContentToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[Content]", OZhComplaintToFilter, YearToFilter));  //FilterComplaints("sp_FilterComplaintsByСontent", "@content", OZhComplaintToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(CommentsToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterLike, "[Comments]", CommentsToFilter, YearToFilter));  //FilterComplaints("sp_FilterComplaintsByName", "@fullName", NameToFilter));  //
                }
                else if (!string.IsNullOrWhiteSpace(ProsecutorToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[ProsecutorName]", ProsecutorToFilter, YearToFilter)); //FilterComplaints("sp_FilterComplaintsByProsecutor", "@prosecutor", ProsecutorToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ChiefToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[ChiefName]", ChiefToFilter, YearToFilter));  //FilterComplaints("sp_FilterComplaintsByChief", "@chiefName", ChiefToFilter));
                }
                else
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaintsByYear(YearToFilter)); //GetAllComplaints());
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void UnFilteromplaints()
        {
            LoadData(YearToFilter);

            NumberToFilter = DateToFilter = NameToFilter = OZhComplaintToFilter = CommentsToFilter = ProsecutorToFilter = ChiefToFilter = null;     //ive replased ContentTo.. with oZh
        }

        #region Editing
        private bool AddToCombobox_CanExecute()
        {
            if (string.IsNullOrWhiteSpace(AddValueToCombobox))
                return false;
            else
                return true;
        }

        private void AddToOzhCombobox()
        {
            try
            {
                bool isAdded = false;

                isAdded = complaintService.AddToOZhClassification(AddValueToCombobox);

                if (isAdded)
                {
                    Message = "Запись добавлеа, чтоб отобразить изменения перезапустите программу";
                }
                else
                {
                    Message = "Не удалось добавить запись";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            ClearValueToAdd();
        }

        private void DeleteOzh()
        {
            try
            {
                bool isDeleted = complaintService.DeleteFromOZhClassification(CurrentComplaint.OZhComplaintText.OZhComplaint);

                if (isDeleted)
                {
                    Message = "Запись удалена, чтобы отобразить изменения перезапустите программу";
                }
                else
                {
                    Message = "Не удалось удалить запись";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            ClearValueToAdd();
        }

        private bool DeleteOzh_CanExecute()
        {
            if (string.IsNullOrEmpty(CurrentComplaint.OZhComplaintText.OZhComplaint))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void AddToChiefsCombobox()
        {
            try
            { //calling EmpService(which already has data in it's ctor) by passing current Employee
                bool isAdded = false;

                isAdded = complaintService.AddToChiefsList(AddValueToCombobox);

                if (isAdded)
                    Message = "Руководитель добавлен, чтобы отобразить изменения перезапустите программу";
                else
                    Message = "Не удалось добавить руководителя";
            }
            catch (Exception ex) ///########///
            {
                Message = ex.Message;
            }

            ClearValueToAdd();
        }

        private void DeleteChief()
        {
            try
            {
                bool isDeleted = complaintService.DeleteFromChiefsList(CurrentComplaint.Chief.ChiefName);

                if (isDeleted)
                {                   
                    Message = "Руководитель удален, чтобы отобразить изменения перезапустите программу";
                }
                else
                {
                    Message = "Не удалось удалить руководителя";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            ClearValueToAdd();
        }

        private bool DeleteChief_CanExecute()
        {
            if (string.IsNullOrEmpty(CurrentComplaint.Chief.ChiefName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ClearValueToAdd()
        {
            AddValueToCombobox = CurrentComplaint.Chief.ChiefName = CurrentComplaint.OZhComplaintText.OZhComplaint = null;
        }
        #endregion


        private void SaveToCsv()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ЖРЖ ({DateTime.Now.ToString("yyyy.MM.dd")}).csv", false, Encoding.Unicode))
                {
                    sw.WriteLine($"Журнал регистрации жалоб по состаянию на {YearToFilter} год");
                    sw.WriteLine("№;Дата/время;Имя заявителя;Жалоба;Примечание;Результат;Принял(а);Принимающий руководитель");

                    for (int i = ComplaintsList.Count-1; i >= 0; i--)
                    {
                        sw.WriteLine($"{ComplaintsList[i].Enumerator};{ComplaintsList[i].ReceiptDate};{ComplaintsList[i].Citizen.CitizenName};{ComplaintsList[i].OZhComplaintText.OZhComplaint};{ComplaintsList[i].Comments};{ComplaintsList[i].Result.Rezolution};{ComplaintsList[i].Prosecutor.ProsecutorName};{ComplaintsList[i].Chief.ChiefName}"); //,");
                    }
                }

                Message = "Таблица сохранена на Рабочем столе";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private void SaveSpreadSheets()
        {
            try
            {
                LoadData(YearToFilter);

                EPPlusService.SaveComplaintsToExcel(ComplaintsList);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}