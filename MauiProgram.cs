using material_manager.Services;
using material_manager.ViewModels;
using Microsoft.Extensions.Logging;

namespace material_manager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.AddPages()
			.AddViewModels()
			.AddRepositories()
			.AddServices();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static MauiAppBuilder AddPages(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<MainPage>();
		return mauiAppBuilder;
	}

	private static MauiAppBuilder AddViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<MainViewModel>();
		return mauiAppBuilder;
	}

	private static MauiAppBuilder AddRepositories(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<IMaterialDatabase, MaterialDatabase>();
		mauiAppBuilder.Services.AddSingleton<IArticleRepository, ArticleRepository>();
		return mauiAppBuilder;
	}

	private static MauiAppBuilder AddServices(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<IArticlesListReader, ArticlesListReader>();
		return mauiAppBuilder;
	}
}
