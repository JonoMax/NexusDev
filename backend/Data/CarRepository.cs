using NexusDev.Models;
using System.Runtime.CompilerServices;

namespace NexusDev.Data
{
    public class CarRepository: ICarRepository
    {
        private static readonly List<Car> _cars = new();
        private static int _nextId = 1;

        //public List<Car> GetAll() => _cars;
        public Task<List<Car>> GetAllAsync()
        {
            return Task.FromResult(_cars);
        }

        //public Car? GetById(int id) => _cars.FirstOrDefault(c => c.Id == id);
        public Task<Car?> GetByIdAsync(int id)
        {
            return Task.FromResult(_cars.FirstOrDefault(c => c.Id == id));
        }
        public Task<Car> AddAsync(Car car)
        {
            car.Id = _nextId++;
            _cars.Add(car);
            return Task.FromResult(car);
        }

        public Task UpdateAsync(Car car)
        {
            var index = _cars.FindIndex(c => c.Id == car.Id); ;
            if (index != -1) _cars[index] = car;
            return Task.CompletedTask;
        }
        
        //public void Delete(int id) => _cars.RemoveAll(c => c.Id == id);
        public Task<bool> DeleteAsync(int id)
        {
            var removedCount = _cars.RemoveAll(c => c.Id == id);
            return Task.FromResult(removedCount > 0);
        }
    }
}
