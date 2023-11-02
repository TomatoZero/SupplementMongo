using MongoDB.Bson;
using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class ProviderEditors
{
    private static ProviderRepository _repository;

    static ProviderEditors()
    {
        _repository = new ProviderRepository();
    }

    public static List<Provider> GetTable()
    {
        return _repository.GetAll().ToList();
    }

    public static Provider GetById(int id)
    {
        return _repository.GetById(id);
    }
    
    public static Provider GetById(ObjectId id)
    {
        return _repository.GetById(id);
    }
    
    public static void Update(Provider provider, Provider newValue)
    {
        provider.Name = newValue.Name;
        provider.RegistrationCountry = newValue.RegistrationCountry;

        _repository.Update(provider);
    }

    public static void Add(Provider provider)
    {
        _repository.Add(provider);
    }

    public static void Remove(Provider provider)
    {
        _repository.Delete(provider.Id);
    }
}