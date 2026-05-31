using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace NutriSnap
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            bool isLarge = Preferences.Default.Get("IsLargeText", false);
            if (isLarge)
            {
                Resources["SmallFont"] = 18.0;
                Resources["NormalFont"] = 22.0;
                Resources["MediumFont"] = 24.0;
                Resources["TitleFont"] = 28.0;
                Resources["HugeFont"] = 40.0;
            }

            MainPage = new LoginPage();
        }
    }
}