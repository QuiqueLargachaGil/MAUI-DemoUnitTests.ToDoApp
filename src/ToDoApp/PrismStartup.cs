using ToDoApp.Infrastructure.Abstractions;
using ToDoApp.Infrastructure.Services.ToDoItems;
using ToDoApp.Services;
using ToDoApp.Services.Abstractions;
using ToDoApp.ViewModels;
using ToDoApp.Views;

namespace ToDoApp
{
	public static class PrismStartup
	{
		public static void Configure(PrismAppBuilder builder)
		{
			builder.RegisterTypes(RegisterTypes).OnAppStart(async (start) =>
			{
				await start.NavigateAsync(nameof(ToDoItemsView));
			});
			builder.ConfigureServices(ConfigureServices);
		}

		private static void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<ToDoItemsView, ToDoItemsViewModel>();
			containerRegistry.RegisterForNavigation<CreateItemView, CreateItemViewModel>();
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IAppNavigationService,  AppNavigationService>();
			serviceCollection.AddSingleton<ICheckConnectivityService, CheckConnectivityService>();

			serviceCollection.AddSingleton<IToDoItemsService, ToDoItemsService>();
		}
	}
}
