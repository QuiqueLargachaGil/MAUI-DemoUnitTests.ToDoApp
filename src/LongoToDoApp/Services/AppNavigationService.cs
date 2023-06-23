using LongoToDoApp.Services.Abstractions;

namespace LongoToDoApp.Services
{
	public class AppNavigationService : IAppNavigationService
	{
		private readonly INavigationService _navigationService;

        public AppNavigationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task NavigateTo(string name, INavigationParameters parameters = null)
		{
			await _navigationService.NavigateAsync(name, parameters);
		}

		public async Task NavigateToBack()
		{
			await _navigationService.GoBackAsync();
		}
	}
}
