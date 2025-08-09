namespace Course.Infrastructure.Data.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .ValueGeneratedNever();

        builder.Property(q => q.QuestionText)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)");

        builder.Property(q => q.QuestionType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(q => q.Marks)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(q => q.Order)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(q => q.QuizId)
            .IsRequired();

        builder.HasOne(q => q.Quiz)
            .WithMany(quiz => quiz.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Questions_Quizzes");

        builder.HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Answers_Questions");

        builder.HasMany(q => q.StudentAnswers)
            .WithOne(sa => sa.Question)
            .HasForeignKey(sa => sa.QuestionId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_StudentAnswers_Questions");

        builder.HasIndex(q => q.QuizId)
            .HasDatabaseName("IX_Questions_QuizId");

        builder.HasIndex(q => new { q.QuizId, q.Order })
            .HasDatabaseName("IX_Questions_Quiz_Order")
            .IsUnique();

        builder.HasIndex(q => q.QuestionType)
            .HasDatabaseName("IX_Questions_QuestionType");

        builder.HasCheckConstraint("CK_Questions_Marks_Positive", "[Marks] > 0");
        builder.HasCheckConstraint("CK_Questions_Order_Positive", "[Order] > 0");
    }
}