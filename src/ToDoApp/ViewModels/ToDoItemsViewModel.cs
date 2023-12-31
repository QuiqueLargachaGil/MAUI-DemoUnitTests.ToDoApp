﻿using ToDoApp.Infrastructure.Abstractions;
using ToDoApp.Infrastructure.Mappers;
using ToDoApp.Infrastructure.Services.ToDoItems.Models;
using ToDoApp.Models;
using ToDoApp.Resources.Translations;
using ToDoApp.Services.Abstractions;
using ToDoApp.Settings;
using ToDoApp.ViewModels.Base;
using ToDoApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ToDoApp.ViewModels
{
	public class ToDoItemsViewModel : BaseViewModel
	{
		private readonly IAppNavigationService _navigationService;
		private readonly IToDoItemsService _toDoItemsService;

        public ToDoItemsViewModel(IPageDialogService dialogService, IAppNavigationService navigationService, IToDoItemsService toDoItemsService) : base(dialogService)
        {
            _toDoItemsService = toDoItemsService;
			_navigationService = navigationService;

			RefreshCommand = new Command(async () => await Refresh());
			DeleteItemCommand = new Command<ToDoItem>(async (itemSelected) => await DeleteItem(itemSelected));
			CheckedCommand = new Command<ToDoItem>(async (selectedItem) => await Checked(selectedItem));
			NavigateToCreateItemCommand = new Command(async () => await NavigateToCreateItem());
		}

        public override async Task OnNavigatedImplementation(INavigationParameters parameters)
        {
            await base.OnNavigatedImplementation(parameters);
            await LoadData();
        }

		public ICommand RefreshCommand { get; }
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

		private async Task Refresh()
		{
			IsBusy = true;
			await LoadData();
			IsBusy = false;
		}

		private async Task DeleteItem(ToDoItem selectedItem)
        {
			try
			{
				if (await DisplayAlert(Configuration.ApplicationName, Translations.RemoveTaskConfirmationMessage, cancelButtonTitle: Translations.No, acceptButtonTitle: Translations.Yes))
				{
					var request = new DeleteItemRequest(Configuration.BaseUrl, selectedItem.Key);
					await _toDoItemsService.DeleteItem(request);
					await LoadData();

					await DisplayAlert(Configuration.ApplicationName, $"{string.Format(Translations.RemoveTaskAlertMessage, selectedItem.Name)}", Translations.Ok);
				}
			}
			catch (Exception exception)
			{
				await HandleException(exception);
			}
        }

		private async Task Checked(ToDoItem selectedItem)
		{
			try
			{
				if (selectedItem is null)
				{
					return;
				}

				NumberItems = ToDoItems.Where(x => x.IsComplete == false).Count();

				var request = new UpdateItemRequest(Configuration.BaseUrl, selectedItem);
				await _toDoItemsService.UpdateItem(request);
			}
			catch (Exception exception)
			{
				await HandleException(exception);
			}
		}

		private async Task NavigateToCreateItem()
		{
			await _navigationService.NavigateTo(nameof(CreateItemView));
		}
	}
}
