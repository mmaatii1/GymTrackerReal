using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using GymTracker.Services;
using GymTracker.Models;
using GymTracker.Views;
using System.Reflection;

namespace GymTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();
        builder.Services.AddScoped(typeof(IRestService<>), typeof(RestService<>));
        builder.Services.AddScoped(typeof(IWrapperService<>), typeof(WrapperService<>));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailsPage>();
        builder.Services.AddTransient<ListOfTrainings>();
        builder.Services.AddTransient<AddTraining>();
        builder.Services.AddTransient<AddWorkoutPlan>();

        builder.Services.AddTransient<CustomWorkoutDetailsViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<CustomWorkoutViewModel>();
        builder.Services.AddTransient<CustomWorkoutAddViewModel>();
        builder.Services.AddTransient<WorkoutPlanAddViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
		//Dependencies

		return builder.Build();
	}
}
