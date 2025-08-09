namespace Course.Infrastructure.Data.Configuration;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .ValueGeneratedNever();

        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        builder.Property(q => q.TotalMarks)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(q => q.PassingMarks)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(q => q.TimeLimitInMinutes)
            .HasColumnType("int");

        builder.Property(q => q.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(q => q.CreatedBy)
            .IsRequired();

        builder.Property(q => q.LectureId)
            .IsRequired();

        builder.HasOne(q => q.Lecture)
            .WithMany(l => l.Quizzes)
            .HasForeignKey(q => q.LectureId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Quizzes_Lectures");

        builder.HasMany(q => q.Questions)
            .WithOne(qu => qu.Quiz)
            .HasForeignKey(qu => qu.QuizId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Questions_Quizzes");

        builder.HasIndex(q => q.LectureId)
            .HasDatabaseName("IX_Quizzes_LectureId");

        builder.HasIndex(q => q.CreatedBy)
            .HasDatabaseName("IX_Quizzes_CreatedBy");

        builder.HasIndex(q => q.CreatedAt)
            .HasDatabaseName("IX_Quizzes_CreatedAt");

        builder.HasCheckConstraint("CK_Quizzes_TotalMarks_Positive", "[TotalMarks] > 0");
        builder.HasCheckConstraint("CK_Quizzes_PassingMarks_Valid", "[PassingMarks] > 0 AND [PassingMarks] <= [TotalMarks]");
        builder.HasCheckConstraint("CK_Quizzes_TimeLimit_Positive", "[TimeLimitInMinutes] IS NULL OR [TimeLimitInMinutes] > 0");
    }
}
