using Complaints_WPF.Models;
using ComplaintsAdmin.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ComplaintsAdmin.Services
{
    public class AccessServiceADO : IAccessService
    {
        private SqlConnection _sqlConnect;
        private SqlCommand _sqlCommand;
        private const string connectionName = "cs_ComplaintsADO";

        public AccessServiceADO()
        {
            _sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnect;
        }

        public IEnumerable<AdminUser> GetAdmins()
        {
            List<AdminUser> usersList = new List<AdminUser>();
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandText = "sp_LoadAdmin";

                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
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
                _sqlConnect.Close();
            }

            return usersList;
        }

        public IEnumerable<Prosecutor> LoadProsecutorsInfo()
        {
            List<Prosecutor> ProsecutorsList = new List<Prosecutor>();

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
                _sqlConnect.Close();
            }

            return ProsecutorsList;
        }

        public void DeleteFromUsersList(Prosecutor prosecutorToDelete)
        {
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteProsecutor";

                _sqlCommand.Parameters.AddWithValue("@prosecutorId", prosecutorToDelete.ProsecutorID);

                _sqlConnect.Open();
                _sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
        }

        public void AddToUsersList(Prosecutor prosecutorToAdd)
        {
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_AddProsecutor";

                _sqlCommand.Parameters.AddWithValue("@login", prosecutorToAdd.Login);
                _sqlCommand.Parameters.AddWithValue("@fio", prosecutorToAdd.ProsecutorName);

                _sqlConnect.Open();

                _sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
        }

    }
}
