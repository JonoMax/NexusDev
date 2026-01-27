using NexusDev.Models;

namespace NexusDev.Data
{
    public class CarRepository: ICarRepository
    {
        private static readonly List<Car> _cars = new();
        private static int _nextId = 1;

        public List<Car> GetAll() => _cars;
        public Car? GetById(int id) => _cars.FirstOrDefault(c => c.Id == id);
        public Car Add(Car car)
        {
            car.Id = _nextId++;
            _cars.Add(car);
            return car;
        }

        public void Update(Car car)
        {
            var index = _cars.FindIndex(c => c.Id == car.Id); ;
            if (index != -1) _cars[index] = car;
        }
        
        public void Delete(int id) => _cars.RemoveAll(c => c.Id == id);
    }
}
