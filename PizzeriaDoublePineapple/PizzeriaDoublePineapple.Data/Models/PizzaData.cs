using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Data.Models
{
    public class PizzaData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PriceS { get; set; }
        public double PriceM { get; set; }
        public double PriceL { get; set; }
        public List<IngredientData> Ingredients { get; set; }
        public SauceData Sauce { get; set; }
        public PizzaSizeData PizzaSize;
    }
}
