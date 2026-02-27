using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VOZ.Shared.Application;
using VOZ.Shared.Components.Pages;
using VOZ.Shared.Database;
using VOZ.Shared.Repositories;

namespace VOZ.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSharedInternalDependencies(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<VOZDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IQuestionRepository, QuestionRepository>()
            .AddScoped<IQuestionGenerator, QuestionGenerator>()
            .AddScoped<QuestionnaireParams>();
}
