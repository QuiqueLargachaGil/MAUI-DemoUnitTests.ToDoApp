using LongoToDoApp.Helpers;

namespace LongoToDoApp.Extensions
{
	[ContentProperty("Value")]
	public class TranslateExtension : IMarkupExtension
	{
		public string Value { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return Value is null ? null : Translate(Value);
		}

		public static string Translate(string requestedValue)
		{
			return LocalizationResourceManager.Instance.GetValue(requestedValue);
		}
	}
}
