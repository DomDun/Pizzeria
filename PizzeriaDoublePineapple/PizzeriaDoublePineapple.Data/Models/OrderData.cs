using System;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Data.Models
{
    public class OrderData
    {
        public List<PizzaData> Pizzas { get; set; }
        public List<SauceData> Sauces { get; set; }
        public double TotalCost;
        public ClientData ClientData;
        public DateTime OrderDate;
    }
}
