using Microsoft.Extensions.Logging;
using System.Globalization;
using VOZ.QuestionGenerator;

namespace VOZ.GUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("cs-CZ");
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services
            .AddLocalization()
            .AddQuestionGenerator()
            .AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
