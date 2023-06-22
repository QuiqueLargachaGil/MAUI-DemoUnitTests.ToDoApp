using FluentAssertions;
using LongoToDoApp.Infrastructure.Mappers;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Test.Infrastructure.Mappers.Base;

namespace LongoToDoApp.Test.Infrastructure.Mappers
{
	public class ToDoItemMapperTest : BaseMapperTest<ToDoItemsResponse>
	{
		private readonly ToDoItemMapper _sut;

        public ToDoItemMapperTest()
        {
            _sut = new ToDoItemMapper();
        }

		[Fact]
		public void ToDoItemMapper_Should_Converts_As_Expected()
		{
			// Arrange
			var source = GetSource();

			// Act
			var result = _sut.Mapper(source);

			// Assert
			result.Name.Should().Be(source.Name);
			result.IsComplete.Should().BeTrue();
		}

		[Fact]
		public void ToDoItemMapper_Should_Accept_Nulls()
		{
			// Act
			var result = _sut.Mapper(null);

			// Assert
			result.Should().BeNull();
		}

		public override ToDoItemsResponse GetSource()
		{
			return new ToDoItemsResponse
			{
				Key = nameof(ToDoItemsResponse.Key),
				Name = nameof(ToDoItemsResponse.Name),
				IsComplete = true
			};
		}
	}
}
