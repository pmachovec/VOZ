using Microsoft.EntityFrameworkCore;
using VOZ.Database.Entities;

namespace VOZ.Database;

public sealed class VozDbContext : DbContext
{
    public VozDbContext()
    {
    }

    public VozDbContext(DbContextOptions<VozDbContext> options)
        : base(options)
    {
    }

    public DbSet<Answer> Answers { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<QuestionImage> QuestionImages { get; set; }

    public DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=Database\\voz.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(answerBuilder =>
        {
            answerBuilder.HasKey(answer => answer.Id);
            answerBuilder.Property(answer => answer.Id).ValueGeneratedNever();

            answerBuilder.
                HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
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
                .HasOne(question => question.Category)
                .WithMany(category => category.Questions)
                .OnDelete(DeleteBehavior.Restrict);

            questionBuilder
                .HasOne(question => question.Subcategory)
                .WithMany(subcategory => subcategory.Questions)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<QuestionImage>(questionImageBuilder =>
        {
            questionImageBuilder.HasKey(questionImage => questionImage.Id);
            questionImageBuilder.Property(questionImage => questionImage.Id).ValueGeneratedNever();

            questionImageBuilder
                .HasOne(questionImage => questionImage.Question)
                .WithMany(question => question.QuestionImages)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Subcategory>(subcategoryBuilder =>
        {
            subcategoryBuilder.HasKey(subcategory => subcategory.Id);
            subcategoryBuilder.Property(subcategory => subcategory.Id).ValueGeneratedNever();

            subcategoryBuilder
                .HasOne(subcategory => subcategory.Category)
                .WithMany(category => category.Subcategories)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
