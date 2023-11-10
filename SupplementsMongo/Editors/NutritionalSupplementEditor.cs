using MongoDB.Bson;
using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class NutritionalSupplementEditor
{
    private static NutritionalSupplementRepository _repository;

    static NutritionalSupplementEditor()
    {
        _repository = new NutritionalSupplementRepository();
    }

    public static List<NutritionalSupplement> GetTable()
    {
        return _repository.GetAll().ToList();
    }
    
    public static List<NutritionalSupplement> GetTableInclude()
    {
        return _repository.GetAllInclude().ToList();
    }

    public static void Update(NutritionalSupplement supplement)
    {
        _repository.Update(supplement);    
    }

    public static void Add(NutritionalSupplement supplement)
    {
        _repository.Add(supplement);
    }

    public static void Remove(NutritionalSupplement supplement)
    {
        RemoveReferenceFromIngredients(supplement.Id);
        _repository.Delete(supplement.Id);
    }
    
    private static void RemoveReferenceFromIngredients(ObjectId id)
    {
        var ingredients = IngredientEditor.GetTable();
        var isInIngredient = false;
        
        foreach (var ingredient in ingredients)
        {
            if (ingredient.NutritionalSupplementsId.Any(objectId => objectId == id))
            {
                isInIngredient = true;
            }

            if (!isInIngredient) continue;
            ingredient.NutritionalSupplementsId.Remove(id);
            IngredientEditor.Update(ingredient);
        }

    }
}