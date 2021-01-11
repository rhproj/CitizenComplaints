using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complaints_WPF.Models
{
    public class ComplaintServiceADO
    {
        SqlConnection SqlConnect;
        SqlCommand SqlCommand;

        #region CTOR
        public ComplaintServiceADO()
        {
            SqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_ComplaintsADO"].ConnectionString);
            SqlCommand = new SqlCommand();
            SqlCommand.Connection = SqlConnect;
            SqlCommand.CommandType = CommandType.StoredProcedure;
        }
        #endregion

        #region METH
        public List<Complaint> GetAllComplaints()  //old one, uses SP
        {
            List<Complaint> listOfComplaints = new List<Complaint>();  //list that will be fed by StoredP
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_SelectAllComplaints"; //used to be sp_SelectAllComplaints01, without prosecs

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        int count = 0;

                        while (dataReader.Read()) //what happens w/o while? it simply won't work
                        {
                            count++;                            
                            Complaint complaint = new Complaint();

                            complaint.Enumerator = count;
                            complaint.ComplaintID = dataReader.GetInt32(0);
                            complaint.ReceiptDate = dataReader.GetDateTime(1);
                            complaint.Citizen.CitizenName = dataReader.GetString(2);
                            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(3);    //b4://complaint.ComplaintText = dataReader.GetString(3);
                            if (!dataReader.IsDBNull(4)) { complaint.Result.Rezolution = dataReader.GetString(4); }
                            //Complaint.Result = dataReader.IsDBNull(4)? null : dataReader.GetString(4);

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

        public List<Complaint> GetAllComplaintsByYear()  //using view
        {
            List<Complaint> listOfComplaints = new List<Complaint>();  //list that will be fed by StoredP
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.CommandText = "select * from v_GetComplaintsByYear order by [N] desc"; //used to be sp_SelectAllComplaints01, without prosecs

                SqlConnect.Open();
                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        //int count = 0;

                        while (dataReader.Read()) //what happens w/o while? it simply won't work
                        {
                            //count++;
                            Complaint complaint = new Complaint();

                            //complaint.Enumerator = count;
                            complaint.Enumerator = dataReader.GetInt32(0);    //added new!
                            complaint.ComplaintID = dataReader.GetInt32(1);
                            complaint.ReceiptDate = dataReader.GetDateTime(2);
                            complaint.Citizen.CitizenName = dataReader.GetString(3);
                            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(4);    //b4://complaint.ComplaintText = dataReader.GetString(3);
                            if (!dataReader.IsDBNull(5)) { complaint.Result.Rezolution = dataReader.GetString(5); }
                            //Complaint.Result = dataReader.IsDBNull(4)? null : dataReader.GetString(4);
                            if (!dataReader.IsDBNull(6)) { complaint.Prosecutor.ProsecutorName = dataReader.GetString(6); }
                            if (!dataReader.IsDBNull(7)) { complaint.Chief.ChiefName = dataReader.GetString(7); }
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

        public List<Complaint> FilterComplaints(string storedProc, string sp_param, string param) // string receiptDate, string name, string content
        {
            List<Complaint> listOfComplaints = new List<Complaint>();
            try
            {
                SqlCommand.Parameters.Clear();
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
                            //Complaint Complaint = new Complaint(dataReader.GetInt32(0), dataReader.GetDateTime(1), dataReader.GetString(2), dataReader.GetString(3));
                            Complaint complaint = new Complaint();

                            complaint.Enumerator = count;
                            complaint.ComplaintID = dataReader.GetInt32(0);
                            complaint.ReceiptDate = dataReader.GetDateTime(1);
                            complaint.Citizen.CitizenName = dataReader.GetString(2);
                            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(3);    //b4://complaint.ComplaintText = dataReader.GetString(3);
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

        public List<string> LoadOZhClassification()
        {
            List<string> OZhClassificationList = new List<string>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_LoadOZhClassification";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            OZhClassification oZh = new OZhClassification();
                            oZh.OZhComplaint = dataReader.GetString(1);
                            OZhClassificationList.Add(oZh.OZhComplaint);
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

        public List<string> LoadProsecutors()
        {
            List<string> ProsecutorsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_LoadProsecutors";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Prosecutor prosecutor = new Prosecutor();
                            prosecutor.ProsecutorName = dataReader.GetString(2);
                            ProsecutorsList.Add(prosecutor.ProsecutorName);
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

        public List<string> LoadChiefs() //29.12
        {
            List<string> ChiefsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_LoadChief";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Chief chief = new Chief();
                            chief.ChiefName = dataReader.GetString(1);
                            ChiefsList.Add(chief.ChiefName);
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

        public List<string> LoadResults()
        {
            List<string> resultsList = new List<string>();

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_SelectAllResults";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            Result result = new Result();
                            result.Rezolution = dataReader.GetString(1);
                            resultsList.Add(result.Rezolution);
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



        #region New Complaint:
        public bool AddToComplaintList(Complaint newComplaint, string prosName)
        {
            bool isAdded = false;
            if (string.IsNullOrWhiteSpace(newComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(newComplaint.OZhComplaintText.OZhComplaint))       //b4: //string.IsNullOrWhiteSpace(newComplaint.ComplaintText))
                throw new ArgumentException("Поля имя и текст жалобы болжны быть заполнены");

            #region works well with sp_2
            //try
            //{
            //    SqlCommand.Parameters.Clear();
            //    SqlCommand.CommandText = "sp_InsertComplaint2"; //think of the way to create if/switch with another SP_ where Citizen already exists

            //    SqlCommand.Parameters.AddWithValue("@fullName", newComplaint.CitizenName);
            //    SqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.CitizenAdress);
            //    SqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.PhoneNumber);
            //    SqlCommand.Parameters.AddWithValue("@content", newComplaint.ComplaintText);
            //    SqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);

            //    SqlConnect.Open(); //is this where the actuall StoredP called?

            //    isAdded = SqlCommand.ExecuteNonQuery() > 0; //true or false
            //} 
            #endregion

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_InsertComplaint"; //think of the way to create if/switch with another SP_ where Citizen already exists

                SqlCommand.Parameters.AddWithValue("@fullName", newComplaint.Citizen.CitizenName);
                SqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                SqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);  //b4: //newComplaint.ComplaintText);
                SqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                SqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@prosecutorName", prosName); // newComplaint.Prosecutor.ProsecutorName);
                SqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName); //29.12

                SqlConnect.Open();
                isAdded = SqlCommand.ExecuteNonQuery() > 0; //true or false
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

        //internal bool AddToComplaintList(Complaint currentComplaint, object prosecutorsLogin)
        //{
        //    throw new NotImplementedException();
        //}

        public bool AddToComplaintListUpd(Complaint newComplaint, string prosName)
        {
            bool isAdded = false;
            if (string.IsNullOrWhiteSpace(newComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(newComplaint.OZhComplaintText.OZhComplaint))  //b4: //string.IsNullOrWhiteSpace(newComplaint.ComplaintText))
                throw new ArgumentException("Поля имя и текст жалобы болжны быть заполнены");

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_InsertComplaintUpd";

                SqlCommand.Parameters.AddWithValue("@personId", newComplaint.Citizen.CitizenID);
                SqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                SqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);   //b4: // newComplaint.ComplaintText);
                SqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                SqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@prosecutorName", prosName); // newComplaint.Prosecutor.ProsecutorName);
                SqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);

                SqlConnect.Open(); //is this where the actuall StoredP called?
                isAdded = SqlCommand.ExecuteNonQuery() > 0; //true or false
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
                //may be: if currentC != selectedC ... //to ovoid overwriting //NO! only in sp it could be done.?. i think
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_UpdateComplaint";

                SqlCommand.Parameters.AddWithValue("@personId", complToUpdate.Citizen.CitizenID);
                SqlCommand.Parameters.AddWithValue("@fullName", complToUpdate.Citizen.CitizenName);
                SqlCommand.Parameters.AddWithValue("@birthDate", complToUpdate.Citizen.BirthDate);
                SqlCommand.Parameters.AddWithValue("@addressLine", complToUpdate.Citizen.CitizenAdress);
                SqlCommand.Parameters.AddWithValue("@occupation", complToUpdate.Citizen.Occupation);
                SqlCommand.Parameters.AddWithValue("@phoneNumber", complToUpdate.Citizen.PhoneNumber);
                SqlCommand.Parameters.AddWithValue("@email", complToUpdate.Citizen.Email);

                SqlCommand.Parameters.AddWithValue("@dateTime", complToUpdate.ReceiptDate);
                SqlCommand.Parameters.AddWithValue("@content", complToUpdate.OZhComplaintText.OZhComplaint);   //b4: // complToUpdate.ComplaintText);
                SqlCommand.Parameters.AddWithValue("@pageNum", complToUpdate.PageNum);
                SqlCommand.Parameters.AddWithValue("@appendNum", complToUpdate.AppendNum);
                SqlCommand.Parameters.AddWithValue("@comments", complToUpdate.Comments);
                SqlCommand.Parameters.AddWithValue("@result", complToUpdate.Result.Rezolution);
                SqlCommand.Parameters.AddWithValue("@chiefName", complToUpdate.Chief.ChiefName);

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

        public Complaint SelectComplaint(string citizenName, DateTime dateTime)
        {
            Complaint selectedComplaint = null;
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_EditComplaint";
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
                        selectedComplaint.OZhComplaintText.OZhComplaint = dataReader.GetString(9);        //b4: //selectedComplaint.ComplaintText = dataReader.GetString(9);
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
                SqlCommand.CommandText = "sp_SelectPersonByName";
                SqlCommand.Parameters.AddWithValue("@fullName", citizenName);

                SqlConnect.Open();

                #region w/o using
                //var dataReader = SqlCommand.ExecuteReader();
                //if (dataReader.HasRows)
                //{
                //    dataReader.Read();
                //    citizen = new ComplaintDT();

                //    citizen.CitizenID = dataReader.GetInt32(0);
                //    //gotta do null check first on these, or nothing happens:
                //    citizen.CitizenAdress = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);                       //int ocupIndex = dataReader.GetOrdinal("Occupation");
                //    citizen.Occupation = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                //    citizen.PhoneNumber = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                //    citizen.Email = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                //    citizen.BirthDate = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                //}
                //dataReader.Close(); 
                #endregion

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        citizen = new Complaint(); //exrtacting only citizen related things
                        citizen.Citizen.CitizenID = dataReader.GetInt32(0);
                        //gotta do null check first on these, or nothing happens:
                        citizen.Citizen.CitizenAdress = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);                       //int ocupIndex = dataReader.GetOrdinal("Occupation");
                        citizen.Citizen.Occupation = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                        citizen.Citizen.PhoneNumber = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                        citizen.Citizen.Email = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                        citizen.Citizen.BirthDate = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
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

        public bool DeleteComplaint(int id) //, DateTime dateTime
        {
            bool isDeleted = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_DeleteComplaintByID";
                SqlCommand.Parameters.AddWithValue("@complaintID", id);
                //SqlCommand.Parameters.AddWithValue("@dateTime", dateTime);

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
        #endregion
    }
}
