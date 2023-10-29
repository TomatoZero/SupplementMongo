using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class PurposeRepository : IPurposeRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public PurposeRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Purpose");
    }

    public Purpose GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }

    public Purpose GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var purposeDocument = _collection.Find(filter).FirstOrDefault();

        if (purposeDocument == null) return new Purpose();
        var purpose = BsonSerializer.Deserialize<Purpose>(purposeDocument);
        return purpose;
    }

    public Purpose GetByIdInclude(int id)
    {
        var purpose = GetById(id);
        SetRelatedSupplements(purpose);
        return purpose;
    }

    public Purpose GetByName(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Name", name);

        var purposeDocument = _collection.Find(filter).FirstOrDefault();

        if (purposeDocument == null) return new Purpose();
        var purpose = BsonSerializer.Deserialize<Purpose>(purposeDocument);
        return purpose;
    }

    public IEnumerable<Purpose> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var purposes = new List<Purpose>();
        foreach (var document in enumerableDocuments)
        {
            var purpose = BsonSerializer.Deserialize<Purpose>(document);
            purposes.Add(purpose);
        }

        return purposes;
    }

    public IEnumerable<Purpose> GetAllInclude()
    {
        var allPurpose = GetAll();

        foreach (var purpose in allPurpose)
            SetRelatedSupplements(purpose);

        return allPurpose;
    }

    public bool CheckNameUnique(string name)
    {
        var allDocuments = GetAll();
        return allDocuments.All(document => document.Name != name);
    }

    public void Add(Purpose purpose)
    {
        var bson = purpose.ToBsonDocument();
        _collection.InsertOne(bson);
    }

    public void Delete(int id)
    {
        var targetId = ObjectId.Parse(id.ToString());
        var filter = Builders<BsonDocument>.Filter.Eq("_id", targetId);
        var result = _collection.DeleteOne(filter);

        if (result.DeletedCount > 0)
        {
            // Document with the specified _id was successfully deleted
        }
    }

    public void Update(Purpose purpose)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", purpose.Id);

        var update = Builders<BsonDocument>.Update
            .Set("Name", purpose.Name)
            .Set("Description", purpose.Description);

        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }

    private void SetRelatedSupplements(Purpose purpose)
    {
        var supplementRepository = new NutritionalSupplementRepository();
        var documents = supplementRepository.GetAll();

        var supplements = new List<NutritionalSupplement>();
        if (supplements == null) throw new ArgumentNullException(nameof(supplements));

        foreach (var document in documents)
        {
            foreach (var documentPurpose in document.PurposesId)
            {
                if (purpose.Id != documentPurpose) continue;
                supplements.Add(document);
                break;
            }
        }

        purpose.NutritionalSupplements = supplements;
    }
}