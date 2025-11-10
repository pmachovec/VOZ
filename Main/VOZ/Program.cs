using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VOZ.Database;

namespace VOZ;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        var services = new ServiceCollection();

        _ = services
            .AddDbContext<VozDbContext>(optionsBuilder => optionsBuilder.UseSqlite("Data Source=Database/asciipinyin.sqlite"))
            .AddTransient<MainWindow>();

        using var provider = services.BuildServiceProvider();
        Application.Run(provider.GetRequiredService<MainWindow>());
    }
}
