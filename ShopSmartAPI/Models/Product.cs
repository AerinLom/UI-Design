namespace ShopSmartAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Retailer { get; set; }
        public string ImagePath { get; set; }
        public ICollection<UserList> UserLists { get; set; }
    }
}
