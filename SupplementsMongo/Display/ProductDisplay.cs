using MongoDB.Bson;
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

        if (_currentProduct.Count == 0)
        {
            Console.WriteLine("Table is empty");
            return;
        }
        
        var str = "Products:\n";
        foreach (var product in _currentProduct)
        {
            str += $"   {product.Name}\n";
        }

        Console.WriteLine(str);
    }

    public static void PrintFullTable()
    {
        _currentProduct = ProductEditor.GetTable();

        if (_currentProduct.Count == 0)
        {
            Console.WriteLine("Table is empty");
            return;
        }
        
        Console.WriteLine("Products:");
        foreach (var product in _currentProduct)
        {
            PrintValue(product);
            Console.WriteLine("---------------------------------");
        }
    }
    
    public static void Update()
    {
        var product = SelectProduct();
        
        Console.WriteLine($"Current value:\n" +
                          $"    {product.Name}\n" +
                          $"    {ProviderEditors.GetById(product.ProviderId).Name}" +
                          $"    {product.ManufacturingDate}\n" +
                          $"    {product.ExpirationDate}\n");

        Console.WriteLine("New Name ('-' - same):");
        var newName = Console.ReadLine();
        
        Console.WriteLine("ManufacturingDate ('-' - same):");
        var manufacturingDate = Console.ReadLine().Trim();
        Console.WriteLine("ExpirationDate ('-' - same):");
        var expirationDate = Console.ReadLine().Trim();
        
        if (newName != "-") product.Name = newName;
        if (manufacturingDate != "-") product.ManufacturingDate = DateTime.Parse(manufacturingDate);
        if (expirationDate != "-") product.ExpirationDate = DateTime.Parse(expirationDate);
        
        ProductEditor.Update(product);
    }

    public static void UpdateReference()
    {
        Console.WriteLine($"Update\n" +
                          $"1. Provider\n" +
                          $"2. Ingredients");

        var input = Console.ReadLine().Trim();

        switch (input)
        {
            case "1": UpdateProvider(); break;
            case "2": UpdateIngredients(); break;
            default: break;
        }
    }
    
    public static void UpdateProvider()
    {
        var product = SelectProduct();
        
        Console.WriteLine("Change Provider ('-' - same, '+' - change):");
        var providerChoice = Console.ReadLine().Trim();

        if (providerChoice == "+")
        {
            Console.WriteLine("Select provider");
            var provider = ProviderDisplay.SelectProvider().Id;
            product.ProviderId = provider;
            ProductEditor.Update(product);
        }
    }

    public static void UpdateIngredients()
    {
        var product = SelectProduct();
        
        Console.WriteLine("Change Ingredients ('-' - same, '+' - add, '--', remove):");
        var ingredientsChoice = Console.ReadLine().Trim();
        var ingredients = new List<ObjectId>();
        var current = product.IngredientsId.ToList();

        if (ingredientsChoice != "-")
        {
            if (ingredientsChoice == "+")
            {
                Console.WriteLine("Select Ingredients:");
                ingredients = IngredientDisplay.SelectIngredientId();

                foreach (var objectId in ingredients)
                {
                    if (!current.Exists(id => id == objectId))
                    {
                        current.Add(objectId);
                    }
                }
            }
            else
            {
                var healthEffectToRemove =
                    IngredientDisplay.SelectIngredientsIdFrom(product.Ingredients.ToList());

                foreach (var effect in healthEffectToRemove) current.Remove(effect);
            }

            product.IngredientsId = current;
            ProductEditor.Update(product);
        }
    }
    
    public static void Add()
    {
        while (true)
        {
            Console.WriteLine($"New product:\n");

            Console.WriteLine("Name:");
            _name = Console.ReadLine();
            var providerId = ProviderDisplay.GetProviderFromList().Id;

            Console.WriteLine("ManufacturingDate (yyyy-mm-dd):");
            _manufactoryDate = Console.ReadLine().Trim();
            Console.WriteLine("ExpirationDate (yyyy-mm-dd):");
            _expirationDate = Console.ReadLine().Trim();

            if (IsInputPossible())
            {
                if (DateTime.TryParse(_manufactoryDate, out var manufacturing) &&
                    DateTime.TryParse(_expirationDate, out var expiration))
                {
                    var provider = new Product()
                    {
                        Name = _name,
                        ProviderId = providerId,
                        ManufacturingDate = manufacturing,
                        ExpirationDate = expiration
                    };

                    ProductEditor.Add(provider);
                    return;
                }
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
    
    private static void PrintValue(Product product)
    {
        var str = $"{product.Name}\n" +
                  $"{product.ManufacturingDate}\n" +
                  $"{product.ExpirationDate}\n" +
                  $"{product.Provider.Name}\n";

        str += $"Ingredients:\n";

        foreach (var ingredient in product.Ingredients)
        {
            str += $"{ingredient}\n";
        }

        Console.WriteLine(str);
    }
}