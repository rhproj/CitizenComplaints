using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
