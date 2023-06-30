using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Models
{
    public class ComplaintServiceADO : IComplaintService
    {
        private SqlConnection _sqlConnect;
        private SqlCommand _sqlCommand;
        private const string connectionName = "cs_ComplaintsADO";

        public ComplaintServiceADO()
        {
            _sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnect;
        }

        /// <summary>
        /// Initial Complaints load 
        /// </summary>
        /// <param name="year">current year by default</param>
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

        #region Filtering
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

        #region Load/Populate Lists
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
                            //Result result = new Result();
                            //result.Rezolution = dataReader.GetString(1);
                            //resultsList.Add(result.Rezolution);
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

        public IEnumerable<string> LoadCategories() //
        {
            var categoryList = new List<string>();

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_LoadCategories";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            categoryList.Add(dataReader.GetString(1));
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

            return categoryList;
        }
        #endregion

        #region New Complaint:
        public bool AddToComplaintList(Complaint newComplaint, string prosName)
        {
            bool isAdded = false;
            if (string.IsNullOrWhiteSpace(newComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(newComplaint.OZhComplaintText.OZhComplaint))
                throw new ArgumentException("Поля имя и текст жалобы болжны быть заполнены");

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_InsertComplaint";

                _sqlCommand.Parameters.AddWithValue("@fullName", newComplaint.Citizen.CitizenName);
                _sqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                _sqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                _sqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

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

        public bool AddToComplaintListUpd(Complaint newComplaint, string prosName)
        {
            bool isAdded = false;
            if (string.IsNullOrWhiteSpace(newComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(newComplaint.OZhComplaintText.OZhComplaint))
                throw new ArgumentException("Поля имя и текст жалобы болжны быть заполнены");

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_InsertComplaintUpd";

                _sqlCommand.Parameters.AddWithValue("@personId", newComplaint.Citizen.CitizenID);
                _sqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                
                _sqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                _sqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

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

        public bool UpdateComplaint(Complaint complToUpdate)
        {
            bool isUpdated = false;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_UpdateComplaint";

                _sqlCommand.Parameters.AddWithValue("@personId", complToUpdate.Citizen.CitizenID);
                _sqlCommand.Parameters.AddWithValue("@fullName", complToUpdate.Citizen.CitizenName);
                _sqlCommand.Parameters.AddWithValue("@birthDate", complToUpdate.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", complToUpdate.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", complToUpdate.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", complToUpdate.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", complToUpdate.Citizen.Email);

                _sqlCommand.Parameters.AddWithValue("@dateTime", complToUpdate.ReceiptDate);
                _sqlCommand.Parameters.AddWithValue("@content", complToUpdate.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", complToUpdate.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", complToUpdate.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", complToUpdate.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", complToUpdate.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@chiefName", complToUpdate.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", complToUpdate.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", complToUpdate.Citizen.Category);

                _sqlConnect.Open();
                isUpdated = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
            return isUpdated; //if ... above > 0 - true
        }
        #endregion

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
                        selectedComplaint.Citizen.Category = dataReader.IsDBNull(17)? null : dataReader.GetString(17);

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

        public Complaint SearchCitizen(string citizenName)
        {
            Complaint citizen = null;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_SelectPersonByNameLike";   //"sp_SelectPersonByName";
                _sqlCommand.Parameters.AddWithValue("@fullName", citizenName);

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        citizen = new Complaint(); //exrtacting only citizen related things
                        citizen.Citizen.CitizenID = dataReader.GetInt32(0);
                        //gotta do null check first on these, or nothing happens:
                        citizen.Citizen.CitizenName = dataReader.GetString(1);  //added 21
                        citizen.Citizen.CitizenAdress = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                        citizen.Citizen.Occupation = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                        citizen.Citizen.PhoneNumber = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                        citizen.Citizen.Email = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                        citizen.Citizen.BirthDate = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);
                        citizen.Citizen.Category = dataReader.IsDBNull(7) ? null : dataReader.GetString(7);
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
            return citizen;
        }

        public bool DeleteComplaint(int id)
        {
            bool isDeleted = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteComplaintByID";
                _sqlCommand.Parameters.AddWithValue("@complaintID", id);

                _sqlConnect.Open();
                //int rowAffected = SqlCommand.ExecuteNonQuery();
                isDeleted = _sqlCommand.ExecuteNonQuery() > 0; //if nothing was deleted = 0
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

        #region Editing Comboboxes
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
        #endregion
    }
}
