using Microsoft.EntityFrameworkCore;
using NexusDev.Data;
using NexusDev.Models;
using Xunit;
// Add this using directive to fix CS1061
using Microsoft.EntityFrameworkCore.InMemory;

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
        public void Add_SavesCarAndReturnsIt()
        {
            var car = new Car { Make = "Honda", Model = "Civic", Year = 2018, Price = 18000m };

            var added = _repo.Add(car);

            Assert.NotNull(added);
            Assert.True(added.Id != 0 || _context.Cars.Any(c => c.Make == "Honda"));
            var fromDb = _context.Cars.FirstOrDefault(c => c.Make == "Honda" && c.Model == "Civic");
            Assert.NotNull(fromDb);
        }

        [Fact]
        public void GetAll_ReturnsAllCars()
        {
            _repo.Add(new Car { Make = "A", Model = "M1", Year = 2000, Price = 1m });
            _repo.Add(new Car { Make = "B", Model = "M2", Year = 2001, Price = 2m });

            var all = _repo.GetAll();

            Assert.Equal(2, all.Count);
        }

        [Fact]
        public void GetById_ReturnsCorrectOrNull()
        {
            var added = _repo.Add(new Car { Make = "Find", Model = "Me", Year = 2012, Price = 5000m });

            var found = _repo.GetById(added.Id);
            var notFound = _repo.GetById(9999);

            Assert.NotNull(found);
            Assert.Equal(added.Id, found!.Id);
            Assert.Null(notFound);
        }

        [Fact]
        public void Update_PersistsChanges()
        {
            var added = _repo.Add(new Car { Make = "Old", Model = "M", Year = 2000, Price = 10m });

            added.Make = "Updated";
            added.Price = 99m;
            _repo.Update(added);

            var fromDb = _context.Cars.Find(added.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Updated", fromDb!.Make);
            Assert.Equal(99m, fromDb.Price);
        }

        [Fact]
        public void Delete_RemovesEntity()
        {
            var added = _repo.Add(new Car { Make = "Trash", Model = "T", Year = 1990, Price = 1m });

            _repo.Delete(added.Id);

            var fromDb = _context.Cars.Find(added.Id);
            Assert.Null(fromDb);
        }
    }
}