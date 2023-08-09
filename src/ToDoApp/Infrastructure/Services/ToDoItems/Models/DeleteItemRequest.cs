using ToDoApp.Infrastructure.Services.Base;

namespace ToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class DeleteItemRequest : BaseRequest
	{
		public DeleteItemRequest(string baseUrl, string key) : base(baseUrl)
		{
			Key = key;
		}

		public string Key { get; }
	}
}
