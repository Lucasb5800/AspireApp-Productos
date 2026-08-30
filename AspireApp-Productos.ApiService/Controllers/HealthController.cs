using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspireApp_Productos.ApiService.Data;

namespace AspireApp_Productos.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _db;

        public HealthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // simple DB query to validate connectivity
                var count = await _db.Products.CountAsync();
                return Ok(new { api = "ok", db = "ok", productCount = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { api = "ok", db = "error", error = ex.Message });
            }
        }
    }
}
