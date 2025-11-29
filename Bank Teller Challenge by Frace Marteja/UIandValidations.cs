using Bank_Teller_Challenge_by_Frace_Marteja;

public class UIandValidations
{
    public static string Title() => @"
                 _______                       __              ________          __  __                     
                |       \                     |  \            |        \        |  \|  \                    
                | $$$$$$$\  ______   _______  | $$   __        \$$$$$$$$______  | $$| $$  ______    ______  
                | $$__/ $$ |      \ |       \ | $$  /  \         | $$  /      \ | $$| $$ /      \  /      \ 
                | $$    $$  \$$$$$$\| $$$$$$$\| $$_/  $$         | $$ |  $$$$$$\| $$| $$|  $$$$$$\|  $$$$$$\
                | $$$$$$$\ /      $$| $$  | $$| $$   $$          | $$ | $$    $$| $$| $$| $$    $$| $$   \$$
                | $$__/ $$|  $$$$$$$| $$  | $$| $$$$$$\          | $$ | $$$$$$$$| $$| $$| $$$$$$$$| $$      
                | $$    $$ \$$    $$| $$  | $$| $$  \$$\         | $$  \$$     \| $$| $$ \$$     \| $$      
                 \$$$$$$$   \$$$$$$$ \$$   \$$ \$$   \$$          \$$   \$$$$$$$ \$$ \$$  \$$$$$$$ \$$      
                                                                                    
                                                                         
";

    public static string CenterText(string text, int width)
    {
        if (text.Length >= width)
            return text.Substring(0, width);

        int leftPadding = (width - text.Length) / 2;
        int rightPadding = width - text.Length - leftPadding;

        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }

    public static decimal ReadDecimal(string message)
    {
        List<string> messages = new List<string>();

        decimal number;
        while (true)
        {
            Console.Write(message);
            if (decimal.TryParse(Console.ReadLine(), out number))
            {
                if (number <= 0)
                {
                    ShowMessage("Invalid Amount. Can't be less than or equal to 0!");
                }
                else
                {
                    return number;
                }
            }
            else
            {
                ShowMessage("Invalid input! Enter numbers only.");
            }
        }
    }

    public static string GetValidAccountName()
    {
        Console.Write("Enter Name of the Account: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            ShowMessage("Name cannot be empty.");
            return null;
        }
        return name;
    }

    public static CustomerAccounts FindAccount(string name)
    {
        var acc = Initializes.customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (acc == null) 
        {
            ShowMessage($"No account found with the name '{name}'.");
            return null;
        }
        return acc;
    }

    public static void DisplayBox(List<string> messages)
    {
        if (messages.Count == 0) return;

        int windowWidth = Console.WindowWidth;
        int boxWidth = windowWidth;

        Console.WriteLine(new string('\u2014', boxWidth));

        foreach (var msg in messages)
        {
            string displayMsg = msg.Length > boxWidth - 4 ? msg.Substring(0, boxWidth - 7) + "..." : msg;
            int leftPadding = (boxWidth - 2 - displayMsg.Length) / 2;
            int rightPadding = boxWidth - 2 - displayMsg.Length - leftPadding;

            Console.WriteLine($"|{new string(' ', leftPadding)}{displayMsg}{new string(' ', rightPadding)}|");
        }

        Console.WriteLine(new string('\u2014', boxWidth));
        Console.WriteLine();
    }

    public static void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(Title());
        DisplayBox(new List<string> { message });
    }

    public static void WaitForKey()
    {
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public static bool AskRepeat(string message)
    {
        Console.Write($"{message} (Y/N): ");
        return Console.ReadLine()?.Trim().ToUpper() == "Y";
    }

    public static bool BackMainMenu()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape)
            {
                return true;
            }
        }
        
        return false;
    }
}