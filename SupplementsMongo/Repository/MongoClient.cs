using MongoDB.Driver;

namespace NutritionalSupplements.Repository;

public static class MongoClient
{
    private static MongoDB.Driver.MongoClient _mongoClient;
    private static IMongoDatabase _database;

    public static MongoDB.Driver.MongoClient MongoDbClient => _mongoClient;
    public static IMongoDatabase Database => _database;

    static MongoClient()
    {
        _mongoClient = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
        _database = _mongoClient.GetDatabase("NutritionalSupplement");
    }
}