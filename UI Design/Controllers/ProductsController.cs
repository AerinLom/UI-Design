using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI_Design.Models;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;

public class ProductController : Controller
{
    private readonly HttpClient _httpClient;

    public ProductController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        // Call the API to fetch products
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7076/api/Products");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response to the list of products
                var responseData = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);
            }
            else
            {
                // Handle unsuccessful API calls
                ViewBag.ErrorMessage = "Could not retrieve products from the API.";
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
        }

        // Pass the products to the view
        return View(products);
    }

    public async Task<IActionResult> Latest()
    {
        // Call the API to fetch products
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7076/api/Products/last");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response to the list of products
                var responseData = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);
            }
            else
            {
                // Handle unsuccessful API calls
                ViewBag.ErrorMessage = "Could not retrieve products from the API.";
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error: {ex.Message}";
        }

        // Pass the products to the view
        return View(products);
    }
    public async Task<IActionResult> CakeView()
    {
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7076/api/Products/type/Cake");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);

            return View(products);
        }
        catch (HttpRequestException)
        {
            // Handle HTTP request errors
            return View("Error");
        }
        catch (JsonException)
        {
            // Handle JSON parsing errors
            return View("Error");
        }
        catch (Exception)
        {
            // Handle other unexpected errors
            return View("Error");
        }
    }

    public async Task<IActionResult> CupcakeView()
    {
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7194/api/Products/type/Cupcake");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);

            return View(products);
        }
        catch (HttpRequestException)
        {
            // Handle HTTP request errors
            return View("Error");
        }
        catch (JsonException)
        {
            // Handle JSON parsing errors
            return View("Error");
        }
        catch (Exception)
        {
            // Handle other unexpected errors
            return View("Error");
        }
    }
    public async Task<IActionResult> CookieView()
    {
        List<ProductModel> products = new List<ProductModel>();
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:7194/api/Products/type/Cookie");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<ProductModel>>(responseData);

            return View(products);
        }
        catch (HttpRequestException)
        {
            // Handle HTTP request errors
            return View("Error");
        }
        catch (JsonException)
        {
            // Handle JSON parsing errors
            return View("Error");
        }
        catch (Exception)
        {
            // Handle other unexpected errors
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToCart(int Id)  // Make sure this matches your form's input name
    {
        // Retrieve userId from session or wherever it's stored
        var userId = HttpContext.Session.GetString("UserId");

        if (userId == null)
        {
            return RedirectToAction("Index", "Home");
        }

        try
        {
            // Prepare UserCart object to send in the POST request
            var userCart = new
            {
                userId = int.Parse(userId),
                Id = Id // Ensure this matches the form input
            };

            // Serialize userCart into JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(userCart), Encoding.UTF8, "application/json");

            // Send POST request to the API endpoint for adding a product to the cart
            var apiURL = "https://localhost:7076/api/UserCart";
            var response = await _httpClient.PostAsync(apiURL, jsonContent);

            // Check if request was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Failed to add product: {errorResponse}" });
            }
        }
        catch (HttpRequestException ex)
        {
            return Json(new { success = false, message = $"Failed to add product: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Failed to add product: {ex.Message}" });
        }
    }
    public async Task<IActionResult> MyCart()
    {
        // Check if the user is authenticated via session
        if (HttpContext.Session.GetString("UserId") == null)
        {
            return RedirectToAction("Index", "Home"); // Redirect to login if user is not authenticated
        }

        try
        {
            // Get the user ID from the session
            var userId = HttpContext.Session.GetString("UserId");

            // Make a GET request to the API to fetch the user's cart items
            var response = await _httpClient.GetAsync($"https://localhost:7194/api/UserCart/MyCart?userId={userId}");

            // Check if the response was successful
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Log the raw JSON content for debugging
                Console.WriteLine("Raw JSON response:");
                Console.WriteLine(content);

                // Deserialize the JSON content into a list of UserProduct (or similar DTO)
                var cartItems = JsonConvert.DeserializeObject<List<ProductModel>>(content);

                // Return the view with the list of cart items
                return View(cartItems);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle case where no items were found in the user's cart
                return View("NoCartItemsFound");
            }
            else
            {
                // Handle other HTTP error cases
                return View("Error");
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request errors
            Console.WriteLine($"HTTP request error: {ex.Message}");
            return View("Error");
        }
        catch (JsonException ex)
        {
            // Handle JSON parsing errors
            Console.WriteLine($"JSON parsing error: {ex.Message}");
            return View("Error");
        }
        catch (Exception ex)
        {
            // Handle other unexpected errors
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return View("Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchProducts(string Name)
    {
        // Check if the search query is empty
        if (string.IsNullOrWhiteSpace(Name))
        {
            return BadRequest("Search query 'Name' cannot be empty.");
        }

        try
        {
            // Send GET request to the API endpoint with the productName parameter
            var response = await _httpClient.GetAsync($"https://localhost:7194/api/Products/search/{Uri.EscapeDataString(Name)}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            // Optional: Clean up the content if necessary (similar to your workouts search)
            var cleanedContent = Regex.Unescape(content).Trim('"').Replace("\\", "");

            // Deserialize JSON string into a List<ProductModel>
            var products = JsonConvert.DeserializeObject<List<ProductModel>>(cleanedContent);

            if (products.Count == 0)
            {
                // No products found, redirect to a specific view
                return RedirectToAction("NoProductsFound");
            }

            // Return the view with the list of products
            return View(products);
        }
        catch (HttpRequestException)
        {
            // Handle HTTP request errors
            return RedirectToAction("NoProductsFound");
        }
        catch (JsonException)
        {
            // Handle JSON parsing errors
            return RedirectToAction("NoProductsFound");
        }
        catch (Exception)
        {
            // Handle other unexpected errors
            return RedirectToAction("NoProductsFound");
        }
    }
  
}
