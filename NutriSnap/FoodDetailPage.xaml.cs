using Microsoft.Maui.Controls;
using NutriSnap.Models;

namespace NutriSnap
{
    [QueryProperty(nameof(CurrentFood), "Food")]
    public partial class FoodDetailPage : ContentPage
    {
        public FoodItem CurrentFood
        {
            set { BindingContext = value; }
        }

        public FoodDetailPage()
        {
            InitializeComponent();
        }
    }
}