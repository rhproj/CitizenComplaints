using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IReadResultService
    {
        IList<string> LoadResults();
    }
}
