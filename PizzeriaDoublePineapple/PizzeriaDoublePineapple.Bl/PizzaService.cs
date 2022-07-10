using PizzeriaDoublePineapple.Bl.Models;
using PizzeriaDoublePineapple.Data;
using PizzeriaDoublePineapple.Data.Models;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl
{
    public class PizzaService
    {
        private readonly PizzaRepository _pizzaRepository = new PizzaRepository();

        public void CreateNewPizza(string name, double priceS, double priceM, double priceL, List<Ingredient> ingredients, Sauce sauce)
        {
            Pizza pizza = new Pizza(name, priceS, priceM, priceL, ingredients, sauce);
            _pizzaRepository.Add(MapToDataModel(pizza));
        }

        private PizzaData MapToDataModel(Pizza pizzaBl)
        {
            List<IngredientData> ingredientsData = new List<IngredientData>();

            foreach (Ingredient ingredient in pizzaBl.Ingredients)
            {
                IngredientData ingredientData = new IngredientData
                {
                    Id = ingredient.Id,
                    Name = ingredient.Name
                };
                ingredientsData.Add(ingredientData);
            }

            SauceData sauceData = new SauceData()
            {
                Id = pizzaBl.Sauce.Id,
                Name = pizzaBl.Sauce.Name,
                Price = pizzaBl.Sauce.Price
            };

            PizzaData pizza = new PizzaData
            {
                Id = pizzaBl.Id,
                Name = pizzaBl.Name,
                PriceS = pizzaBl.PriceS,
                PriceM = pizzaBl.PriceM,
                PriceL = pizzaBl.PriceL,
                PizzaSize = (PizzaSizeData)pizzaBl.PizzaSize,
                Sauce = sauceData,
                Ingredients = ingredientsData
            };

            return pizza;
        }

        public List<Pizza> GetAllPizzas()
        {
            List<PizzaData> pizzas = _pizzaRepository.GetAllPizzas();
            List<Pizza> pizzaBls = new List<Pizza>();

            foreach (PizzaData pizzaData in pizzas)
            {
                Pizza pizza = MapToBlModel(pizzaData);

                pizzaBls.Add(pizza);
            }

            return pizzaBls;
        }

        public Pizza GetPizzaById(int id)
        {
            return MapToBlModel(_pizzaRepository.GetPizzaById(id));
        }

        private Pizza MapToBlModel(PizzaData pizzaData)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (IngredientData ingredientData in pizzaData.Ingredients)
            {
                Ingredient ingredient = new Ingredient
                {
                    Id = ingredientData.Id,
                    Name = ingredientData.Name,
                };

                ingredients.Add(ingredient);
            }

            Sauce sauce = new Sauce(pizzaData.Sauce.Name, pizzaData.Sauce.Price);

            Pizza pizza = new Pizza(pizzaData.Name, pizzaData.PriceS, pizzaData.PriceM, pizzaData.PriceL, ingredients, sauce)
            {
                Id = pizzaData.Id,
            };

            return pizza;
        }
    }
}
