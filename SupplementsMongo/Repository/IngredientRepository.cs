using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class IngredientRepository : IIngredientRepository
{
    private IMongoCollection<BsonDocument> _collection;

    public IngredientRepository()
    {
        _collection = MongoClient.Database.GetCollection<BsonDocument>("Ingredient");
    }

    public Ingredient GetById(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetById(objectId);
    }

    public Ingredient GetById(ObjectId id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new Ingredient();
        var healthEffect = BsonSerializer.Deserialize<Ingredient>(document);
        return healthEffect;
    }

    public Ingredient GetByIdInclude(int id)
    {
        var objectId = ObjectId.Parse(id.ToString());
        return GetByIdInclude(objectId);
    }
    
    public Ingredient GetByIdInclude(ObjectId id)
    {
        var healthEffect = GetById(id);
        SetRelatedSupplements(healthEffect);
        return healthEffect;
    }

    public Ingredient GetByName(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Name", name);

        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null) return new Ingredient();
        var ingredient = BsonSerializer.Deserialize<Ingredient>(document);
        return ingredient;
    }

    public IEnumerable<Ingredient> GetAll()
    {
        var allDocuments = _collection.Find(_ => true).ToList();
        var enumerableDocuments = allDocuments.AsEnumerable();

        var ingredients = new List<Ingredient>();
        foreach (var document in enumerableDocuments)
        {
            var purpose = BsonSerializer.Deserialize<Ingredient>(document);
            ingredients.Add(purpose);
        }

        return ingredients;
    }

    public IEnumerable<Ingredient> GetAllInclude()
    {
        var allIngredient = GetAll();

        foreach (var ingredient in allIngredient)
            SetRelatedSupplements(ingredient);
        
        return allIngredient;
    }

    public bool CheckNameUnique(string name)
    {
        var allDocuments = GetAll();
        return allDocuments.All(document => document.Name != name);
    }

    public void Add(Ingredient ingredient)
    {
        var bson = ingredient.ToBsonDocument();
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

    public void Update(Ingredient ingredient)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ingredient.Id);
        
        var update = Builders<BsonDocument>.Update
            .Set("IngredientSource", ingredient.IngredientSource)
            .Set("Name", ingredient.Name)
            .Set("NutritionalSupplementId", ingredient.NutritionalSupplementsId);
        
        var result = _collection.UpdateOne(filter, update);
        if (result.ModifiedCount > 0)
        {
            // Document with the specified _id was successfully updated
        }
    }
    
    private void SetRelatedSupplements(Ingredient ingredient)
    {
        var supplementRepository = new NutritionalSupplementRepository();
        foreach (var id in ingredient.NutritionalSupplementsId)
        {
            var supplement = supplementRepository.GetById(id);
            ingredient.NutritionalSupplements.Add(supplement);
        }
    }
}