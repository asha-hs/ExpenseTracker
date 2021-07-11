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
        public Expense expense { get; set; }
        public AddExpensePage()
        {
            InitializeComponent();
            
        }

        

        protected override void OnAppearing()
        {
            expense = (Expense)BindingContext;

            var catSelectionTapped = new TapGestureRecognizer();
            catSelectionTapped.Tapped += CatSelectionTapped_Tapped;
            CatSelection.GestureRecognizers.Add(catSelectionTapped);
        }

        private async void CatSelectionTapped_Tapped(object sender, EventArgs e)
        {
            var expense = (Expense)BindingContext;
            await Navigation.PushModalAsync(new NavigationPage(new CategoryChoice { BindingContext = expense }));
        }

        private async void ExpenseDoneBtn_Clicked(object sender, EventArgs e)
        {
            var expense = (Expense)BindingContext;
            ExpenseManager.AddMonthlyExpense(DateTime.Now.Month, DateTime.Now.Year, expense);
            await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage()));
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            //go back to original
            await Navigation.PopModalAsync();
        }
    }
}