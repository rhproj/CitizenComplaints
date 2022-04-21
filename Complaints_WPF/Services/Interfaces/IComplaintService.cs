using Complaints_WPF.Models;
using System;
using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IComplaintService : ICategoryReadService, ICategoryWriteService, IChiefReadService, IChiefWriteService, IComplaintReadService, IComplaintWriteService, IFilterComplaintService, IOZhReadService, IOZhWriteService, IReadCitizenService, IReadProsecutorService, IReadResultService
    {

    }
}