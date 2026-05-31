using System;
using SQLite;
using System.Text.Json.Serialization;

namespace NutriSnap.Models
{
    public class FoodItem
    {
        [PrimaryKey]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("calories")]
        public int Calories { get; set; }

        [JsonPropertyName("photoPath")]
        public string PhotoPath { get; set; } = string.Empty;

        [JsonPropertyName("locationText")]
        public string LocationText { get; set; } = string.Empty;

        [Ignore]
        [JsonIgnore]
        public string AccessibleSummary => $"{Name}. Category: {Category}. Calories: {Calories} kcal.";
    }
}