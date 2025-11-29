using System.Globalization;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Bank_Teller_Challenge_by_Frace_Marteja;


public class Initializes
{
    public static List<CustomerAccounts> customers = new();
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int retries = 0;
        Console.WriteLine(UIandValidations.Title());

        while (true)
        {
            if (ShowLoginForm())
            {
                Navi();
            }
            else
            {
                retries++;
                UIandValidations.ShowMessage("Access Denied!!");
                if (retries >= 3)
                {
                    Console.Clear();
                    Console.WriteLine(UIandValidations.Title());
                    UIandValidations.DisplayBox(new List<string> { "Too many attempts. Program will automatically close in 3sec!" });
                    Thread.Sleep(3000);
                    Environment.Exit(0);
                }
            }
        }
    }

    static bool ShowLoginForm()
    {
        var teller = new Teller();

        Console.Write("Enter UserName: ");
        string userName = Console.ReadLine();

        Console.Write("Enter PIN: ");
        string pin = HidePassword();

        return userName == teller.Name && int.TryParse(pin, out int correctpin) && correctpin == teller.Pin;
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
        var transaction = TransactionType.CreateAccount;
        var transactions = Enum.GetValues<TransactionType>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine(UIandValidations.Title());

            foreach (var trans in transactions)
            {
                Console.ForegroundColor = trans == transaction ? ConsoleColor.Green : ConsoleColor.White;
                Console.WriteLine($"{(trans == transaction ? "> " : "  ")}{trans}");
            }
            Console.ResetColor();

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow && transaction > 0) transaction--;
            else if (key == ConsoleKey.DownArrow && (int)transaction < transactions.Length - 1) transaction++;
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                Console.WriteLine(UIandValidations.Title());
                Menu(transaction.ToString());
            }
        }
    }

    static void Menu(string transact) => new BankTransactions().ExecuteTransaction(transact);
}