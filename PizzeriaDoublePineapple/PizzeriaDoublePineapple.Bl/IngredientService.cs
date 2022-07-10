using PizzeriaDoublePineapple.Bl.Models;
using PizzeriaDoublePineapple.Data;
using PizzeriaDoublePineapple.Data.Models;
using System.Collections.Generic;

namespace PizzeriaDoublePineapple.Bl
{
    public class IngredientService
    {
        private readonly IngredientRepository _ingredientRepository = new IngredientRepository();

        public void CreateNewIngredient(string name)
        {
            Ingredient ingredientBl = new Ingredient
            {
                Name = name,
            };
            _ingredientRepository.Add(MapToDataModel(ingredientBl));
        }

        public bool IsAnyIngredientAdded()
        {
            bool result = _ingredientRepository.GetAllIngredients().Count != 0;
            return result;
        }

        private IngredientData MapToDataModel(Ingredient ingredientBl)
        {
            IngredientData ingredient = new IngredientData
            {
                Id = ingredientBl.Id,
                Name = ingredientBl.Name,
            };

            return ingredient;
        }

        private Ingredient MapToBusinessModel(IngredientData ingredient)
        {
            Ingredient ingredientBl = new Ingredient()
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };


            return ingredientBl;
        }

        public List<Ingredient> GetAllIngredients()
        {
            List<IngredientData> ingredients = _ingredientRepository.GetAllIngredients();
            List<Ingredient> ingredientBls = new List<Ingredient>();

            foreach (IngredientData ingredient in ingredients)
            {
                ingredientBls.Add(MapToBusinessModel(ingredient));
            }

            return ingredientBls;
        }

        public Ingredient GetIngredientById(int id)
        {
            IngredientData ingredient = _ingredientRepository.GetIngredientById(id);
            Ingredient ingredientBl = MapToBusinessModel(ingredient);

            return ingredientBl;
        }
    }
}
