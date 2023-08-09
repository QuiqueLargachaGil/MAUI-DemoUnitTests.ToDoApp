using ToDoApp.Models.Exceptions;
using ToDoApp.Resources.Translations;
using ToDoApp.Settings;

namespace ToDoApp.ViewModels.Base
{
	public class BaseViewModel : BindableBase, INavigatedAware
	{
		private readonly IPageDialogService _dialogsService;

        public BaseViewModel(IPageDialogService dialogsService)
        {
            _dialogsService = dialogsService;
        }

		public void OnNavigatedFrom(INavigationParameters parameters)
		{

		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			OnNavigatedImplementation(parameters);
		}

		public virtual Task OnNavigatedImplementation(INavigationParameters parameters)
		{
			return Task.CompletedTask;
		}

		private string _title;
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get => _isBusy;
			set => SetProperty(ref _isBusy, value);
		}

		protected async Task HandleException(Exception exception)
		{
			switch (exception)
			{
				case NoInternetException:
					await DisplayAlert(Configuration.ApplicationName, Translations.ExceptionNoInternet, Translations.Ok);
					break;
				case BadRequestException:
					await DisplayAlert(Configuration.ApplicationName, exception.Message, Translations.Ok);
					break;
				case ServiceUnavailableException:
					await DisplayAlert(Configuration.ApplicationName, Translations.ExceptionServiceUnabailable, Translations.Ok);
					break;
				case ApiException:
					await DisplayAlert(Configuration.ApplicationName, exception.Message, Translations.Ok);
					break;
				default:
					await DisplayAlert(Configuration.ApplicationName, exception.Message, Translations.Ok);
					break;
			}
		}

		protected async Task<bool> DisplayAlert(string title, string message, string cancelButtonTitle, string acceptButtonTitle = null)
		{
			return await _dialogsService.DisplayAlertAsync(title, message, acceptButtonTitle, cancelButtonTitle);
		}
	}
}
