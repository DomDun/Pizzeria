using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl.Models
{
    public class Pizza
    {
        public Pizza(string name, double priceS, double priceM, double priceL, List<Ingredient> ingredients, Sauce sauce)
        {
            Id = Id;
            Name = name;
            PriceS = priceS;
            PriceM = priceM;
            PriceL = priceL;
            Ingredients = ingredients;
            Sauce = sauce;
            PizzaSize = new PizzaSize();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double PriceS { get; set; }
        public double PriceM { get; set; }
        public double PriceL { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public Sauce Sauce { get; set; }
        public PizzaSize PizzaSize;
    }
}
