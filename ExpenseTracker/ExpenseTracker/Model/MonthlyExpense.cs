using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Model
{
    public class MonthlyExpense
    {
        public double Budget { get; set; }

        public List<Expense> ExpenseList { get; set; }
        public int Month { get; set; }
        public double Balance
        {
            get
            {
                double totalExp = 0.0;
                ExpenseList.ForEach(exp => totalExp += exp.Amount);
                return Budget - totalExp;

            }
        }
    }
}
