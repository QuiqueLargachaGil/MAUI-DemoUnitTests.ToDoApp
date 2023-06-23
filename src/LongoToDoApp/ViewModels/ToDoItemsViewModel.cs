using LongoToDoApp.Infrastructure.Abstractions;
using LongoToDoApp.Infrastructure.Mappers;
using LongoToDoApp.Infrastructure.Services.ToDoItems.Models;
using LongoToDoApp.Models;
using LongoToDoApp.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LongoToDoApp.ViewModels
{
	public class ToDoItemsViewModel : BaseViewModel
	{
        private readonly IToDoItemsService _toDoItemsService;

        public ToDoItemsViewModel(IPageDialogService dialogService, IToDoItemsService toDoItemsService) : base(dialogService)
        {
            _toDoItemsService = toDoItemsService;
        }

        public override async Task OnNavigatedImplementation(INavigationParameters parameters)
        {
            await base.OnNavigatedImplementation(parameters);
            await LoadData();
        }

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
	}
}
