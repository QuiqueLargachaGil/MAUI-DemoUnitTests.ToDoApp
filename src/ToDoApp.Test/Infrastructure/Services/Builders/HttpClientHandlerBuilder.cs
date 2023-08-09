using Moq;
using Moq.Protected;

namespace ToDoApp.Test.Infrastructure.Services.Builders
{
	public static class HttpClientHandlerBuilder
	{
		public static HttpClientHandler WithResponse(HttpResponseMessage response)
		{
			var handler = new Mock<HttpClientHandler>();

			handler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response)
				.Verifiable();

			return handler.Object;
		}
	}
}
