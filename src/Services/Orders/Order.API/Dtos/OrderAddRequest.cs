namespace Order.API.Dtos
{
    public class OrderAddRequest
    {
        public Guid UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
