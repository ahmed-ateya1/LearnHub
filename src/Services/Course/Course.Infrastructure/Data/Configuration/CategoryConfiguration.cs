namespace Course.Infrastructure.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();
          
            builder.HasOne(c=>c.BaseCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.BaseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Courses)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Courses_Categories");


        }
    }
}
