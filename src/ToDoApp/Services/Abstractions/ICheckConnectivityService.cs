namespace ToDoApp.Services.Abstractions
{
	public interface ICheckConnectivityService
	{
		Task<bool> HasInternetConnection();
	}
}
