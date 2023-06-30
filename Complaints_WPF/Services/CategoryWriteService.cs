using Complaints_WPF.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class CategoryWriteService : ADOServiceBase, ICategoryWriteService
    {
        public bool AddToCategoryList(string categoryTitle)
        {
            bool isAdded = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_AddCategory";
                _sqlCommand.Parameters.AddWithValue("@categoryTitle", categoryTitle);

                _sqlConnect.Open();
                isAdded = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
            return isAdded;
        }
        public bool DeleteFromCategoryList(string categoryTitle)
        {
            bool isDeleted = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteCategory";
                _sqlCommand.Parameters.AddWithValue("@categoryTitle", categoryTitle);

                _sqlConnect.Open();
                isDeleted = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }

            return isDeleted;
        }
    }
}
