using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;

namespace LongoToDoApp.Infrastructure.Abstractions
{
	public interface IToDoItemsService
	{
		Task<List<ToDoItemsResponse>> GetToDoItems(ToDoItemsRequest request);
	}
}
