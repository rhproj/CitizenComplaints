using System.Configuration;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ADOServiceBase
    {
        protected SqlConnection _sqlConnect;
        protected SqlCommand _sqlCommand;
        private const string connectionName = "cs_ComplaintsADO";

        public ADOServiceBase()
        {
            _sqlConnect = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            _sqlCommand = new SqlCommand();
            _sqlCommand.Connection = _sqlConnect;
        }
    }
}
