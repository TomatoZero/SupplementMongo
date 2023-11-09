using SupplementsMongo.Display;

while (true)
{
    Console.WriteLine($"Choose Action (Number):\n" +
                      $"1. Show brief table\n" +
                      $"2. Show full table\n" +
                      $"3. Add value\n" +
                      $"4. Remove value\n" +
                      $"5. Update value");
    
    var input = Console.ReadLine().Trim();
    switch (input)
    {
        case "1": ShowBriefTable(); break;
        case "2": ShowFullTable(); break;
        default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
    }
}

// Console.WriteLine("Choose Table (Number):\n" +
//                   " 1. Provider\n" +
//                   " 2. Product\n" +
//                   " 3. Ingredient\n" +
//                   " 4. Nutritional Supplement\n" +
//                   " 5. Health Effect\n" +
//                   " 6. Purpose");
//
// var input = Console.ReadLine();
//
// switch (input)
// {
//     case "1":
//         ProviderEditors.PrintTable();
//         break;
//     case "2":
//         break;
//     case "3":
//         break;
//     case "4":
//         break;
//     case "5":
//         break;
//     case "6":
//         break;
//     default:
//         Console.WriteLine("Try again");
//         break;
// }

return;

void ShowBriefTable()
{
    Console.Clear();
    var input = SelectTable();

    switch(input)
    {
        case "1": ProviderDisplay.PrintTable(); break;
        case "2": ProductDisplay.PrintTable(); break;
        // case "3": ProviderDisplay.PrintTable(); break;
        case "4": NutritionalSupplementDisplay.PrintTable(); break;
        case "5": HealthEffectDisplay.PrintTable(); break;
        case "6": ProductDisplay.PrintTable(); break;
        default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
    }
}

void ShowFullTable()
{
    Console.Clear();
    var input = SelectTable();

    switch(input)
    {
        case "1": ProviderDisplay.PrintTable(); break;
        case "2": ProductDisplay.PrintFullTable(); break;
        // case "3": ProviderDisplay.PrintTable(); break;
        case "4": NutritionalSupplementDisplay.PrintFullTable(); break;
        case "5": HealthEffectDisplay.PrintTable(); break;
        case "6": ProductDisplay.PrintTable(); break;
        default: Console.Clear(); Console.WriteLine("Wrong input try again:"); break;
    }
}


string SelectTable()
{
    Console.WriteLine("Choose Table (Number):\n" +
                      " 1. Provider\n" +
                      " 2. Product\n" +
                      // " 3. Ingredient\n" +
                      " 4. Nutritional Supplement\n" +
                      " 5. Health Effect\n" +
                      " 6. Purpose");

    var input = Console.ReadLine().Trim();
    return input;
}
