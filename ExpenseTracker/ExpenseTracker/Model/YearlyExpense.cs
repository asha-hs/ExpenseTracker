using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Model
{
    public class YearlyExpense
    {
        public int Year { get; set; }
        public List<MonthlyExpense> MonthlyExpenseList = new List<MonthlyExpense>();
    }
}
