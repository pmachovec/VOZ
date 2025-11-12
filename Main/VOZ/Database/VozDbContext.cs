using Microsoft.EntityFrameworkCore;
using VOZ.Database.Entities;

namespace VOZ.Database;

public sealed class VozDbContext(DbContextOptions<VozDbContext> _options) : DbContext(_options)
{
    public DbSet<Answer> Answers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<QuestionImage> QuestionImages { get; set; }

    public DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(answerBuilder =>
        {
            answerBuilder.HasKey(answer => answer.Id);
            answerBuilder.Property(answer => answer.Id).ValueGeneratedNever();

            answerBuilder
                .HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(categoryBuilder =>
        {
            categoryBuilder.HasKey(category => category.Id);
            categoryBuilder.Property(category => category.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Question>(questionBuilder =>
        {
            questionBuilder.HasKey(question => question.Id);
            questionBuilder.Property(question => question.Id).ValueGeneratedNever();

            questionBuilder
                .HasOne(question => question.Subcategory)
                .WithMany(subcategory => subcategory.Questions)
                .HasForeignKey(question => question.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QuestionImage>(questionImageBuilder =>
        {
            questionImageBuilder.HasKey(questionImage => questionImage.Id);
            questionImageBuilder.Property(questionImage => questionImage.Id).ValueGeneratedNever();

            questionImageBuilder
                .HasOne(questionImage => questionImage.Question)
                .WithOne(question => question.QuestionImage)
                .HasForeignKey<QuestionImage>(questionImage => questionImage.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Subcategory>(subcategoryBuilder =>
        {
            subcategoryBuilder.HasKey(subcategory => subcategory.Id);
            subcategoryBuilder.Property(subcategory => subcategory.Id).ValueGeneratedNever();

            subcategoryBuilder
                .HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .HasForeignKey(subcategory => subcategory.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
