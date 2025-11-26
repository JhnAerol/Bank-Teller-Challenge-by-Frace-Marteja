using System.Globalization;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public enum TransactionType
{
    CreateAccount,
    CheckBalance,
    Deposit,
    Withdraw,
    ViewTransactions,
    ListOfAccounts,
    Exit
}

public record Transaction(DateTime timeStamp, TransactionType transactionType, decimal amount);
public record TellerTransaction(DateTime timeStamp, string actions, decimal amount, string accountName);

public class Initializes
{
    public static List<CustomerAccounts> customers = new List<CustomerAccounts>();
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        List<string> messages = new List<string>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine(Title());
            if (ShowLoginForm() == true)
            {
                Navi();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(Title());
                messages.Add("Access Denied!!");
                BankTransactions.DisplayBox(messages);
                messages.Clear();
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    static bool ShowLoginForm()
    {
        Teller teller = new Teller();

        Console.Write("Enter UserName: ");
        string userName = Console.ReadLine();

        Console.Write("Enter PIN: ");
        string pin = HidePassword();

        if (userName == teller.Name && int.TryParse(pin, out int correctpin))
        {
            if (correctpin == teller.Pin)
            {
                return true;
            }
        }

        return false;
    }

    static string HidePassword()
    {
        string password = "";
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }

        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    static void Navi()
    {
        TransactionType transaction = TransactionType.CreateAccount;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(Title());
            foreach (TransactionType transactionArrow in Enum.GetValues(typeof(TransactionType)))
            {
                if (transactionArrow == transaction)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("> " + transactionArrow);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + transactionArrow);
                }
            }

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                if (transaction > 0)
                {
                    transaction--;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                if ((int)transaction < Enum.GetValues(typeof(TransactionType)).Length - 1)
                    transaction++;
            }
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                Console.WriteLine(Title());
                Menu(transaction.ToString());
            }
        }
    }

    static void Menu(string transact)
    {
        switch (transact)
        {
            case "CreateAccount":
                new BankTransactions().CreateAccount();
                break;
            case "CheckBalance":
                new BankTransactions().CheckBalance();
                break;
            case "Deposit":
                new BankTransactions().Deposit();
                break;
            case "Withdraw":
                new BankTransactions().Withdraw();
                break;
            case "ViewTransactions":
                new BankTransactions().ViewTransactions();
                break;
            case "ListOfAccounts":
                new BankTransactions().ListOfAccounts();
                break;
            case "Exit":
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }

    public static string Title()
    {
        return @"
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
    }
}

public class Teller
{
    public string Name = "Aerol";
    public int Pin = 12345;
    public static List<TellerTransaction> Transactions { get; set; } = new List<TellerTransaction>();

    public void AddTransaction(TellerTransaction transact)
    {
        Transactions.Add(transact);
    }
}

public class CustomerAccounts
{
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public CustomerAccounts(string _name, decimal _balance)
    {
        Name = _name;
        Balance = _balance;
    }

    public void AddTransaction(Transaction transact)
    {
        Transactions.Add(transact);
    }
}

public class BankTransactions
{
    public void CreateAccount()
    {
        while (true)
        {
            List<string> messages = new List<string>();
            
            Console.Clear();
            Console.WriteLine(Program.Title());
            Console.WriteLine("You Select: Create an Account");
            Console.WriteLine();
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            decimal amount = ReadDecimal("Enter First Deposit: ");

            if (amount <= 0)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Invalid Amount entered!");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            bool exist = Initializes.customers.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (exist)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add($"Account Name is Already exist.");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                continue; 
            }

            CustomerAccounts acc = new CustomerAccounts(name, amount);
            Teller teller = new Teller();

            var tellerTransact = new TellerTransaction(DateTime.Now, "Creating an Account", amount, name);
            var transact = new Transaction(DateTime.Now, TransactionType.Deposit, amount);

            teller.AddTransaction(tellerTransact);
            acc.AddTransaction(transact);

            Initializes.customers.Add(acc);

            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add("Created Successfully!");
            DisplayBox(messages);

            Console.Write("Do you want to create another account? (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (response != "Y")
            {
                break;
            }
        }
    }

    public void CheckBalance()
    {
        List<string> messages = new List<string>();

        Console.Clear();
        Console.WriteLine(Program.Title());
        Console.WriteLine("You Select: Check Balance");
        Console.WriteLine();
        Console.Write("Enter Name of the Account: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add("Name cannot be empty.");
            DisplayBox(messages);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        CustomerAccounts? acc = Initializes.customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (acc == null)
        {
            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add($"No account found with the name '{name}'.");
            DisplayBox(messages);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        decimal amount = acc.Balance;

        Console.Clear();
        Console.WriteLine(Program.Title());
        messages.Add($"Current Amount for {acc.Name}: ₱{amount}");
        DisplayBox(messages);
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void ListOfAccounts()
    {
        List<string> messages = new List<string>();

        Console.Clear();
        Console.WriteLine(Program.Title());

        Console.WriteLine("You Select: List of Account");
        Console.WriteLine();

        if (Initializes.customers.Count > 0)
        {
            foreach (var list in Initializes.customers)
            {
                messages.Add($"{list.Name}");
            }
        }
        else
        {
            messages.Add("No Records");
        }

        DisplayBox(messages);
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void Deposit()
    {
        while (true)
        {
            List<string> messages = new List<string>();

            Console.Clear();
            Console.WriteLine(Program.Title());
            Console.WriteLine("You Select: Deposit");
            Console.WriteLine();
            Console.Write("Enter Name of the Account: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Name cannot be empty.");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            CustomerAccounts acc = Initializes.customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (acc == null)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add($"No account found with the name '{name}'.");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            decimal deposit = ReadDecimal("Enter Deposit Amount: ");

            if (deposit <= 0)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Invalid Amount entered!");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Teller teller = new Teller();
            teller.AddTransaction(new TellerTransaction(DateTime.Now, "Deposit to an Account", deposit, name));
            acc.AddTransaction(new Transaction(DateTime.Now, TransactionType.Deposit, deposit));

            acc.Balance += deposit;

            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add($"Deposit successful! Current Balance for {acc.Name}: ₱{acc.Balance}");
            DisplayBox(messages);

            Console.Write("Do you want to make another deposit? (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (response != "Y")
            {
                break;
            }
        }
    }

    public void Withdraw()
    {
        while (true)
        {
            List<string> messages = new List<string>();

            Console.Clear();
            Console.WriteLine(Program.Title());
            Console.WriteLine("You Select: Withdraw");
            Console.WriteLine();
            Console.Write("Enter Name of the Account: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Name cannot be empty.");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            CustomerAccounts acc = Initializes.customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (acc == null)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add($"No account found with the name '{name}'.");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            decimal withdraw = ReadDecimal("Enter Withdraw Amount: ");

            if (withdraw <= 0)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Invalid Amount entered!");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            if (withdraw > acc.Balance)
            {
                Console.Clear();
                Console.WriteLine(Program.Title());
                messages.Add("Insufficient funds!");
                DisplayBox(messages);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Teller teller = new Teller();
            teller.AddTransaction(new TellerTransaction(DateTime.Now, "Withdraw from an Account", withdraw, name));
            acc.AddTransaction(new Transaction(DateTime.Now, TransactionType.Withdraw, withdraw));

            acc.Balance -= withdraw;

            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add($"Withdrawal successful! Current Balance for {acc.Name}: ₱{acc.Balance}");
            DisplayBox(messages);

            Console.Write("Do you want to make another withdrawal? (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (response != "Y")
            {
                break;
            }
        }
    }

    public void ViewTransactions()
    {
        Console.Clear();
        Console.WriteLine(Program.Title());
        Console.WriteLine("You Select: View Transaction");
        Console.WriteLine();

        Console.WriteLine("1. Teller Transaction");
        Console.WriteLine("2. Customer Transaction");
        Console.Write("Select option: ");
        string pick = Console.ReadLine();

        switch (pick)
        {
            case "1":
                Console.Clear();
                Console.WriteLine(Program.Title());
                if (Teller.Transactions.Count == 0)
                {
                    List<string> noTellerMsg = new List<string> { "No teller transactions found." };
                    DisplayBox(noTellerMsg);
                }
                else
                {
                    Console.WriteLine(CenterText($"Transactions for Teller", 125));
                    Console.WriteLine();
                    DisplayTellerTransactionTable(Teller.Transactions);
                }
                break;
            case "2":
                Console.Clear();
                Console.WriteLine(Program.Title());
                Console.Write("Enter Name of the Account: ");
                string? name = Console.ReadLine();

                Console.Clear();
                Console.WriteLine(Program.Title());

                if (string.IsNullOrWhiteSpace(name))
                {
                    List<string> emptyMsg = new List<string> { "Name cannot be empty." };
                    DisplayBox(emptyMsg);
                }
                else
                {
                    CustomerAccounts acc = Initializes.customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (acc == null)
                    {
                        List<string> notFoundMsg = new List<string> { $"No account found with the name '{name}'." };
                        DisplayBox(notFoundMsg);
                    }
                    else if (acc.Transactions.Count == 0)
                    {
                        List<string> noTransMsg = new List<string> { "No transactions found for this account." };
                        DisplayBox(noTransMsg);
                    }
                    else
                    {

                        Console.WriteLine(CenterText($"Transactions for {name}", 125));
                        Console.WriteLine();
                        DisplayCustomerTransactionTable(acc.Transactions, acc.Balance);
                    }
                }
                break;
            default:
                Console.Clear();
                Console.WriteLine(Program.Title());
                List<string> invalidMsg = new List<string> { "Invalid option selected." };
                DisplayBox(invalidMsg);
                break;
        }

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    private void DisplayTellerTransactionTable(List<TellerTransaction> transactions)
    {
        int count = transactions.Count;
        Console.WriteLine($"All Transactions (Count: {count})");
        Console.WriteLine();

        int dateTimeWidth = 35;
        int typeWidth = 25;
        int amountWidth = 20;
        int accountWidth = 27;

        string horizontalLine = new string('\u2014', Console.WindowWidth);
        Console.WriteLine(horizontalLine);

        Console.WriteLine($"| {CenterText("Date Time", dateTimeWidth)} | {CenterText("Type", typeWidth)} | {CenterText("Amount", amountWidth)} | {CenterText("Account Name", accountWidth)} |");
        Console.WriteLine(horizontalLine);

        foreach (var transact in transactions)
        {
            string dateTime = transact.timeStamp.ToString("MM/dd/yyyy hh:mm:ss tt");
            string type = transact.actions;
            string amount = $"₱{transact.amount:N2}";
            string accountName = transact.accountName;

            Console.WriteLine($"| {CenterText(dateTime, dateTimeWidth)} | {CenterText(type, typeWidth)} | {CenterText(amount, amountWidth)} | {CenterText(accountName, accountWidth)} |");
        }

        Console.WriteLine(horizontalLine);
        Console.WriteLine();
    }

    private void DisplayCustomerTransactionTable(List<Transaction> transactions, decimal balance)
    {
        int count = transactions.Count;
        Console.WriteLine(CenterText($"(Balance: ₱{balance:N2})", 125));
        Console.WriteLine();

        int dateTimeWidth = 40;
        int typeWidth = 40;
        int amountWidth = 30;

        string horizontalLine = new string('\u2014', Console.WindowWidth);
        Console.WriteLine(horizontalLine);
        
        Console.WriteLine($"| {CenterText("Date Time", dateTimeWidth)} | {CenterText("Type", typeWidth)} | {CenterText("Amount", amountWidth)} |");
        Console.WriteLine(horizontalLine);

        foreach (var transact in transactions)
        {
            string dateTime = transact.timeStamp.ToString("MM/dd/yyyy hh:mm:ss tt");
            string type = transact.transactionType.ToString();
            string amount = $"₱{transact.amount:N2}";

            Console.WriteLine($"| {CenterText(dateTime, dateTimeWidth)} | {CenterText(type, typeWidth)} | {CenterText(amount, amountWidth)} |");
        }

        Console.WriteLine(horizontalLine);
        Console.WriteLine($"Transaction Count: {count}");
        Console.WriteLine();
    }

    private string CenterText(string text, int width)
    {
        if (text.Length >= width)
            return text.Substring(0, width);

        int leftPadding = (width - text.Length) / 2;
        int rightPadding = width - text.Length - leftPadding;

        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }

    public decimal ReadDecimal(string message)
    {
        List<string> messages = new List<string>();

        decimal number;
        while (true)
        {
            Console.Write(message);
            if (decimal.TryParse(Console.ReadLine(), out number))
                return number;

            Console.Clear();
            Console.WriteLine(Program.Title());
            messages.Add("Invalid input! Enter numbers only.");
            DisplayBox(messages);
            messages.Clear();
        }
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
}
