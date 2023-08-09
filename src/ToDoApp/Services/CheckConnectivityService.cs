using ToDoApp.Services.Abstractions;

namespace ToDoApp.Services
{
	public class CheckConnectivityService : ICheckConnectivityService
	{
		public Task<bool> HasInternetConnection()
		{
			var hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;
			return Task.FromResult(hasInternetConnection);
		}
	}
}
