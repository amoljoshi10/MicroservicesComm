namespace CartAPI.Controllers
{
    public class CartItemLine
    {
        
        public int ItemId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
    }
}