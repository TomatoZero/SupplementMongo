using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class ProductRepository : IProductRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public ProductRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Product");
    }

    public Product GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }

    public Product GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new Product();
        var product = BsonSerializer.Deserialize<Product>(document);
        return product;
    }

    public Product GetByIdInclude(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetByIdInclude(objectId);
    }

    public Product GetByIdInclude(ObjectId id)
    {
        var product = GetById(id);
        SetRelatedSupplements(product);
        return product;
    }

    public IEnumerable<Product> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var ingredients = new List<Product>();
        foreach (var document in enumerableDocuments)
        {
            var product = BsonSerializer.Deserialize<Product>(document);
            ingredients.Add(product);
        }

        return ingredients;
    }

    public IEnumerable<Product> GetAllInclude()
    {
        var allProduct = GetAll();

        foreach (var product in allProduct)
            SetRelatedSupplements(product);

        return allProduct;
    }

    public bool CheckNameUnique(string name)
    {
        var allDocuments = GetAll();
        return allDocuments.All(document => document.Name != name);
    }

    public void Add(Product product)
    {
        var bson = product.ToBsonDocument();
        _collection.InsertOne(bson);
    }

    public void Delete(int id)
    {
        var targetId = ObjectId.Parse(id.ToString());
        Delete(targetId);
    }

    public void Delete(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
        var result = _collection.DeleteOne(filter);

        if (result.DeletedCount > 0)
        {
            // Document with the specified _id was successfully deleted
        }
    }

    public void Update(Product product)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", product.Id);

        var update = Builders<BsonDocument>.Update
            .Set("Name", product.Name)
            .Set("ManufacturingDate", product.ManufacturingDate)
            .Set("ExpirationDate", product.ExpirationDate)
            .Set("ProviderId", product.ProviderId)
            .Set("IngredientsId", product.IngredientsId);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }

    private void SetRelatedSupplements(Product purpose)
    {
        var supplementRepository = new IngredientRepository();
        foreach (var id in purpose.IngredientsId)
        {
            var supplement = supplementRepository.GetById(id);
            purpose.Ingredients.Add(supplement);
        }
    }
}