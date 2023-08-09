using ToDoApp.Infrastructure.Services.Base;
using Newtonsoft.Json;

namespace ToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class AddItemRequest : BaseRequest
	{
		public AddItemRequest(string baseUrl) : base(baseUrl)
		{

		}

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("IsComplete")]
		public bool IsComplete { get; set; }
	}
}
