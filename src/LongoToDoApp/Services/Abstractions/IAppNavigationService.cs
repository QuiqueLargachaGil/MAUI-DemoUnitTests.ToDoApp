namespace LongoToDoApp.Services.Abstractions
{
	public interface IAppNavigationService
	{
		Task NavigateTo(string name, INavigationParameters parameters = null);

		Task NavigateToBack();
	}
}
