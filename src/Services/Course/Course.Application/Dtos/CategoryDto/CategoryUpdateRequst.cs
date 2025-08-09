namespace Course.Application.Dtos.CategoryDto
{
    public class CategoryUpdateRequst
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? BaseCategoryId { get; set; }

    }
}
