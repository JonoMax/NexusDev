using NexusDev.Models;

namespace NexusDev.Data
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task<Car> AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task<bool> DeleteAsync(int id);
    }
}
