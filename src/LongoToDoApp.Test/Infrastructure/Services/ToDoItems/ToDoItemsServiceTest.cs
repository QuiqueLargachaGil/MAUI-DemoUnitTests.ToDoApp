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
		private readonly ToDoItemsService _sut;

        public ToDoItemsServiceTest()
        {
			var handler = HttpClientHandlerBuilder.WithResponse(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(GetToDoItemsResponse)
			});

			_sut = new ToDoItemsService(CheckConnectivityService.Object, handler);
		}

        [Fact]
		public async Task GetToDoItems_Throw_NoInternetException_If_Not_Has_Internet_Connection()
		{
			// Arrange
			CheckConnectivityService.Setup(m => m.HasInternetConnection()).ReturnsAsync(false);
			var request = new ToDoItemsRequest("http://url.fake");

			// Act
			Func<Task> response = async () => await _sut.GetToDoItems(request);

			// Assert
			await response.Should().ThrowAsync<NoInternetException>();
		}

		[Fact]
		public async Task GetToDoItems_Should_Return_Data()
		{
			// Arrange
			var request = new ToDoItemsRequest("http://url.fake");
			var expected = GetExpectedToDoItems();

			// Act
			var response = await _sut.GetToDoItems(request);

			// Assert
			response.Should().BeEquivalentTo(expected);
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void DeleteItem_Throws_Exception_If_Id_Param_Is_Null_Or_Empty(string itemId)
		{
			// Arrange
			var request = new DeleteItemRequest("http://url.fake", itemId);

			// Act
			Func<Task> function = async () => await _sut.DeleteItem(request);

			//Assert
			function.Should().ThrowAsync<Exception>();
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
