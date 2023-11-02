using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public class ProviderDisplay
{
    private static List<Provider> _currentProviders;

    public static void PrintTable()
    {
        _currentProviders = ProviderEditors.GetTable();

        var str = "Providers:\n";
        foreach (var provider in _currentProviders)
        {
            str += $"   {provider.Name}";
        }

        Console.WriteLine(str);
    }

    public static Provider GetProviderFromList()
    {
        Console.WriteLine($"Select provider:\n");
        var provider = SelectProvider();
        return provider;
    }

    public static void Update()
    {
        var provider = SelectProvider();

        Console.WriteLine($"Current value:\n" +
                          $"    {provider.Name}\n" +
                          $"    {provider.RegistrationCountry}\n");

        Console.WriteLine("New Name ('-' - same):");
        var newName = Console.ReadLine();
        Console.WriteLine("New RegistrationCountry ('-' - same):");
        var newRegistrationCountry = Console.ReadLine();

        if (newName != "-") provider.Name = newName;
        if (newRegistrationCountry != "-") provider.RegistrationCountry = newRegistrationCountry;

        var newProviderValue = new Provider()
        {
            Name = newName,
            RegistrationCountry = newRegistrationCountry
        };

        ProviderEditors.Update(provider, newProviderValue);
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New provider:\n");

            Console.WriteLine("Name:");
            var newName = Console.ReadLine();
            Console.WriteLine("RegistrationCountry:");
            var newRegistrationCountry = Console.ReadLine();

            if (IsInputPossible(newName, newRegistrationCountry))
            {
                var provider = new Provider
                {
                    Name = newName,
                    RegistrationCountry = newRegistrationCountry
                };

                ProviderEditors.Add(provider);
                return;
            }

            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Remove()
    {
        var provider = SelectProvider();
        ProviderEditors.Remove(provider);
    }

    private static Provider SelectProvider()
    {
        while (true)
        {
            PrintTable();

            var inputProvider = Console.ReadLine();

            foreach (var provider in _currentProviders)
            {
                if (inputProvider == provider.Name) return provider;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    private static bool IsInputPossible(string name, string country)
    {
        return !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(country) &&
               !string.IsNullOrWhiteSpace(country);
    }
}