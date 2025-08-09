namespace Course.Infrastructure.Data.Configuration;

public class CourseConfiguration : IEntityTypeConfiguration<Domain.Models.Course>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Course> builder)
    {
        // Table configuration
        builder.ToTable("Courses");

        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever(); // Assuming Guid is generated in domain

        // Basic properties
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .HasColumnType("nvarchar(2000)");

        builder.Property(c => c.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        builder.Property(c => c.PosterUrl)
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        builder.Property(c => c.InstructorId)
            .IsRequired();

        builder.Property(c => c.CourseLevel)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("Level");

        builder.Property(c => c.CourseStatus)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("Status");

        builder.Property(c => c.Language)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)")
            .HasDefaultValue("English");

        builder.Property(c => c.DurationInMinutes)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.CategoryId)
            .IsRequired();


        builder.HasOne(c => c.Category)
            .WithMany(c=>c.Courses) 
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict) 
            .HasConstraintName("FK_Courses_Categories");

        builder.HasMany(c => c.Sections)
            .WithOne(c=>c.Course)
            .HasForeignKey(c=>c.CourseId) 
            .OnDelete(DeleteBehavior.Cascade) 
            .HasConstraintName("FK_Sections_Courses");

        builder.HasMany(c => c.Reviews)
            .WithOne(c=>c.Course) 
            .HasForeignKey(c=>c.CourseId) 
            .OnDelete(DeleteBehavior.Cascade) 
            .HasConstraintName("FK_Reviews_Courses");

        builder.HasIndex(c => c.CategoryId)
            .HasDatabaseName("IX_Courses_CategoryId");

        builder.HasIndex(c => c.InstructorId)
            .HasDatabaseName("IX_Courses_InstructorId");

        builder.HasIndex(c => c.CourseStatus)
            .HasDatabaseName("IX_Courses_Status");

        builder.HasIndex(c => c.CourseLevel)
            .HasDatabaseName("IX_Courses_Level");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Courses_CreatedAt");

        builder.HasIndex(c => c.Price)
            .HasDatabaseName("IX_Courses_Price");

        builder.HasIndex(c => new { c.CategoryId, c.CourseStatus, c.CourseLevel })
            .HasDatabaseName("IX_Courses_Category_Status_Level");

        builder.HasIndex(c => new { c.InstructorId, c.CourseStatus })
            .HasDatabaseName("IX_Courses_Instructor_Status");
    }
}