using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintsAdmin.Services
{
    public class AccessServiceADO
    {
        SqlConnection SqlConnect;
        SqlCommand SqlCommand;

        public AccessServiceADO()
        {
            SqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_ComplaintsADO"].ConnectionString);
            SqlCommand = new SqlCommand();
            SqlCommand.Connection = SqlConnect;
        }

        public bool Authenticate(string userName, string password)
        {
            bool isSucceeded = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.CommandText = $"select * from [dbo].[Admin] where [UserName] = '{userName}' and [Password] = '{password}'";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        isSucceeded = true;
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

            return isSucceeded;
        }

        public List<AdminUser> GetAdmins()
        {
            List<AdminUser> usersList = new List<AdminUser>();
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandText = "sp_LoadAdmin";

                SqlConnect.Open();

                using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            AdminUser adminUser = new AdminUser();

                            adminUser.Login =  dataReader.GetString(1);
                            adminUser.Password = dataReader.IsDBNull(2)? null : dataReader.GetString(2);

                            usersList.Add(adminUser);
                        }
                    }
                }

                #region delete
                //using (SqlDataReader dataReader = SqlCommand.ExecuteReader())
                //{
                //    if (dataReader.HasRows)
                //    {
                //        int count = 0;

                //        while (dataReader.Read())
                //        {
                //            count++;
                //            Complaint complaint = new Complaint();

                //            complaint.Enumerator = count;
                //            complaint.ComplaintID = dataReader.GetInt32(0);
                //            complaint.ReceiptDate = dataReader.GetDateTime(1);
                //            complaint.Citizen.CitizenName = dataReader.GetString(2);
                //            complaint.OZhComplaintText.OZhComplaint = dataReader.GetString(3);
                //            if (!dataReader.IsDBNull(4)) { complaint.Result.Rezolution = dataReader.GetString(4); }
                //            //Complaint.Result = dataReader.IsDBNull(4)? null : dataReader.GetString(4);

                //            if (!dataReader.IsDBNull(5)) { complaint.Prosecutor.ProsecutorName = dataReader.GetString(5); }

                //            if (!dataReader.IsDBNull(6)) { complaint.Chief.ChiefName = dataReader.GetString(6); }
                //            listOfComplaints.Add(complaint);
                //        }
                //    } 
                #endregion
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }

            return usersList;
        }

        public List<Prosecutor> LoadProsecutorsInfo()
        {
            List<Prosecutor> ProsecutorsList = new List<Prosecutor>();

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
                            Prosecutor prosecutor = new Prosecutor();

                            prosecutor.ProsecutorID = dataReader.GetInt32(0);
                            prosecutor.Login = dataReader.GetString(1);
                            prosecutor.ProsecutorName = dataReader.GetString(2);

                            ProsecutorsList.Add(prosecutor);
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

        internal void DeleteFromUsersList(Prosecutor prosecutorToDelete)
        {
            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_DeleteProsecutor";

                SqlCommand.Parameters.AddWithValue("@prosecutorId", prosecutorToDelete.ProsecutorID);

                SqlConnect.Open();
                SqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
        }

        public void AddToUsersList(Prosecutor prosecutorToAdd)
        {
            //bool isAdded = false;

            try
            {
                SqlCommand.Parameters.Clear();
                SqlCommand.CommandType = CommandType.StoredProcedure;
                SqlCommand.CommandText = "sp_AddProsecutor";

                SqlCommand.Parameters.AddWithValue("@login", prosecutorToAdd.Login);
                SqlCommand.Parameters.AddWithValue("@fio", prosecutorToAdd.ProsecutorName);


                SqlConnect.Open();
                //isAdded = SqlCommand.ExecuteNonQuery() > 0;
                SqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
            //return isAdded;
        }

        //public bool DeleteFromChiefsList(string chiefName)
        //{
        //    bool isDeleted = false;

        //    try
        //    {
        //        SqlCommand.Parameters.Clear();
        //        SqlCommand.CommandType = CommandType.StoredProcedure;
        //        SqlCommand.CommandText = "sp_DeleteChief";
        //        SqlCommand.Parameters.AddWithValue("@chiefName", chiefName);

        //        SqlConnect.Open();
        //        isDeleted = SqlCommand.ExecuteNonQuery() > 0;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        SqlConnect.Close();
        //    }

        //    return isDeleted;
        //}
    }
}
