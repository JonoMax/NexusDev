using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NexusDev.Data;
using NexusDev.Models;
using Xunit;

namespace NexusDev.Tests
{
    public class CarRepositoryEFTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CarRepositoryEF _repo;

        public CarRepositoryEFTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new CarRepositoryEF(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Add_SavesCarAndReturnsIt()
        {
            var car = new Car { Make = "Honda", Model = "Civic", Year = 2018, Price = 18000m };

            var added = await _repo.AddAsync(car);

            Assert.NotNull(added);
            Assert.NotEqual(0, added.Id);
            var fromDb = await _context.Cars.FirstOrDefaultAsync(c => c.Make == "Honda" && c.Model == "Civic");
            Assert.NotNull(fromDb);
        }

        [Fact]
        public async Task GetAll_ReturnsAllCars()
        {
            await _repo.AddAsync(new Car { Make = "A", Model = "M1", Year = 2000, Price = 1m });
            await _repo.AddAsync(new Car { Make = "B", Model = "M2", Year = 2001, Price = 2m });

            var all = await _repo.GetAllAsync();

            Assert.Equal(2, all.Count);
        }

        [Fact]
        public async Task GetById_ReturnsCorrectOrNull()
        {
            var added = await _repo.AddAsync(new Car { Make = "Find", Model = "Me", Year = 2012, Price = 5000m });

            var found = await _repo.GetByIdAsync(added.Id);
            var notFound = await _repo.GetByIdAsync(9999);

            Assert.NotNull(found);
            Assert.Equal(added.Id, found!.Id);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task Update_PersistsChanges()
        {
            var added = await _repo.AddAsync(new Car { Make = "Old", Model = "M", Year = 2000, Price = 10m });

            added.Make = "Updated";
            added.Price = 99m;
            await _repo.UpdateAsync(added);

            var fromDb = await _context.Cars.FindAsync(added.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Updated", fromDb!.Make);
            Assert.Equal(99m, fromDb.Price);
        }

        [Fact]
        public async Task Delete_RemovesEntity()
        {
            var added = await _repo.AddAsync(new Car { Make = "Trash", Model = "T", Year = 1990, Price = 1m });

            await _repo.DeleteAsync(added.Id);

            var fromDb = await _context.Cars.FindAsync(added.Id);
            Assert.Null(fromDb);
        }
    }
}