using Complaints_WPF.Models;
using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IOZhReadService
    {
        IEnumerable<string> LoadOZhClassification();
        IEnumerable<OZhClassification> LoadOZhWithSumm(string year);
    }
}
