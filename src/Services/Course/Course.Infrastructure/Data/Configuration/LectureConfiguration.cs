namespace Course.Infrastructure.Data.Configuration;

public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lectures");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        builder.Property(l => l.VideoUrl)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        builder.Property(l => l.SectionId)
            .IsRequired();

        builder.HasOne(l => l.Section)
            .WithMany(s => s.Lectures)
            .HasForeignKey(l => l.SectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Lectures_Sections");

        builder.HasMany(l => l.Quizzes)
            .WithOne(q => q.Lecture)
            .HasForeignKey(q => q.LectureId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Quizzes_Lectures");

        builder.HasIndex(l => l.SectionId)
            .HasDatabaseName("IX_Lectures_SectionId");

        builder.HasIndex(l => l.Title)
            .HasDatabaseName("IX_Lectures_Title");
    }
}