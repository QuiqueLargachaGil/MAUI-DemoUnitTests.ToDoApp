using FluentAssertions;
using LongoToDoApp.Infrastructure.Services.ToDoItems;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models.Exceptions;
using LongoToDoApp.Test.Infrastructure.Services.Base;
using LongoToDoApp.Test.Infrastructure.Services.Builders;
using Moq;
using System.Net;

namespace LongoToDoApp.Test.Infrastructure.Services.ToDoItems
{
	public class ToDoItemsServiceTest : BaseServiceTest
	{
		[Fact]
		public async Task GetToDoItems_Throw_NoInternetException_If_Not_Has_Internet_Connection()
		{
			// Arrange
			var handler = HttpClientHandlerBuilder.WithResponse(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(GetToDoItemsResponse)
			});

			var sut = new ToDoItemsService(CheckConnectivityService.Object, handler);

			CheckConnectivityService.Setup(m => m.HasInternetConnection()).ReturnsAsync(false);
			var request = new ToDoItemsRequest("http://url.fake");

			// Act
			Func<Task> response = async () => await sut.GetToDoItems(request);

			// Assert
			await response.Should().ThrowAsync<NoInternetException>();
		}

		[Fact]
		public async Task GetToDoItems_Should_Return_Data()
		{
			// Arrange
			var handler = HttpClientHandlerBuilder.WithResponse(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(GetToDoItemsResponse)
			});

			var sut = new ToDoItemsService(CheckConnectivityService.Object, handler);

			var request = new ToDoItemsRequest("http://url.fake");
			var expected = GetExpectedToDoItems();

			// Act
			var response = await sut.GetToDoItems(request);

			// Assert
			response.Should().BeEquivalentTo(expected);
		}

		private static string GetToDoItemsResponse => "[{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":true},{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":false}]";

		private static List<ToDoItemsResponse> GetExpectedToDoItems()
		{
			return new List<ToDoItemsResponse>
			{
				new ToDoItemsResponse
				{
					Key = nameof(ToDoItemsResponse.Key),
					Name = nameof(ToDoItemsResponse.Name),
					IsComplete = true,
				},
				new ToDoItemsResponse
				{
					Key = nameof(ToDoItemsResponse.Key),
					Name = nameof(ToDoItemsResponse.Name),
					IsComplete = false,
				}
			};
		}
	}
}
