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

        // GET: api/products/cheapest/{name}
        [HttpGet("product/{name}")]
        public async Task<ActionResult<Product>> GetProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            // Find products that match the given name (case insensitive)
            var products = await _context.Products
                .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
                .OrderBy(p => p.Price)  // Sort by price in ascending order (cheapest first)
                .ToListAsync(); // Take the first one (the cheapest)

            if (products == null)
            {
                return NotFound("No product found with the given name.");
            }

            return Ok(products); // Return the cheapest product found
        }

        // GET: api/products/type/{type}
        [HttpGet("products/{retailer}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByRetailer(string retailer)
        {
            // Validate type
            if (string.IsNullOrWhiteSpace(retailer))
            {
                return BadRequest("Product type cannot be empty.");
            }

            var products = await _context.Products
                .Where(p => p.Retailer.ToLower() == retailer.ToLower()) // Case-insensitive comparison
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound("No products found for the specified type.");
            }

            return Ok(products);
        }

        // POST: api/products/cheapest
        [HttpPost("cheapest")]
        public async Task<ActionResult<List<Product>>> GetCheapestProducts([FromBody] List<string> names)
        {
            if (names == null || !names.Any())
            {
                return BadRequest("Product names cannot be empty.");
            }

            var cheapestProducts = new List<Product>();

            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                // Find the cheapest product matching the name
                var product = await _context.Products
                    .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
                    .OrderBy(p => p.Price)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    cheapestProducts.Add(product);
                }
            }

            if (!cheapestProducts.Any())
            {
                return NotFound("No products found for the given names.");
            }

            return Ok(cheapestProducts);
        }

    }
}
