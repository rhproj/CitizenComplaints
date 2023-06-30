using ComplaintsAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.ViewModels
{
    internal class ViewModelServiceSetter
    {
        internal static LoginViewModel SetUpViewModel()
        {
            var dbService = new AccessService();
            var loginService = new LoginService();

            LoginViewModel viewModel = new LoginViewModel(dbService, loginService);
            return viewModel;
        }
    }
}
