using System;
using Microsoft.Maui.Controls;
using NutriSnap.Services;

namespace NutriSnap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            FoodListView.ItemsSource = null;
            FoodListView.ItemsSource = FoodCatalogService.GetAllFoods();
        }

        private async void OnAddNewClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddRecordPage));
        }
    }
}