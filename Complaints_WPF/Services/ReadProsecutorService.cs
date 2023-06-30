using Complaints_WPF.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ReadProsecutorService : ADOServiceBase, IReadProsecutorService
    {
        public IEnumerable<string> LoadProsecutors()
        {
            var prosecutorsList = new List<string>();

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_LoadProsecutors";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            prosecutorsList.Add(dataReader.GetString(2));
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

            return prosecutorsList;
        }
    }
}
