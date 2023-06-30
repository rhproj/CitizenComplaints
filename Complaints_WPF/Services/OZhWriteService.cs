using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Services
{
    public class OZhWriteService : ADOServiceBase, IOZhWriteService
    {
        public bool AddToOZhClassification(string ozhComplaint)
        {
            bool isAddel = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_AddOZhClasiff";
                _sqlCommand.Parameters.AddWithValue("@oZhComplaint", ozhComplaint);

                _sqlConnect.Open();
                isAddel = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
            return isAddel;
        }

        public bool DeleteFromOZhClassification(string ozhComplaint)
        {
            bool isDeleted = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteOZhClasiff";
                _sqlCommand.Parameters.AddWithValue("@oZhComplaint", ozhComplaint);

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
