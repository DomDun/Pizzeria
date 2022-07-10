using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace PizzeriaDoublePineapple.Data
{
    public class SauceRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PizzeriaDoublePineappleDbConnectionString"].ConnectionString;

        public List<SauceData> GetAllSauces()
        {
            List<SauceData> sauces = new List<SauceData>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT * FROM [Sauces]";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        SauceData sauce = new SauceData
                        {
                            Id = int.Parse(dataReader["ID"].ToString()),
                            Name = dataReader["Name"].ToString(),
                            Price = double.Parse(dataReader["Price"].ToString())
                        };

                        sauces.Add(sauce);
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {
                sauces = new List<SauceData>();
            }

            return sauces;
        }

        public SauceData GetSauceById(int id)
        {
            SauceData sauceData = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT * FROM [Sauces] WHERE [Id] = {id}";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    dataReader.Read();

                    sauceData = new SauceData
                    {
                        Id = int.Parse(dataReader["Id"].ToString()),
                        Name = dataReader["Name"].ToString(),
                        Price = double.Parse(dataReader["Price"].ToString()),
                    };

                    connection.Close();
                }
            }
            catch (Exception)
            {
                sauceData = new SauceData
                {
                    Id = 0,
                    Name = string.Empty,
                };
            }

            return sauceData;
        }

        public bool Add(SauceData sauce)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"INSERT INTO [Sauces] ([Name],[Price]) VALUES ('{sauce.Name}', '{sauce.Price}')";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    int rowsAffected = command.ExecuteNonQuery();

                    success = rowsAffected == 1;

                    connection.Close();
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }
}
