using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IReadResultService
    {
        IEnumerable<string> LoadResults();
    }
}
