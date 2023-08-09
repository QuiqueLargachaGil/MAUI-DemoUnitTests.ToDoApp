using ToDoApp.Infrastructure.Services.ToDoItems.Models;

namespace ToDoApp.Infrastructure.Abstractions
{
	public interface IToDoItemsService
	{
		Task<List<ToDoItemsResponse>> GetToDoItems(ToDoItemsRequest request);

		Task<List<ToDoItemsResponse>> AddItem(AddItemRequest request);

		Task UpdateItem(UpdateItemRequest request);

		Task DeleteItem(DeleteItemRequest request);
	}
}
