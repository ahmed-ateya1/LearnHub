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

            SeedData(builder);
        }

        private void SeedData(EntityTypeBuilder<Category> builder)
        { 
            builder.HasData(
                new Category
                {
                    Id = Guid.Parse("B5A3C6B0-9CBF-49B7-9C84-75385D694EAC"),
                    Name = "Programming & Development",
                    BaseCategoryId = null
                },
                new Category
                {
                    Id = Guid.Parse("C9C068FB-2A9C-4488-B4B5-1004E9C4A801"),
                    Name = "Business",
                    BaseCategoryId = null
                },
                new Category
                {
                    Id = Guid.Parse("1AB2568E-5C0F-409E-B762-3E3E0FC2B1E3"),
                    Name = "Design",
                    BaseCategoryId = null
                },
                new Category
                {
                    Id = Guid.Parse("A4ADAFD3-9899-43FB-B326-8BC2D041335A"),
                    Name = "Marketing",
                    BaseCategoryId = null
                },

                new Category
                {
                    Id = Guid.Parse("1FDCD95F-24B8-419E-9A16-BF8BEF7AF3D3"),
                    Name = "Web Development",
                    BaseCategoryId = Guid.Parse("B5A3C6B0-9CBF-49B7-9C84-75385D694EAC")
                },
                new Category
                {
                    Id = Guid.Parse("E9E34B50-1BAA-46B2-9FB5-04C6090ABB08"),
                    Name = "Mobile Development",
                    BaseCategoryId = Guid.Parse("B5A3C6B0-9CBF-49B7-9C84-75385D694EAC")
                },
                new Category
                {
                    Id = Guid.Parse("A4E55167-A17F-46C4-87D8-F40E1CEDD130"),
                    Name = "Data Science",
                    BaseCategoryId = Guid.Parse("B5A3C6B0-9CBF-49B7-9C84-75385D694EAC")
                },

                new Category
                {
                    Id = Guid.Parse("8B20252D-936F-4136-9C12-BB1932D802D1"),
                    Name = "Management",
                    BaseCategoryId = Guid.Parse("C9C068FB-2A9C-4488-B4B5-1004E9C4A801")
                },
                new Category
                {
                    Id = Guid.Parse("BC108EC8-8885-4D9C-A544-FB8F6B12E064"),
                    Name = "Finance",
                    BaseCategoryId = Guid.Parse("C9C068FB-2A9C-4488-B4B5-1004E9C4A801")
                }
            );
        }
    }
}
