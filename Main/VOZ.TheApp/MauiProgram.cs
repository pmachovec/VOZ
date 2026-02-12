using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VOZ.Shared.Extensions;

#if DEBUG
using Microsoft.Extensions.Logging;
#endif


namespace VOZ.TheApp;

public static class MauiProgram
{
    private const string DB_FILE_NAME = "voz.sqlite";

    public static MauiApp CreateMauiApp()
    {
        // Do not use cs-CZ or similar 4-letter culture, it causes troubles in Android.
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("cs");
        var builder = MauiApp.CreateBuilder();
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_FILE_NAME);

        // Copy the database file to an app-dedicated folder in the current system AppData.
        // This is the accepted way of handling resources in cross-platform MAUI.
        using (var dbAssetStream = FileSystem.OpenAppPackageFileAsync($"Resources/Raw/{DB_FILE_NAME}").GetAwaiter().GetResult())
        using (var dbFileStream = new FileStream(dbPath, FileMode.OpenOrCreate))
        {
            dbAssetStream.CopyTo(dbFileStream);
        }

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services
            .ConfigureSharedInternalDependencies(dbPath)
            .AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
