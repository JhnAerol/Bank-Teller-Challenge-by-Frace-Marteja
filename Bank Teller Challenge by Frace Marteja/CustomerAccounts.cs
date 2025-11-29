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
        public List<CustomerTransaction> Transactions { get; set; } = new List<CustomerTransaction>();

        public CustomerAccounts(string _name, decimal _balance)
        {
            Name = _name;
            Balance = _balance;
        }

        public void AddTransaction(CustomerTransaction transact)
        {
            Transactions.Add(transact);
        }
    }
}
