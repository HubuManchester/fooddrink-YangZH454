using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using NutriSnap.Models;

namespace NutriSnap.Services
{
    public static class FoodCatalogService
    {
        private static SQLiteAsyncConnection? _db;

        private static async Task Init()
        {
            if (_db != null)
                return;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NutriSnap.db3");
            _db = new SQLiteAsyncConnection(databasePath);
            await _db.CreateTableAsync<FoodItem>();
        }

        public static async Task<List<FoodItem>> GetAllFoodsAsync()
        {
            await Init();
            return await _db!.Table<FoodItem>().ToListAsync();
        }

        public static async Task AddFoodAsync(FoodItem item)
        {
            await Init();
            await _db!.InsertAsync(item);
        }

        public static async Task DeleteFoodAsync(FoodItem item)
        {
            await Init();
            await _db!.DeleteAsync(item);
        }
    }
}