using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IChiefWriteService
    {
        bool AddToChiefsList(string chiefName);
        bool DeleteFromChiefsList(string chiefName);
    }
}
