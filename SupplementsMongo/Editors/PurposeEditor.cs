using MongoDB.Bson;
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
        RemoveReferenceFromNutritionalSupplement(purpose.Id);
        _repository.Delete(purpose.Id);
    }
    
    private static void RemoveReferenceFromNutritionalSupplement(ObjectId id)
    {
        var supplements = NutritionalSupplementEditor.GetTable();
        var isInSupplement = false;
        
        foreach (var supplement in supplements)
        {
            if (supplement.PurposesId.Any(objectId => objectId == id))
            {
                isInSupplement = true;
            }

            if (!isInSupplement) continue;
            supplement.PurposesId.Remove(id);
            NutritionalSupplementEditor.Update(supplement);
        }

    }
}