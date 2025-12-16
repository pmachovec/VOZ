using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VOZ.QuestionGenerator.Database;

namespace VOZ.QuestionGenerator;

public static class QuestionGeneratorServiceCollectionExtensions
{
    public static IServiceCollection AddScopedQuestionGenerator(this IServiceCollection services)
    {
        var optionsBuilder = new DbContextOptionsBuilder<QuestionGeneratorDbContext>();
        optionsBuilder.UseSqlite("Data Source=Database/voz.sqlite");

        var dbContext = new QuestionGeneratorDbContext(optionsBuilder.Options);
        var questionGenerator = new QuestionGenerator(dbContext);
        services.AddScoped<IQuestionGenerator>(_ => questionGenerator);

        return services;
    }
}
