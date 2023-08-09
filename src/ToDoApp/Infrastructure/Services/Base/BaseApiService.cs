using ToDoApp.Models.Exceptions;
using ToDoApp.Services.Abstractions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ToDoApp.Infrastructure.Services.Base
{
	public abstract class BaseApiService
	{
		private readonly ICheckConnectivityService _checkConnectivityService;
		private readonly HttpClientHandler _handler;

		protected BaseApiService(ICheckConnectivityService checkConnectivityService, HttpClientHandler handler)
		{
			_checkConnectivityService = checkConnectivityService;
			_handler = handler;
		}

		protected async Task<List<TResponse>> HttpCall<TResponse, TRequest>(HttpMethod httpMethod, string endpoint, TRequest request) where TResponse : new() where TRequest : BaseRequest
		{
			if (!await _checkConnectivityService.HasInternetConnection())
			{
				throw new NoInternetException(nameof(NoInternetException));
			}

			var url = GetUrl(request.BaseUrl, endpoint);

			using (var client = GetHttpClient())
			{
				var serializedRequest = JsonConvert.SerializeObject(request);
				var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

				HttpResponseMessage response;
				switch (httpMethod)
				{
					case HttpMethod method when method == HttpMethod.Get:
						response = await client.GetAsync(url);
						break;
					case HttpMethod method when method == HttpMethod.Post:
						response = await client.PostAsync(url, httpContent);
						break;
					case HttpMethod method when method == HttpMethod.Put:
						response = await client.PutAsync(url, httpContent);
						break;
					case HttpMethod method when method == HttpMethod.Delete:
						response = await client.DeleteAsync(url);
						break;
					default:
						throw new ArgumentException($"{nameof(httpMethod)} should be HttpMethod.Get, HttpMethod.Post, HttpMethod.Put or HttpMethod.Delete");
				}

				if (!response.IsSuccessStatusCode)
				{
					HandleApiException(response);
				}

				List<TResponse> value;
				var jsonSerializer = GetJsonSerializer();

				using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
				using (var reader = new StreamReader(stream))
				using (var content = new JsonTextReader(reader))
				{
					value = jsonSerializer.Deserialize<List<TResponse>>(content);
				}

				return value ?? default(List<TResponse>);
			}
		}

		private string GetUrl(string baseUrl, string endpoint)
		{
			return $"{baseUrl}{endpoint}";
		}

		private HttpClient GetHttpClient()
		{
			var timeSpan = new TimeSpan(0, 0, 90);

			return _handler is null
				?
				new HttpClient
				{
					Timeout = timeSpan
				}
				: new HttpClient(_handler, false)
				{
					Timeout = timeSpan
				};
		}

		private void HandleApiException(HttpResponseMessage response)
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.BadRequest:
                    throw new BadRequestException(response.ReasonPhrase);
                case HttpStatusCode.InternalServerError:
                    throw new ServiceUnavailableException(response.ReasonPhrase);
				default:
					throw new ApiException(response.ReasonPhrase);
			}
		}

		private JsonSerializer GetJsonSerializer()
		{
			return new JsonSerializer
			{
				NullValueHandling = NullValueHandling.Ignore,
			};
		}
	}
}
