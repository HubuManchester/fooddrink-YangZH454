using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SQLite;
using NutriSnap.Models;

namespace NutriSnap.Services
{
    public static class FoodCatalogService
    {
        private static SQLiteAsyncConnection? _db;
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string ApiUrl = "https://6a1c60f18858a003817bd597.mockapi.io/foods";

        private static async Task Init()
        {
            if (_db != null) return;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutriSnap.db3");
            _db = new SQLiteAsyncConnection(databasePath);
            await _db.CreateTableAsync<FoodItem>();
        }

        public static async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            await Init();

            try
            {
                var cloudItems = await _httpClient.GetFromJsonAsync<List<FoodItem>>(ApiUrl);
                if (cloudItems != null)
                {
                    await _db!.DeleteAllAsync<FoodItem>();
                    await _db.InsertAllAsync(cloudItems);
                }
            }
            catch (Exception)
            {
            }

            return await _db!.Table<FoodItem>().ToListAsync();
        }

        public static async Task AddFoodAsync(FoodItem item)
        {
            await Init();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, item);
                if (response.IsSuccessStatusCode)
                {
                    var cloudItem = await response.Content.ReadFromJsonAsync<FoodItem>();
                    if (cloudItem != null) item = cloudItem;
                }
            }
            catch (Exception) {}

            await _db!.InsertAsync(item);
        }

        public static async Task DeleteFoodAsync(FoodItem item)
        {
            await Init();

            try
            {
                await _httpClient.DeleteAsync($"{ApiUrl}/{item.Id}");
            }
            catch (Exception) {}

            await _db!.DeleteAsync(item);
        }
    }
}