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
        _repository.Delete(supplement.Id);
    }
}