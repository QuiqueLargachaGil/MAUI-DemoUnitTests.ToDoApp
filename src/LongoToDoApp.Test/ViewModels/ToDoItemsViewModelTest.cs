using FluentAssertions;
using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Services.Abstractions;
using LongoToDoApp.ViewModels;
using Moq;
using Newtonsoft.Json;

namespace LongoToDoApp.Test.ViewModels
{
	public class ToDoItemsViewModelTest
	{
		private readonly ToDoItemsViewModel _sut;
		private readonly Mock<IToDoItemsService> _toDoItemsService;

        public ToDoItemsViewModelTest()
        {
			NavigationService = new Mock<IAppNavigationService>();
			DialogService = new Mock<IPageDialogService>();

			_toDoItemsService = new Mock<IToDoItemsService>();
			_toDoItemsService.Setup(m => m.GetToDoItems(It.IsAny<ToDoItemsRequest>())).ReturnsAsync(GetResponse());

			_sut = new ToDoItemsViewModel(DialogService.Object, _toDoItemsService.Object);
		}

		protected Mock<IAppNavigationService> NavigationService { get; }
		protected Mock<IPageDialogService> DialogService { get; }

		[Fact]
		public async Task OnNavigatedImplementation_Should_LoadData()
		{
			// Act
			await _sut.OnNavigatedImplementation(null);

			// Assert
			_toDoItemsService.Verify(m => m.GetToDoItems(It.IsAny<ToDoItemsRequest>()));
			
			_sut.ToDoItems.Should().HaveCount(2);
			_sut.ToDoItems.First().Name.Should().Be("Name");
			_sut.ToDoItems.First().IsComplete.Should().BeTrue();
			_sut.NumberItems.Should().Be(1);
		}

		[Fact]
		public async Task OnNavigatedImplementation_Should_Handle_Errors()
		{
			// Arrange
			_toDoItemsService.Setup(m => m.GetToDoItems(It.IsAny<ToDoItemsRequest>())).ThrowsAsync(new Exception());

			// Act
			await _sut.OnNavigatedImplementation(null);

			// Assert
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
		}

		private static List<ToDoItemsResponse> GetResponse()
		{
			return JsonConvert.DeserializeObject<List<ToDoItemsResponse>>(GetAllToDoItems);
		}

		private const string GetAllToDoItems = "[{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":true},{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":false}]";
	}
}
