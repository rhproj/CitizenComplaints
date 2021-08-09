using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Complaints_WPF.ViewModels
{
    class SummOZhViewModel : INotifyPropertyChanged
    {
        IComplaintService complaintService;

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

        public RelayCommand ToCsvCommand { get; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        public SummOZhViewModel()
        {
            OZhClassif = new OZhClassification();
            complaintService = new ComplaintServiceADO();

            ToCsvCommand = new RelayCommand(SaveToCsv, null);

            OZhClassificationList = new ObservableCollection<OZhClassification>(complaintService.LoadOZhWithSumm(ComplaintsViewModel.YearToFilter)); //LoadOZhClassif()); //
        }


        private void SaveToCsv()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ЖРЖ Статистика ({DateTime.Now.ToString("yyyy.MM.dd")}).csv", false, Encoding.Unicode))
                {
                    sw.WriteLine($"Регистрация жалоб, статистика на {ComplaintsViewModel.YearToFilter} год");
                    sw.WriteLine("Категория обращения;Кол-во");

                    foreach (var ozh in OZhClassificationList)
                    {
                        sw.WriteLine($"{ozh.OZhComplaint};{ozh.SummOzh}");
                    }

                    //for (int i = ComplaintsList.Count - 1; i >= 0; i--)
                    //{
                    //    sw.WriteLine($"{ComplaintsList[i].Enumerator};{ComplaintsList[i].ReceiptDate};{ComplaintsList[i].Citizen.CitizenName};{ComplaintsList[i].OZhComplaintText.OZhComplaint};{ComplaintsList[i].Comments};{ComplaintsList[i].Result.Rezolution};{ComplaintsList[i].Prosecutor.ProsecutorName};{ComplaintsList[i].Chief.ChiefName}"); //,");
                    //}
                }
                Message = "ЖРЖ Отчет сохранен на Рабочем столе";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
