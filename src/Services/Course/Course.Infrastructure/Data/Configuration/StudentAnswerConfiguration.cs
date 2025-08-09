namespace Course.Infrastructure.Data.Configuration;

public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
{
    public void Configure(EntityTypeBuilder<StudentAnswer> builder)
    {
        builder.ToTable("StudentAnswers");

        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id)
            .ValueGeneratedNever();

        builder.Property(sa => sa.StudentId)
            .IsRequired();

        builder.Property(sa => sa.QuestionId)
            .IsRequired();

        builder.Property(sa => sa.SelectedAnswerID)
            .IsRequired(false);

        builder.Property(sa => sa.AnswerText)
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)");

        builder.Property(sa => sa.IsCorrect)
            .IsRequired()
            .HasColumnType("bit");

        builder.Property(sa => sa.SubmittedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(sa => sa.Question)
            .WithMany(q => q.StudentAnswers)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.NoAction)  
            .HasConstraintName("FK_StudentAnswers_Questions");

        builder.HasOne(sa => sa.Answer)
            .WithMany()
            .HasForeignKey(sa => sa.SelectedAnswerID)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_StudentAnswers_Answers");

        builder.HasIndex(sa => sa.StudentId)
            .HasDatabaseName("IX_StudentAnswers_StudentId");

        builder.HasIndex(sa => sa.QuestionId)
            .HasDatabaseName("IX_StudentAnswers_QuestionId");

        builder.HasIndex(sa => sa.SelectedAnswerID)
            .HasDatabaseName("IX_StudentAnswers_SelectedAnswerID");

        builder.HasIndex(sa => new { sa.StudentId, sa.QuestionId })
            .HasDatabaseName("IX_StudentAnswers_Student_Question")
            .IsUnique();

        builder.HasIndex(sa => sa.SubmittedAt)
            .HasDatabaseName("IX_StudentAnswers_SubmittedAt");

        builder.HasIndex(sa => sa.IsCorrect)
            .HasDatabaseName("IX_StudentAnswers_IsCorrect");
    }
}