using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class PurposeEditor
{
    private static PurposeRepository _repository;

    static PurposeEditor()
    {
        _repository = new PurposeRepository();
    }
    
    public static List<Purpose> GetTable()
    {
        return _repository.GetAll().ToList();
    }

    public static void Update(Purpose purpose, Purpose newValue)
    {
        purpose.Name = newValue.Name;
        purpose.Description = newValue.Description;
        
        _repository.Update(purpose);
    }

    public static void Update(Purpose purpose)
    {
        _repository.Update(purpose);
    }

    public static void Add(Purpose purpose)
    {
        _repository.Add(purpose);
    }

    public static void Remove(Purpose purpose)
    {
        _repository.Delete(purpose.Id);
    }
}