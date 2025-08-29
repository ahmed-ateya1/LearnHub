using Order.API.Models.Enums;

namespace Order.API.Dtos
{
    public class OrderUpdateRequest
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
