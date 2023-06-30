using Complaints_WPF.Models;
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
    public class ComplaintReadService : ADOServiceBase, IComplaintReadService
    {
        public IEnumerable<Complaint> GetAllComplaintsByYear(string year)
        {
            var listOfComplaints = new List<Complaint>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = $"select * from f_GetComplaintsByYear ({year}) order by [N] desc";

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

        public Complaint SelectComplaint(string year, string citizenName, DateTime dateTime)
        {
            Complaint selectedComplaint = null;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_EditComplaintFromFunction";
                _sqlCommand.Parameters.AddWithValue("@year", year);
                _sqlCommand.Parameters.AddWithValue("@fullName", citizenName);
                _sqlCommand.Parameters.AddWithValue("@dateTime", dateTime);

                _sqlConnect.Open();
                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        selectedComplaint = new Complaint();

                        selectedComplaint.Citizen.CitizenID = dataReader.GetInt32(0);
                        selectedComplaint.Citizen.CitizenName = dataReader.GetString(1);
                        selectedComplaint.Citizen.CitizenAdress = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                        selectedComplaint.Citizen.Occupation = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                        selectedComplaint.Citizen.PhoneNumber = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                        selectedComplaint.Citizen.Email = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                        selectedComplaint.Citizen.BirthDate = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);

                        selectedComplaint.ComplaintID = dataReader.GetInt32(7);
                        selectedComplaint.ReceiptDate = dataReader.GetDateTime(8);
                        selectedComplaint.OZhComplaintText.OZhComplaint = dataReader.GetString(9);
                        selectedComplaint.PageNum = dataReader.IsDBNull(10) ? null : dataReader.GetString(10);
                        selectedComplaint.AppendNum = dataReader.IsDBNull(11) ? null : dataReader.GetString(11);
                        selectedComplaint.Comments = dataReader.IsDBNull(12) ? null : dataReader.GetString(12);
                        selectedComplaint.Result.Rezolution = dataReader.IsDBNull(13) ? null : dataReader.GetString(13);
                        selectedComplaint.Chief.ChiefName = dataReader.IsDBNull(14) ? null : dataReader.GetString(14);
                        selectedComplaint.DigitalStorage = dataReader.IsDBNull(15) ? null : dataReader.GetString(15);
                        selectedComplaint.Enumerator = dataReader.GetInt32(16);
                        selectedComplaint.Citizen.Category = dataReader.IsDBNull(17) ? null : dataReader.GetString(17);

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
            return selectedComplaint;
        }

        public Complaint SelectComplaintFun(string year, string citizenName, DateTime dateTime)
        {
            Complaint selectedComplaint = null;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandText = $"select * from f_PickAComplaint({year}) where [FullName] = '{citizenName}' and [ReceiptDate] = '{dateTime}'";

                _sqlConnect.Open();
                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        selectedComplaint = new Complaint();

                        selectedComplaint.Citizen.CitizenID = dataReader.GetInt32(0);
                        selectedComplaint.Citizen.CitizenName = dataReader.GetString(1);
                        selectedComplaint.Citizen.CitizenAdress = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                        selectedComplaint.Citizen.Occupation = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                        selectedComplaint.Citizen.PhoneNumber = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                        selectedComplaint.Citizen.Email = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                        selectedComplaint.Citizen.BirthDate = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);

                        selectedComplaint.ComplaintID = dataReader.GetInt32(7);
                        selectedComplaint.ReceiptDate = dataReader.GetDateTime(8);
                        selectedComplaint.OZhComplaintText.OZhComplaint = dataReader.GetString(9);
                        selectedComplaint.PageNum = dataReader.IsDBNull(10) ? null : dataReader.GetString(10);
                        selectedComplaint.AppendNum = dataReader.IsDBNull(11) ? null : dataReader.GetString(11);
                        selectedComplaint.Comments = dataReader.IsDBNull(12) ? null : dataReader.GetString(12);
                        selectedComplaint.Result.Rezolution = dataReader.IsDBNull(13) ? null : dataReader.GetString(13);
                        selectedComplaint.Chief.ChiefName = dataReader.IsDBNull(14) ? null : dataReader.GetString(14);
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
            return selectedComplaint;
        }
    }
}
