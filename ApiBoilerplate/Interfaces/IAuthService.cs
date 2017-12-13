

namespace ApiBoilerplate.Interfaces
{
	using System.IdentityModel.Tokens.Jwt;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Models.DomainModels;
	using AutoMapper;
	using Infrastructure.Entities;
	using Microsoft.AspNetCore.Identity;

	public interface IAuthService
	{
		Task<ApplicationUser> GetUserByUsernameOrEmail(string modelUsername, UserManager<ApplicationUser> userManager);
		Task<JwtSecurityToken> GenerateJwt(ApplicationUser user, string key, string issuer, string audience, UserManager<ApplicationUser> userManager);
		Task<User> GetCurrentUser(ClaimsPrincipal claimsUser, UserManager<ApplicationUser> userManager, IMapper mapper);
	}
}
