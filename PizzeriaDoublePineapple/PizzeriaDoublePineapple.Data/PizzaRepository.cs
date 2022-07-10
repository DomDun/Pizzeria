using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PizzeriaDoublePineapple.Data
{
    public class PizzaRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PizzeriaDoublePineappleDbConnectionString"].ConnectionString;

        public List<PizzaData> GetAllPizzas()
        {
            List<PizzaData> listOfPizzas = new List<PizzaData>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                connection.Open();
                string commandText =
@" SELECT 
    [PizzaId],
    [Pizzas].[Name] AS [PizzaName],
    [IngredientId],
    [Ingredients].[Name] AS [IngredientName],
    [Sauces].[Id] AS [SauceId],
    [Sauces].[Name] AS [SauceName],
    [Pizzas].[PriceS],
    [Pizzas].[PriceM],
    [Pizzas].[PriceL]
FROM [PizzaIngredients]
INNER JOIN [Pizzas] ON [Pizzas].[Id] = [PizzaIngredients].[PizzaId]
INNER JOIN [Ingredients] ON [Ingredients].[Id] = [PizzaIngredients].[IngredientId]
INNER JOIN [Sauces] ON [Sauces].[Id] = [Pizzas].[SauceId];";
                SqlCommand command = new SqlCommand(commandText, connection);

                SqlDataReader reader = command.ExecuteReader();

                IngredientData ingredientData = null;
                int previousPizzaId = 0;
                PizzaData previousPizza = null;

                while (reader.Read())
                {
                    int pizzaId = int.Parse(reader["PizzaId"].ToString());

                    if (previousPizzaId != pizzaId)
                    {
                        PizzaData currentPizza = new PizzaData
                        {
                            Id = int.Parse(reader["PizzaId"].ToString()),
                            Name = reader["PizzaName"].ToString(),
                            Ingredients = new List<IngredientData>(),
                            PriceS = double.Parse(reader["PriceS"].ToString()),
                            PriceM = double.Parse(reader["PriceM"].ToString()),
                            PriceL = double.Parse(reader["PriceL"].ToString()),
                            Sauce = new SauceData
                            {
                                Id = int.Parse(reader["SauceId"].ToString()),
                                Name = reader["SauceName"].ToString(),
                            },
                        };

                        listOfPizzas.Add(currentPizza);

                        previousPizzaId = pizzaId;
                        previousPizza = currentPizza;
                    }

                    ingredientData = new IngredientData
                    {
                        Id = int.Parse(reader["IngredientId"].ToString()),
                        Name = reader["IngredientName"].ToString(),
                    };
                    previousPizza.Ingredients.Add(ingredientData);
                }
                connection.Close();
            }
            return listOfPizzas;
        }

        public PizzaData GetPizzaById(int id)
        {
            List<PizzaData> pizzas = GetAllPizzas();

            try
            {
                return pizzas.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {
                return new PizzaData
                {
                    Name = string.Empty,
                    PriceS = 0,
                    PriceM = 0,
                    PriceL = 0,
                    Ingredients = new List<IngredientData>(),
                    Sauce = new SauceData(),
                };
            }
        }

        public void Add(PizzaData pizza)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string commandAddPizzaSql = "INSERT INTO [Pizzas] ([Name], [PriceS], [PriceM], [PriceL], [SauceId]) VALUES (@Name, @PriceS, @PriceM, @PriceL, @SauceId)";

                    SqlCommand commandAddPizza = new SqlCommand(commandAddPizzaSql, connection);
                    commandAddPizza.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = pizza.Name;
                    commandAddPizza.Parameters.Add("@PriceS", SqlDbType.Float, 8).Value = pizza.PriceS;
                    commandAddPizza.Parameters.Add("@PriceM", SqlDbType.Float, 8).Value = pizza.PriceM;
                    commandAddPizza.Parameters.Add("@PriceL", SqlDbType.Float, 8).Value = pizza.PriceL;
                    commandAddPizza.Parameters.Add("@SauceId", SqlDbType.Int).Value = pizza.Sauce.Id;
                    commandAddPizza.ExecuteNonQuery();

                    string getLastPizzaIdSql = $"SELECT IDENT_CURRENT('Pizzas') AS [PizzaId];";
                    SqlCommand getLastPizzaId = new SqlCommand(getLastPizzaIdSql, connection);

                    SqlDataReader reader = getLastPizzaId.ExecuteReader();
                    reader.Read();

                    int lastPizzaId = int.Parse(reader["PizzaId"].ToString());

                    reader.Close();

                    foreach (IngredientData ingredientData in pizza.Ingredients)
                    {
                        string commandAddPizzaIngredientsSql = $"INSERT INTO [PizzaIngredients] ([PizzaId], [IngredientId]) VALUES (@PizzaId, @IngredientId);";
                        SqlCommand commandAddPizzaIngredient = new SqlCommand(commandAddPizzaIngredientsSql, connection);
                        commandAddPizzaIngredient.Parameters.Add("PizzaId", SqlDbType.Int).Value = lastPizzaId;
                        commandAddPizzaIngredient.Parameters.Add("IngredientId", SqlDbType.Int).Value = ingredientData.Id;
                        commandAddPizzaIngredient.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
