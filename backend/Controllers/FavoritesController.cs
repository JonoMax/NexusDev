using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusDev.Data;
using NexusDev.Models;

namespace NexusDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _context;
        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{carId}")]
        public async Task<IActionResult> AddFavorites(int carId, [FromQuery] int userId)
        {
            var exists = await _context.Favorites
            .AnyAsync(f => f.CarId == carId && f.UserId == userId);

            if (exists)
            {
                return BadRequest("Already favorited");
            }

            var favorite = new Favorite
            {
                UserId = userId,
                CarId = carId
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            {
                var cars = await _context.Favorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.Car)
                    .ToListAsync();

                return Ok(cars);
            }
        }
        [HttpDelete("{carId}")]
        public async Task<IActionResult> RemoveFavorite(int carId, int userId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.CarId == carId && f.UserId == userId);

            if (favorite == null)
            {
                return NotFound();
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
