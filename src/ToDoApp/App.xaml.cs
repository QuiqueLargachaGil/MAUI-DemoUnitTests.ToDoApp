using ToDoApp.Helpers;

namespace ToDoApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		LocalizationResourceManager.Init();
	}
}
