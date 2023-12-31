﻿using ToDoApp.Infrastructure.Services.Base;
using ToDoApp.Models;
using Newtonsoft.Json;

namespace ToDoApp.Infrastructure.Services.ToDoItems.Models
{
	public class UpdateItemRequest : BaseRequest
	{
		public UpdateItemRequest(string baseUrl, ToDoItem item) : base(baseUrl)
		{
			Key = item.Key;
			Name = item.Name;
			IsComplete = item.IsComplete;
		}

		[JsonProperty("Key")]
		public string Key { get; }

		[JsonProperty("Name")]
		public string Name { get; }

		[JsonProperty("IsComplete")]
		public bool IsComplete { get; }
	}
}
