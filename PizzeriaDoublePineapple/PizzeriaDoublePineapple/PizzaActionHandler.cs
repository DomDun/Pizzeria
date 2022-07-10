using PizzeriaDoublePineapple.Bl;
using PizzeriaDoublePineapple.Bl.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzeriaDoublePineapple
{
    public class PizzaActionHandler
    {
        private readonly CliHelper _cliHelper = new CliHelper();
        private readonly IngredientService _ingredientsService = new IngredientService();
        private readonly SauceService _saucesService = new SauceService();
        private readonly PizzaService _pizzasService = new PizzaService();

        public void AddIngredient()
        {
            Console.Clear();
            string name = _cliHelper.GetStringFromUser("Give me name of ingredient");

            _ingredientsService.CreateNewIngredient(name);
        }

        public void AddSauce()
        {
            Console.Clear();
            string name = _cliHelper.GetStringFromUser("Give me name of sauce");
            double price = _cliHelper.GetDoubleFromUser("Type price of sauce");

            _saucesService.CreateNewSauce(name, price);
        }

        public void AddPizza()
        {
            Console.Clear();
            if (!_ingredientsService.IsAnyIngredientAdded())
            {
                Console.WriteLine("There are no ingredients ");
            }
            else if (!_saucesService.IsAnySauceAdded())
            {
                Console.WriteLine("There are no sauces ");
            }
            else
            {
                string name = _cliHelper.GetStringFromUser("Write name of pizza");
                double priceS;
                double priceM;
                double priceL;
                do
                {
                    priceS = _cliHelper.GetDoubleFromUser("Write price of pizza for S size");
                    priceM = _cliHelper.GetDoubleFromUser("Write price of pizza for M size");
                    priceL = _cliHelper.GetDoubleFromUser("Write price of pizza for L size");

                    if (priceS >= priceM || priceM >= priceL)
                    {
                        Console.WriteLine("S size have to be cheaper than M and L, and M have to be cheaper than L");
                    }
                } while (priceS >= priceM || priceM >= priceL);

                List<Ingredient> ingredients = GetIngredientsFromUser();

                if (ingredients == null)
                {
                    Console.WriteLine("We don't have that ingredient");
                }

                Sauce sauce = GetSauceFromUser("Type Id of sauce You want to add to Your pizza");

                _pizzasService.CreateNewPizza(name, priceS, priceM, priceL, ingredients, sauce);
            }
        }

        public Sauce GetSauceFromUser(string message)
        {
            while (true)
            {
                ShowAllSauces();

                int sauceId = _cliHelper.GetIntFromUser(message);
                Sauce sauce = _saucesService.GetSauceById(sauceId);

                if (!string.IsNullOrEmpty(sauce.Name))
                {
                    return sauce;
                }
                else
                {
                    Console.WriteLine("I don't have that sauce");
                }
            }
        }

        private void ShowAllSauces()
        {
            Console.WriteLine("");
            foreach (Sauce sauce in _saucesService.GetAllSauces())
            {
                Console.WriteLine($"Id: {sauce.Id} | {sauce.Name}");
            }
            Console.WriteLine("");
        }

        private List<Ingredient> GetIngredientsFromUser()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            bool addNextIngredient = false;
            do
            {
                Ingredient ingredientForPizza = GetIngredientForPizza();

                if (ingredients.Count(x => x.Id == ingredientForPizza.Id) < 2)
                {
                    ingredients.Add(ingredientForPizza);
                }
                else
                {
                    Console.WriteLine("There is too many same ingredients, I can't add this ingredient");
                }

                if (ingredients.Count < 7)
                {
                    addNextIngredient = _cliHelper.GetBoolFromUser("Do You want to add next ingredient? (true/false)");
                }
                else
                {
                    addNextIngredient = false;
                }

            } while (addNextIngredient != false && ingredients.Count <= 7);

            return ingredients;
        }

        private Ingredient GetIngredientForPizza()
        {
            Ingredient ingredientBl = null;
            bool isIngredientEmpty = true;
            do
            {
                ShowAllIngredients();
                int ingredientIdFromUser = _cliHelper.GetIntFromUser("Type Id of ingredient You want to add to Your pizza (max. 7 ingredients)");
                ingredientBl = _ingredientsService.GetIngredientById(ingredientIdFromUser);
                isIngredientEmpty = CheckIfIngredientIsEmpty(ingredientBl);
            } while (isIngredientEmpty);
            return ingredientBl;
        }

        private void ShowAllIngredients()
        {
            Console.WriteLine("");
            foreach (Ingredient ingredient in _ingredientsService.GetAllIngredients())
            {
                Console.WriteLine($"ID: {ingredient.Id} | {ingredient.Name}");
            }
            Console.WriteLine("");
        }

        private bool CheckIfIngredientIsEmpty(Ingredient ingredient)
        {
            if (ingredient.Name == string.Empty)
            {
                Console.WriteLine("We don't have that ingredient");
                return true;
            }
            return false;
        }

        public void ShowAllPizzas()
        {
            Console.WriteLine("");
            foreach (Pizza pizza in _pizzasService.GetAllPizzas())
            {
                Console.WriteLine($"ID: {pizza.Id} | {pizza.Name}, price S: {pizza.PriceS}, price M: {pizza.PriceM}, price L: {pizza.PriceL}");
                Console.WriteLine($"includes: {pizza.Sauce.Name}");
                foreach (Ingredient ingredient in pizza.Ingredients)
                {
                    Console.WriteLine($"{ingredient.Name}");
                }
            }
            Console.WriteLine("");
        }

        public void AddSauceToBasket(List<Sauce> sauceBasket)
        {
            bool buySauce = _cliHelper.GetBoolFromUser("Do You want to buy sauce? (true/false)");
            if (buySauce)
            {
                do
                {
                    Sauce sauce = GetSauceFromUser("Which sauce do You want to order?");
                    sauceBasket.Add(sauce);
                    buySauce = _cliHelper.GetBoolFromUser("Do You want to buy another sauce? (true/false)");
                } while (buySauce);
            }
        }

        public void AddPizzaAndFreeSaucesToBaskets(DayOfWeek dayOfWeek, List<Pizza> pizzaBasket, List<Sauce> sauceBasket)
        {
            bool buyNextPizza = false;
            do
            {
                ShowAllPizzas();

                int pizzaId = _cliHelper.GetIntFromUser("Choose Id of pizza that You want");
                Pizza pizza = _pizzasService.GetPizzaById(pizzaId);

                if (pizza == null)
                {
                    Console.WriteLine("We don't have pizza with that Id");
                }
                else
                {
                    PizzaSize pizzaSize = GetPizzaSizeFromUser();

                    switch (pizzaSize)
                    {
                        case PizzaSize.S:
                            pizza.PizzaSize = PizzaSize.S;
                            Sauce sauceForSmallPizza = GetSauceFromUser("Type Id of a sauce You want for free to Your pizza");
                            sauceForSmallPizza.Price = 0;
                            sauceBasket.Add(sauceForSmallPizza);

                            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                            {
                                pizza.PriceS /= 2;
                            }
                            pizza.PriceM = 0;
                            pizza.PriceL = 0;
                            pizzaBasket.Add(pizza);
                            break;
                        case PizzaSize.M:
                            pizza.PizzaSize = PizzaSize.M;
                            Sauce sauceForMediumPizza = GetSauceFromUser("Type Id of a sauce You want for free to Your pizza");
                            sauceForMediumPizza.Price = 0;
                            sauceBasket.Add(sauceForMediumPizza);

                            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                            {
                                pizza.PriceM /= 2;
                            }
                            pizza.PriceS = 0;
                            pizza.PriceL = 0;
                            pizzaBasket.Add(pizza);
                            break;
                        case PizzaSize.L:
                            pizza.PizzaSize = PizzaSize.L;
                            Sauce firstSauceForLargePizza = GetSauceFromUser("Type Id of a first sauce You want for free to Your pizza");
                            firstSauceForLargePizza.Price = 0;
                            sauceBasket.Add(firstSauceForLargePizza);
                            Sauce secondSauceForLargePizza = GetSauceFromUser("Type Id of a second sauce You want for free to Your pizza");
                            secondSauceForLargePizza.Price = 0;
                            sauceBasket.Add(secondSauceForLargePizza);

                            if (dayOfWeek == DayOfWeek.Sunday)
                            {
                                pizza.PriceL = pizza.PriceM;
                            }
                            else if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                            {
                                pizza.PriceL /= 2;
                            }
                            pizza.PriceS = 0;
                            pizza.PriceM = 0;
                            pizzaBasket.Add(pizza);
                            break;
                    }
                    buyNextPizza = _cliHelper.GetBoolFromUser("Do You want to buy another pizza? (true/false)");
                }
            } while (buyNextPizza);
        }

        PizzaSize GetPizzaSizeFromUser()
        {
            Console.WriteLine("which pizza size do You want to order?");
            foreach (string name in Enum.GetNames(typeof(PizzaSize)))
            {
                Console.WriteLine(name);
            }

            PizzaSize selectedOption;
            while (!Enum.TryParse(Console.ReadLine(), out selectedOption))
            {
                Console.WriteLine("We don't have that size, please try again!");
            }

            return selectedOption;
        }
    }
}
