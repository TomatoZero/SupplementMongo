using MongoDB.Bson;
using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class IngredientEditor
{
    private static IngredientRepository _repository;

    static IngredientEditor()
    {
        _repository = new IngredientRepository();
    }

    public static List<Ingredient> GetTable()
    {
        return _repository.GetAll().ToList();
    }
    
    public static List<Ingredient> GetTableInclude()
    {
        return _repository.GetAllInclude().ToList();
    }

    public static void Update(Ingredient ingredient)
    {
        _repository.Update(ingredient);    
    }

    public static void Add(Ingredient ingredient)
    {
        _repository.Add(ingredient);
    }

    public static void Remove(Ingredient ingredient)
    {
        RemoveReferenceFromProduct(ingredient.Id);
        _repository.Delete(ingredient.Id);
    }
    
    private static void RemoveReferenceFromProduct(ObjectId id)
    {
        var products = ProductEditor.GetTable();
        var isInIngredient = false;
        
        foreach (var ingredient in products)
        {
            if (ingredient.IngredientsId.Any(objectId => objectId == id))
            {
                isInIngredient = true;
            }

            if (!isInIngredient) continue;
            ingredient.IngredientsId.Remove(id);
            ProductEditor.Update(ingredient);
        }

    }
}