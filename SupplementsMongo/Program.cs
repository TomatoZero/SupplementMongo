using SupplementsMongo.Display;

while (true)
{
    Console.WriteLine("Print something to start");
    Console.ReadLine();
    
    Console.Clear();
    Console.WriteLine($"Choose Action (Number):\n" +
                      $"1. Show brief table\n" +
                      $"2. Show full table\n" +
                      $"3. Add value\n" +
                      $"4. Remove value\n" +
                      $"5. Update value\n" +
                      $"6. Update reference");
    
    var input = Console.ReadLine().Trim();
    Console.Clear();
    switch (input)
    {
        case "1": ShowBriefTable(); break;
        case "2": ShowFullTable(); break;
        case "3": AddValue(); break;
        case "4": RemoveValue(); break;
        case "5": UpdateValue(); break;
        case "6": UpdateReference(); break;
        default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
    }
}

return;

void ShowBriefTable()
{
    while (true)
    {
        Console.Clear();
        var input = SelectTable();

        switch(input)
        {
            case "1": ProviderDisplay.PrintTable(); return;
            case "2": ProductDisplay.PrintTable(); return;
            case "3": IngredientDisplay.PrintTable(); return;
            case "4": NutritionalSupplementDisplay.PrintTable(); return;
            case "5": HealthEffectDisplay.PrintTable(); return;
            case "6": PurposeDisplay.PrintTable(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
    
}

void ShowFullTable()
{
    while (true)
    {
        Console.Clear();
        var input = SelectTable();

        switch(input)
        {
            case "1": ProviderDisplay.PrintTable(); return;
            case "2": ProductDisplay.PrintFullTable(); return;
            case "3": IngredientDisplay.PrintFullTable(); return;
            case "4": NutritionalSupplementDisplay.PrintFullTable(); return;
            case "5": HealthEffectDisplay.PrintTable(); return;
            case "6": PurposeDisplay.PrintTable(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
    
}

void AddValue()
{
    while (true)
    {
        Console.Clear();
        var input = SelectTable();
        
        switch(input)
        {
            case "1": ProviderDisplay.Add(); return;
            case "2": ProductDisplay.Add(); return;
            case "3": IngredientDisplay.Add(); return;
            case "4": NutritionalSupplementDisplay.Add(); return;
            case "5": HealthEffectDisplay.Add(); return;
            case "6": PurposeDisplay.Add(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
    
}

void RemoveValue()
{
    while (true)
    {
        Console.Clear();
        var input = SelectTable();
        
        switch(input)
        {
            case "1": ProviderDisplay.Remove(); return;
            case "2": ProductDisplay.Remove(); return;
            case "3": IngredientDisplay.Remove(); return;
            case "4": NutritionalSupplementDisplay.Remove(); return;
            case "5": HealthEffectDisplay.Remove(); return;
            case "6": PurposeDisplay.Remove(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
    
}

void UpdateValue()
{
    while (true)
    {
        Console.Clear();
        var input = SelectTable();
        
        switch(input)
        {
            case "1": ProviderDisplay.Update(); return;
            case "2": ProductDisplay.Update(); return;
            case "3": IngredientDisplay.Update(); return;
            case "4": NutritionalSupplementDisplay.Update(); return;
            case "5": HealthEffectDisplay.Update(); return;
            case "6": PurposeDisplay.Update(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
}

void UpdateReference()
{
    while (true)
    {
        var input = SelectTableSmall();
        
        switch(input)
        {
            case "1": ProductDisplay.UpdateReference(); return;
            case "2": IngredientDisplay.UpdateNutritionalSupplements(); return;
            case "3": NutritionalSupplementDisplay.UpdateReference(); return;
            default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
        }
    }
}

string SelectTable()
{
    Console.WriteLine("Choose Table (Number):\n" +
                      " 1. Provider\n" +
                      " 2. Product\n" +
                      " 3. Ingredient\n" +
                      " 4. Nutritional Supplement\n" +
                      " 5. Health Effect\n" +
                      " 6. Purpose");

    var input = Console.ReadLine().Trim();
    Console.Clear();
    return input;
}

string SelectTableSmall()
{
    Console.WriteLine("Choose Table (Number):\n" +
                      " 1. Product\n" +
                      " 2. Ingredient\n" +
                      " 3. Nutritional Supplement\n");

    var input = Console.ReadLine().Trim();
    Console.Clear();
    return input;
}
