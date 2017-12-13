namespace ApiBoilerplate.Controllers
{
	using Infrastructure.Providers;
	using Microsoft.AspNetCore.Mvc;

	[ServiceFilter(typeof(GlobalExceptionHandler))]
	public class BaseController : Controller
	{
	}
}
