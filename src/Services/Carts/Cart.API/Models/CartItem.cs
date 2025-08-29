namespace Cart.API.Models
{
    public class CartItem
    {
        public Guid CourseId { get; set; } 
        public string CourseName { get; set; } 
        public decimal Price { get; set; }
    }
}
