using UI_Design.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI_Design.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Retailer { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public ICollection<UserList> UserLists { get; set; }
    }
}