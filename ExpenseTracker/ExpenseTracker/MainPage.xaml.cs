using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpenseTracker
{
    public partial class MainPage : ContentPage
    {
        public double Budget;
        public MainPage()
        {
            InitializeComponent();
            //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YearlyExpenses.xml");
            //File.Delete(filePath);
            ExpenseManager.InitializeData();
        }

        protected override async void OnAppearing()
        {
            string yearMonth = $"{DateTime.Now.Month}.{DateTime.Now.Year}.First";
            if (ExpenseManager.IsMonthInitialized(DateTime.Now.Year, DateTime.Now.Month))
            {
                await Navigation.PushModalAsync(new NavigationPage(new ExpenseDisplayPage { BindingContext = yearMonth}));
            }else
            {
                await Navigation.PushModalAsync(new NavigationPage(new AddBudgetPage { BindingContext = yearMonth }));
            }
           
        }
        

       
    }
}
