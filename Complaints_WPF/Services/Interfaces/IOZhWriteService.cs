namespace Complaints_WPF.Services.Interfaces
{
    public interface IOZhWriteService
    {
        bool AddToOZhClassification(string ozhComplaint);
        bool DeleteFromOZhClassification(string ozhComplaint);
    }
}
