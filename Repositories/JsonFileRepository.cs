using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvoicingSystem.Repositories
{
    // Generic repository class for handling data storage and retrieval in JSON files
    public class JsonFileRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath; // Path to the JSON file
        private List<T> _items; // In-memory list of items

        // Constructor that initializes the repository and loads data from the file
        public JsonFileRepository(string filePath)
        {
            _filePath = filePath;
            _items = LoadFromFile();
        }

        // Loads data from the JSON file
        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        // Saves data to the JSON file
        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_items);
            File.WriteAllText(_filePath, json);
        }

        // Retrieves all items
        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        // Retrieves an item by its ID
        public Task<T> GetById(int id)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            var item = _items.FirstOrDefault(i => (int)propertyInfo.GetValue(i) == id);
            return Task.FromResult(item);
        }

        // Adds a new item and assigns a new ID
        public Task Add(T entity)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            var newId = _items.Count > 0 ? (int)propertyInfo.GetValue(_items.Last()) + 1 : 1;
            propertyInfo.SetValue(entity, newId);
            _items.Add(entity);
            SaveToFile();
            return Task.CompletedTask;
        }

        // Updates an existing item
        public Task Update(T entity)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            var id = (int)propertyInfo.GetValue(entity);
            var index = _items.FindIndex(i => (int)propertyInfo.GetValue(i) == id);
            if (index != -1)
            {
                _items[index] = entity;
                SaveToFile();
            }
            return Task.CompletedTask;
        }

        // Deletes an item by its ID
        public Task Delete(int id)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            var item = _items.FirstOrDefault(i => (int)propertyInfo.GetValue(i) == id);
            if (item != null)
            {
                _items.Remove(item);
                SaveToFile();
            }
            return Task.CompletedTask;
        }
    }
}


