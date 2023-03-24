﻿using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using Complaints_WPF.Services;
using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;

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

        private ICollectionView _complaintsListView;
        public ICollectionView ComplaintsListView
        {
            get { return _complaintsListView; }
            set { _complaintsListView = value; OnPropertyChanged(nameof(ComplaintsListView)); }
        }

        private string _searchWord;
        public string SearchWord
        {
            get { return _searchWord; }
            set { _searchWord = value; ComplaintsListView.Refresh(); }
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


        private string _addValueToCombobox;  // Combobox
        public string AddValueToCombobox
        {
            get { return _addValueToCombobox; }
            set { _addValueToCombobox = value; OnPropertyChanged("AddValueToCombobox"); }
        }


        #region AUTHORIZATION
        public static string YearToFilter { get; set; }
        private string _prosecutorLogin;
        public string ProsecutorLogin 
        {
            get { return _prosecutorLogin; }
            set { _prosecutorLogin = value; OnPropertyChanged("ProsecutorLogin"); }
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
            var window = new MainWindow(this);
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
        public RelayCommand UnFilterCommand { get; }

        #region ComboConstruct
        public RelayCommand AddOzhCommand { get; }
        public RelayCommand AddChiefCommand { get; }
        public RelayCommand DeleteChiefCommand { get; }
        public RelayCommand AddCategoryCommand { get; }
        public RelayCommand DeleteCategoryCommand { get; }
        //public RelayCommand DeleteOzhCommand { get; }
        #endregion
        #endregion
        #endregion

        public ComplaintsViewModel(IComplaintService dbService)
        {
            //TestServerAccess();
            YearToFilter = DateTime.Now.Year.ToString();

            complaintService = dbService;
            CurrentComplaint = new Complaint(); 

            EnterCommand = new RelayCommand(Enter, Enter_CanExecute);

            NewEntryCommand = new RelayCommand(NewEntry, null);
            RegisterCommand = new RelayCommand(RegisterComplaint, RegisterComplaint_CanExecute); 

            FindCitizenCommand = new RelayCommand(FindCitizen, null);

            EditCommand = new RelayCommand(EditComplaint, null);
            DeleteComplaintCommand = new RelayCommand(DeleteComplaint, DeleteComplaint_CanExecute);


            AddOzhCommand = new RelayCommand(AddToOzhCombobox, AddToCombobox_CanExecute); //ComboEditView
            AddChiefCommand = new RelayCommand(AddToChiefsCombobox, AddToCombobox_CanExecute);
            DeleteChiefCommand = new RelayCommand(DeleteChief, DeleteChief_CanExecute);
            AddCategoryCommand = new RelayCommand(AddCategory, AddToCombobox_CanExecute);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, DeleteCategory_CanExecute);
            //DeleteOzhCommand = new RelayCommand(DeleteOzh, DeleteOzh_CanExecute);

            OZhClassificationList = new ObservableCollection<string>(complaintService.LoadOZhClassification());
            ResultsList = new ObservableCollection<string>(complaintService.LoadResults());           
            ProsecutorsList = new ObservableCollection<string>(complaintService.LoadProsecutors()); //for login window only
            ChiefsList = new ObservableCollection<string>(complaintService.LoadChiefs());
            CategoryList = new ObservableCollection<string>(complaintService.LoadCategories());

            LoadData(YearToFilter);

            SaveSpreadSheetsCommand = new RelayCommand(SaveSpreadSheets, null);
        }

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
            ComplaintsList = new ObservableCollection<Complaint>(complaintService.GetAllComplaintsByYear(year));
            CurrentNum = ComplaintsList.Count;
            ComplaintsListView = CollectionViewSource.GetDefaultView(ComplaintsList);
            ComplaintsListView.Filter += Filter;
        }

        private bool Filter(object obj)
        {
            var complaint = obj as Complaint;
            if (!string.IsNullOrEmpty(SearchWord))
            {
                return complaint.Enumerator.ToString().Contains(SearchWord) ||
                    complaint.ReceiptDate.ToString().Contains(SearchWord) ||
                    complaint.Citizen.CitizenName.ToLower().Contains(SearchWord.ToLower()) ||
                    complaint.OZhComplaintText.OZhComplaint.ToLower().Contains(SearchWord.ToLower()) ||

                    !string.IsNullOrEmpty(complaint.Comments) && complaint.Comments.ToLower().Contains(SearchWord.ToLower()) ||
                    complaint.Prosecutor.ProsecutorName.ToLower().Contains(SearchWord.ToLower()) ||

                    !string.IsNullOrEmpty(complaint.Result.Rezolution) && complaint.Result.Rezolution.ToLower().Contains(SearchWord.ToLower()) ||
                    !string.IsNullOrEmpty(complaint.Chief.ChiefName) && complaint.Chief.ChiefName.ToLower().Contains(SearchWord.ToLower());
            }
            else
            {
                return true;
            }
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
                CurrentComplaint = complaintService.SelectComplaint(YearToFilter, SelectedComplaint.Citizen.CitizenName, SelectedComplaint.ReceiptDate);
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
                    bool isDeleted = complaintService.DeleteComplaint(CurrentComplaint.ComplaintID);
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
            AddValueToCombobox = CurrentComplaint.Chief.ChiefName = CurrentComplaint.OZhComplaintText.OZhComplaint = CurrentComplaint.Citizen.Category = null;
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


        #region Category Methods
        private void AddCategory()
        {
            try
            {
                bool isAdded = false;

                isAdded = complaintService.AddToCategoryList(AddValueToCombobox);

                if (isAdded)
                    Message = "Категория добавлена, чтобы отобразить изменения перезапустите программу";
                else
                    Message = "Не удалось добавить категорию";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            ClearValueToAdd();
        }

        private void DeleteCategory()
        {
            try
            {
                bool isDeleted = complaintService.DeleteFromCategoryList(CurrentComplaint.Citizen.Category);

                if (isDeleted)
                {
                    Message = "Категория удалена, чтобы отобразить изменения перезапустите программу";
                }
                else
                {
                    Message = "Не удалось удалить категорию";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            ClearValueToAdd();
        }

        private bool DeleteCategory_CanExecute()
        {
            if (string.IsNullOrEmpty(CurrentComplaint.Citizen.Category))
            {
                return false;
            }
            else
            {
                return true;
            }
        } 
        #endregion
    }
}

