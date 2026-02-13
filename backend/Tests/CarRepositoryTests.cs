using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NexusDev.Data;
using NexusDev.Models;
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
        public async Task Add_AssignsIdAndReturnsCar()
        {
            var car = new Car { Make = "Toyota", Model = "Corolla", Year = 2020, Price = 25000m };

            var added = await _repo.AddAsync(car);

            Assert.NotNull(added);
            Assert.NotEqual(0, added.Id);
            var all = await _repo.GetAllAsync();
            Assert.Contains(all, c => c.Id == added.Id);
        }

        [Fact]
        public async Task GetAll_ReturnsAllCars()
        {
            var car1 = await _repo.AddAsync(new Car { Make = "A", Model = "M1", Year = 2000, Price = 1m });
            var car2 = await _repo.AddAsync(new Car { Make = "B", Model = "M2", Year = 2001, Price = 2m });

            var all = await _repo.GetAllAsync();

            Assert.Equal(2, all.Count);
            Assert.Contains(all, c => c.Id == car1.Id);
            Assert.Contains(all, c => c.Id == car2.Id);
        }

        [Fact]
        public async Task GetById_ReturnsCorrectOrNull()
        {
            var car = await _repo.AddAsync(new Car { Make = "X", Model = "Y", Year = 2010, Price = 100m });

            var found = await _repo.GetByIdAsync(car.Id);
            var notFound = await _repo.GetByIdAsync(999);

            Assert.NotNull(found);
            Assert.Equal(car.Id, found!.Id);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task Update_ReplacesExistingCar()
        {
            var car = await _repo.AddAsync(new Car { Make = "Old", Model = "Model", Year = 1999, Price = 10m });

            car.Make = "New";
            car.Price = 20m;
            await _repo.UpdateAsync(car);

            var updated = await _repo.GetByIdAsync(car.Id);
            Assert.NotNull(updated);
            Assert.Equal("New", updated!.Make);
            Assert.Equal(20m, updated.Price);
        }

        [Fact]
        public async Task Delete_RemovesCar()
        {
            var car = await _repo.AddAsync(new Car { Make = "ToDelete", Model = "D", Year = 2005, Price = 5m });

            await _repo.DeleteAsync(car.Id);

            var found = await _repo.GetByIdAsync(car.Id);
            var all = await _repo.GetAllAsync();

            Assert.Null(found);
            Assert.Empty(all);
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