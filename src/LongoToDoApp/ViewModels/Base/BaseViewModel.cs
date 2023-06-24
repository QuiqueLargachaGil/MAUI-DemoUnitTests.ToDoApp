using LongoToDoApp.Models.Exceptions;

namespace LongoToDoApp.ViewModels.Base
{
	public class BaseViewModel : BindableBase, IInitializeAsync, INavigatedAware
	{
		private readonly IPageDialogService _dialogsService;

        public BaseViewModel(IPageDialogService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public virtual async Task InitializeAsync(INavigationParameters parameters)
		{
			await Task.Delay(0);
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
					await DisplayAlert("Alert", exception.Message, "Ok");
					break;
				case BadRequestException:
					await DisplayAlert("Alert", exception.Message, "Ok");
					break;
				case ServiceUnavailableException:
					await DisplayAlert("Alert", exception.Message, "Ok");
					break;
				case ApiException:
					await DisplayAlert("Alert", exception.Message, "Ok");
					break;
				default:
					await DisplayAlert("Alert", exception.Message, "Ok");
					break;
			}
		}

		protected async Task<bool> DisplayAlert(string title, string message, string cancelButtonTitle, string acceptButtonTitle = null)
		{
			return await _dialogsService.DisplayAlertAsync(title, message, acceptButtonTitle, cancelButtonTitle);
		}
	}
}
