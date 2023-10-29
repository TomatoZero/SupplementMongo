using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class NutritionalSupplementRepository : INutritionalSupplementsRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public NutritionalSupplementRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Nutritional_Supplements");
    }

    public NutritionalSupplement GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }

    public NutritionalSupplement GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new NutritionalSupplement();
        var supplement = BsonSerializer.Deserialize<NutritionalSupplement>(document);
        return supplement;
    }


    public NutritionalSupplement GetByIdInclude(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetByIdInclude(objectId);
    }

    public NutritionalSupplement GetByIdInclude(ObjectId id)
    {
        var healthEffect = GetById(id);
        SetRelatedSupplements(healthEffect);
        return healthEffect;
    }

    public NutritionalSupplement GetByName(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Name", name);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new NutritionalSupplement();
        var supplement = BsonSerializer.Deserialize<NutritionalSupplement>(document);
        return supplement;
    }

    public IEnumerable<NutritionalSupplement> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var supplements = new List<NutritionalSupplement>();
        foreach (var document in enumerableDocuments)
        {
            var supplement = BsonSerializer.Deserialize<NutritionalSupplement>(document);
            supplements.Add(supplement);
        }

        return supplements;
    }

    public IEnumerable<NutritionalSupplement> GetAllInclude()
    {
        var allSupplement = GetAll();

        foreach (var supplement in allSupplement)
            SetRelatedSupplements(supplement);

        return allSupplement;
    }

    public bool CheckNameUnique(string name)
    {
        var allDocuments = GetAll();
        return allDocuments.All(document => document.Name != name);
    }

    public void Add(NutritionalSupplement nutritionalSupplement)
    {
        var bson = nutritionalSupplement.ToBsonDocument();
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

    public void Update(NutritionalSupplement supplement)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", supplement.Id);

        var update = Builders<BsonDocument>.Update
            .Set("Name", supplement.Name)
            .Set("ENum", supplement.ENum)
            .Set("Description", supplement.Description)
            .Set("AcceptableDailyIntake", supplement.AcceptableDailyIntake)
            .Set("HealthEffectsId", supplement.HealthEffectsId)
            .Set("PurposesId", supplement.PurposesId);
        
        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }

    private void SetRelatedSupplements(NutritionalSupplement supplement)
    {
        var effectRepository = new HealthEffectRepository();
        foreach (var id in supplement.HealthEffectsId)
        {
            var effect = effectRepository.GetById(id);
            supplement.HealthEffects.Add(effect);
        }

        var purposeRepository = new PurposeRepository();
        foreach (var id in supplement.PurposesId)
        {
            var purpose = purposeRepository.GetById(id);
            supplement.Purposes.Add(purpose);
        }
    }
}