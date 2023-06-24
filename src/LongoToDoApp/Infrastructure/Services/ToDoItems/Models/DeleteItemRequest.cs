using LongoToDoApp.Infrastructure.Services.Base;

namespace LongoToDoApp.Infrastructure.Services.ToDoItems.Models
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
