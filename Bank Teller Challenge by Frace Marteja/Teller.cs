using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Teller_Challenge_by_Frace_Marteja
{
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
}
