namespace Order.API.Dtos
{
    public class OrderItemDto
    {
        public Guid CourseId { get; set; }
        public decimal Price { get; set; }
    }
}
