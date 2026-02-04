using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VOZ.Shared.Components.Pages;
using VOZ.Shared.Database;
using VOZ.Shared.Generator;

namespace VOZ.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSharedInternalDependencies(this IServiceCollection services, string dbPath) =>
        services
            .AddDbContext<VOZDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={dbPath}"))
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddScoped<QuestionGenerator>()
            .AddScoped<QuestionnaireParams>();
}
