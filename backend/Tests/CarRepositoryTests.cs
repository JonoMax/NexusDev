using NexusDev.Data;
using NexusDev.Models;
using System.Reflection;
using Xunit;

namespace NexusDev.Tests
{
    public class CarRepositoryTests : IDisposable
    {
        private readonly CarRepository _repo;

        public CarRepositoryTests()
        {
            // Reset private static fields on CarRepository to isolate tests
            var repoType = typeof(CarRepository);
            var carsField = repoType.GetField("_cars", BindingFlags.NonPublic | BindingFlags.Static);
            var nextIdField = repoType.GetField("_nextId", BindingFlags.NonPublic | BindingFlags.Static);

            carsField?.SetValue(null, new List<Car>());
            nextIdField?.SetValue(null, 1);

            _repo = new CarRepository();
        }

        public void Dispose()
        {
            // Ensure static state cleaned after test class usage
            var repoType = typeof(CarRepository);
            var carsField = repoType.GetField("_cars", BindingFlags.NonPublic | BindingFlags.Static);
            var nextIdField = repoType.GetField("_nextId", BindingFlags.NonPublic | BindingFlags.Static);

            carsField?.SetValue(null, new List<Car>());
            nextIdField?.SetValue(null, 1);
        }

        [Fact]
        public void Add_AssignsIdAndReturnsCar()
        {
            var car = new Car { Make = "Toyota", Model = "Corolla", Year = 2020, Price = 25000m };

            var added = _repo.Add(car);

            Assert.NotNull(added);
            Assert.Equal(1, added.Id);
            Assert.Contains(added, _repo.GetAll());
        }

        [Fact]
        public void GetAll_ReturnsAllCars()
        {
            var car1 = _repo.Add(new Car { Make = "A", Model = "M1", Year = 2000, Price = 1m });
            var car2 = _repo.Add(new Car { Make = "B", Model = "M2", Year = 2001, Price = 2m });

            var all = _repo.GetAll();

            Assert.Equal(2, all.Count);
            Assert.Contains(car1, all);
            Assert.Contains(car2, all);
        }

        [Fact]
        public void GetById_ReturnsCorrectOrNull()
        {
            var car = _repo.Add(new Car { Make = "X", Model = "Y", Year = 2010, Price = 100m });

            var found = _repo.GetById(car.Id);
            var notFound = _repo.GetById(999);

            Assert.NotNull(found);
            Assert.Equal(car.Id, found!.Id);
            Assert.Null(notFound);
        }

        [Fact]
        public void Update_ReplacesExistingCar()
        {
            var car = _repo.Add(new Car { Make = "Old", Model = "Model", Year = 1999, Price = 10m });

            car.Make = "New";
            car.Price = 20m;
            _repo.Update(car);

            var updated = _repo.GetById(car.Id);
            Assert.NotNull(updated);
            Assert.Equal("New", updated!.Make);
            Assert.Equal(20m, updated.Price);
        }

        [Fact]
        public void Delete_RemovesCar()
        {
            var car = _repo.Add(new Car { Make = "ToDelete", Model = "D", Year = 2005, Price = 5m });

            _repo.Delete(car.Id);

            Assert.Null(_repo.GetById(car.Id));
            Assert.Empty(_repo.GetAll());
        }

        [Fact]
        public void DisplayInfo_ReturnsFormattedString()
        {
            var car = new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Price = 25000m };

            var s = car.displayInfo();

            var expected = $"{car.Year} {car.Make} {car.Model} - ${car.Price}";
            Assert.Equal(expected, s);
        }
    }
}