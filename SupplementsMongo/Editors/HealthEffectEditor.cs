using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public class HealthEffectEditor
{
    private static HealthEffectRepository _repository;

    static HealthEffectEditor()
    {
        _repository = new HealthEffectRepository();
    }
    
    public static List<HealthEffect> GetTable()
    {
        return _repository.GetAll().ToList();
    }

    public static void Update(HealthEffect purpose, HealthEffect newValue)
    {
        purpose.Category = newValue.Category;
        purpose.Description = newValue.Description;
        
        _repository.Update(purpose);
    }
    
    public static void Update(HealthEffect purpose)
    {
        _repository.Update(purpose);
    }

    public static void Add(HealthEffect purpose)
    {
        _repository.Add(purpose);
    }

    public static void Remove(HealthEffect purpose)
    {
        _repository.Delete(purpose.Id);
    }
}