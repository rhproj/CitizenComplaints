using Complaints_WPF.Models;
using Complaints_WPF.Services.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Complaints_WPF.Services
{
    public class ComplaintWriteService : ADOServiceBase, IComplaintWriteService
    {
        public bool AddToComplaintList(Complaint newComplaint, string prosName)
        {
            bool isAdded = false;
            if (string.IsNullOrWhiteSpace(newComplaint.Citizen.CitizenName) || string.IsNullOrWhiteSpace(newComplaint.OZhComplaintText.OZhComplaint))
                throw new ArgumentException("Поля имя и текст жалобы болжны быть заполнены");

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_InsertComplaint";

                _sqlCommand.Parameters.AddWithValue("@fullName", newComplaint.Citizen.CitizenName);
                _sqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);
                _sqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                _sqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

                _sqlConnect.Open();
                isAdded = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
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
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_InsertComplaintUpd";

                _sqlCommand.Parameters.AddWithValue("@personId", newComplaint.Citizen.CitizenID);
                _sqlCommand.Parameters.AddWithValue("@birthDate", newComplaint.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", newComplaint.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", newComplaint.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", newComplaint.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", newComplaint.Citizen.Email);

                _sqlCommand.Parameters.AddWithValue("@content", newComplaint.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", newComplaint.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", newComplaint.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", newComplaint.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", newComplaint.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@prosecutorName", prosName);
                _sqlCommand.Parameters.AddWithValue("@chiefName", newComplaint.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", newComplaint.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", newComplaint.Citizen.Category);

                _sqlConnect.Open();
                isAdded = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }

            return isAdded;
        }

        public bool UpdateComplaint(Complaint complToUpdate)
        {
            bool isUpdated = false;
            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_UpdateComplaint";

                _sqlCommand.Parameters.AddWithValue("@personId", complToUpdate.Citizen.CitizenID);
                _sqlCommand.Parameters.AddWithValue("@fullName", complToUpdate.Citizen.CitizenName);
                _sqlCommand.Parameters.AddWithValue("@birthDate", complToUpdate.Citizen.BirthDate);
                _sqlCommand.Parameters.AddWithValue("@addressLine", complToUpdate.Citizen.CitizenAdress);
                _sqlCommand.Parameters.AddWithValue("@occupation", complToUpdate.Citizen.Occupation);
                _sqlCommand.Parameters.AddWithValue("@phoneNumber", complToUpdate.Citizen.PhoneNumber);
                _sqlCommand.Parameters.AddWithValue("@email", complToUpdate.Citizen.Email);

                _sqlCommand.Parameters.AddWithValue("@dateTime", complToUpdate.ReceiptDate);
                _sqlCommand.Parameters.AddWithValue("@content", complToUpdate.OZhComplaintText.OZhComplaint);
                _sqlCommand.Parameters.AddWithValue("@pageNum", complToUpdate.PageNum);
                _sqlCommand.Parameters.AddWithValue("@appendNum", complToUpdate.AppendNum);
                _sqlCommand.Parameters.AddWithValue("@comments", complToUpdate.Comments);
                _sqlCommand.Parameters.AddWithValue("@result", complToUpdate.Result.Rezolution);
                _sqlCommand.Parameters.AddWithValue("@chiefName", complToUpdate.Chief.ChiefName);
                _sqlCommand.Parameters.AddWithValue("@digitalStorage", complToUpdate.DigitalStorage);
                _sqlCommand.Parameters.AddWithValue("@category", complToUpdate.Citizen.Category);

                _sqlConnect.Open();
                isUpdated = _sqlCommand.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
            return isUpdated; //if ... above > 0 - true
        }

        public bool DeleteComplaint(int id)
        {
            bool isDeleted = false;

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_DeleteComplaintByID";
                _sqlCommand.Parameters.AddWithValue("@complaintID", id);

                _sqlConnect.Open();

                isDeleted = _sqlCommand.ExecuteNonQuery() > 0; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                _sqlConnect.Close();
            }
            return isDeleted;
        }
    }
}
