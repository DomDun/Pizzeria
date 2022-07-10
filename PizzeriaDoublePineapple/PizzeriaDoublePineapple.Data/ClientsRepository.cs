using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PizzeriaDoublePineapple.Data
{
    public class ClientsRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PizzeriaDoublePineappleDbConnectionString"].ConnectionString;

        public bool AddClient(ClientData client)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandAddPizzaSql = "INSERT INTO [Clients] ([PhoneNumber], [Email], [Name], [Surname], [Address]) VALUES (@PhoneNumber, @Email, @Name, @Surname, @Address)";

                    SqlCommand commandAddPizza = new SqlCommand(commandAddPizzaSql, connection);
                    commandAddPizza.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 255).Value = client.PhoneNumber;
                    commandAddPizza.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = client.Email;
                    commandAddPizza.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = client.Name;
                    commandAddPizza.Parameters.Add("@Surname", SqlDbType.NVarChar, 255).Value = client.Surname;
                    commandAddPizza.Parameters.Add("@Address", SqlDbType.NVarChar, 255).Value = client.Address;

                    if (commandAddPizza.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public ClientData GetClientPhoneNumber(string phoneNumber)
        {
            ClientData clientData = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT * FROM [Clients] WHERE [PhoneNumber] = @PhoneNumber";

                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 255).Value = phoneNumber;

                    SqlDataReader dataReader = command.ExecuteReader();

                    dataReader.Read();

                    clientData = new ClientData
                    {
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        Name = dataReader["Name"].ToString(),
                        Surname = dataReader["Surname"].ToString(),
                        Address = dataReader["Address"].ToString(),
                    };

                    connection.Close();
                }
            }
            catch (Exception)
            {
                clientData = null;
            }

            return clientData;
        }
    }
}
