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

        Console.WriteLine("New Name ('-' - same):");
        var newName = Console.ReadLine();
        Console.WriteLine("Description ('-' - same):");
        var description = Console.ReadLine().Trim();
        
        if (newName != "-") purpose.Name = newName;
        if (description != "-") purpose.Description = description;
        
        PurposeEditor.Update(purpose);
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

    public static List<ObjectId> SelectPurposeId()
    {
        var purposes = SelectPurposes();
        return purposes.Select(purpose => purpose.Id).ToList();
    }

    public static List<Purpose> SelectPurposes()
    {
        var healthEffects = new List<Purpose>();

        PrintTable();

        var printedPurposes = Console.ReadLine().Trim().Split();

        foreach (var printedPurpose in printedPurposes)
        {
            foreach (var purpose in _current)
            {
                if (printedPurpose == purpose.Name)
                {
                    healthEffects.Add(purpose);
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
}