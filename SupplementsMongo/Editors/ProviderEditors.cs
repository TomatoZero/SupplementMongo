using NutritionalSupplements.Data;

namespace SupplementsMongo.Editors;

public static class ProviderEditors
{
    private static ProviderRepository _repository;

    private static List<Provider> _currentProviders;

    static ProviderEditors()
    {
        _repository = new ProviderRepository();
    }

    public static void PrintTable()
    {
        _currentProviders = _repository.GetAll().ToList();

        var str = "Providers:\n";
        foreach (var provider in _currentProviders)
        {
            str += $"   {provider.Name}";
        }

        Console.WriteLine(str);
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
        
        _repository.Update(provider);
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
}