namespace ApiBoilerplate.Infrastructure.Entities
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{ }

		public DbSet<ApplicationUser> Users { get; set; }
	}
}
