namespace Course.Application.Dtos.CategoryDto
{
    public class CategoryUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? BaseCategoryId { get; set; }

    }
}
