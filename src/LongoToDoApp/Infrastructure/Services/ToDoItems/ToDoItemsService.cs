using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Services.Base;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Services.Abstractions;

namespace LongoToDoApp.Infrastructure.Services.ToDoItems
{
    public class ToDoItemsService : BaseApiService, IToDoItemsService
    {
		private const string GetToDoItemsEndpoint = "/todo";
        private const string AddItemEndpoint = "/todo";
		private const string DeleteItemEndpoint = "/todo?id={0}";

		public ToDoItemsService(ICheckConnectivityService checkConnectivityService, HttpClientHandler handler) : base(checkConnectivityService, handler)
        {
            
        }

		public async Task<List<ToDoItemsResponse>> GetToDoItems(ToDoItemsRequest request)
        {
            return await HttpCall<ToDoItemsResponse, ToDoItemsRequest>(HttpMethod.Get, GetToDoItemsEndpoint, request);
        }

		public async Task<List<ToDoItemsResponse>> AddItem(AddItemRequest request)
		{
			return await HttpCall<ToDoItemsResponse, AddItemRequest>(HttpMethod.Post, AddItemEndpoint, request);
		}

        public async Task DeleteItem(DeleteItemRequest request)
        {
			if (string.IsNullOrEmpty(request.Id))
			{
				throw new Exception($"Parameter {nameof(DeleteItemRequest.Id)} is required");
			}

			var endpoint = $"{string.Format(DeleteItemEndpoint, request.Id)}";
			await HttpCall<ToDoItemsResponse, DeleteItemRequest>(HttpMethod.Delete, endpoint, request);
		}
	}
}
