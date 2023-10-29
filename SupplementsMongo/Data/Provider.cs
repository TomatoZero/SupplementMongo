using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class Provider
{
    public ObjectId Id { get; set; }

    public string? Name { get; set; }

    public string? RegistrationCountry { get; set; } = null!;

    [BsonIgnore] public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}