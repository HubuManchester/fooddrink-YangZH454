using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace NutriSnap.Models
{
    public class FoodItem
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Calories { get; set; }

        [Ignore]
        public string AccessibleSummary => $"{Name}. Category: {Category}. Calories: {Calories} kcal.";
    }
}
