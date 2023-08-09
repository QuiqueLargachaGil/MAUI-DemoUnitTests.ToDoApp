using ToDoApp.Resources.Translations;
using System.Globalization;
using System.Resources;

namespace ToDoApp.Helpers
{
	public class LocalizationResourceManager : BindableBase
	{
		private readonly ResourceManager _resourceManager;
		private CultureInfo _currentCulture;

		public static LocalizationResourceManager Instance;

		public LocalizationResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public static void Init()
        {
			Instance = new LocalizationResourceManager(Translations.ResourceManager);
		}
		public CultureInfo CurrentCulture
		{
			get => _currentCulture;
			set => SetProperty(ref _currentCulture, value);
		}

		public string GetValue(string text)
		{
			return _resourceManager.GetString(text, CurrentCulture);
		}
	}
}
