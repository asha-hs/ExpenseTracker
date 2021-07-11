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
    public partial class AddBudgetPage : ContentPage
    {
        private string context;
        public AddBudgetPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            context = (string)BindingContext;
            string[] info = context.Split('.');

            BudgetInputTextBox.Text = ExpenseManager.GetMonthlyBudget(int.Parse(info[0]), int.Parse(info[1])).ToString();
            YearMonthLabel.Text = $"{Enum.GetName(typeof(Months), int.Parse(info[0]) - 1)}{info[1]}";
        }
        private async void ContinueButton_Clicked(object sender, EventArgs e)
        {
            string inputstr = BudgetInputTextBox.Text;
            if (String.IsNullOrEmpty(inputstr) || Double.TryParse(inputstr,out _) || Double.Parse(inputstr) <= 0)
            {
                await DisplayAlert("Error", "Please Enter Budget Amount", "OK");
            }
            else
            {
                string[] info = context.Split('.');
                double Budget = double.Parse(BudgetInputTextBox.Text);
                ExpenseManager.SetMonthlyBudget(Budget, int.Parse(info[1]), int.Parse(info[0]));
                //move to expense Page
                if(info.Length == 3)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new AddExpensePage
                    { BindingContext = context }));
                }else
                {
                    await Navigation.PopModalAsync();
                }
                
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            string[] info = context.Split('.');
            if (info.Length == 3)
            {
                await Navigation.PushModalAsync(new NavigationPage(new AddExpensePage
                { BindingContext = context }));
            }
            else
            {
                await Navigation.PopModalAsync();
            }
        }
    }
}