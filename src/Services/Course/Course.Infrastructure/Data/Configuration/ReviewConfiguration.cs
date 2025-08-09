namespace Course.Infrastructure.Data.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.Content)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)");

        builder.Property(r => r.Rating)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(r => r.CourseId)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.ParentReviewId)
            .IsRequired(false);

        builder.HasOne(r => r.Course)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reviews_Courses");

        builder.HasOne(r => r.ParentReview)
            .WithMany(r => r.Replies)
            .HasForeignKey(r => r.ParentReviewId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Reviews_ParentReviews");

        builder.HasIndex(r => r.CourseId)
            .HasDatabaseName("IX_Reviews_CourseId");

        builder.HasIndex(r => r.UserId)
            .HasDatabaseName("IX_Reviews_UserId");

        builder.HasIndex(r => r.ParentReviewId)
            .HasDatabaseName("IX_Reviews_ParentReviewId");

        builder.HasIndex(r => r.Rating)
            .HasDatabaseName("IX_Reviews_Rating");

        builder.HasIndex(r => r.CreatedAt)
            .HasDatabaseName("IX_Reviews_CreatedAt");

        builder.HasIndex(r => new { r.CourseId, r.Rating })
            .HasDatabaseName("IX_Reviews_Course_Rating");

        builder.HasIndex(r => new { r.UserId, r.CourseId })
            .HasDatabaseName("IX_Reviews_User_Course");

        builder.HasCheckConstraint("CK_Reviews_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5");
        
        
    }
}
