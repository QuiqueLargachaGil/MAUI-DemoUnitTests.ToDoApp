using LongoToDoApp.Models.Exceptions;

namespace LongoToDoApp.ViewModels.Base
{
	public class BaseViewModel : BindableBase, IInitializeAsync, INavigatedAware
	{
		private readonly IPageDialogService _dialogService;

        public BaseViewModel(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
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
					await _dialogService.DisplayAlertAsync("Alert", exception.Message, "Ok");
					break;
				case BadRequestException:
					await _dialogService.DisplayAlertAsync("Alert", exception.Message, "Ok");
					break;
				case ServiceUnavailableException:
					await _dialogService.DisplayAlertAsync("Alert", exception.Message, "Ok");
					break;
				case ApiException:
					await _dialogService.DisplayAlertAsync("Alert", exception.Message, "Ok");
					break;
				default:
					await _dialogService.DisplayAlertAsync("Alert", exception.Message, "Ok");
					break;
			}
		}
	}
}
