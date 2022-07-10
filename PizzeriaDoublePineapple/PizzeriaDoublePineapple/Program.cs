using PizzeriaDoublePineapple.Bl;
using PizzeriaDoublePineapple.Bl.Models;
using System;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple
{
    internal class Program
    {
        private readonly CliHelper _cliHelper = new CliHelper();
        private readonly PizzaActionHandler _pizzaActionHandler = new PizzaActionHandler();
        private readonly ClientService _clientService = new ClientService();
        static void Main(string[] args)
        {
            new Program().Run();
        }

        private void Run()
        {
            Console.WriteLine("Welcome in Pizzeria Double Pineapple");

            bool exit = false;

            while (exit == false)
            {
                string action = _cliHelper.GetStringFromUser("Please choose Your option \n1.Add client\n2.Add ingredient\n3.Add sauce\n4.Add pizza\n5.Show menu\n6.Make order\n7.Exit");

                switch (action)
                {
                    case "1":
                        AddClient();
                        break;
                    case "2":
                        _pizzaActionHandler.AddIngredient();
                        break;
                    case "3":
                        _pizzaActionHandler.AddSauce();
                        break;
                    case "4":
                        _pizzaActionHandler.AddPizza();
                        break;
                    case "5":
                        _cliHelper.ShowMenu();
                        break;
                    case "6":
                        MakeOrder();
                        break;
                    case "7":
                        Console.WriteLine("Exiting...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("I don't know what You want, please choose again");
                        break;
                }
            }
        }

        private void AddClient()
        {
            Console.Clear();
            string phoneNumebr = _cliHelper.GetStringFromUser("Add phone number");
            string email = _cliHelper.GetStringFromUser("Add email address");
            string name = _cliHelper.GetStringFromUser("Add user name");
            string surname = _cliHelper.GetStringFromUser("Add user surname");
            string address = _cliHelper.GetStringFromUser("Add address");

            Client client = new Client
            {
                PhoneNumber = phoneNumebr,
                Email = email,
                Name = name,
                Surname = surname,
                Address = address
            };

            bool success = _clientService.AddClient(client);
            string message = success
                ? "Client added successfully"
                : "Something went wrong, client not added";

            Console.WriteLine(message);
        }

        public void MakeOrder()
        {
            Console.Clear();
            string clientNumber = _cliHelper.GetStringFromUser("Type phone number");
            bool checkClient = CheckIfClientExistInSystem(clientNumber);
            if (!checkClient)
            {
                AddClient();
            }

            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

            Console.WriteLine($"\nToday is {dayOfWeek}\n");
            Console.WriteLine($"\n On Wednesdays You can buy pizza with size L in price of size M ! \n On weekend every pizza is for half price!!!\n");

            List<Pizza> pizzaBasket = new List<Pizza>();
            List<Sauce> sauceBasket = new List<Sauce>();
            _pizzaActionHandler.AddPizzaAndFreeSaucesToBaskets(dayOfWeek, pizzaBasket, sauceBasket);
            _pizzaActionHandler.AddSauceToBasket(sauceBasket);

            OrderService orderService = new OrderService();
            Invoice invoice = orderService.PlaceOrder(clientNumber, pizzaBasket, sauceBasket);
            _cliHelper.PrintInvoice(invoice);

            //todo:  dodać serializację faktury
        }

        private bool CheckIfClientExistInSystem(string clientNumber)
        {
            bool clientExist = _clientService.CheckClient(clientNumber);

            return clientExist;
        }
    }
}
