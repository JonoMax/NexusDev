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

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Car> AddAsync(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            return car;
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null) return;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

    }
}
