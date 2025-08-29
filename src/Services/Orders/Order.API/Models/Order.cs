using Order.API.Models.Enums;

namespace Order.API.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItems> OrderItems { get; set; } = [];
    }
}
