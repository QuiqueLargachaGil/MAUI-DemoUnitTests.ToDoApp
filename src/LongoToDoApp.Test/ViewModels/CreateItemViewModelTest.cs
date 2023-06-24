using FluentAssertions;
using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Services.Abstractions;
using LongoToDoApp.ViewModels;
using Moq;
using Newtonsoft.Json;

namespace LongoToDoApp.Test.ViewModels
{
	public class CreateItemViewModelTest
	{
		private readonly CreateItemViewModel _sut;
		private readonly Mock<IToDoItemsService> _toDoItemsService;

        public CreateItemViewModelTest()
        {
			DialogService = new Mock<IPageDialogService>();
			NavigationService = new Mock<IAppNavigationService>();
			NavigationService2 = new Mock<INavigationService>();

			_toDoItemsService = new Mock<IToDoItemsService>();
			_toDoItemsService.Setup(m => m.AddItem(It.IsAny<AddItemRequest>())).ReturnsAsync(GetResponse());

			_sut = new CreateItemViewModel(DialogService.Object, NavigationService.Object, NavigationService2.Object, _toDoItemsService.Object);
		}

		protected Mock<IPageDialogService> DialogService { get; }
		protected Mock<IAppNavigationService> NavigationService { get; }
		protected Mock<INavigationService> NavigationService2 { get; }

		[Fact]
		public void Title_Should_Be_()
		{
			// Assert
			_sut.Title.Should().Be("Create new Task");//Be(Translations.CreateTaskScreenTitle);
		}

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("name", true)]
		public void CreateNewItemCommand_Can_Execute_If_Name_Is_Not_Null_Or_Empty(string name, bool expected)
		{
			// Arrange
			_sut.Name = name;

			// Act
			var result = _sut.CreateNewItemCommand.CanExecute(null);

			// Assert
			result.Should().Be(expected);
		}

		[Fact]
		public void CreateNewItemCommand_Should_Handle_Exceptions()
		{
			// Arrange
			_toDoItemsService.Setup(m => m.AddItem(It.IsAny<AddItemRequest>())).ThrowsAsync(new Exception());

			// Act
			_sut.CreateNewItemCommand.Execute(null);

			// Assert
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>()));
		}



		private static List<ToDoItemsResponse> GetResponse()
		{
			return JsonConvert.DeserializeObject<List<ToDoItemsResponse>>(GetAllToDoItems);
		}

		private const string GetAllToDoItems = "[{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":false}]";
	}
}
