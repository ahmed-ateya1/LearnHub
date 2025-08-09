namespace Course.Infrastructure.Data.Configuration;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("Sections");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        builder.Property(s => s.CourseId)
            .IsRequired();

        builder.HasOne(s => s.Course)
            .WithMany(c => c.Sections)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Sections_Courses");

        builder.HasMany(s => s.Lectures)
            .WithOne(l => l.Section)
            .HasForeignKey(l => l.SectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Lectures_Sections");

        builder.HasIndex(s => s.CourseId)
            .HasDatabaseName("IX_Sections_CourseId");

        builder.HasIndex(s => s.Title)
            .HasDatabaseName("IX_Sections_Title");
    }
}
