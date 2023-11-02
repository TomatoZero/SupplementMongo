using NutritionalSupplements.Data;
using SupplementsMongo.Editors;

namespace SupplementsMongo.Display;

public static class ProductDisplay
{
    private static List<Product> _currentProduct;

    private static string _name;
    private static string _manufactoryDate;
    private static string _expirationDate;
    
    public static void PrintTable()
    {
        _currentProduct = ProductEditor.GetTable();

        var str = "Products:\n";
        foreach (var product in _currentProduct)
        {
            str += $"   {product.Name}";
        }

        Console.WriteLine(str);
    }
    
    public static void Update()
    {
        var product = SelectProduct();
        
        Console.WriteLine($"Current value:\n" +
                          $"    {product.Name}\n" +
                          $"    {ProviderEditors.GetById(product.ProviderId)}" +
                          $"    {product.ManufacturingDate}\n" +
                          $"    {product.ExpirationDate}\n" +
                          $"    Print Ingredients");

        //TODO: print ingredients
        
        Console.WriteLine("New Name ('-' - same):");
        var newName = Console.ReadLine();
        Console.WriteLine("Select New Provider ('-' - no, '+' - yes):");
        var change = Console.ReadLine().Trim();

        if (change == "+") product.ProviderId = ProviderDisplay.GetProviderFromList().Id;

        Console.WriteLine("ManufacturingDate ('-' - same):");
        var manufacturingDate = Console.ReadLine().Trim();
        Console.WriteLine("ExpirationDate ('-' - same):");
        var expirationDate = Console.ReadLine().Trim();
        
        //TODO: ingredients
        
        if (newName != "-") product.Name = newName;
        if (manufacturingDate != "-") product.ManufacturingDate = DateTime.Parse(manufacturingDate);
        if (expirationDate != "-") product.ExpirationDate = DateTime.Parse(expirationDate);
        
        ProductEditor.Update(product);
    }

    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New product:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();
            var providerId = ProviderDisplay.GetProviderFromList().Id;

            Console.WriteLine("ManufacturingDate ('-' - same):");
            _manufactoryDate = Console.ReadLine().Trim();
            Console.WriteLine("ExpirationDate ('-' - same):");
            _expirationDate = Console.ReadLine().Trim();

            if (IsInputPossible())
            {
                var provider = new Product()
                {
                    Name = _name,
                    ProviderId = providerId,
                    ManufacturingDate = DateTime.Parse(_manufactoryDate),
                    ExpirationDate = DateTime.Parse(_expirationDate)
                };
                
                ProductEditor.Add(provider);
                return;
            }
            
            Console.Clear();
            Console.WriteLine("Input error. Try again");
        }
    }

    public static void Remove()
    {
        var provider = SelectProduct();
        ProductEditor.Remove(provider);
    }
    
    private static Product SelectProduct()
    {
        while (true)
        {
            PrintTable();

            var inputProvider = Console.ReadLine();

            foreach (var provider in _currentProduct)
            {
                if (inputProvider == provider.Name) return provider;
            }

            Console.Clear();
            Console.WriteLine($"Error: Wrong Input. Select again");
        }
    }

    private static bool IsInputPossible()
    {
        return IsInputPossible(_name) & IsInputPossible(_manufactoryDate) & IsInputPossible(_expirationDate);
    }

    private static bool IsInputPossible(string str)
    {
        return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
    }
}