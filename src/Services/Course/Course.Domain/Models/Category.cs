namespace Course.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? BaseCategoryId { get; set; }
        public virtual Category? BaseCategory { get; set; }

        public virtual ICollection<Category> SubCategories { get; set; } = [];
        public virtual ICollection<Course> Courses { get; set; } = [];
    }
}
