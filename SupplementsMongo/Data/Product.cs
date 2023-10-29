using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class Product
{
    public ObjectId Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime ManufacturingDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public ObjectId ProviderId { get; set; }

    public virtual ICollection<ObjectId> IngredientsId { get; set; } = new List<ObjectId>();
    
    [BsonIgnore]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    [BsonIgnore]
    public virtual Provider? Provider { get; set; }
}