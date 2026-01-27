using Microsoft.AspNetCore.Mvc;
using NexusDev.Data;
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

        [HttpGet] public List<Car> Get() => _repo.GetAll();
        [HttpGet("{id}")] public Car? Get(int id) => _repo.GetById(id);
        [HttpPost] public Car Post([FromBody] Car car) => _repo.Add(car);
        [HttpPut("{id}")] public void Put(int id, [FromBody] Car car) { car.Id = id; _repo.Update(car); }
        [HttpDelete("{id}")] public void Delete(int id) => _repo.Delete(id);
    }
}
