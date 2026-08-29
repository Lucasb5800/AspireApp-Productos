using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspireApp_Productos.ApiService.Data;
using AspireApp_Productos.ApiService.Models;

namespace AspireApp_Productos.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _db.Products.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            product.CreatedAt = DateTime.UtcNow;
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null)
                return NotFound();

            _db.Products.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
