using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class HealthEffect
{
    public ObjectId Id { get; set; }

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;
    [BsonIgnore]
    public List<NutritionalSupplement> NutritionalSupplements { get; set; } = new List<NutritionalSupplement>();
}