using System;
using Microsoft.Maui.Controls;
using NutriSnap.Models;
using NutriSnap.Services;

namespace NutriSnap
{
    public partial class AddRecordPage : ContentPage
    {
        public AddRecordPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(CategoryEntry.Text))
            {
                ShowError("Name and Category cannot be empty.");
                return;
            }

            if (!int.TryParse(CaloriesEntry.Text, out int calories) || calories < 0)
            {
                ShowError("Please enter a valid non-negative number for calories.");
                return;
            }

            var newItem = new FoodItem
            {
                Name = NameEntry.Text.Trim(),
                Category = CategoryEntry.Text.Trim(),
                Calories = calories
            };

            await FoodCatalogService.AddFoodAsync(newItem);
            await DisplayAlert("Success", "Record added successfully!", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}