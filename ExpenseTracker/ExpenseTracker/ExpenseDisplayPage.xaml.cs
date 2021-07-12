using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpenseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseDisplayPage : ContentPage
    {
        private int currentMonth;
        private int currentYear;
        public double balance { get; set; }
        public ExpenseDisplayPage()
        {
            InitializeComponent();

            //load month and year picker

            MonthPicker.ItemsSource = Enum.GetNames(typeof(Months)).ToList();
            int currentyear = DateTime.Now.Year;
            YearPicker.ItemsSource = new List<int> { currentyear, currentyear - 1, currentyear - 2 };
            
        }
        protected override async void OnAppearing()
        {
            var context = (string)BindingContext;
            string[] info = context.Split('.');
            currentMonth = int.Parse(info[0]);
            currentYear = int.Parse(info[1]);


            MonthlyExpense monthlyExpense = new MonthlyExpense();
            ExpenseManager.GetMonthlyExpenses(currentMonth, currentYear, ref monthlyExpense);

            MonthPicker.SelectedIndex = currentMonth - 1;
            YearPicker.SelectedItem = currentYear;

            MonthPicker.SelectedIndexChanged += MonthPicker_SelectedIndexChanged;
            YearPicker.SelectedIndexChanged += YearPicker_SelectedIndexChanged;

            var numberFormat = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            numberFormat.CurrencyNegativePattern = 1;
            MonthBudget.Text = monthlyExpense.Budget.ToString("C", numberFormat);
            balance = monthlyExpense.Balance;
            BalanceDisplay.Text = balance.ToString("C", numberFormat);

            if(monthlyExpense.Budget <= 0)
            {
                // Budget not set for the selected month, prompt the user to set the budget to able to track the expense
                // disable add expense button

                await DisplayAlert("Alert", "Please click on the budget to set budget and get started with expense tracking", "OK");
                AddExpenseButton.IsVisible = false;
            }
            else
            {
                AddExpenseButton.IsVisible = true;
                ExpenseListView.ItemsSource = monthlyExpense.ExpenseList.OrderByDescending(x => x.Date);
                EditDeleteStack.IsVisible = false;

            }

            var BudgetTapped = new TapGestureRecognizer();
            BudgetTapped.Tapped += BudgetTapped_Tapped;
            MonthBudget.GestureRecognizers.Clear();
            MonthBudget.GestureRecognizers.Add(BudgetTapped);

            EditDeleteStack.IsVisible = false;
            
            if(currentMonth == DateTime.Now.Month && currentYear == DateTime.Now.Year)
            {
                NextMonthBtn.IsEnabled = false;
            }
            else
            {
                NextMonthBtn.IsEnabled = true;
            }

            ExpenseListView.SelectedItem = null;
            
        }

        private async void BudgetTapped_Tapped(object sender, EventArgs e)
        {
            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var selectedYear = (int)YearPicker.SelectedItem;
            string yearmonth = $"{selectedMonth}.{selectedYear}";
            await Navigation.PushModalAsync(new NavigationPage(new AddBudgetPage { BindingContext = yearmonth }));
        }

        private async void YearPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var selectedYear = (int)YearPicker.SelectedItem;
            string yearmonth = $"{selectedMonth}.{selectedYear}";

            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearmonth }));
        }

        private async void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var selectedYear = (int)YearPicker.SelectedItem;
            string yearmonth = $"{selectedMonth}.{selectedYear}";

            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearmonth }));
        }

        private async void PreviousMonthBtn_Clicked(object sender, EventArgs e)
        {
            string yearmonth;
            if(currentMonth != 12)
            {
                yearmonth = $"{currentMonth - 1}.{currentYear}";
            }else
            {
                yearmonth = $"1.{currentYear-1}";
            }
            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearmonth }));

        }

        private async void NextMonthBtn_Clicked(object sender, EventArgs e)
        {
            string yearmonth;
            if(currentMonth != 12)
            {
                yearmonth = $"{currentMonth + 1}.{currentYear}";
            }else
            {
                yearmonth = $"1.{currentYear + 1}";
            }

            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearmonth }));
        }

        private async void DeleteExpense_Clicked(object sender, EventArgs e)
        {
            
            var expense = (Expense)ExpenseListView.SelectedItem;
            string yearmonth = $"{currentMonth}.{currentYear}";
            ExpenseManager.DeleteMonthlyExpense(currentMonth, currentYear, expense);
            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearmonth}));
        }

        private async void EditExpense_Clicked(object sender, EventArgs e)
        {
            var expense = (Expense)ExpenseListView.SelectedItem;
            
            await Navigation.PushModalAsync(new NavigationPage(new AddExpensePage
            { BindingContext = expense }));

        }

        private void CancelSelection_Clicked(object sender, EventArgs e)
        {
            ExpenseListView.SelectedItem = null;
            EditDeleteStack.IsVisible = false;
        }

        private void ExpenseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ExpenseListView.SelectedItem != null)
            {
                EditDeleteStack.IsVisible = true;
            }else
            {
                EditDeleteStack.IsVisible = false;
            }
            
        }

        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new AddExpensePage
            { BindingContext = new Expense { Date = new DateTime(currentYear, currentMonth, 1) } }));
        }
    }
}