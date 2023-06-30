using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ComplaintsAdmin.Services
{
    public class LoginService : ILoginService
    {
        private SqlConnection _sqlConnect;
        private SqlCommand _sqlCommand;
        private const string connectionName = "cs_ComplaintsADO";

        public LoginService()
        {
            _sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnect;
        }

        public bool Authenticate(string userName, string password)
        {
            bool isSucceeded = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.CommandText = $"select * from [dbo].[Admin] where [UserName] = '{userName}' and [Password] = '{password}'";
                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
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
                _sqlConnect.Close();
            }

            return isSucceeded;
        }
    }
}
