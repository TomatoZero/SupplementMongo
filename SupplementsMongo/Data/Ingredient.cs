using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionalSupplements.Data;

public partial class Ingredient
{
    public ObjectId Id { get; set; }

    public string IngredientSource { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<ObjectId> NutritionalSupplementsId { get; set; } =
        new List<ObjectId>();

    [BsonIgnore]
    public List<NutritionalSupplement> NutritionalSupplements { get; set; } = new List<NutritionalSupplement>();

    public override string ToString()
    {
        return $"{Name} {IngredientSource}";
    }
}