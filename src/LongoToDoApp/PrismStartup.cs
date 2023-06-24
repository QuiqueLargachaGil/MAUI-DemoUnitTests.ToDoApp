﻿using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Services.ToDoItems;
using LongoToDoApp.Services;
using LongoToDoApp.Services.Abstractions;
using LongoToDoApp.ViewModels;
using LongoToDoApp.Views;

namespace LongoToDoApp
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