using System;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl.Models
{
    public class Order
    {
        public List<Pizza> Pizzas { get; set; }
        public List<Sauce> Sauces { get; set; }
        public double TotalCost { get; set; }
        public Client Client;
        public DateTime OrderDate;
    }
}
