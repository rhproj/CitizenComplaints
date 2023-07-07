using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System.Collections.Generic;

namespace ComplaintsAdmin.Services
{
    public interface IAccessService
    {
        void AddToUsersList(Prosecutor prosecutorToAdd);
        void DeleteFromUsersList(Prosecutor prosecutorToDelete);
        IEnumerable<AdminUser> GetAdmins();
        IEnumerable<Prosecutor> LoadProsecutorsInfo();
    }
}
