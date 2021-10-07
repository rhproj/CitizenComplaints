using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using ComplaintsAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.Tests
{
    public class FakeAccessService : AccessService
    {
        List<AdminUser> testUsers = new List<AdminUser>()
        {
            
        }


        public void AddToUsersList(Prosecutor prosecutorToAdd)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromUsersList(Prosecutor prosecutorToDelete)
        {
            throw new NotImplementedException();
        }

        public IList<AdminUser> GetAdmins()
        {
            throw new NotImplementedException();
        }

        public IList<Prosecutor> LoadProsecutorsInfo()
        {
            throw new NotImplementedException();
        }
    }
}
