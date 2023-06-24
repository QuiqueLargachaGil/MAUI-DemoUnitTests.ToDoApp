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
		private const string UpdateItemEndpoint = "/todo?id={0}";
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
			if (string.IsNullOrEmpty(request.Name))
			{
				throw new Exception($"Parameter {nameof(AddItemRequest.Name)} is required");
			}

			return await HttpCall<ToDoItemsResponse, AddItemRequest>(HttpMethod.Post, AddItemEndpoint, request);
		}

		public async Task UpdateItem(UpdateItemRequest request)
		{
			if (string.IsNullOrEmpty(request.Key))
			{
				throw new Exception($"Parameter {nameof(UpdateItemRequest.Key)} is required");
			}

			var endpoint = $"{string.Format(UpdateItemEndpoint, request.Key)}";
			await HttpCall<ToDoItemsResponse, UpdateItemRequest>(HttpMethod.Put, endpoint, request);
		}

		public async Task DeleteItem(DeleteItemRequest request)
        {
			if (string.IsNullOrEmpty(request.Key))
			{
				throw new Exception($"Parameter {nameof(DeleteItemRequest.Key)} is required");
			}

			var endpoint = $"{string.Format(DeleteItemEndpoint, request.Key)}";
			await HttpCall<ToDoItemsResponse, DeleteItemRequest>(HttpMethod.Delete, endpoint, request);
		}
	}
}
