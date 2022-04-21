using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface ICategoryReadService
    {
        IList<string> LoadCategories();
    }
}
