using System;
using System.Collections.Generic;
using System.Text;

namespace BOLayer
{
    public class Transaction
    {
        public int AccountNo { get; set; }
        public string Username { get; set; }
        public string HoldersName { get; set; }
        public string TransactionType { get; set; }
        public int TransactionAmount { get; set; }
        public string Date { get; set; }
        public int Balance { get; set; }
    }
}
