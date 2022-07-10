using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace PizzeriaDoublePineapple.Data
{
    public class IngredientRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PizzeriaDoublePineappleDbConnectionString"].ConnectionString;

        public List<IngredientData> GetAllIngredients()
        {
            List<IngredientData> ingredients = new List<IngredientData>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT * FROM [Ingredients]";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        IngredientData ingredient;

                        try
                        {
                            ingredient = new IngredientData
                            {
                                Id = int.Parse(dataReader["Id"].ToString()),
                                Name = dataReader["Name"].ToString()
                            };
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        ingredients.Add(ingredient);
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {
                ingredients = new List<IngredientData>();
            }

            return ingredients;
        }

        public IngredientData GetIngredientById(int id)
        {
            IngredientData ingredientData = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"SELECT * FROM [Ingredients] WHERE [Id] = {id}";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    dataReader.Read();

                    ingredientData = new IngredientData
                    {
                        Id = int.Parse(dataReader["Id"].ToString()),
                        Name = dataReader["Name"].ToString(),
                    };

                    connection.Close();
                }
            }
            catch (Exception)
            {
                ingredientData = new IngredientData
                {
                    Id = 0,
                    Name = string.Empty,
                };
            }

            return ingredientData;
        }

        public bool Add(IngredientData ingredient)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandText = $"INSERT INTO [Ingredients] ([Name]) VALUES ('{ingredient.Name}')";
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
