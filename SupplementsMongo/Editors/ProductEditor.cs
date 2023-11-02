using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class ProductEditor
{
    private static ProductRepository _repository;

    static ProductEditor()
    {
        _repository = new ProductRepository();
    }
    
    public static List<Product> GetTable()
    {
        return _repository.GetAll().ToList();
    }

    public static void Update(Product product, Product newValue)
    {
        product.Name = newValue.Name;
        product.ProviderId = newValue.ProviderId;
        product.IngredientsId = newValue.IngredientsId;
        product.ExpirationDate = newValue.ExpirationDate;
        product.ManufacturingDate = newValue.ManufacturingDate;

        _repository.Update(product);
    }

    public static void Update(Product product)
    {
        _repository.Update(product);
    }

    public static void Add(Product provider)
    {
        _repository.Add(provider);
    }

    public static void Remove(Product provider)
    {
        _repository.Delete(provider.Id);
    }
}