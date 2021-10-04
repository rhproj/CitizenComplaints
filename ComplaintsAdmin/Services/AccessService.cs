﻿using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System.Collections.Generic;

namespace ComplaintsAdmin.Services
{
    public abstract class AccessService
    {
        internal abstract void AddToUsersList(Prosecutor prosecutorToAdd);
        internal abstract bool Authenticate(string userName, string password);
        internal abstract void DeleteFromUsersList(Prosecutor prosecutorToDelete);
        internal abstract IList<AdminUser> GetAdmins();
        internal abstract IList<Prosecutor> LoadProsecutorsInfo();
    }
}