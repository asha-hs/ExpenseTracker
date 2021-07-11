using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Model
{
    public enum Category
    {
        Essentials,
        Dining,
        Entertainment,
        Retail,
        Bills,
        Miscellaneous
    }
    public class Expense
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public string CategoryName { get; set; }

        public string CategoryIcon { get; set; }
        
    }
}
