using Microsoft.EntityFrameworkCore;
using VOZ.Database.Entities;

namespace VOZ.Database;

internal sealed class VOZDbContext(DbContextOptions<VOZDbContext> _options) : DbContext(_options)
{
    public DbSet<Answer> Answers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<QuestionImage> QuestionImages { get; set; }

    public DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Answer>(answerBuilder =>
        {
            _ = answerBuilder.HasKey(answer => answer.Id);
            _ = answerBuilder.Property(answer => answer.Id).ValueGeneratedNever();

            _ = answerBuilder
                .HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<Category>(categoryBuilder =>
        {
            _ = categoryBuilder.HasKey(category => category.Id);
            _ = categoryBuilder.Property(category => category.Id).ValueGeneratedNever();
        });

        _ = modelBuilder.Entity<Question>(questionBuilder =>
        {
            _ = questionBuilder.HasKey(question => question.Id);
            _ = questionBuilder.Property(question => question.Id).ValueGeneratedNever();

            _ = questionBuilder
                .HasOne(question => question.Subcategory)
                .WithMany(subcategory => subcategory.Questions)
                .HasForeignKey(question => question.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<QuestionImage>(questionImageBuilder =>
        {
            _ = questionImageBuilder.HasKey(questionImage => questionImage.Id);
            _ = questionImageBuilder.Property(questionImage => questionImage.Id).ValueGeneratedNever();

            _ = questionImageBuilder
                .HasOne(questionImage => questionImage.Question)
                .WithOne(question => question.QuestionImage)
                .HasForeignKey<QuestionImage>(questionImage => questionImage.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<Subcategory>(subcategoryBuilder =>
        {
            _ = subcategoryBuilder.HasKey(subcategory => subcategory.Id);
            _ = subcategoryBuilder.Property(subcategory => subcategory.Id).ValueGeneratedNever();

            _ = subcategoryBuilder
                .HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .HasForeignKey(subcategory => subcategory.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
