using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace NutriSnap
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            ThemeSwitch.IsToggled = Application.Current!.RequestedTheme == AppTheme.Dark;
            FontSwitch.IsToggled = Preferences.Default.Get("IsLargeText", false);
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            Application.Current!.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }

        private void OnLargeTextToggled(object sender, ToggledEventArgs e)
        {
            Preferences.Default.Set("IsLargeText", e.Value);

            if (e.Value)
            {
                Application.Current!.Resources["SmallFont"] = 18.0;
                Application.Current.Resources["NormalFont"] = 22.0;
                Application.Current.Resources["MediumFont"] = 24.0;
                Application.Current.Resources["TitleFont"] = 28.0;
                Application.Current.Resources["HugeFont"] = 40.0;
                DemoText.Text = "Large text mode is ON. All pages are updated globally.";
            }
            else
            {
                Application.Current!.Resources["SmallFont"] = 14.0;
                Application.Current.Resources["NormalFont"] = 16.0;
                Application.Current.Resources["MediumFont"] = 18.0;
                Application.Current.Resources["TitleFont"] = 22.0;
                Application.Current.Resources["HugeFont"] = 32.0;
                DemoText.Text = "This is a preview of your standard text size.";
            }
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            Application.Current!.MainPage = new LoginPage();
        }
    }
}