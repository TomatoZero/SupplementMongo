using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public class ProviderDisplay
{
    private static List<Provider> _currentProviders;

    private static string _name;
    private static string _registrationCountry;
    
    public static void PrintTable()
    {
        _currentProviders = ProviderEditors.GetTable();

        var str = "Providers:\n";
        foreach (var provider in _currentProviders)
        {
            str += $"   {provider.Name}\n";
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

        if(IsInputPossible())
        {
            CheckInput(provider);
            
            provider.Name = newName;
            provider.RegistrationCountry = newRegistrationCountry;
            
            ProviderEditors.Update(provider);
        }
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New provider:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();
            Console.WriteLine("RegistrationCountry:");
            _registrationCountry = Console.ReadLine();

            if (IsInputPossible())
            {
                var provider = new Provider
                {
                    Name = _name,
                    RegistrationCountry = _registrationCountry
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

    public static Provider SelectProvider()
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

    
    private static bool IsInputPossible()
    {
        return IsInputPossible(_name) & IsInputPossible(_registrationCountry);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
    
    private static void CheckInput(Provider provider)
    {
        if (_name == "-") _name = provider.Name;
        if (_registrationCountry == "-") _registrationCountry = provider.RegistrationCountry;
    }
}