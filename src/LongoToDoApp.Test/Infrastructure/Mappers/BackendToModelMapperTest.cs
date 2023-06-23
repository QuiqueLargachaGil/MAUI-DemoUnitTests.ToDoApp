using FluentAssertions;
using LongoToDoApp.Infrastructure.Mappers;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using System.Collections;

namespace LongoToDoApp.Test.Infrastructure.Mappers
{
	public class BackendToModelMapperTest
	{
        [Theory]
        [ClassData(typeof(ToDoItemsResponseTestData))]
        public void GetToDoItems_Should_Return_Expected_Data(IEnumerable<ToDoItemsResponse> toDoItemsResponse)
        {
            // Act
            var result = BackendToModelMapper.GetToDoItems(toDoItemsResponse);

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be(toDoItemsResponse.First().Name);
        }

        private class ToDoItemsResponseTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new List<ToDoItemsResponse>
                    {
						new ToDoItemsResponse
					    {
						    Key = nameof(ToDoItemsResponse.Key),
						    Name = nameof(ToDoItemsResponse.Name),
						    IsComplete = true
					    },
					    new ToDoItemsResponse
					    {
						    Key = nameof(ToDoItemsResponse.Key),
						    Name = nameof(ToDoItemsResponse.Name),
						    IsComplete = false
					    }
					}
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
	}
}
