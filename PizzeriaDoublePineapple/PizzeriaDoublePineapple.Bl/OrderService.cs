using PizzeriaDoublePineapple.Bl.Models;
using PizzeriaDoublePineapple.Data;
using PizzeriaDoublePineapple.Data.Models;
using System;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl
{
    public class OrderService
    {
        public Invoice PlaceOrder(string clientNumber, List<Pizza> pizzaBasket, List<Sauce> sauceBasket)
        {
            OrdersRepository _ordersrepository = new OrdersRepository();
            ClientsRepository _clientsRepository = new ClientsRepository();

            List<PizzaData> pizzasData = new List<PizzaData>();
            double totalCost = 0;

            foreach (Pizza pizzas in pizzaBasket)
            {
                PizzaData pizzaData = new PizzaData
                {
                    Id = pizzas.Id,
                    Name = pizzas.Name,
                    PriceS = pizzas.PriceS,
                    PriceM = pizzas.PriceM,
                    PriceL = pizzas.PriceL,
                    PizzaSize = (PizzaSizeData)pizzas.PizzaSize
                };
                pizzasData.Add(pizzaData);
                totalCost += pizzas.PriceS + pizzas.PriceM + pizzas.PriceL;
            }

            List<SauceData> saucesData = new List<SauceData>();

            foreach (Sauce sauces in sauceBasket)
            {
                SauceData sauceData = new SauceData
                {
                    Id = sauces.Id,
                    Name = sauces.Name,
                    Price = sauces.Price
                };
                saucesData.Add(sauceData);
                totalCost += sauces.Price;
            }

            ClientData clientData = _clientsRepository.GetClientPhoneNumber(clientNumber);

            OrderData newOrder = new OrderData
            {

                OrderDate = DateTime.Now,
                Pizzas = pizzasData,
                Sauces = saucesData,
                ClientData = new ClientData
                {
                    PhoneNumber = clientData.PhoneNumber,
                },
                TotalCost = totalCost,
            };

            _ordersrepository.AddOrder(newOrder);

            Invoice newInvoice = new Invoice
            {
                Pizzas = pizzaBasket,
                Sauces = sauceBasket,
                TotalCost = totalCost,
                Client = new Client
                {
                    PhoneNumber = clientData.PhoneNumber,
                    Address = clientData.Address,
                    Email = clientData.Email,
                    Name = clientData.Name,
                    Surname = clientData.Surname,
                },
                OrderDate = DateTime.Now
            };

            return newInvoice;
        }
    }
}
