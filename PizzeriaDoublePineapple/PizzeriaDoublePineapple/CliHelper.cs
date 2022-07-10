using PizzeriaDoublePineapple.Bl;
using PizzeriaDoublePineapple.Bl.Models;
using System;
using System.Globalization;

namespace PizzeriaDoublePineapple
{
    public class CliHelper
    {
        private readonly PizzaService _pizzasService = new PizzaService();
        private readonly SauceService _saucesService = new SauceService();

        public bool GetBoolFromUser(string message)
        {
            bool result = true;
            bool success = false;

            while (!success)
            {
                string text = GetStringFromUser(message);
                success = bool.TryParse(text, out result);

                if (!success)
                {
                    Console.WriteLine("Not a bool. Try again...");
                }
            }

            return result;
        }

        public int GetIntFromUser(string message)
        {
            int result = 0;
            bool success = false;

            while (!success)
            {
                string text = GetStringFromUser(message);
                success = int.TryParse(text, out result);

                if (!success)
                {
                    Console.WriteLine("Not a number. Try again...");
                }
            }
            return result;
        }

        public double GetDoubleFromUser(string message)
        {
            double result = 0;
            bool success = false;

            while (!success)
            {
                string text = GetStringFromUser(message);
                success = double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                if (!success)
                {
                    Console.WriteLine("Not a number, please try again...");
                }
            }
            return result;
        }

        public string GetStringFromUser(string message)
        {
            string inputFromUser;

            do
            {
                Console.Write($"{message}: ");
                inputFromUser = Console.ReadLine();

                if (string.IsNullOrEmpty(inputFromUser))
                {
                    Console.WriteLine("You have to type something");
                }

            } while (string.IsNullOrEmpty(inputFromUser));

            return inputFromUser;
        }

        public void ShowMenu()
        {
            Console.Clear();
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

            Console.WriteLine($"\nToday is {dayOfWeek}\n");
            Console.WriteLine($"\n On Wednesdays You can buy pizza with size L in price of size M ! \n On weekend every pizza is for half price!!!\n");

            Console.WriteLine("Sauces: ");

            foreach (Sauce sauce in _saucesService.GetAllSauces())
            {
                Console.WriteLine($"ID: {sauce.Id} | {sauce.Name}, price: {sauce.Price}");
            }

            Console.WriteLine(" ");
            Console.WriteLine("Pizza: ");

            foreach (Pizza pizza in _pizzasService.GetAllPizzas())
            {
                Console.WriteLine($"ID: {pizza.Id} | {pizza.Name}, price S: {pizza.PriceS}, price M: {pizza.PriceM}, price L: {pizza.PriceL}");
                Console.WriteLine($"includes: {pizza.Sauce.Name} sauce");
                foreach (Ingredient ingredient in pizza.Ingredients)
                {
                    Console.WriteLine($"{ingredient.Name}");
                }
            }
            Console.WriteLine("");
        }

        public void PrintInvoice(Invoice invoice)
        {
            Console.Clear();
            Console.WriteLine($"Client: {invoice.Client.Name}, {invoice.Client.Surname}, email: {invoice.Client.Email}, phone number: {invoice.Client.PhoneNumber}, address: {invoice.Client.Address}");

            foreach (Pizza purchase in invoice.Pizzas)
            {
                if (purchase.PizzaSize == PizzaSize.S)
                {
                    Console.WriteLine($"{purchase.Id} | {purchase.Name} | size: {purchase.PizzaSize}    {purchase.PriceS} [PLN]");
                }
                else if (purchase.PizzaSize == PizzaSize.M)
                {
                    Console.WriteLine($"{purchase.Id} | {purchase.Name} | size: {purchase.PizzaSize}    {purchase.PriceM} [PLN]");
                }
                else
                {
                    Console.WriteLine($"{purchase.Id} | {purchase.Name} | size: {purchase.PizzaSize}    {purchase.PriceL} [PLN]");
                }
            }

            foreach (Sauce purchase in invoice.Sauces)
            {
                Console.WriteLine($"{purchase.Id} | {purchase.Name} |     {purchase.Price} [PLN]");
            }

            Console.WriteLine($"Total cost of purchase is {invoice.TotalCost} [PLN]");
        }
    }
}
