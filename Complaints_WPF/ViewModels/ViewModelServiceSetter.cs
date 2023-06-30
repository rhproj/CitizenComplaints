using Complaints_WPF.Services;
using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.ViewModels
{
    internal class ViewModelServiceSetter
    {
        internal static ComplaintsViewModel SetUpViewModel()
        {
            ICategoryReadService categoryReadService = new CategoryReadService();
            ICategoryWriteService categoryWriteService = new CategoryWriteService();
            IChiefReadService chiefReadService = new ChiefReadService();
            IChiefWriteService chiefWriteService = new ChiefWriteService();
            IComplaintReadService complaintReadService = new ComplaintReadService();
            IComplaintWriteService complaintWriteService = new ComplaintWriteService();
            IFilterComplaintService filterComplaintService = new FilterComplaintService(); 
            IOZhReadService oZhReadService = new OZhReadService();
            IOZhWriteService oZhWriteService = new OZhWriteService();
            IReadCitizenService readCitizenService = new ReadCitizenService();
            IReadProsecutorService readProsecutorService = new ReadProsecutorService();
            IReadResultService readResultService = new ReadResultService();


            var viewModel = new ComplaintsViewModel(
                                    categoryReadService,
                                    categoryWriteService,
                                    chiefReadService,
                                    chiefWriteService,
                                    complaintReadService,
                                    complaintWriteService,
                                    filterComplaintService,
                                    oZhReadService,
                                    oZhWriteService,
                                    readCitizenService,
                                    readProsecutorService,
                                    readResultService);
            return viewModel;
        }
    }
}
