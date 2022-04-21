using Complaints_WPF.Models;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IComplaintWriteService
    {
        bool AddToComplaintList(Complaint newComplaint, string prosName);
        bool AddToComplaintListUpd(Complaint newComplaint, string prosName);
        bool DeleteComplaint(int id);
        bool UpdateComplaint(Complaint complToUpdate);
    }
}
