﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ShopSmartAPI.Models
{
    [Table("userProfile")] // Maps this class to the "userProfile" table in the database
    public class UserProfile
    {
        [Key] // Specifies that UserId is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the UserId is auto-generated by the database
        public int UserId { get; set; }

        [MaxLength(50)] // Limits the maximum length of Username to 50 characters
        public string? Username { get; set; }

        [MaxLength(255)] // Limits the maximum length of Password to 255 characters
        public string? Password { get; set; }

        [MaxLength(100)] // Limits the maximum length of Email to 100 characters
        public string? Email { get; set; }
        public ICollection<UserList> UserLists { get; set; } // Collection of UserProduct
    }
}