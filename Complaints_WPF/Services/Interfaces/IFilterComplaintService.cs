using Complaints_WPF.Models;
using System;
using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IFilterComplaintService
    {
        IEnumerable<Complaint> FilterComplaints(string storedProc, string sp_param, string param);
        IEnumerable<Complaint> FilterComplaintsFun(Func<string, string, string, string> filterComplaintDel, string sqlParam, string param, string year);
        string SqlCommandFilterByDate(string sqlParam, string param, string year);
        string SqlCommandFilterEquals(string sqlParam, string param, string year);
        string SqlCommandFilterLike(string sqlParam, string param, string year);
    }
}
