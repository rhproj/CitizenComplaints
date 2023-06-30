using Complaints_WPF.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ChiefWriteService : ADOServiceBase, IChiefWriteService
    {
        public bool AddToChiefsList(string chiefName)
        {
            bool isAdded = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_AddChief";
                _sqlCommand.Parameters.AddWithValue("@chiefName", chiefName);

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

        public bool DeleteFromChiefsList(string chiefName)
        {
            bool isDeleted = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteChief";
                _sqlCommand.Parameters.AddWithValue("@chiefName", chiefName);

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
