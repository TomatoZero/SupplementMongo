using MongoDB.Bson;
using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public static class NutritionalSupplementDisplay
{
    private static List<NutritionalSupplement> _currentSupplements;

    private static string _name;
    private static string _enum;
    private static string _description;
    private static string _acceptableDailyIntake;

    public static void PrintTable()
    {
        _currentSupplements = NutritionalSupplementEditor.GetTableInclude();

        var str = "Supplement:\n";
        foreach (var supplement in _currentSupplements)
        {
            Console.WriteLine(supplement);
        }

        Console.WriteLine(str);
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New Nutritional Supplement:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();

            Console.WriteLine("Enum:");
            _enum = Console.ReadLine().Trim();

            Console.WriteLine("Description:");
            _description = Console.ReadLine().Trim();

            Console.WriteLine("Acceptable daily intake (,):");
            _acceptableDailyIntake = Console.ReadLine().Trim();

            Console.WriteLine("Select Health Effect:");
            var healthEffect = HealthEffectDisplay.SelectHealthEffectsId();

            Console.WriteLine("Select Purpose:");
            var purpose = PurposeDisplay.SelectPurposeId();

            if (IsInputPossible())
            {
                if (Decimal.TryParse(_acceptableDailyIntake, out var daily))
                {
                    var provider = new NutritionalSupplement()
                    {
                        Name = _name,
                        ENum = _enum,
                        Description = _description,
                        AcceptableDailyIntake = daily,
                        HealthEffectsId = healthEffect,
                        PurposesId = purpose
                    };

                    NutritionalSupplementEditor.Add(provider);
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Update()
    {
        var supplement = SelectSupplement();

        PrintValue(supplement);

        Console.WriteLine($"Update Nutritional Supplement:\n");

        Console.WriteLine("Name ('-' - same):");
        _name = Console.ReadLine();

        Console.WriteLine("Enum ('-' - same):");
        _enum = Console.ReadLine().Trim();

        Console.WriteLine("Description ('-' - same):");
        _description = Console.ReadLine().Trim();

        Console.WriteLine("Acceptable daily intake (,) ('-' - same):");
        _acceptableDailyIntake = Console.ReadLine().Trim();
        
        if (IsInputPossible())
        {
            if (Decimal.TryParse(_acceptableDailyIntake, out var daily))
            {
                supplement.Name = _name;
                supplement.ENum = _enum;
                supplement.Description = _description;
                supplement.AcceptableDailyIntake = daily;

                NutritionalSupplementEditor.Update(supplement);
                return;
            }
        }
    }

    public static void UpdateHealthEffect()
    {
        var supplement = SelectSupplement();

        Console.WriteLine("Change Health Effects ('-' - same, '+' - add, '--', remove):");
        var healthEffectChoise = Console.ReadLine().Trim();
        var healthEffect = new List<ObjectId>();
        var current = supplement.HealthEffectsId.ToList();

        if (healthEffectChoise != "-")
        {
            if (healthEffectChoise == "+")
            {
                Console.WriteLine("Select Health Effect:");
                healthEffect = HealthEffectDisplay.SelectHealthEffectsId();

                foreach (var effect in healthEffect)
                {
                    if (!current.Exists(id => id == effect))
                    {
                        current.Add(effect);
                    }
                }
            }
            else
            {
                var healthEffectToRemove =
                    HealthEffectDisplay.SelectHealthEffectsIdFrom(supplement.HealthEffects.ToList());

                foreach (var effect in healthEffectToRemove) current.Remove(effect);
            }

            supplement.HealthEffectsId = current;
            NutritionalSupplementEditor.Update(supplement);
        }
    }

    private static NutritionalSupplement SelectSupplement()
    {
        while (true)
        {
            PrintTable();

            var inputSupplement = Console.ReadLine();

            foreach (var supplement in _currentSupplements)
            {
                if (inputSupplement == supplement.Name) return supplement;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    private static void PrintValue(NutritionalSupplement supplement)
    {
        var str = $"Current value:\n" +
                  $"    {supplement.Name}\n" +
                  $"    {supplement.ENum}\n" +
                  $"    {supplement.Description}\n" +
                  $"    {supplement.AcceptableDailyIntake}\n";

        str += $"   Health effects:\n";

        foreach (var healthEffect in supplement.HealthEffects)
        {
            str += $"       {healthEffect}\n";
        }

        str += $"   Purpose\n";

        foreach (var purpose in supplement.Purposes)
        {
            str += $"       {purpose}\n";
        }

        Console.WriteLine(str);
    }

    private static bool IsInputPossible()
    {
        return IsInputPossible(_name) & IsInputPossible(_enum) & IsInputPossible(_description) &
               IsInputPossible(_acceptableDailyIntake);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
}