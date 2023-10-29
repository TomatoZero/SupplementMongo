using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class Purpose
{
    public ObjectId Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [BsonIgnore]
    public List<NutritionalSupplement> NutritionalSupplements { get; set; } = new List<NutritionalSupplement>();
}