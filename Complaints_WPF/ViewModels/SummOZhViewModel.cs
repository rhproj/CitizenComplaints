using Complaints_WPF.Commands;
using Complaints_WPF.Models;
using Complaints_WPF.Services;
using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Complaints_WPF.ViewModels
{
    class SummOZhViewModel : BaseViewModel
    {
        //IComplaintService complaintService;
        IOZhReadService _oZhReadService;

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

        public SummOZhViewModel(IOZhReadService oZhReadService)
        {
            OZhClassif = new OZhClassification();
            _oZhReadService = oZhReadService;
            //complaintService = new ComplaintServiceADO();

            ToCsvCommand = new RelayCommand(SaveToCsv, null);

            OZhClassificationList = new ObservableCollection<OZhClassification>(_oZhReadService.LoadOZhWithSumm(ComplaintsViewModel.YearToFilter)); //LoadOZhClassif()); //
        }


        private void SaveToCsv()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ЖРЖ Статистика ({DateTime.Now.ToString("yyyy.MM.dd")}).csv", false, Encoding.Unicode))
                {
                    sw.WriteLine("Категория обращения;Кол-во");

                    foreach (var ozh in OZhClassificationList)
                    {
                        sw.WriteLine($"{ozh.OZhComplaint};{ozh.SummOzh}");
                    }
                }
                Message = "ЖРЖ Отчет сохранен на Рабочем столе";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
