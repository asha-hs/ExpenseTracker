using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExpenseTracker.Model
{
    public static class ExpenseManager
    {
        private static List<YearlyExpense> yearlyExpenseList = new List<YearlyExpense>(); 

        public static void InitializeData()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"YearlyExpenses.xml");
            bool isFileThere = File.Exists(filePath);
            if(isFileThere)
            {
                DeserializeData(filePath);
            }
        }

        internal static double GetMonthlyBudget(int month, int year)
        {
            double budget = 0;

            foreach(YearlyExpense item in yearlyExpenseList)
            {
                if(item.Year == year)
                {
                    foreach(MonthlyExpense monthly in item.MonthlyExpenseList)
                    {
                        if(monthly.Month == month)
                        {
                            budget = monthly.Budget;
                            break;
                        }
                    }
                }
            }
            return budget;
        }

        public static void AddMonthlyExpense(int month,int year, Expense expense)
        {
            foreach(YearlyExpense item in yearlyExpenseList)
            {
                if(item.Year == year)
                {
                    var toAdd = item.MonthlyExpenseList.Where(eachmonth => eachmonth.Month == month).ToList();
                    toAdd[0].ExpenseList.Add(expense);
                    break;
                }
            }
            SerializeData();
        }

        // This method will get the list of expenses for the month provided
        public static void GetMonthlyExpenses(int month,int year, ref MonthlyExpense monthlyexpense)
        {
            foreach(YearlyExpense item in yearlyExpenseList)
            {
                if(item.Year == year)
                {
                    var expenselist = item.MonthlyExpenseList.Where(eachmonth => eachmonth.Month == month).ToList();
                    monthlyexpense = expenselist[0];
                    break;
                }
            }
        }

        public static void DeleteMonthlyExpense(int month,int year,Expense expense)
        {
            foreach(YearlyExpense item in yearlyExpenseList)
            {
                if(item.Year == year)
                {
                    var monthlyexpense = item.MonthlyExpenseList.Where(eachmonth => eachmonth.Month == month).ToList();
                    monthlyexpense[0].ExpenseList.Remove(expense);
                    break;
                }
            }
            SerializeData();
        }
        private static void DeserializeData(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<YearlyExpense>));

            Stream reader = new FileStream(filePath, FileMode.Open);

            yearlyExpenseList = (List<YearlyExpense>)serializer.Deserialize(reader);
            reader.Close();
        }

        public static bool IsMonthInitialized(int year,int month)
        {
            bool retval = false;

            foreach(YearlyExpense item in yearlyExpenseList)
            {

                if(item.Year == year)
                {
                    foreach(MonthlyExpense exp in item.MonthlyExpenseList)
                    {
                        if(exp.Month == month)
                        {
                            retval = true;
                        }
                    }
                }

            }
            return retval;
        }

        public static void SetMonthlyBudget(double budget,int year,int month)
        {
            bool foundYear = false;

            MonthlyExpense newMonth = new MonthlyExpense { Budget = budget, 
                ExpenseList = new List<Expense>(), Month = month };

            foreach(YearlyExpense item in yearlyExpenseList)
            {
                if(item.Year == year)
                {
                    item.MonthlyExpenseList.Add(newMonth);
                    foundYear = true;
                    break;
                }
            }
            if (!foundYear)
            {
                YearlyExpense newYear = new YearlyExpense
                {
                    Year = year,
                    MonthlyExpenseList = new List<MonthlyExpense>()
                };
                newYear.MonthlyExpenseList.Add(newMonth);
                yearlyExpenseList.Add(newYear);
            }

            SerializeData();
        }

        private static void SerializeData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<YearlyExpense>));
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YearlyExpenses.xml");
            Stream writer = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(writer, yearlyExpenseList);
            writer.Close();
        }

      
    }
}
