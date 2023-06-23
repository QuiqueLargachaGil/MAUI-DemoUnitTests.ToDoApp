using FluentAssertions;
using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models;
using LongoToDoApp.Services.Abstractions;
using LongoToDoApp.ViewModels;
using Moq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;

namespace LongoToDoApp.Test.ViewModels
{
	public class ToDoItemsViewModelTest
	{
		private readonly ToDoItemsViewModel _sut;
		private readonly Mock<IToDoItemsService> _toDoItemsService;

        public ToDoItemsViewModelTest()
        {
			DialogService = new Mock<IPageDialogService>();
			NavigationService = new Mock<IAppNavigationService>();

			_toDoItemsService = new Mock<IToDoItemsService>();
			_toDoItemsService.Setup(m => m.GetToDoItems(It.IsAny<ToDoItemsRequest>())).ReturnsAsync(GetResponse());

			_sut = new ToDoItemsViewModel(DialogService.Object, NavigationService.Object, _toDoItemsService.Object);
		}

		
		protected Mock<IPageDialogService> DialogService { get; }
		protected Mock<IAppNavigationService> NavigationService { get; }

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

		[Theory]
		[ClassData(typeof(ToDoItemsListTestData))]
		public void CheckedCommand_Should_Returns_The_Number_Of_Task_Incompletes(IEnumerable<ToDoItem> toDoItemsList)
		{
			// Assert
			_sut.ToDoItems = new ObservableCollection<ToDoItem>(toDoItemsList);

			// Act
			_sut.CheckedCommand.Execute(null);

			// Assert
			_sut.NumberItems.Should().Be(4);
		}

		[Fact]
		public void NavigateToCreateItemCommand_Should_Navigate()
		{
			// Arrange
			NavigationService.Setup(m => m.NavigateTo(It.IsAny<string>(), It.IsAny<INavigationParameters>()));

			// Act
			_sut.NavigateToCreateItemCommand.Execute(It.IsAny<INavigationParameters>());

			// Assert
			NavigationService.Verify(m => m.NavigateTo("CreateItemView", It.IsAny<INavigationParameters>()));
		}

		private static List<ToDoItemsResponse> GetResponse()
		{
			return JsonConvert.DeserializeObject<List<ToDoItemsResponse>>(GetAllToDoItems);
		}

		private const string GetAllToDoItems = "[{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":true},{\"Key\":\"Key\",\"Name\":\"Name\",\"IsComplete\":false}]";

		private class ToDoItemsListTestData : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				yield return new object[]
				{
					new List<ToDoItem>
					{
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = true
						},
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = false
						},
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = true
						},
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = false
						},
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = false
						},
						new ToDoItem
						{
							Key = nameof(ToDoItem.Key),
							Name = nameof(ToDoItem.Name),
							IsComplete = false
						}
					}
				};
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();	
		}
	}
}
