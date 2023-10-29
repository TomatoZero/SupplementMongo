using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class NutritionalSupplement
{
    public ObjectId Id { get; set; }

    public string Name { get; set; } = null!;

    public string ENum { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? AcceptableDailyIntake { get; set; }

    public virtual ICollection<ObjectId> HealthEffectsId { get; set; } = new List<ObjectId>();
    public virtual ICollection<ObjectId> PurposesId { get; set; } = new List<ObjectId>();

    [BsonIgnore] public virtual ICollection<HealthEffect> HealthEffects { get; set; } = new List<HealthEffect>();
    [BsonIgnore] public virtual ICollection<Purpose> Purposes { get; set; } = new List<Purpose>();
}