namespace Complaints_WPF.Services.Interfaces
{
    public interface IChiefWriteService
    {
        bool AddToChiefsList(string chiefName);
        bool DeleteFromChiefsList(string chiefName);
    }
}
