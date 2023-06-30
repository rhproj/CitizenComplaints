using Complaints_WPF.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ReadResultService : ADOServiceBase, IReadResultService
    {
        public IEnumerable<string> LoadResults()
        {
            var resultsList = new List<string>();

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_SelectAllResults";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            resultsList.Add(dataReader.GetString(1));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }

            return resultsList;
        }
    }
}
