using CommunityToolkit.Maui;
using LorenaReels.Interfaces;
using LorenaReels.Services;
using LorenaReels.ViewModels;
using LorenaReels.Views;
using Microsoft.Extensions.Logging;

namespace LorenaReels
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement(false)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IVideoScanner, VideoScanner>();
            builder.Services.AddSingleton<ReelsViewModel>();
            builder.Services.AddSingleton<ReelsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}