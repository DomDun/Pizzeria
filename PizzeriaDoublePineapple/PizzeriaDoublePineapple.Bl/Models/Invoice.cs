using System;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl.Models
{
    public class Invoice
    {
        public List<Pizza> Pizzas;
        public List<Sauce> Sauces;
        public double TotalCost;
        public Client Client;
        public DateTime OrderDate;
    }
}
