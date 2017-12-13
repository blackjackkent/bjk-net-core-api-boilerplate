namespace ApiBoilerplate.Infrastructure.Seeders
{
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Entities;
	using Microsoft.AspNetCore.Identity;

	public class RoleInitializer
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public RoleInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task Seed()
		{
			var userRole = await _roleManager.FindByNameAsync("User");
			if (userRole == null)
			{
				userRole = new IdentityRole("User");
				await _roleManager.CreateAsync(userRole);
			}
			var adminRole = await _roleManager.FindByNameAsync("Admin");
			if (adminRole == null)
			{
				adminRole = new IdentityRole("Admin");
				await _roleManager.CreateAsync(adminRole);
			}
		}
	}
}
