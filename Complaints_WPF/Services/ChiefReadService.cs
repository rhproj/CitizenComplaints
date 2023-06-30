using Complaints_WPF.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ChiefReadService : ADOServiceBase, IChiefReadService
    {
        public IEnumerable<string> LoadChiefs()
        {
            var chiefsList = new List<string>();

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_LoadChief";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            chiefsList.Add(dataReader.GetString(1));
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

            return chiefsList;
        }
    }
}
