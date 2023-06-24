using LongoToDoApp.Infrastructure.Services.Base;

namespace LongoToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class DeleteItemRequest : BaseRequest
	{
		public DeleteItemRequest(string baseUrl, string id) : base(baseUrl)
		{
			Id = id;
		}

		public string Id { get; }
	}
}
