using NexusDev.Models;
using Microsoft.EntityFrameworkCore;

namespace NexusDev.Data
{
    public class CarRepositoryEF : ICarRepository
    {
        private readonly AppDbContext _context;

        public CarRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public List<Car> GetAll()
        {
            return _context.Cars.ToList();
        }

        public Car? GetById(int id)
        {
            return _context.Cars.FirstOrDefault(c => c.Id == id);
        }

        public Car Add(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            return car;
        }

        public void Update(Car car)
        {
            _context.Cars.Update(car);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null) return;

            _context.Cars.Remove(car);
            _context.SaveChanges();
        }

    }
}
