using Microsoft.AspNetCore.Mvc;
using ShopSmartAPI.Data;
using ShopSmartAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ShopSmartAPI.Data;
using ShopSmartAPI.Models;

namespace ShopSmartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products/cheapest/{name}
        [HttpGet("cheapest/{name}")]
        public async Task<ActionResult<Product>> GetCheapestProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            // Find products that match the given name (case insensitive)
            var products = await _context.Products
                .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
                .OrderBy(p => p.Price)  // Sort by price in ascending order (cheapest first)
                .FirstOrDefaultAsync(); // Take the first one (the cheapest)

            if (products == null)
            {
                return NotFound("No product found with the given name.");
            }

            return Ok(products); // Return the cheapest product found
        }
    }
}
