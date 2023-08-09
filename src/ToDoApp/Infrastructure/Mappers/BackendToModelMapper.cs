using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Models;

namespace ToDoApp.Infrastructure.Mappers
{
	public static class BackendToModelMapper
	{
		public static IEnumerable<ToDoItem> GetToDoItems(IEnumerable<ToDoItemsResponse> toDoItemDto)
		{
			if (toDoItemDto is null || !toDoItemDto.Any())
			{
				return Enumerable.Empty<ToDoItem>();
			}

			var mapper = new ToDoItemMapper();
			return toDoItemDto.Select(c => mapper.Mapper(c));
		}
	}
}
