﻿using Complaints_WPF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Complaints_WPF.Services
{
    public class CategoryReadService : ADOServiceBase, ICategoryReadService
    {
        public IEnumerable<string> LoadCategories() 
        {
            var categoryList = new List<string>();

            try
            {
                _sqlCommand.Parameters.Clear();
                _sqlCommand.CommandType = CommandType.StoredProcedure;
                _sqlCommand.CommandText = "sp_LoadCategories";
                _sqlConnect.Open();

                using (SqlDataReader dataReader = _sqlCommand.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            categoryList.Add(dataReader.GetString(1));
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

            return categoryList;
        }
    }
}
