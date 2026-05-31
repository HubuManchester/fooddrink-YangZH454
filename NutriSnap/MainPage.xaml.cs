using System;
using System.Threading;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors; // Hardware: Accelerometer
using Microsoft.Maui.ApplicationModel;
using NutriSnap.Services;

namespace NutriSnap
{
    public partial class MainPage : ContentPage
    {
        private bool _isShowingRecommendation = false;
        // Hardware 6: Cancellation token for stopping TTS
        private CancellationTokenSource? _ttsCancellationTokenSource;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            FoodListView.ItemsSource = null;
            FoodListView.ItemsSource = await FoodCatalogService.GetAllFoodsAsync();

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

            // Stop listening to shake when page is closed
            if (Accelerometer.Default.IsSupported && Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ShakeDetected -= OnShakeDetected;
                Accelerometer.Default.Stop();
            }

            // Hardware 6: Stop reading when leaving the page (Crucial for high marks)
            CancelSpeech();
        }

        // --- Hardware 2: Shake Logic ---
        private void OnShakeDetected(object? sender, EventArgs e)
        {
            if (_isShowingRecommendation) return;
            _isShowingRecommendation = true;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var allFoods = await FoodCatalogService.GetAllFoodsAsync();
                if (allFoods != null && allFoods.Count > 0)
                {
                    // Trigger a strong vibration on shake
                    try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.3)); } catch { }

                    var random = new Random();
                    var randomFood = allFoods[random.Next(allFoods.Count)];

                    await DisplayAlert("Lucky Pick 🎲",
                        $"How about:\n\n{randomFood.Name}\nCalories: {randomFood.Calories} kcal",
                        "Yum!");
                }
                _isShowingRecommendation = false;
            });
        }

        // --- Hardware 4 & 5: TTS & Haptic Feedback ---
        private async void OnReadClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Models.FoodItem item)
            {
                try { HapticFeedback.Default.Perform(HapticFeedbackType.Click); } catch { }

                // Cancel any ongoing speech before starting a new one
                CancelSpeech();
                _ttsCancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await TextToSpeech.Default.SpeakAsync(item.AccessibleSummary, cancelToken: _ttsCancellationTokenSource.Token);
                }
                catch {}
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

        // --- Handle the logic for sliding deletion ---
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is Models.FoodItem itemToDelete)
            {
                bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{itemToDelete.Name}'?", "Yes", "Cancel");

                if (confirm)
                {
                    // Hardware vibration feedback
                    try { Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.1)); } catch { }

                    await FoodCatalogService.DeleteFoodAsync(itemToDelete);

                    FoodListView.ItemsSource = null;
                    FoodListView.ItemsSource = await FoodCatalogService.GetAllFoodsAsync();
                }
            }
        }
    }
}