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
        SqlConnection SqlConnect;
        SqlCommand SqlCommand;

        public ComplaintServiceADO()
        {
            SqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_ComplaintsADO"].ConnectionString);
            SqlCommand = new SqlCommand();
            SqlCommand.Connection = SqlConnect;
            //SqlCommand.CommandType = CommandType.StoredProcedure;
        }


        public IList<Complaint> GetAllComplaints()
        {
            List<Complaint> listOfComplaints = new List<Complaint>();  //list that will be fed by StoredP
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_SelectAllComplaints";

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return listOfComplaints;
        }
        /// <summary>
        /// Initial Complaints load 
        /// </summary>
        /// <param name="year">current year by default</param>
        public IList<Complaint> GetAllComplaintsByYear(string year)
        {
            List<Complaint> listOfComplaints = new List<Complaint>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.CommandText = $"select * from f_GetComplaintsByYear ({year}) order by [N] desc";

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return listOfComplaints;
        }

        public IList<Complaint> FilterComplaints(string storedProc, string sp_param, string param)
        {
            List<Complaint> listOfComplaints = new List<Complaint>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = storedProc;
                SqlCommand.Parameters.AddWithValue(sp_param, param);

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return listOfComplaints;
        }   //depricated

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

        public IList<Complaint> FilterComplaintsFun(Func<string, string, string, string> filterComplaintDel, 
                                                        string sqlParam, string param, string year)
        {
            List<Complaint> listOfComplaints = new List<Complaint>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.CommandText = filterComplaintDel(sqlParam, param, year);

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return listOfComplaints;
        }
        #endregion

        #region Load/Populate Lists
        public IList<OZhClassification> LoadOZhWithSumm(string year)
        {
            List<OZhClassification> OZhClassificationList = new List<OZhClassification>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.CommandText = $"select * from f_OZhComplaintSummByYear({year})";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return OZhClassificationList;
        }

        public IList<string> LoadOZhClassification() //first to run
        {
            List<string> OZhClassificationList = new List<string>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_LoadOZhClassification";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }

            return OZhClassificationList;
        }

        public IList<string> LoadProsecutors()
        {
            List<string> ProsecutorsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_LoadProsecutors";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ProsecutorsList.Add(dataReader.GetString(2));
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
                SqlConnect.Close();
            }

            return ProsecutorsList;
        }

        public IList<string> LoadChiefs()
        {
            List<string> ChiefsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_LoadChief";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ChiefsList.Add(dataReader.GetString(1));
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
                SqlConnect.Close();
            }

            return ChiefsList;
        }

        public IList<string> LoadResults()
        {
            List<string> resultsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_SelectAllResults";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }

            return resultsList;
        }

        public IList<string> LoadCategories()
        {
            List<string> CategoryList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_LoadCategories";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            CategoryList.Add(dataReader.GetString(1));
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
                SqlConnect.Close();
            }

            return CategoryList;
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
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_InsertComplaint";

                SqlCommand.Parameters.AddWithValue("@fullName", newComplaint.Citizen.CitizenName);
                SqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                SqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                SqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                SqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                SqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                SqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                SqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

                SqlConnect.Open();
                isAdded = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
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
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_InsertComplaintUpd";

                SqlCommand.Parameters.AddWithValue("@personId", newComplaint.Citizen.CitizenID);
                SqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                
                SqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                SqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                SqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                SqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                SqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                SqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

                SqlConnect.Open();
                isAdded = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }

            return isAdded;
        }

        public bool UpdateComplaint(Complaint complToUpdate)
        {
            bool isUpdated = false;
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_UpdateComplaint";

                SqlCommand.Parameters.AddWithValue("@personId", complToUpdate.Citizen.CitizenID);
                SqlCommand.Parameters.AddWithValue("@fullName", complToUpdate.Citizen.CitizenName);
                SqlCommand.Parameters.AddWithValue("@birthDate", complToUpdate.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", complToUpdate.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", complToUpdate.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", complToUpdate.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", complToUpdate.Citizen.Email);

                SqlCommand.Parameters.AddWithValue("@dateTime", complToUpdate.ReceiptDate);
                SqlCommand.Parameters.AddWithValue("@content", complToUpdate.OZhComplaintText.OZhComplaint);
                SqlCommand.Parameters.AddWithValue("@pageNum", complToUpdate.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", complToUpdate.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", complToUpdate.Comments);
                SqlCommand.Parameters.AddWithValue("@result", complToUpdate.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@chiefName", complToUpdate.Chief.ChiefName);
                SqlCommand.Parameters.AddWithValue("@digitalStorage", complToUpdate.DigitalStorage);
                SqlCommand.Parameters.AddWithValue("@category", complToUpdate.Citizen.Category);

                SqlConnect.Open();
                isUpdated = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isUpdated; //if ... above > 0 - true
        }
        #endregion

        public Complaint SelectComplaint(string year, string citizenName, DateTime dateTime)
        {
            Complaint selectedComplaint = null;
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_EditComplaintFromFunction";
                SqlCommand.Parameters.AddWithValue("@year", year);
                SqlCommand.Parameters.AddWithValue("@fullName", citizenName);
                SqlCommand.Parameters.AddWithValue("@dateTime", dateTime);

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return selectedComplaint;
        }

        public Complaint SelectComplaintFun(string year, string citizenName, DateTime dateTime)
        {
            Complaint selectedComplaint = null;
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = $"select * from f_PickAComplaint({year}) where [FullName] = '{citizenName}' and [ReceiptDate] = '{dateTime}'";

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return selectedComplaint;
        }

        public Complaint SearchCitizen(string citizenName)
        {
            Complaint citizen = null;
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_SelectPersonByNameLike";   //"sp_SelectPersonByName";
                SqlCommand.Parameters.AddWithValue("@fullName", citizenName);

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
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
                SqlConnect.Close();
            }
            return citizen;
        }

        public bool DeleteComplaint(int id)
        {
            bool isDeleted = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_DeleteComplaintByID";
                SqlCommand.Parameters.AddWithValue("@complaintID", id);

                SqlConnect.Open();
                //int rowAffected = SqlCommand.ExecuteNonQuery();
                isDeleted = SqlCommand.ExecuteNonQuery() > 0; //if nothing was deleted = 0
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isDeleted;
        }

        #region Editing Comboboxes
        public bool AddToOZhClassification(string ozhComplaint)
        {
            bool isAddel = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_AddOZhClasiff";
                SqlCommand.Parameters.AddWithValue("@oZhComplaint", ozhComplaint);

                SqlConnect.Open();
                isAddel = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isAddel;
        }

        public bool DeleteFromOZhClassification(string ozhComplaint)
        {
            bool isDeleted = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_DeleteOZhClasiff";
                SqlCommand.Parameters.AddWithValue("@oZhComplaint", ozhComplaint);

                SqlConnect.Open();
                isDeleted = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isDeleted;
        }

        public bool AddToChiefsList(string chiefName)
        {
            bool isAdded = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_AddChief";
                SqlCommand.Parameters.AddWithValue("@chiefName", chiefName);

                SqlConnect.Open();
                isAdded = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isAdded;
        }

        public bool DeleteFromChiefsList(string chiefName)
        {
            bool isDeleted = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_DeleteChief";
                SqlCommand.Parameters.AddWithValue("@chiefName", chiefName);

                SqlConnect.Open();
                isDeleted = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }

            return isDeleted;
        }

        public bool AddToCategoryList(string categoryTitle)
        {
            bool isAdded = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_AddCategory";
                SqlCommand.Parameters.AddWithValue("@categoryTitle", categoryTitle);

                SqlConnect.Open();
                isAdded = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            return isAdded;
        }
        public bool DeleteFromCategoryList(string categoryTitle)
        {
            bool isDeleted = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_DeleteCategory";
                SqlCommand.Parameters.AddWithValue("@categoryTitle", categoryTitle);

                SqlConnect.Open();
                isDeleted = SqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }

            return isDeleted;
        }
        #endregion
    }
}
