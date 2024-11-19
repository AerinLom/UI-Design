using Microsoft.AspNetCore.Mvc;
using ShopSmartAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopSmartAPI.Data;
using ShopSmartAPI.Data;

namespace TazziexTreats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            // Retrieve all user profiles from the database
            return await _context.UserProfiles.ToListAsync();
        }

        // GET: api/UserProfile/Id/{userId}
        [HttpGet("Id/{userId}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByID(int userId)
        {
            try
            {
                // Retrieve a user profile by userId from the database
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);

                if (userProfile == null)
                {
                    // Return 404 Not Found if user profile with specified userId is not found
                    return NotFound();
                }

                // Return the user profile if found
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                // Example: _logger.LogError(ex, $"Error occurred while fetching UserProfile with userId {userId}");
                return StatusCode(500, "An error occurred while fetching the user profile.");
            }
        }

        // GET: api/UserProfile/Username/{username}
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByUsername(string username)
        {
            try
            {
                // Retrieve a user profile by username from the database
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == username);

                if (userProfile == null)
                {
                    // Return 404 Not Found if user profile with specified username is not found
                    return NotFound();
                }

                // Return the user profile if found
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                // Example: _logger.LogError(ex, $"Error occurred while fetching UserProfile with username {username}");
                return StatusCode(500, "An error occurred while fetching the user profile.");
            }
        }

        // PUT: api/UserProfile/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfileInputModel updatedUserProfile)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                // Return 404 Not Found if user profile with specified id is not found
                return NotFound("UserProfile not found.");
            }

            // Update user profile fields based on the provided updatedUserProfile
            if (updatedUserProfile.Username != null)
            {
                userProfile.Username = updatedUserProfile.Username;
            }
            if (updatedUserProfile.Password != null)
            {
                userProfile.Password = updatedUserProfile.Password;
            }
            if (updatedUserProfile.Email != null)
            {
                userProfile.Email = updatedUserProfile.Email;
            }

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    // Return 404 Not Found if user profile with specified id is not found
                    return NotFound("UserProfile not found.");
                }
                else
                {
                    throw; // Bubble up the exception for logging purposes
                }
            }

            // Return HTTP 204 No Content after successful update
            return NoContent();
        }

        // POST: api/UserProfile
        [HttpPost("UserProfile")]
        public async Task<IActionResult> PostUserProfile([FromBody] UserProfileInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Return 400 Bad Request if model state is not valid
                    return BadRequest(ModelState);
                }

                // Add a new user profile to the database
                _context.UserProfiles.Add(new UserProfile
                {
                    UserId = model.UserId,  // Ensure UserId is correctly assigned
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email
                    // Add other properties as needed
                });

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return HTTP 200 OK after successful creation
                return Ok();
            }
            catch (Exception ex)
            {
                // Log and return 500 Internal Server Error for any exceptions
                // Example: _logger.LogError(ex, "Error occurred while saving UserProfile");
                return StatusCode(500, "An error occurred while saving the user profile.");
            }
        }

        // DELETE: api/UserProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                // Return 404 Not Found if user profile with specified id is not found
                return NotFound();
            }

            // Remove user profile from the database
            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            // Return HTTP 204 No Content after successful deletion
            return NoContent();
        }

        // Check if a user profile with specified id exists
        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.UserId == id);
        }
    }
}
