namespace Complaints_WPF.Services.Interfaces
{
    public interface ICategoryWriteService
    {
        bool AddToCategoryList(string categoryTitle);
        bool DeleteFromCategoryList(string categoryTitle);
    }
}
