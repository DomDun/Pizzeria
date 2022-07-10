namespace PizzeriaDoublePineapple.Bl.Models
{
    public class Sauce
    {
        public Sauce(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
