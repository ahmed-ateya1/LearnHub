namespace Order.API.Models
{
    public class OrderItems
    {
        public Guid Id { get; set; } 
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public Guid CourseId { get; set; }
        public decimal Price { get; set; }
    }
}
