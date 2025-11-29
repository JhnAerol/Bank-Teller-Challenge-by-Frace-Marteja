using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Teller_Challenge_by_Frace_Marteja
{
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
}
