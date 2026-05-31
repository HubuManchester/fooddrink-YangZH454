using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel; // For Hardware
using NutriSnap.Models;
using NutriSnap.Services;

namespace NutriSnap
{
    public partial class AddRecordPage : ContentPage
    {
        private Location? _currentLocation;
        public AddRecordPage()
        {
            InitializeComponent();
        }

        // --- Hardware 1: Camera ---
        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        var stream = await photo.OpenReadAsync();
                        FoodPhoto.Source = ImageSource.FromStream(() => stream);
                        FoodPhoto.IsVisible = true;
                        PhotoLabel.Text = "Photo captured!";

                        // Hardware 5: Strong Vibration on success
                        try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.2)); } catch { }
                    }
                }
            }
            catch (Exception)
            {
                PhotoLabel.Text = "Camera permission denied.";
            }
        }

        // --- Hardware 3: Location ---
        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                LocationLabel.Text = "Fetching location...";
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _currentLocation = await Geolocation.Default.GetLocationAsync(request);

                if (_currentLocation != null)
                {
                    LocationLabel.Text = $"Lat: {_currentLocation.Latitude:F2}, Lon: {_currentLocation.Longitude:F2}";

                    ViewMapButton.IsVisible = true;

                    try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.5)); } catch { }
                }
                else
                {
                    LocationLabel.Text = "Location not available.";
                }
            }
            catch (Exception)
            {
                LocationLabel.Text = "Location permission denied.";
            }
        }

        // --- App Integration ---
        private async void OnViewMapClicked(object sender, EventArgs e)
        {
            if (_currentLocation != null)
            {
                try
                {
                    var options = new MapLaunchOptions { Name = string.IsNullOrWhiteSpace(NameEntry.Text) ? "Food Location" : NameEntry.Text };
                    await Map.Default.OpenAsync(_currentLocation, options);
                }
                catch (Exception)
                {
                    await DisplayAlert("Error", "Could not open map app.", "OK");
                }
            }
        }

        // Validation and Save Logic
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
            await DisplayAlert("Success", "Record saved!", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private void ShowError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.IsVisible = true;
        }
    }
}