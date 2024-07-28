using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(); // Get all items
    Task<T> GetById(int id); // Get item by ID
    Task Add(T entity); // Add new item
    Task Update(T entity); // Update existing item
    Task Delete(int id); // Delete item by ID
}

