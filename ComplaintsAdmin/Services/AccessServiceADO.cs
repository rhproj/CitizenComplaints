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

        public List<string> GetAdmins()
        {
            List<string> usersList = new List<string>();
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
                            usersList.Add(dataReader.GetString(1));
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
