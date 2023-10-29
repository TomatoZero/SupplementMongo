using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class HealthEffectRepository : IHealthEffectRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public HealthEffectRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Health_Effect");
    }

    public HealthEffect GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }

    public HealthEffect GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new HealthEffect();
        var healthEffect = BsonSerializer.Deserialize<HealthEffect>(document);
        return healthEffect;
    }

    public HealthEffect GetByCategory(string category)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Category", category);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new HealthEffect();
        var healthEffect = BsonSerializer.Deserialize<HealthEffect>(document);
        return healthEffect;
    }

    public HealthEffect GetByIdInclude(int id)
    {
        var healthEffect = GetById(id);
        SetRelatedSupplements(healthEffect);
        return healthEffect;
    }

    public IEnumerable<HealthEffect> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var healthEffect = new List<HealthEffect>();
        foreach (var document in enumerableDocuments)
        {
            var purpose = BsonSerializer.Deserialize<HealthEffect>(document);
            healthEffect.Add(purpose);
        }

        return healthEffect;
    }

    public IEnumerable<HealthEffect> GetAllInclude()
    {
        var allEffect = GetAll();

        foreach (var effect in allEffect)
            SetRelatedSupplements(effect);

        return allEffect;
    }

    public void Add(HealthEffect healthEffect)
    {
        var bson = healthEffect.ToBsonDocument();
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

    public void Update(HealthEffect healthEffect)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", healthEffect.Id);

        var update = Builders<BsonDocument>.Update
            .Set("Category", healthEffect.Category)
            .Set("Description", healthEffect.Description);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }

    private void SetRelatedSupplements(HealthEffect healthEffect)
    {
        var supplementRepository = new NutritionalSupplementRepository();
        var documents = supplementRepository.GetAll();

        var supplements = new List<NutritionalSupplement>();
        if (supplements == null) throw new ArgumentNullException(nameof(supplements));

        foreach (var document in documents)
        {
            foreach (var documentPurpose in document.PurposesId)
            {
                if (healthEffect.Id != documentPurpose) continue;
                supplements.Add(document);
                break;
            }
        }

        healthEffect.NutritionalSupplements = supplements;
    }
}