using FluentAssertions;
using ToDoApp.Infrastructure.Abstractions;
using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Models;
using ToDoApp.Services.Abstractions;
using ToDoApp.Settings;
using ToDoApp.ViewModels;
using Moq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;

namespace ToDoApp.Test.ViewModels
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
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>()));
		}

		[Fact]
		public void RefreshCommand_Should_LoadData()
		{
			// Act
			_sut.RefreshCommand.Execute(null);

			//Assert
			_toDoItemsService.Verify(m => m.GetToDoItems(It.IsAny<ToDoItemsRequest>()));

			_sut.ToDoItems.Should().HaveCount(2);
			_sut.ToDoItems.First().Name.Should().Be("Name");
			_sut.ToDoItems.First().IsComplete.Should().BeTrue();
			_sut.NumberItems.Should().Be(1);
		}

		[Fact]
		public void DeleteItemCommand_Should_Handle_Exceptions()
		{
			// Arrange
			DialogService.Setup(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
			_toDoItemsService.Setup(m => m.DeleteItem(It.IsAny<DeleteItemRequest>())).ThrowsAsync(new Exception());

			// Act
			_sut.DeleteItemCommand.Execute(null);

			// Assert
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>()));
		}

		[Fact]
		public void DeleteItemCommand_Should_Call_ToDoItemService_DeleteItem_Method_With_Expected_Request()
		{
			// Arrange
			DialogService.Setup(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

			var selectedItem = GetItem();

			DeleteItemRequest request = null;
			_toDoItemsService.Setup(m => m.DeleteItem(It.IsAny<DeleteItemRequest>())).Callback<DeleteItemRequest>(r => request = r);

			var expectedRequest = GetDeleteItemExpectedRequest();

			// Act
			_sut.DeleteItemCommand.Execute(selectedItem);

			// Assert
			request.Should().BeEquivalentTo(expectedRequest);
			_toDoItemsService.Verify(m => m.DeleteItem(It.IsAny<DeleteItemRequest>()));
		}

		[Fact]
		public void DeleteItemCommand_Should_Display_Alert_If_The_Item_Have_Been_Correctly_Removed()
		{
			// Arrange
			DialogService.Setup(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
			_toDoItemsService.Setup(m => m.DeleteItem(It.IsAny<DeleteItemRequest>()));

			// Act
			_sut.DeleteItemCommand.Execute(null);

			// Assert
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>()));
		}

		[Fact]
		public void CheckedCommand_Should_Handle_Exceptions()
		{
			// Arrange
			DialogService.Setup(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
			_toDoItemsService.Setup(m => m.UpdateItem(It.IsAny<UpdateItemRequest>())).ThrowsAsync(new Exception());

			// Act
			_sut.DeleteItemCommand.Execute(null);

			// Assert
			DialogService.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<string>()));
		}

		[Theory]
		[ClassData(typeof(ToDoItemsListTestData))]
		public void CheckedCommand_Should_Call_ToDoItemService_UpdateItem_Method_With_Expected_Request(IEnumerable<ToDoItem> toDoItemsList)
		{
			// Arrange
			var selectedItem = GetItem();
			_sut.ToDoItems = new ObservableCollection<ToDoItem>(toDoItemsList);

			UpdateItemRequest request = null;
			_toDoItemsService.Setup(m => m.UpdateItem(It.IsAny<UpdateItemRequest>())).Callback<UpdateItemRequest>(r => request = r);

			var expectedRequest = GetUpdateItemExpectedRequest();

			// Act
			_sut.CheckedCommand.Execute(selectedItem);

			// Assert
			request.Should().BeEquivalentTo(expectedRequest);
			_toDoItemsService.Verify(m => m.UpdateItem(It.IsAny<UpdateItemRequest>()));
		}

		[Theory]
		[ClassData(typeof(ToDoItemsListTestData))]
		public void CheckedCommand_Should_Returns_The_Number_Of_Task_Incompletes(IEnumerable<ToDoItem> toDoItemsList)
		{
			// Assert
			var selectedItem = GetItem();
			_sut.ToDoItems = new ObservableCollection<ToDoItem>(toDoItemsList);

			// Act
			_sut.CheckedCommand.Execute(selectedItem);

			// Assert
			_sut.NumberItems.Should().Be(4);
		}

		[Fact]
		public void NavigateToCreateItemCommand_Should_Navigate()
		{
			// Arrange
			NavigationService.Setup(m => m.NavigateTo(It.IsAny<string>(), It.IsAny<INavigationParameters>()));

			// Act
			_sut.NavigateToCreateItemCommand.Execute(null);

			// Assert
			NavigationService.Verify(m => m.NavigateTo("CreateItemView", It.IsAny<INavigationParameters>()));
		}

		private static ToDoItem GetItem()
		{
			return new ToDoItem
			{
				Key = nameof(ToDoItem.Key),
				Name = nameof(ToDoItem.Name),
				IsComplete = true,
			};
		}

		private static DeleteItemRequest GetDeleteItemExpectedRequest()
		{
			return new DeleteItemRequest(Configuration.BaseUrl, nameof(DeleteItemRequest.Key));
		}

		private static UpdateItemRequest GetUpdateItemExpectedRequest()
		{
			return new UpdateItemRequest(Configuration.BaseUrl, GetItem());
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
