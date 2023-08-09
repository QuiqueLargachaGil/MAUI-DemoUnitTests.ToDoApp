using FluentAssertions;
using ToDoApp.Infrastructure.Mappers;
using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Test.Infrastructure.Mappers.Base;

namespace ToDoApp.Test.Infrastructure.Mappers
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
