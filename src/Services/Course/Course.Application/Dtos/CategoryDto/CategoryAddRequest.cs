namespace Course.Application.Dtos.CategoryDto
{
    public class CategoryAddRequest
    {
        public string Name { get; set; }
        public Guid? BaseCategoryId { get; set; }
    }
}
