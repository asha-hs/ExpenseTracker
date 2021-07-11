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
    public partial class CategoryChoice : ContentPage
    {
        public List<CategoryItem> categoryItems = new List<CategoryItem>();
        public Expense expense { get; set; }
        public CategoryChoice()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            expense = (Expense)BindingContext;
            categoryItems = new List<CategoryItem>
            {
                
                new CategoryItem
                {
                    Name = "Bills",
                    IconImage = "Resources/bill.png"
                },
                new CategoryItem
                {
                    Name = "Dining",
                    IconImage = "Resources/dining.png"
                },
                new CategoryItem
                {
                    Name = "Entertainment",
                    IconImage = "Resources/entertainment.png"
                },
                new CategoryItem
                {
                    Name = "Essentials",
                    IconImage = "Resources/essentials.png"
                },
                new CategoryItem
                {
                    Name = "Retails",
                    IconImage = "Resources/retail.png"
                }
            };
            CategoryIconView.ItemsSource = categoryItems;
        }

        private async void CategoryIconView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as CategoryItem;
            expense.CategoryName = selected.Name;
            await Navigation.PushModalAsync(new NavigationPage(new AddExpensePage
            {
                BindingContext = expense
            }));
        }
    }
}