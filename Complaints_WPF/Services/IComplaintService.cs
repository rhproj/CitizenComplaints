using Complaints_WPF.Models;
using System;
using System.Collections.Generic;

namespace Complaints_WPF.Services
{
    public interface IComplaintService
    {
        bool AddToChiefsList(string chiefName);
        bool AddToComplaintList(Complaint newComplaint, string prosName);
        bool AddToComplaintListUpd(Complaint newComplaint, string prosName);
        bool AddToOZhClassification(string ozhComplaint);
        bool DeleteComplaint(int id);
        bool DeleteFromChiefsList(string chiefName);
        bool DeleteFromOZhClassification(string ozhComplaint);
        IList<Complaint> FilterComplaints(string storedProc, string sp_param, string param);
        IList<Complaint> FilterComplaintsFun(Func<string, string, string, string> filterComplaintDel, string sqlParam, string param, string year);
        IList<Complaint> GetAllComplaints();
        IList<Complaint> GetAllComplaintsByYear(string year);
        IList<string> LoadChiefs();
        IList<string> LoadOZhClassification();
        IList<OZhClassification> LoadOZhWithSumm(string year);
        IList<string> LoadProsecutors();
        IList<string> LoadResults();
        Complaint SearchCitizen(string citizenName);
        Complaint SelectComplaint(string year, string citizenName, DateTime dateTime);
        Complaint SelectComplaintFun(string year, string citizenName, DateTime dateTime);
        string SqlCommandFilterByDate(string sqlParam, string param, string year);
        string SqlCommandFilterEquals(string sqlParam, string param, string year);
        string SqlCommandFilterLike(string sqlParam, string param, string year);
        bool UpdateComplaint(Complaint complToUpdate);
    }
}