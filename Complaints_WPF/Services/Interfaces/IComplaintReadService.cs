using Complaints_WPF.Models;
using System;
using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IComplaintReadService
    {
        IList<Complaint> GetAllComplaints();
        IList<Complaint> GetAllComplaintsByYear(string year);
        Complaint SelectComplaint(string year, string citizenName, DateTime dateTime);
        Complaint SelectComplaintFun(string year, string citizenName, DateTime dateTime);
    }
}
