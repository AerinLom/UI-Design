using UI_Design.Models;

namespace UI_Design.Models
{
    public class UserList
    {
        public int UserId { get; set; } // Foreign key for UserProfile
        public int Id { get; set; } // Foreign key for Product
        public UserModel User { get; set; } // Navigation property
        public ProductModel Product { get; set; } // Navigation property
    }
}
