using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using NutriSnap.Services;
using NutriSnap.Models;

namespace NutriSnap
{
    public partial class MainPage : ContentPage
    {
        private bool _isShowingRecommendation = false;
        // Hardware 6: Cancellation token for stopping TTS
        private CancellationTokenSource? _ttsCancellationTokenSource;
        private List<FoodItem> _allFoods = new();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _allFoods = await FoodCatalogService.GetAllFoodsAsync();
            FoodListView.ItemsSource = _allFoods;

            if (FoodSearchBar != null)
            {
                FoodSearchBar.Text = string.Empty;
            }

            // Hardware 2: Accelerometer (Shake to Recommend)
            if (Accelerometer.Default.IsSupported && !Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.Start(SensorSpeed.Game);
                Accelerometer.Default.ShakeDetected += OnShakeDetected;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Accelerometer.Default.IsSupported && Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ShakeDetected -= OnShakeDetected;
                Accelerometer.Default.Stop();
            }

            CancelSpeech();
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = e.NewTextValue?.ToLowerInvariant() ?? "";

            if (string.IsNullOrWhiteSpace(keyword))
            {
                FoodListView.ItemsSource = _allFoods;
            }
            else
            {
                FoodListView.ItemsSource = _allFoods.Where(f =>
                    f.Name.ToLowerInvariant().Contains(keyword) ||
                    f.Category.ToLowerInvariant().Contains(keyword) ||
                    f.Calories.ToString().Contains(keyword)
                ).ToList();
            }
        }

        private async void OnFoodItemTapped(object sender, TappedEventArgs e)
        {
            if (sender is View clickedView && clickedView.BindingContext is FoodItem selectedFood)
            {
                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }

                var navigationParameter = new Dictionary<string, object>
                {
                    { "Food", selectedFood }
                };
                await Shell.Current.GoToAsync(nameof(FoodDetailPage), navigationParameter);
            }
        }

        // --- Hardware 2: Shake Logic ---
        private void OnShakeDetected(object? sender, EventArgs e)
        {
            if (_isShowingRecommendation) return;
            _isShowingRecommendation = true;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (_allFoods != null && _allFoods.Count > 0)
                {
                    try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.3)); } catch { }

                    var random = new Random();
                    var randomFood = _allFoods[random.Next(_allFoods.Count)];

                    await DisplayAlert("Lucky Pick",
                        $"How about:\n\n{randomFood.Name}\nCalories: {randomFood.Calories} kcal",
                        "Yum!");
                }
                _isShowingRecommendation = false;
            });
        }

        // --- Hardware 4 & 5: TTS & Haptic Feedback ---
        private async void OnReadClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is FoodItem item)
            {
                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }

                CancelSpeech();
                _ttsCancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await TextToSpeech.Default.SpeakAsync(item.AccessibleSummary, cancelToken: _ttsCancellationTokenSource.Token);
                }
                catch { }
            }
        }

        // --- Hardware 6: Stop Reading ---
        private void OnStopReadClicked(object sender, EventArgs e)
        {
            try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }
            CancelSpeech();
        }

        private void CancelSpeech()
        {
            if (_ttsCancellationTokenSource?.IsCancellationRequested == false)
            {
                _ttsCancellationTokenSource.Cancel();
            }
        }

        private async void OnAddNewClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddRecordPage));
        }

        // --- Delete Logic ---
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is FoodItem itemToDelete)
            {
                bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{itemToDelete.Name}'?", "Yes", "Cancel");

                if (confirm)
                {
                    try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.1)); } catch { }

                    await FoodCatalogService.DeleteFoodAsync(itemToDelete);

                    _allFoods = await FoodCatalogService.GetAllFoodsAsync();
                    FoodListView.ItemsSource = _allFoods;

                    OnSearchBarTextChanged(FoodSearchBar, new TextChangedEventArgs(FoodSearchBar.Text, FoodSearchBar.Text));
                }
            }
        }
    }
}
