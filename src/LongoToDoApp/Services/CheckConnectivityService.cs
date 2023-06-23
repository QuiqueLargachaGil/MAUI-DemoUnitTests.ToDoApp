using LongoToDoApp.Services.Abstractions;

namespace LongoToDoApp.Services
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
