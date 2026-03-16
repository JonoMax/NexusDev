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
        public async Task<ActionResult<List<CarDto>>> Get() /*=> await _repo.GetAllAsync();*/
        {
           var cars = await _repo.GetAllAsync();

           var carsDto = cars.Select(c => new CarDto
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Price = c.Price
            }).ToList();

            return Ok(carsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetById(int id)
        {
            var car = await _repo.GetByIdAsync(id);

            if (car == null)
                return NotFound();

            var carDto = new CarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Price = car.Price
            };

            return Ok(carDto);
        }

        [HttpPost] 
        public async Task<ActionResult<CarDto>> Create(CreateCarDto dto)
        {
            var car = new Car
            {
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                Price = dto.Price
            };

            var savedCar = await _repo.AddAsync(car);

            var carDto = new CarDto
            {
                Id = savedCar.Id,
                Make = savedCar.Make,
                Model = savedCar.Model,
                Year = savedCar.Year,
                Price = savedCar.Price
            };

            return CreatedAtAction(nameof(GetById), new { id = carDto.Id }, carDto);
        }

        [HttpPut("{id}")]
        /*public void Put(int id, [FromBody] Car car) { car.Id = id; _repo.UpdateAsync(car); }*/
        public async Task<IActionResult> Update(int id, UpdateCarDto dto)
        {
            var existingCar = await _repo.GetByIdAsync(id);

            if (existingCar == null) 
                return NotFound(new { message = $"Car with id {id} not found" });

            existingCar.Make = dto.Make;
            existingCar.Model = dto.Model;
            existingCar.Price = dto.Price;

            await _repo.UpdateAsync(existingCar);

            return NoContent();
        }

        [HttpDelete("{id}")]
        /*public void Delete(int id) => _repo.DeleteAsync(id);*/
        public async Task<IActionResult> Delete(int id)
        {
           var deleted = await _repo.DeleteAsync(id);

            if(!deleted)
                return NotFound(new { message = $"Car with id {id} not found" });
            
            return NoContent();
        }
    }
}
