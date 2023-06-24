using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Mappers;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models;
using LongoToDoApp.Services.Abstractions;
using LongoToDoApp.Settings;
using LongoToDoApp.ViewModels.Base;
using LongoToDoApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LongoToDoApp.ViewModels
{
	public class ToDoItemsViewModel : BaseViewModel
	{
		private readonly IAppNavigationService _navigationService;
		private readonly IToDoItemsService _toDoItemsService;

        public ToDoItemsViewModel(IPageDialogService dialogService, IAppNavigationService navigationService, IToDoItemsService toDoItemsService) : base(dialogService)
        {
            _toDoItemsService = toDoItemsService;
			_navigationService = navigationService;

			DeleteItemCommand = new Command<ToDoItem>(async (itemSelected) => await DeleteItem(itemSelected));
			CheckedCommand = new Command(Checked);
			NavigateToCreateItemCommand = new Command(async () => await NavigateToCreateItem());
		}

        public override async Task OnNavigatedImplementation(INavigationParameters parameters)
        {
            await base.OnNavigatedImplementation(parameters);
            await LoadData();
        }

        public ICommand DeleteItemCommand { get; }
		public ICommand CheckedCommand { get; }
		public ICommand NavigateToCreateItemCommand { get; }

		private ObservableCollection<ToDoItem> _toDoItems;
        public ObservableCollection<ToDoItem> ToDoItems
        {
            get => _toDoItems;
            set => SetProperty(ref _toDoItems, value);
        }

        private int _numberItems;
        public int NumberItems
        {
            get => _numberItems;
            set => SetProperty(ref _numberItems, value);
        }

		private async Task LoadData()
        {
            try
            {
				var request = new ToDoItemsRequest(Configuration.BaseUrl);
				var response = await _toDoItemsService.GetToDoItems(request);
				var toDoItems = BackendToModelMapper.GetToDoItems(response);

                ToDoItems = new ObservableCollection<ToDoItem>(toDoItems);
                NumberItems = toDoItems.Where(x => x.IsComplete == false).Count();
			}
			catch (Exception exception)
            {
                await HandleException(exception);
            }
        }

        private async Task DeleteItem(ToDoItem selectedItem)
        {
			try
			{
				if (await DisplayAlert("LongoToDo", "Do you want to remove this item?", cancelButtonTitle: "No", acceptButtonTitle: "Yes"))
				{
					var request = new DeleteItemRequest(Configuration.BaseUrl, selectedItem.Key);
					await _toDoItemsService.DeleteItem(request);
					await LoadData();

					await DisplayAlert("LongoToDo", $"{string.Format("ToDo item {0} has been deleted correctly", selectedItem.Name)}", "Ok");
				}
			}
			catch (Exception exception)
			{
				await HandleException(exception);
			}
        }

		private void Checked()
		{
			NumberItems = ToDoItems.Where(x => x.IsComplete == false).Count();
		}

		private async Task NavigateToCreateItem()
		{
			await _navigationService.NavigateTo(nameof(CreateItemView));
		}
	}
}
