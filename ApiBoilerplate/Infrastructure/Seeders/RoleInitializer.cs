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

			var adminRole = await _roleManager.FindByNameAsync("Admin");
			if (adminRole == null)
			{
				adminRole = new IdentityRole("Admin");
				await _roleManager.CreateAsync(adminRole);

				await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", "manage-locations"));
			}
		}
	}
}
