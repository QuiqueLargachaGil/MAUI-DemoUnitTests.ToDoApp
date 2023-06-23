using LongoToDoApp.Infrastructure.Services.Base;

namespace LongoToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class ToDoItemsRequest : BaseRequest
	{
        public ToDoItemsRequest(string baseUrl) : base(baseUrl)
        {
            
        }
    }
}
