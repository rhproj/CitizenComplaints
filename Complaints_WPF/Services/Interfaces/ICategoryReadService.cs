using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface ICategoryReadService
    {
        IEnumerable<string> LoadCategories();
    }
}
