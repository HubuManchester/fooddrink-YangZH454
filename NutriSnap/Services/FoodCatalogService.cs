using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NutriSnap.Models;

namespace NutriSnap.Services
{
    public static class FoodCatalogService
    {
        private static List<FoodItem> _items = new List<FoodItem>
        {
            new FoodItem { Name = "Avocado Toast", Category = "Breakfast", Calories = 250 },
            new FoodItem { Name = "Grilled Chicken Salad", Category = "Lunch", Calories = 400 },
            new FoodItem { Name = "Black Coffee", Category = "Drink", Calories = 5 }
        };

        public static List<FoodItem> GetAllFoods()
        {
            return _items;
        }

        public static void AddFood(FoodItem item)
        {
            _items.Add(item);
        }
    }
}
