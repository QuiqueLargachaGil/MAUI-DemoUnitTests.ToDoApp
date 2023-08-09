namespace ToDoApp.Infrastructure.Services.Base
{
	public abstract class BaseRequest
	{
		protected BaseRequest(string baseUrl)
		{
			if (!baseUrl.EndsWith("/"))
			{
				baseUrl += "/";
			}

			BaseUrl = $"{baseUrl}api";
		}

		public string BaseUrl { get; }
	}
}
