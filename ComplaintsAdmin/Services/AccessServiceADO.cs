using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ComplaintsAdmin.Services
{
    public class AccessServiceADO : AccessService
    {
        SqlConnection SqlConnect;
        SqlCommand SqlCommand;

        public AccessServiceADO()
        {
            SqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["cs_ComplaintsADO"].ConnectionString);
            SqlCommand = new SqlCommand();
            SqlCommand.Connection = SqlConnect;
        }

        internal override bool Authenticate(string userName, string password)
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

        internal override IList<AdminUser> GetAdmins()
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

                            adminUser.Login = dataReader.GetString(1);
                            adminUser.Password = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);

                            usersList.Add(adminUser);
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

            return usersList;
        }

        internal override IList<Prosecutor> LoadProsecutorsInfo()
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

        internal override void DeleteFromUsersList(Prosecutor prosecutorToDelete)
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

        internal override void AddToUsersList(Prosecutor prosecutorToAdd)
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
    }
}
