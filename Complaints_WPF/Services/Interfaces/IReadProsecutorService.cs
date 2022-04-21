using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IReadProsecutorService
    {
        IList<string> LoadProsecutors();
    }
}
