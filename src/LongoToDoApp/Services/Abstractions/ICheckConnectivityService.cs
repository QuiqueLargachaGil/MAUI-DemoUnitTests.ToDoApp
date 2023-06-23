namespace LongoToDoApp.Services.Abstractions
{
	public interface ICheckConnectivityService
	{
		Task<bool> HasInternetConnection();
	}
}
