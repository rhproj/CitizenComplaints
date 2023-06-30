using System.Data;
using System.Data.SqlClient;

namespace ComplaintsAdmin.Services
{
    public class LoginService : ADOServiceBase, ILoginService
    {
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
