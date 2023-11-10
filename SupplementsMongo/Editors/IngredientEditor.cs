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
        _repository.Delete(ingredient.Id);
    }
}