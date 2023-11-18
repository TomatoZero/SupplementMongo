using MongoDB.Bson;
using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public class PurposeDisplay
{
    private static List<Purpose> _current;

    private static string _name;
    private static string _description;
    
    public static void PrintTable()
    {
        _current = PurposeEditor.GetTable();

        var str = "Purposes:\n";
        foreach (var purpose in _current)
        {
            str += $"   {purpose.Name}\n";
        }

        Console.WriteLine(str);
    }
    
    public static void Update()
    {
        var purpose = SelectProduct();

        Console.WriteLine($"Current value:\n" +
                          $"    {purpose.Name}\n" +
                          $"    {purpose.Description}");

        Console.WriteLine($"Update Purpose:\n");
        
        Console.WriteLine("Name ('-' - same):");
        var newName = Console.ReadLine();
        
        Console.WriteLine("Description ('-' - same):");
        var description = Console.ReadLine().Trim();

        if (IsInputPossible())
        {
            CheckInput(purpose);
            
            purpose.Name = newName;
            purpose.Description = description;
            PurposeEditor.Update(purpose);
        }
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New purpose:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();
            Console.WriteLine("Description:");
            _description = Console.ReadLine().Trim();

            if (IsInputPossible())
            {
                var purpose = new Purpose()
                {
                    Name = _name,
                    Description = _description
                };
                
                PurposeEditor.Add(purpose);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Remove()
    {
        var provider = SelectProduct();
        PurposeEditor.Remove(provider);
    }

    public static List<ObjectId> SelectPurposesId()
    {
        var purposes = SelectPurposes();
        return purposes.Select(purpose => purpose.Id).ToList();
    }

    public static List<ObjectId> SelectPurposeIdFrom(List<Purpose> from)
    {
        var purposes = SelectPurposeFrom(from);
        return purposes.Select(purpose => purpose.Id).ToList();
    }
    
    public static List<Purpose> SelectPurposes()
    {
        _current = PurposeEditor.GetTable();
        return SelectPurposeFrom(_current);
    }

    public static List<Purpose> SelectPurposeFrom(List<Purpose> selectFrom)
    {
        var str = "Select from:\n";
        foreach (var purpose in selectFrom)
        {
            str += $"   {purpose.Name}\n";
        }
        Console.WriteLine(str);
        
        var healthEffects = new List<Purpose>();

        Console.WriteLine("Print for select (, - separator)");
        var printedHealthEffects = Console.ReadLine().Trim().Split(',');

        foreach (var printedEffect in printedHealthEffects)
        {
            foreach (var healthEffect in selectFrom)
            {
                if (printedEffect.Trim() == healthEffect.Name)
                {
                    healthEffects.Add(healthEffect);
                    break;
                }
            }
        }

        return healthEffects;
    }
    
    private static Purpose SelectProduct()
    {
        while (true)
        {
            PrintTable();

            var inputProvider = Console.ReadLine();

            foreach (var provider in _current)
            {
                if (inputProvider == provider.Name) return provider;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    private static bool IsInputPossible()
    {
        return IsInputPossible(_name) & IsInputPossible(_description);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
    
    private static void CheckInput(Purpose supplement)
    {
        if (_name == "-") _name = supplement.Name;
        if (_description == "-") _description = supplement.Description;
    }
}