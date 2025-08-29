namespace Cart.API.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(i => i.Price);
    }
}
