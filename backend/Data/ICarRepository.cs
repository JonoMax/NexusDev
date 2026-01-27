using NexusDev.Models;

namespace NexusDev.Data
{
    public interface ICarRepository
    {
        List<Car> GetAll();
        Car? GetById(int id);
        Car Add(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}
