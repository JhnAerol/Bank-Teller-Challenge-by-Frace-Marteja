using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Teller_Challenge_by_Frace_Marteja
{
    public class BankTransactions
    {
        public void ExecuteTransaction(string transact)
        {
            switch (transact)
            {
                case "CreateAccount": CreateAccount(); break;
                case "CheckBalance": CheckBalance(); break;
                case "Deposit": Deposit(); break;
                case "Withdraw": Withdraw(); break;
                case "ViewTransactions": ViewTransactions(); break;
                case "ListOfAccounts": ListOfAccounts(); break;
                case "Exit": Environment.Exit(0); break;
            }
        }

        public void CreateAccount()
        {
            while (true)
            {
                Console.WriteLine("You Select: Create an Account\n");   

                Console.Write("Enter Name: ");
                string? name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    UIandValidations.ShowMessage("Name can't be empty");
                    continue;
                }

                decimal amount = UIandValidations.ReadDecimal("Enter First Deposit: ");

                if (Initializes.customers.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    UIandValidations.ShowMessage("Account Name is Already exist.");
                    continue;
                }


                var acc = new CustomerAccounts(name, amount);
                var teller = new Teller();

                teller.AddTransaction(new TellerTransaction(DateTime.Now, "Creating an Account", amount, name));
                acc.AddTransaction(new Transaction(DateTime.Now, TransactionType.Deposit, amount));
                Initializes.customers.Add(acc);

                UIandValidations.ShowMessage("Created Successfully!");

                if (!UIandValidations.AskRepeat("Do you want to create another account?")) break;
            }
        }
        

        public void CheckBalance()
        {
            while (true)
            {
                Console.WriteLine("You Select: Check Balance\n");

                var name = UIandValidations.GetValidAccountName();
                if (name == null) continue;

                var acc = UIandValidations.FindAccount(name);
                if (acc == null)
                {
                    if (UIandValidations.AskRepeat("Do you want to continue?"))
                    {
                        Console.Clear();
                        Console.WriteLine(UIandValidations.Title());
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }

                UIandValidations.ShowMessage($"Current Amount for {acc.Name}: ₱{acc.Balance}");
                UIandValidations.WaitForKey();
                return;
            }
        }

        public void ListOfAccounts()
        {
            Console.Clear();
            Console.WriteLine(UIandValidations.Title());
            Console.WriteLine("You Select: List of Account\n");

            var messages = Initializes.customers.Count > 0
                ? Initializes.customers.Select(c => c.Name).ToList()
                : new List<string> { "No Records" };

            UIandValidations.DisplayBox(messages);
            UIandValidations.WaitForKey();
        }

        private void ProcessTransaction(string type, Func<CustomerAccounts, decimal, string> process)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(UIandValidations.Title());
                Console.WriteLine($"You Select: {type}\n");

                var name = UIandValidations.GetValidAccountName();
                if (name == null) continue;

                var acc = UIandValidations.FindAccount(name);
                if(acc == null)
                {
                    if (UIandValidations.AskRepeat("Do you want to continue?"))
                    {
                        Console.Clear(); 
                        Console.WriteLine(UIandValidations.Title()); 
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                

                decimal amount = UIandValidations.ReadDecimal($"Enter {type} Amount: ");
                if (amount <= 0)
                {
                    UIandValidations.ShowMessage("Invalid Amount entered!");
                    return;
                }

                var result = process(acc, amount);
                if (result.StartsWith("Withdrawal successful") || result.StartsWith("Deposit successful"))
                {
                    new Teller().AddTransaction(new TellerTransaction(DateTime.Now, $"{type} from an Account", amount, name));
                    acc.AddTransaction(new Transaction(DateTime.Now,
                        type == "Deposit" ? TransactionType.Deposit : TransactionType.Withdraw, amount));
                }

                UIandValidations.ShowMessage(result);
                if (!UIandValidations.AskRepeat($"Do you want to make another {type.ToLower()}?")) break;
            }
        }

        public void Deposit() => ProcessTransaction("Deposit", (acc, amount) =>
        {
            acc.Balance += amount;
            return $"Deposit successful! Current Balance for {acc.Name}: ₱{acc.Balance}";
        });

        public void Withdraw() => ProcessTransaction("Withdraw", (acc, amount) =>
        {
            if (amount > acc.Balance) return "Insufficient funds!";

            acc.Balance -= amount;
            return $"Withdrawal successful! Current Balance for {acc.Name}: ₱{acc.Balance}";
        });

        public void ViewTransactions()
        {
            Console.Clear();
            Console.WriteLine(UIandValidations.Title());
            Console.WriteLine("You Select: View Transaction\n");

            Console.WriteLine("1. Teller Transaction");
            Console.WriteLine("2. Customer Transaction");
            Console.Write("Select option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowTellerTransactions();
                    break;
                case "2":
                    ShowCustomerTransactions();
                    break;
                default:
                    UIandValidations.ShowMessage("Invalid option selected.");
                    break;
            }
            UIandValidations.WaitForKey();
        }

        private void ShowTellerTransactions()
        {
            Console.Clear();
            Console.WriteLine(UIandValidations.Title());

            if (Teller.Transactions.Count == 0)
            {
                UIandValidations.DisplayBox(new List<string> { "No teller transactions found." });
                return;
            }

            Console.Clear();
            Console.WriteLine(UIandValidations.Title());
            Console.WriteLine(UIandValidations.CenterText("Transactions for Teller", 125));
            Console.WriteLine();
            DisplayTellerTransactionTable(Teller.Transactions);
        }

        private void ShowCustomerTransactions()
        {
            string name = UIandValidations.GetValidAccountName();

            var acc = UIandValidations.FindAccount(name);
            if (acc == null) return;

            if (acc.Transactions.Count == 0)
            {
                UIandValidations.DisplayBox(new List<string> { "No transactions found for this account." });
                return;
            }

            Console.Clear();
            Console.WriteLine(UIandValidations.Title());
            Console.WriteLine(UIandValidations.CenterText($"Transactions for {name}", 125));
            Console.WriteLine();
            DisplayCustomerTransactionTable(acc.Transactions, acc.Balance);
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

            Console.WriteLine($"| {UIandValidations.CenterText("Date Time", dateTimeWidth)} | {UIandValidations.CenterText("Type", typeWidth)} | {UIandValidations.CenterText("Amount", amountWidth)} | {UIandValidations.CenterText("Account Name", accountWidth)} |");
            Console.WriteLine(horizontalLine);

            foreach (var transact in transactions)
            {
                string dateTime = transact.timeStamp.ToString("MM/dd/yyyy hh:mm:ss tt");
                string type = transact.actions;
                string amount = $"₱{transact.amount:N2}";
                string accountName = transact.accountName;

                Console.WriteLine($"| {UIandValidations.CenterText(dateTime, dateTimeWidth)} | {UIandValidations.CenterText(type, typeWidth)} | {UIandValidations.CenterText(amount, amountWidth)} | {UIandValidations.CenterText(accountName, accountWidth)} |");
            }

            Console.WriteLine(horizontalLine);
            Console.WriteLine();
        }

        private void DisplayCustomerTransactionTable(List<Transaction> transactions, decimal balance)
        {
            int count = transactions.Count;
            Console.WriteLine(UIandValidations.CenterText($"(Balance: ₱{balance:N2})", 125));
            Console.WriteLine();

            int dateTimeWidth = 40;
            int typeWidth = 40;
            int amountWidth = 30;

            string horizontalLine = new string('\u2014', Console.WindowWidth);
            Console.WriteLine(horizontalLine);

            Console.WriteLine($"| {UIandValidations.CenterText("Date Time", dateTimeWidth)} | {UIandValidations.CenterText("Type", typeWidth)} | {UIandValidations.CenterText("Amount", amountWidth)} |");
            Console.WriteLine(horizontalLine);

            foreach (var transact in transactions)
            {
                string dateTime = transact.timeStamp.ToString("MM/dd/yyyy hh:mm:ss tt");
                string type = transact.transactionType.ToString();
                string amount = $"₱{transact.amount:N2}";

                Console.WriteLine($"| {UIandValidations.CenterText(dateTime, dateTimeWidth)} | {UIandValidations.CenterText(type, typeWidth)} | {UIandValidations.CenterText(amount, amountWidth)} |");
            }

            Console.WriteLine(horizontalLine);
            Console.WriteLine($"Transaction Count: {count}");
            Console.WriteLine();
        }
    }
}
