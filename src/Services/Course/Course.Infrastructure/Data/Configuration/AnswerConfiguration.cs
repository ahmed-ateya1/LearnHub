public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.AnswerText)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        builder.Property(a => a.IsCorrect)
            .IsRequired()
            .HasColumnType("bit");

        builder.Property(a => a.Order)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(a => a.QuestionId)
            .IsRequired();

        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Answers_Questions");

        builder.HasIndex(a => a.QuestionId)
            .HasDatabaseName("IX_Answers_QuestionId");

        builder.HasIndex(a => new { a.QuestionId, a.Order })
            .HasDatabaseName("IX_Answers_Question_Order")
            .IsUnique();

        builder.HasIndex(a => new { a.QuestionId, a.IsCorrect })
            .HasDatabaseName("IX_Answers_Question_IsCorrect");

        builder.HasCheckConstraint("CK_Answers_Order_Positive", "[Order] > 0");
    }
}
