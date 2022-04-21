using Complaints_WPF.Models;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IReadCitizenService
    {
        Complaint SearchCitizen(string citizenName);
    }
}
