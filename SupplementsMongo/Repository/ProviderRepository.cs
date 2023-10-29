using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class ProviderRepository : IProviderRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public ProviderRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Provider");
    }
    
    public Provider GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }
    
    public Provider GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new Provider();
        var provider = BsonSerializer.Deserialize<Provider>(document);
        return provider;
    }

    public Provider GetByIdInclude(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetByIdInclude(objectId);
    }
    
    public Provider GetByIdInclude(ObjectId id)
    {
        var product = GetById(id);
        SetRelatedSupplements(product);
        return product;
    }

    public Provider GetByName(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Name", name);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new Provider();
        var provider = BsonSerializer.Deserialize<Provider>(document);
        return provider;
    }

    public IEnumerable<Provider> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var providers = new List<Provider>();
        foreach (var document in enumerableDocuments)
        {
            var provider = BsonSerializer.Deserialize<Provider>(document);
            providers.Add(provider);
        }

        return providers;
    }

    public IEnumerable<Provider> GetAllInclude()
    {
        var allProviders = GetAll();

        foreach (var provider in allProviders)
            SetRelatedSupplements(provider);

        return allProviders;
    }

    public bool CheckNameUnique(string name)
    {
        var allDocuments = GetAll();
        return allDocuments.All(document => document.Name != name);
    }

    public void Add(Provider provider)
    {
        var bson = provider.ToBsonDocument();
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
    
    public void Update(Provider provider)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", provider.Id);

        var update = Builders<BsonDocument>.Update
            .Set("Name", provider.Name)
            .Set("RegistrationCountry", provider.RegistrationCountry);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }
    
    private void SetRelatedSupplements(Provider provider)
    {
        var supplementRepository = new ProductRepository();
        var documents = supplementRepository.GetAll();

        var supplements = new List<Product>();
        if (supplements == null) throw new ArgumentNullException(nameof(supplements));

        foreach (var document in documents)
        {
            if (provider.Id != document.ProviderId) continue;
            supplements.Add(document);
            break;
        }

        provider.Products = supplements;
    }
}