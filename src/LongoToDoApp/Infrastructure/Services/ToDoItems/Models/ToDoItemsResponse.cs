using Newtonsoft.Json;

namespace LongoToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class ToDoItemsResponse
	{
		[JsonProperty("Key")]
		public string Key { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("IsComplete")]
		public bool IsComplete { get; set; }
	}
}
