using ToDoApp.Infrastructure.Services.Base;

namespace ToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class ToDoItemsRequest : BaseRequest
	{
        public ToDoItemsRequest(string baseUrl) : base(baseUrl)
        {
            
        }
    }
}
