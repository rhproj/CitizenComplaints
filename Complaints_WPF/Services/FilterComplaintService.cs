using Complaints_WPF.Models;
using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class FilterComplaintService : ADOServiceBase, IFilterComplaintService
    {
        public IEnumerable<Complaint> FilterComplaints(string storedProc, string sp_param, string param)
        {
            var listOfComplaints = new List<Complaint>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = storedProc;
                _sqlCommand.Parameters.AddWithValue(sp_param, param);

                _sqlConnect.Open();
                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        int count = 0;
                        while (dataReader.Read())
                        {
                            count++;
                            Complaint complaint = new Complaint();

                            complaint.Enumerator = count;
                            complaint.ComplaintID = dataReader.GetInt32(0);
                            complaint.ReceiptDate = dataReader.GetDateTime(1);
                            complaint.Citizen.CitizenName = dataReader.GetString(2);
                            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(3);
                            if (!dataReader.IsDBNull(4)) { complaint.Result.Rezolution = dataReader.GetString(4); }

                            if (!dataReader.IsDBNull(5)) { complaint.Prosecutor.ProsecutorName = dataReader.GetString(5); }

                            if (!dataReader.IsDBNull(6)) { complaint.Chief.ChiefName = dataReader.GetString(6); }

                            listOfComplaints.Add(complaint);
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
            return listOfComplaints;
        }

        #region SqlCommands
        public string SqlCommandFilterEquals(string sqlParam, string param, string year)
        {
            return $"select * from f_GetComplaintsByYear({year}) where {sqlParam} = '{param}' order by [N] desc";
        }

        public string SqlCommandFilterByDate(string sqlParam, string param, string year)
        {
            return $"select * from f_GetComplaintsByYear({year}) where SUBSTRING(Convert(varchar,{sqlParam},104),0,7)+SUBSTRING(Convert(varchar,{sqlParam},104),7,2) like '%' + '{param}' + '%' order by[N] desc";
        }

        public string SqlCommandFilterLike(string sqlParam, string param, string year)
        {
            return $"select * from f_GetComplaintsByYear({year}) where {sqlParam} like '%'+ '{param}' +'%' order by [N] desc";
        }

        public IEnumerable<Complaint> FilterComplaintsFun(Func<string, string, string, string> filterComplaintDel,
                                                        string sqlParam, string param, string year)
        {
            var listOfComplaints = new List<Complaint>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = filterComplaintDel(sqlParam, param, year);

                _sqlConnect.Open();
                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Complaint complaint = new Complaint();

                            complaint.Enumerator = dataReader.GetInt32(0);
                            complaint.ComplaintID = dataReader.GetInt32(1);
                            complaint.ReceiptDate = dataReader.GetDateTime(2);
                            complaint.Citizen.CitizenName = dataReader.GetString(3);
                            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(4);

                            if (!dataReader.IsDBNull(5)) { complaint.Comments = dataReader.GetString(5); }

                            if (!dataReader.IsDBNull(6)) { complaint.Result.Rezolution = dataReader.GetString(6); }
                            if (!dataReader.IsDBNull(7)) { complaint.Prosecutor.ProsecutorName = dataReader.GetString(7); }
                            if (!dataReader.IsDBNull(8)) { complaint.Chief.ChiefName = dataReader.GetString(8); }
                            if (!dataReader.IsDBNull(9)) { complaint.Citizen.Category = dataReader.GetString(9); }
                            listOfComplaints.Add(complaint);
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
            return listOfComplaints;
        }
        #endregion
    }
}
