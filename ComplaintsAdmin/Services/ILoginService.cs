using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.Services
{
    public interface ILoginService
    {
        bool Authenticate(string userName, string password);
    }
}
