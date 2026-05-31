using Microsoft.Maui.Controls;

namespace NutriSnap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddRecordPage), typeof(AddRecordPage));
            Routing.RegisterRoute(nameof(FoodDetailPage), typeof(FoodDetailPage));
        }
    }
}