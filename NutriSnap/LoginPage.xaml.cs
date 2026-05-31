using System;
using Microsoft.Maui.Controls;

namespace NutriSnap
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                ErrorLabel.Text = "Please enter both username and password.";
                ErrorLabel.IsVisible = true;
                return;
            }

            Application.Current!.MainPage = new AppShell();
        }
    }
}