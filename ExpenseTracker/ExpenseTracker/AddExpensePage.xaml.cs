using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExpensePage : ContentPage
    {
        private int currentMonth;
        private int currentYear;

        private string oldCatSelection;
        public Expense expenseContext { get; set; }
        public AddExpensePage()
        {
            InitializeComponent();
            
        }

        

        protected override void OnAppearing()
        {
            expenseContext = (Expense)BindingContext;

            currentMonth = expenseContext.Date.Month;
            currentYear = expenseContext.Date.Year;
            CatSelection.Text = expenseContext.CategoryName;

            ExpenseDatePicker.MinimumDate = new DateTime(currentYear, currentMonth, 1);
            int daysallowed;
            if(DateTime.Now.Year == currentYear && DateTime.Now.Month == currentMonth)
            {
                daysallowed = DateTime.Now.Day;
            }
            else
            {
                daysallowed = DateTime.DaysInMonth(currentYear, currentMonth);
            }
            ExpenseDatePicker.MaximumDate = new DateTime(currentYear, currentMonth, daysallowed);

            var catSelectionTapped = new TapGestureRecognizer();
            catSelectionTapped.Tapped += CatSelectionTapped_Tapped;
            CatSelection.GestureRecognizers.Add(catSelectionTapped);
        }

        private async void CatSelectionTapped_Tapped(object sender, EventArgs e)
        {
            oldCatSelection = CatSelection.Text;
            var expense = (Expense)BindingContext;
            await Navigation.PushModalAsync(new NavigationPage(new CategoryChoice { BindingContext = expense }));
        }

        private async void ExpenseDoneBtn_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(ExpenseAmountEntry.Text) || !Double.TryParse(ExpenseAmountEntry.Text,out _) || Double.Parse(ExpenseAmountEntry.Text) <= 0 || string.IsNullOrEmpty(CatSelection.Text))
            {
                await DisplayAlert("Error", "Please enter valid values for all entries", "OK");
            }
            var expense = new Expense();
            expense.Name = ExpenseNameEntry.Text;
            expense.Amount = Double.Parse(ExpenseAmountEntry.Text);
            expense.CategoryName = CatSelection.Text;
            expense.Date = ExpenseDatePicker.Date;
            ExpenseManager.AddModifyMonthlyExpense(currentMonth, currentYear, expenseContext, expense);
           // ExpenseManager.AddMonthlyExpense(DateTime.Now.Month, DateTime.Now.Year, expense);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(oldCatSelection))
            {
                expenseContext.CategoryName = oldCatSelection;
            }
            //go back to original
            await Navigation.PopModalAsync();
        }
    }
}