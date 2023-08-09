using ToDoApp.Services.Abstractions;
using Moq;

namespace ToDoApp.Test.Infrastructure.Services.Base
{
	public abstract class BaseServiceTest
	{
		protected readonly Mock<ICheckConnectivityService> CheckConnectivityService;

		protected BaseServiceTest()
		{
			CheckConnectivityService = new Mock<ICheckConnectivityService>();
			CheckConnectivityService.Setup(m => m.HasInternetConnection()).ReturnsAsync(true);
		}
	}
}
