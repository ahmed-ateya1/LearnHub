namespace Order.API.Dtos
{
    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public decimal Price { get; set; }
    }
}
