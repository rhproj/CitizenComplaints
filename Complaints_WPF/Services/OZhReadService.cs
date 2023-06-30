using Complaints_WPF.Models;
using Complaints_WPF.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class OZhReadService : ADOServiceBase, IOZhReadService
    {
        public IEnumerable<OZhClassification> LoadOZhWithSumm(string year)
        {
            var OZhClassificationList = new List<OZhClassification>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = $"select * from f_OZhComplaintSummByYear({year})";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            OZhClassification oZh = new OZhClassification();
                            oZh.OZhComplaint = dataReader.GetString(0);
                            oZh.SummOzh = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);

                            OZhClassificationList.Add(oZh);
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
            return OZhClassificationList;
        }

        public IEnumerable<string> LoadOZhClassification() //first to run
        {
            var OZhClassificationList = new List<string>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_LoadOZhClassification";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            OZhClassificationList.Add(dataReader.GetString(1));
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

            return OZhClassificationList;
        }
    }
}
