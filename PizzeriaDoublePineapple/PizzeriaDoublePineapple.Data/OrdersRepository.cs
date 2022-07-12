using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PizzeriaDoublePineapple.Data
{
    public class OrdersRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["PizzeriaDoublePineappleDbConnectionString"].ConnectionString;
        public void AddOrder(OrderData order)
        {
            SqlTransaction transaction;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    string insertOrderCommandText = $"INSERT INTO [Orders] ([PhoneNumber], [Date], [TotalCost]) VALUES (@PhoneNumber, @Date, @TotalCost)";

                    SqlCommand insertOrderCommand = new SqlCommand(insertOrderCommandText, connection, transaction);
                    insertOrderCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 255).Value = order.ClientData.PhoneNumber;
                    insertOrderCommand.Parameters.Add("@Date", SqlDbType.DateTime2).Value = order.OrderDate;
                    insertOrderCommand.Parameters.Add("@TotalCost", SqlDbType.Float, 8).Value = order.TotalCost;
                    insertOrderCommand.ExecuteNonQuery();

                    string getLastOrderIdCommandText = $"SELECT max(id) From Orders";
                    SqlCommand getLastOrderIdCommand = new SqlCommand(getLastOrderIdCommandText, connection, transaction);
                    SqlDataReader sqlDataReader = getLastOrderIdCommand.ExecuteReader();

                    sqlDataReader.Read();
                    int orderId = int.Parse(sqlDataReader[0].ToString());
                    sqlDataReader.Close();

                    string insertOrdersCommandText = $"INSERT INTO [PizzasOrders] ([OrderId], [PizzaId], [PizzaName], [PriceS], [PriceM], [PriceL], [PizzaSize]) VALUES (@OrderId, @PizzaId, @PizzaName, @PriceS, @PriceM, @PriceL, @PizzaSize)";
                    SqlCommand insertPizzaOrdersCommand = new SqlCommand(insertOrdersCommandText, connection, transaction);
                    foreach (PizzaData item in order.Pizzas)
                    {
                        insertPizzaOrdersCommand.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                        insertPizzaOrdersCommand.Parameters.Add("@PizzaId", SqlDbType.Int).Value = item.Id;
                        insertPizzaOrdersCommand.Parameters.Add("@PizzaName", SqlDbType.NVarChar, 255).Value = item.Name;
                        insertPizzaOrdersCommand.Parameters.Add("@PriceS", SqlDbType.Float, 8).Value = item.PriceS;
                        insertPizzaOrdersCommand.Parameters.Add("@PriceM", SqlDbType.Float, 8).Value = item.PriceM;
                        insertPizzaOrdersCommand.Parameters.Add("@PriceL", SqlDbType.Float, 8).Value = item.PriceL;
                        insertPizzaOrdersCommand.Parameters.Add("@PizzaSize", SqlDbType.NVarChar, 255).Value = item.PizzaSize;
                        insertPizzaOrdersCommand.ExecuteNonQuery();
                    }

                    string insertSauceOrdersCommandText = $"INSERT INTO [SaucesOrders] ([OrderId], [SauceId], [SauceName], [Price]) VALUES (@OrderId, @SauceId, @SauceName, @Price)";
                    SqlCommand insertSauceOrdersCommand = new SqlCommand(insertOrdersCommandText, connection, transaction);
                    foreach (SauceData sauce in order.Sauces)
                    {
                        insertSauceOrdersCommand.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                        insertSauceOrdersCommand.Parameters.Add("@SauceId", SqlDbType.Int).Value = sauce.Id;
                        insertSauceOrdersCommand.Parameters.Add("@SauceName", SqlDbType.NVarChar, 255).Value = sauce.Name;
                        insertSauceOrdersCommand.Parameters.Add("@Price", SqlDbType.Float, 8).Value = sauce.Price;
                        insertSauceOrdersCommand.ExecuteNonQuery();
                    }

                    // todo: zrobić update dateOfLastOrder   string changeLastDateOfOrder = $"UPDATE [Clients] SET [DateOfLastOrder] VALUES @Date WHERE [Orders][PhoneNumber] = @PhoneNumber";

                    transaction.Commit();
                    connection.Close();
                }
                //todo: dodawanie do sqla się wysypuje, sprawdzić to.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
