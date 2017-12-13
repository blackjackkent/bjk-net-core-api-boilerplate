﻿

namespace ApiBoilerplate.Infrastructure.Services
{
	using Entities;
	using Interfaces;
	using Models.DomainModels;
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Text;
	using System.Threading.Tasks;
	using AutoMapper;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.IdentityModel.Tokens;

	public class AuthService : IAuthService
	{
		public async Task<ApplicationUser> GetUserByUsernameOrEmail(string usernameOrEmail, UserManager<ApplicationUser> userManager)
		{
			var user = await userManager.FindByNameAsync(usernameOrEmail) ?? await userManager.FindByEmailAsync(usernameOrEmail);
			return user;
		}

		public async Task<JwtSecurityToken> GenerateJwt(ApplicationUser user, string key, string issuer, string audience, UserManager<ApplicationUser> userManager)
		{
			var claims = await GetUserClaims(user, userManager);
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer,
				audience,
				claims,
				expires: DateTime.UtcNow.AddMinutes(15),
				signingCredentials: creds);
			return token;
		}

		public async Task<User> GetCurrentUser(ClaimsPrincipal claimsUser, UserManager<ApplicationUser> userManager, IMapper mapper)
		{
			var identityUser = await userManager.GetUserAsync(claimsUser);
			return identityUser == null ? null : mapper.Map<User>(identityUser);
		}

		private static async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			var userClaims = await userManager.GetClaimsAsync(user);
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
			}.Union(userClaims);
			return claims;
		}
	}
}
