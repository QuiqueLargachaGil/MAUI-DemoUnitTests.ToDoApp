using FluentAssertions;
using ToDoApp.Converters;
using System.Globalization;

namespace ToDoApp.Test.Converter
{
	public class BoolToTextColorConverterTest
	{
		private readonly BoolToTextColorConverter _sut;

        private Color _grey = Color.FromArgb("#696969");
		private Color _black = Color.FromArgb("#000000");

		public BoolToTextColorConverterTest()
        {
            _sut = new BoolToTextColorConverter();
        }

        [Fact]
        public void Convert_Should_Returns_Grey_If_Value_Is_True()
        {
            // Act
            var result = _sut.Convert(value: true, typeof(bool), null, CultureInfo.CurrentCulture);

            // Assert
            result.Should().BeEquivalentTo(_grey);
        }

		[Fact]
		public void Convert_Should_Returns_Black_If_Value_Is_False()
		{
			// Act
			var result = _sut.Convert(value: false, typeof(bool), null, CultureInfo.CurrentCulture);

			// Assert
			result.Should().BeEquivalentTo(_black);
		}
	}
}
