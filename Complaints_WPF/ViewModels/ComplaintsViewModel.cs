using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using Complaints_WPF.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;

namespace Complaints_WPF.ViewModels
{/// <summary>
/// Main view model
/// </summary>
    public class ComplaintsViewModel : INotifyPropertyChanged
    {
        #region Prop
        IComplaintService complaintService;

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

        private ObservableCollection<string> _resultsList;
        public ObservableCollection<string> ResultsList
        {
            get { return _resultsList; }
            set { _resultsList = value; OnPropertyChanged("ResultsList"); }
        }

        private ObservableCollection<string> _prosecutorsList;
        public ObservableCollection<string> ProsecutorsList
        {
            get { return _prosecutorsList; }
            set { _prosecutorsList = value; OnPropertyChanged("ProsecutorsList"); }
        }

        private ObservableCollection<string> _chiefsList;
        public ObservableCollection<string> ChiefsList
        {
            get { return _chiefsList; }
            set { _chiefsList = value; OnPropertyChanged("ChiefsList"); }
        }

        private ObservableCollection<string> _categoryList;
        public ObservableCollection<string> CategoryList
        {
            get { return _categoryList; }
            set { _categoryList = value; OnPropertyChanged("CategoryList"); }
        }

        private int _currentNum;
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
        //public static string ProsecutorLogin { get; set; }
        //public static string YearToFilter { get; set; }
        private string _prosecutorLogin;
        public string ProsecutorLogin 
        {
            get { return _prosecutorLogin; }
            set { _prosecutorLogin = value; OnPropertyChanged("ProsecutorLogin"); }
        }
        private string _yearToFilter;
        public string YearToFilter
        {
            get { return _yearToFilter; }
            set { _yearToFilter = value; OnPropertyChanged("YearToFilter"); }
        }

        public RelayCommand EnterCommand { get; }

        private bool Enter_CanExecute()
        {
            if (String.IsNullOrWhiteSpace(YearToFilter) || (String.IsNullOrWhiteSpace(ProsecutorLogin)))
                return false;
            else
                return true;
        }

        public Action CloseAction { get; set; }

        private void Enter()
        {
            var window = new MainWindow(this); //owner?
            window.lblProsecutor.Content = ProsecutorLogin;
            window.Show();

            CloseAction();
        }
        #endregion

        #region COMMAND props

        public RelayCommand NewEntryCommand { get; }

        public RelayCommand RegisterCommand { get;  } 

        public RelayCommand FindCitizenCommand { get; }

        public RelayCommand EditCommand { get; }

        public RelayCommand DeleteComplaintCommand { get; }

        public RelayCommand SaveSpreadSheetsCommand { get; }  // SaveToCsvCommand

        public RelayCommand FilterCommand { get; }
        public RelayCommand UnFilterCommand { get; }

        #region ComboConstruct
        public RelayCommand AddOzhCommand { get; }
        public RelayCommand AddChiefCommand { get; }
        public RelayCommand DeleteChiefCommand { get; }
        //public RelayCommand DeleteOzhCommand { get; }
        #endregion

        #endregion
        #endregion

        #region CTOR
        public ComplaintsViewModel(IComplaintService dbService)
        {
            //TestServerAccess();
            YearToFilter = DateTime.Now.Year.ToString();

            complaintService = dbService;
            CurrentComplaint = new Complaint(); 

            EnterCommand = new RelayCommand(Enter, Enter_CanExecute); //

            NewEntryCommand = new RelayCommand(NewEntry, null);
            RegisterCommand = new RelayCommand(RegisterComplaint, RegisterComplaint_CanExecute); 

            FindCitizenCommand = new RelayCommand(FindCitizen, null);

            EditCommand = new RelayCommand(EditComplaint, null);
            DeleteComplaintCommand = new RelayCommand(DeleteComplaint, DeleteComplaint_CanExecute);

            FilterCommand = new RelayCommand(FilterComplaints, null);   
            UnFilterCommand = new RelayCommand(UnFilteromplaints, null);

            AddOzhCommand = new RelayCommand(AddToOzhCombobox, AddToCombobox_CanExecute); //ComboEditView
            AddChiefCommand = new RelayCommand(AddToChiefsCombobox, AddToCombobox_CanExecute);
            DeleteChiefCommand = new RelayCommand(DeleteChief, DeleteChief_CanExecute);
            //DeleteOzhCommand = new RelayCommand(DeleteOzh, DeleteOzh_CanExecute);

            OZhClassificationList = new ObservableCollection<string>(complaintService.LoadOZhClassification());
            ResultsList = new ObservableCollection<string>(complaintService.LoadResults());     //moved this 2 from Load() so they don't have to reload every entry         
            ProsecutorsList = new ObservableCollection<string>(complaintService.LoadProsecutors()); //for login window only
            ChiefsList = new ObservableCollection<string>(complaintService.LoadChiefs());
            CategoryList = new ObservableCollection<string>(complaintService.LoadCategories());

            LoadData(YearToFilter);

            SaveSpreadSheetsCommand = new RelayCommand(SaveSpreadSheets, null);
        }
        #endregion

        #region METHODS
        private static void TestServerAccess()
        {
            string adress = ConfigurationManager.AppSettings["address"];
            if (ServerAccess.TestConnection(adress) == false)
            {
                MessageBox.Show($"Отсутствует связь с {adress}");
                Environment.Exit(0);
            }
        }

        private void LoadData(string year) 
        {
            ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaintsByYear(year)); //GetAllComplaints());
            CurrentNum = ComplaintsList.Count;
        }

        private void ClearEntryFields(bool withName, bool withChief, bool withMessage)
        {
            if (withName)
            {
                CurrentComplaint.Citizen.CitizenName = null;
            }
            if (withChief)
            {
                CurrentComplaint.Chief.ChiefName = null;
            }
            if (withMessage)
            {
                Message = null;
            }

            CurrentComplaint.Enumerator = CurrentNum + 1;
            CurrentComplaint.Citizen.BirthDate = null;
            CurrentComplaint.Citizen.CitizenAdress = null;
            CurrentComplaint.Comments = null;
            CurrentComplaint.PageNum = null;
            CurrentComplaint.AppendNum = null;

            CurrentComplaint.DigitalStorage = null;

            CurrentComplaint.OZhComplaintText.OZhComplaint = null;
            CurrentComplaint.Result.Rezolution = null;
            CurrentComplaint.Citizen.Occupation = null;
            CurrentComplaint.Citizen.PhoneNumber = null;
            CurrentComplaint.Citizen.Email = null;

            CurrentComplaint.ComplaintID = 0;
            CurrentComplaint.Citizen.CitizenID = 0;
            CurrentComplaint.Citizen.Category = null;
        }

        public void NewEntry()
        {
            ClearEntryFields(true, true, true);
        }

        public void RegisterComplaint()
        {
            try
            {
                bool isSaved = false;

                if (CurrentComplaint.Citizen.CitizenID == 0)
                {
                    isSaved = complaintService.AddToComplaintList(CurrentComplaint, ProsecutorLogin);
                }
                else if (CurrentComplaint.ComplaintID == 0) //existing citizen makes anover complaint
                {
                    isSaved = complaintService.AddToComplaintListUpd(CurrentComplaint, ProsecutorLogin);
                }
                else
                {
                    isSaved = complaintService.UpdateComplaint(CurrentComplaint);
                }

                if (isSaved)
                    Message = "Заявление сохранено";
                else
                    Message = "Не удалось сохранить заявление";

                LoadData(YearToFilter); //refreshes

                ClearEntryFields(true, false, false);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        private bool RegisterComplaint_CanExecute()
        {
            if (string.IsNullOrWhiteSpace(CurrentComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(CurrentComplaint.OZhComplaintText.OZhComplaint))
                return false;
            else
                return true;
        }

        public void FindCitizen()
        {
            ClearEntryFields(false, false, true);
            try
            {
                Complaint citizen = complaintService.SearchCitizen(CurrentComplaint.Citizen.CitizenName);
                if (citizen != null)
                {
                    CurrentComplaint.Citizen.CitizenID = citizen.Citizen.CitizenID;
                    CurrentComplaint.Citizen.CitizenName = citizen.Citizen.CitizenName;
                    CurrentComplaint.Citizen.CitizenAdress = citizen.Citizen.CitizenAdress;
                    CurrentComplaint.Citizen.Occupation = citizen.Citizen.Occupation;
                    CurrentComplaint.Citizen.PhoneNumber = citizen.Citizen.PhoneNumber;
                    CurrentComplaint.Citizen.Email = citizen.Citizen.Email;
                    CurrentComplaint.Citizen.BirthDate = citizen.Citizen.BirthDate;
                    CurrentComplaint.Citizen.Category = citizen.Citizen.Category;
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
                //Clear Fields is not required since we assigning every field anyway
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
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterByDate, "[ReceiptDate]", DateToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(NameToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterLike, "[FullName]", NameToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(OZhComplaintToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[Content]", OZhComplaintToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(CommentsToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterLike, "[Comments]", CommentsToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ProsecutorToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[ProsecutorName]", ProsecutorToFilter, YearToFilter));
                }
                else if (!string.IsNullOrWhiteSpace(ChiefToFilter))
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.FilterComplaintsFun(complaintService.SqlCommandFilterEquals, "[ChiefName]", ChiefToFilter, YearToFilter));  //FilterComplaints("sp_FilterComplaintsByChief", "@chiefName", ChiefToFilter));
                }
                else
                {
                    ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaintsByYear(YearToFilter));
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

            NumberToFilter = DateToFilter = NameToFilter = OZhComplaintToFilter = CommentsToFilter = ProsecutorToFilter = ChiefToFilter = null;
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
                    Message = "Запись добавлена, чтобы отобразить изменения перезапустите программу";
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
            {
                bool isAdded = false;

                isAdded = complaintService.AddToChiefsList(AddValueToCombobox);

                if (isAdded)
                    Message = "Руководитель добавлен, чтобы отобразить изменения перезапустите программу";
                else
                    Message = "Не удалось добавить руководителя";
            }
            catch (Exception ex)
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


        private void SaveToCsv() //deprecated,been changed to EPPlus method
        {
            try
            {
                using (StreamWriter sw = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ЖРЖ ({DateTime.Now.ToString("yyyy.MM.dd")}).csv", false, Encoding.Unicode))
                {
                    sw.WriteLine($"Журнал регистрации жалоб по состаянию на {YearToFilter} год");
                    sw.WriteLine("№;Дата/время;Имя заявителя;Жалоба;Примечание;Результат;Принял(а);Принимающий руководитель");

                    for (int i = ComplaintsList.Count-1; i >= 0; i--)
                    {
                        sw.WriteLine($"{ComplaintsList[i].Enumerator};{ComplaintsList[i].ReceiptDate};{ComplaintsList[i].Citizen.CitizenName};{ComplaintsList[i].OZhComplaintText.OZhComplaint};{ComplaintsList[i].Comments};{ComplaintsList[i].Result.Rezolution};{ComplaintsList[i].Prosecutor.ProsecutorName};{ComplaintsList[i].Chief.ChiefName}");
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