using ToDoApp.Infrastructure.Mappers.Base;
using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Models;

namespace ToDoApp.Infrastructure.Mappers
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
