using Microsoft.AspNetCore.Mvc;
using NexusDev.Data;
using NexusDev.Dtos;
using NexusDev.Models;

namespace NexusDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _repo;

        public CarController(ICarRepository repo)
        {
            _repo = repo;
        }

        [HttpGet] 
        public async Task<List<Car>> Get() => await _repo.GetAllAsync();

        [HttpGet("{id}")] 
        public async Task<Car?> Get(int id) => await _repo.GetByIdAsync(id);

        [HttpPost] 
        public async Task<CarDto> Post(CreateCarDto dto)
        {
            var car = new Car
            {
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year
            };

            var savedCar = await _repo.AddAsync(car);

            return new CarDto
            {
                Id = savedCar.Id,
                Make = savedCar.Make,
                Model = savedCar.Model,
                Year = savedCar.Year
            };
        } 

        [HttpPut("{id}")] 
        public void Put(int id, [FromBody] Car car) { car.Id = id; _repo.UpdateAsync(car); }

        [HttpDelete("{id}")] 
        public void Delete(int id) => _repo.DeleteAsync(id);
    }
}
