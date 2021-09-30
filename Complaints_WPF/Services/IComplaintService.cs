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
        List<Complaint> FilterComplaints(string storedProc, string sp_param, string param);
        List<Complaint> FilterComplaintsFun(ComplaintServiceADO.FilterComplaintDel filterComplaintDel, string sqlParam, string param, string year);
        List<Complaint> GetAllComplaints();
        List<Complaint> GetAllComplaintsByYear(string year);
        List<string> LoadChiefs();
        List<string> LoadOZhClassification();
        List<OZhClassification> LoadOZhWithSumm(string year);
        List<string> LoadProsecutors();
        List<string> LoadResults();
        Complaint SearchCitizen(string citizenName);
        Complaint SelectComplaint(string year, string citizenName, DateTime dateTime);
        Complaint SelectComplaintFun(string year, string citizenName, DateTime dateTime);
        string SqlCommandFilterByDate(string sqlParam, string param, string year);
        string SqlCommandFilterEquals(string sqlParam, string param, string year);
        string SqlCommandFilterLike(string sqlParam, string param, string year);
        bool UpdateComplaint(Complaint complToUpdate);
    }
}