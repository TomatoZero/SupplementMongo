using MongoDB.Bson;
using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public static class IngredientDisplay
{
    private static List<Ingredient> _current;

    private static string _name;
    private static string _source;

    public static void PrintTable()
    {
        _current = IngredientEditor.GetTableInclude();

        var str = "Supplement:\n";
        foreach (var ingredient in _current)
        {
            Console.WriteLine(ingredient);
        }

        Console.WriteLine(str);
    }

    public static void PrintFullTable()
    {
        _current = IngredientEditor.GetTableInclude();

        Console.WriteLine("Ingredients:");
        foreach (var ingredient in _current)
        {
            PrintValue(ingredient);
            Console.WriteLine("---------------------------------");
        }
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New Ingredient:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();

            Console.WriteLine("Ingredient Source:");
            _source = Console.ReadLine().Trim();

            Console.WriteLine("Select Nutritional Supplements:");
            var supplements = NutritionalSupplementDisplay.SelectSupplementsId();

            if (IsInputPossible())
            {
                var ingredient = new Ingredient()
                {
                    Name = _name,
                    IngredientSource = _source,
                    NutritionalSupplementsId = supplements
                };

                IngredientEditor.Add(ingredient);
                return;
            }

            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Remove()
    {
        var ingredient = SelectIngredient();
        IngredientEditor.Remove(ingredient);
    }

    public static void Update()
    {
        var ingredient = SelectIngredient();

        PrintValue(ingredient);

        Console.WriteLine($"Update Nutritional Supplement:\n");

        Console.WriteLine("Name ('-' - same):");
        _name = Console.ReadLine();

        Console.WriteLine("Ingredient Source ('-' - same):");
        _source = Console.ReadLine().Trim();

        if (IsInputPossible())
        {
            CheckInput(ingredient);
            
            ingredient.Name = _name;
            ingredient.IngredientSource = _source;
            
            IngredientEditor.Update(ingredient);
        }
    }


    private static Ingredient SelectIngredient()
    {
        while (true)
        {
            PrintTable();

            var input = Console.ReadLine();

            foreach (var ingredient in _current)
            {
                if (input == ingredient.Name) return ingredient;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    public static List<ObjectId> SelectIngredientId()
    {
        var healthEffects = SelectIngredients();
        return healthEffects.Select(effect => effect.Id).ToList();
    }
    
    public static List<ObjectId> SelectIngredientsIdFrom(List<Ingredient> selectFrom)
    {
        var ingredientFrom = SelectIngredientsFrom(selectFrom);
        return ingredientFrom.Select(healthEffect => healthEffect.Id).ToList();
    }

    public static List<Ingredient> SelectIngredients()
    {
        _current = IngredientEditor.GetTable();
        return SelectIngredientsFrom(_current);
    }
    
    public static List<Ingredient> SelectIngredientsFrom(List<Ingredient> selectFrom)
    {
        var str = "Select from:\n";
        foreach (var ingredient in selectFrom)
        {
            str += $"   {ingredient.Name}\n";
        }
        Console.WriteLine(str);
        
        var supplements = new List<Ingredient>();

        Console.WriteLine("Print for select (, - separator)");
        var printedIngredients = Console.ReadLine().Trim().Split(',');

        foreach (var printedIngredient in printedIngredients)
        {
            foreach (var ingredient in selectFrom)
            {
                if (printedIngredient == ingredient.Name)
                {
                    supplements.Add(ingredient);
                    break;
                }
            }
        }

        return supplements;
    }
    
    private static void PrintValue(Ingredient ingredient)
    {
        var str = $"{ingredient.Name}\n" +
                  $"{ingredient.IngredientSource}\n";

        str += $"Nutritional supplements :\n";

        foreach (var supplement in ingredient.NutritionalSupplements)
        {
            str += $"{supplement}\n";
        }

        Console.WriteLine(str);
    }

    public static void UpdateNutritionalSupplements()
    {
        var ingredient = SelectIngredient();

        Console.WriteLine("Change Health Effects ('-' - same, '+' - add, '--', remove):");
        var supplementChoise = Console.ReadLine().Trim();
        var supplements = new List<ObjectId>();
        var current = ingredient.NutritionalSupplementsId.ToList();

        if (supplementChoise != "-")
        {
            if (supplementChoise == "+")
            {
                Console.WriteLine("Select Nutritional Supplements:");
                supplements = NutritionalSupplementDisplay.SelectSupplementsId();

                foreach (var effect in supplements)
                {
                    if (!current.Exists(id => id == effect))
                    {
                        current.Add(effect);
                    }
                }
            }
            else
            {
                var supplementsToRemove =
                    NutritionalSupplementDisplay.SelectSupplementsIdFrom(ingredient.NutritionalSupplements.ToList());

                foreach (var effect in supplementsToRemove) current.Remove(effect);
            }

            ingredient.NutritionalSupplementsId = current;
            IngredientEditor.Update(ingredient);
        }
    }
    
    private static bool IsInputPossible()
    {
        return IsInputPossible(_name) & IsInputPossible(_source);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
    
    private static void CheckInput(Ingredient ingredient)
    {
        if (_name == "-") _name = ingredient.Name;
        if (_source == "-") _source = ingredient.IngredientSource;
    }
}