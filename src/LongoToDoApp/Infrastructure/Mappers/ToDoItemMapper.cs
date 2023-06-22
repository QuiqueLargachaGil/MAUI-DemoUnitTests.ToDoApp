using LongoToDoApp.Infrastructure.Mappers.Base;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models;

namespace LongoToDoApp.Infrastructure.Mappers
{
	public class ToDoItemMapper : BaseMapper<ToDoItemsResponse, ToDoItem>
	{
		protected override ToDoItem MapperImplementation(ToDoItemsResponse source)
		{
			var toDoItem = new ToDoItem
			{
				Key = source.Key,
				Name = source.Name,
				IsComplete = source.IsComplete,
			};

			return toDoItem;
		}
	}
}
