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
        foreach (var purpose in _current)
        {
            str += $"   {purpose.Category}\n";
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
    
    private static HealthEffect SelectHealthEffect()
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

    private static bool IsInputPossible()
    {
        return IsInputPossible(_category) & IsInputPossible(_description);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
}