using Complaints_WPF.Models;
using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IOZhReadService
    {
        IList<string> LoadOZhClassification();
        IList<OZhClassification> LoadOZhWithSumm(string year);
    }
}
