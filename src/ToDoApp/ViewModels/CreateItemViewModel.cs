using ToDoApp.Infrastructure.Abstractions;
using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Resources.Translations;
using ToDoApp.Services.Abstractions;
using ToDoApp.Settings;
using ToDoApp.ViewModels.Base;
using System.Windows.Input;

namespace ToDoApp.ViewModels
{
	public class CreateItemViewModel : BaseViewModel
	{
		private readonly IAppNavigationService _navigationService;
		private readonly INavigationService _navigationServiceFactory;
		private readonly IToDoItemsService _toDoItemsService;

		public CreateItemViewModel(IPageDialogService dialogService, IAppNavigationService navigationService, INavigationService navigationServiceFactory, IToDoItemsService toDoItemsService) : base(dialogService)
        {
            _navigationService = navigationService;
			_navigationServiceFactory = navigationServiceFactory;
			_toDoItemsService = toDoItemsService;

			NavigateToBackCommand = new Command(async () => await NavigateToBack());
			CreateNewItemCommand = new Command(async () => await CreateNewItem(), canExecute: () => { return !string.IsNullOrEmpty(Name); });

			Title = Translations.CreateTaskScreenTitle;
        }

		public ICommand NavigateToBackCommand { get; }
		public ICommand CreateNewItemCommand { get; }

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				SetProperty(ref _name, value);
				((Command)CreateNewItemCommand).ChangeCanExecute();
			}
		}

		private async Task NavigateToBack()
		{
			//await _navigationService.NavigateToBack();
			await _navigationServiceFactory.GoBackAsync();
		}

		private async Task CreateNewItem()
		{
			try
			{
				var request = new AddItemRequest(Configuration.BaseUrl)
				{
					Name = Name,
					IsComplete = false
				};

				var response = await _toDoItemsService.AddItem(request);

				await NavigateToBack();
			}
			catch (Exception exception)
			{
				await HandleException(exception);
			}
		}
	}
}
