using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Mappers;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models;
using LongoToDoApp.ViewModels.Base;
using LongoToDoApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LongoToDoApp.ViewModels
{
	public class ToDoItemsViewModel : BaseViewModel
	{
        private readonly INavigationService _navigationService;
        private readonly IToDoItemsService _toDoItemsService;

        public ToDoItemsViewModel(IPageDialogService dialogService, INavigationService navigationService, IToDoItemsService toDoItemsService) : base(dialogService)
        {
            _navigationService = navigationService;
            _toDoItemsService = toDoItemsService;

			NavigateToCreateItemCommand = new Command(async () => await NavigateToCreateItem());
        }

        public override async Task OnNavigatedImplementation(INavigationParameters parameters)
        {
            await base.OnNavigatedImplementation(parameters);
            await LoadData();
        }

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
				var request = new ToDoItemsRequest("http://10.0.2.2:8080");
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

        private async Task NavigateToCreateItem()
        {
            await _navigationService.NavigateAsync(nameof(CreateItemView));
		}
	}
}
