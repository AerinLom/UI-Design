using ShopSmartAPI.Models;

namespace ShopSmartAPI.Models
{
    public class UserList
    {
        public int UserId { get; set; } // Foreign key for UserProfile
        public UserProfile User { get; set; } // Navigation property

        public int Id { get; set; } // Foreign key for Product
        public Product Product { get; set; } // Navigation property
    }
}
