using MongoDB.Bson;
using NutritionalSupplements.Data;
using SupplementsMongo.Display;

namespace SupplementsMongo.Editors;

public static class HealthEffectEditor
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
        RemoveReferenceFromNutritionalSupplement(purpose.Id);
        _repository.Delete(purpose.Id);
    }

    private static void RemoveReferenceFromNutritionalSupplement(ObjectId id)
    {
        var supplements = NutritionalSupplementEditor.GetTable();
        var isInSupplement = false;
        
        foreach (var supplement in supplements)
        {
            if (supplement.HealthEffectsId.Any(objectId => objectId == id))
            {
                isInSupplement = true;
            }

            if (!isInSupplement) continue;
            supplement.HealthEffectsId.Remove(id);
            NutritionalSupplementEditor.Update(supplement);
        }

    }
}