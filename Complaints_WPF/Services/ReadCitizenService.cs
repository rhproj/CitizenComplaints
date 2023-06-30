using Complaints_WPF.Models;
using Complaints_WPF.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ReadCitizenService : ADOServiceBase, IReadCitizenService
    {
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
    }
}
