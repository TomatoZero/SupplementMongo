using MongoDB.Bson;
using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public class HealthEffectDisplay
{
    private static List<HealthEffect> _current;

    private static string _category;
    private static string _description;

    public static void PrintTable()
    {
        _current = HealthEffectEditor.GetTable();

        var str = "Health effect:\n";
        foreach (var effect in _current)
        {
            str += $"   {effect.Category}\n";
        }

        Console.WriteLine(str);
    }

    public static void Update()
    {
        var healthEffect = SelectHealthEffect();

        Console.WriteLine($"Current value:\n" +
                          $"    {healthEffect.Category}\n" +
                          $"    {healthEffect.Description}");

        Console.WriteLine("New Category ('-' - same):");
        _category = Console.ReadLine();
        Console.WriteLine("Description ('-' - same):");
        _description = Console.ReadLine().Trim();

        if (_category != "-") healthEffect.Category = _category;
        if (_description != "-") healthEffect.Description = _description;

        HealthEffectEditor.Update(healthEffect);
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New HealthEffect:\n");

            Console.WriteLine("Category:");
            _category = Console.ReadLine();
            Console.WriteLine("Description:");
            _description = Console.ReadLine().Trim();

            if (IsInputPossible())
            {
                var purpose = new HealthEffect()
                {
                    Category = _category,
                    Description = _description
                };

                HealthEffectEditor.Add(purpose);
                return;
            }

            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Remove()
    {
        var healthEffect = SelectHealthEffect();
        HealthEffectEditor.Remove(healthEffect);
    }

    public static HealthEffect SelectHealthEffect()
    {
        while (true)
        {
            PrintTable();

            var inputProvider = Console.ReadLine();

            foreach (var provider in _current)
            {
                if (inputProvider == provider.Category) return provider;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    public static List<ObjectId> SelectHealthEffectsId()
    {
        var purposes = SelectHealthEffects();
        return purposes.Select(purpose => purpose.Id).ToList();
    }
    
    public static List<ObjectId> SelectHealthEffectsIdFrom(List<HealthEffect> selectFrom)
    {
        var purposes = SelectHealthEffectsFrom(selectFrom);
        return purposes.Select(purpose => purpose.Id).ToList();
    }

    public static List<HealthEffect> SelectHealthEffects()
    {
        PrintTable();
        return SelectHealthEffectsFrom(_current);
    }
    
    public static List<HealthEffect> SelectHealthEffectsFrom(List<HealthEffect> selectFrom)
    {
        var str = "Current Health effect:\n";
        foreach (var effect in selectFrom)
        {
            str += $"   {effect.Category}\n";
        }
        Console.WriteLine(str);
        
        var healthEffects = new List<HealthEffect>();

        Console.WriteLine("Print for select (, - separator)");
        var printedHealthEffects = Console.ReadLine().Trim().Split(',');

        foreach (var printedEffect in printedHealthEffects)
        {
            foreach (var healthEffect in selectFrom)
            {
                if (printedEffect == healthEffect.Category)
                {
                    healthEffects.Add(healthEffect);
                    break;
                }
            }
        }

        return healthEffects;
    }

    private static bool IsInputPossible()
    {
        return IsInputPossible(_category) & IsInputPossible(_description);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
}