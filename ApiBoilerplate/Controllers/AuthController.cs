namespace ApiBoilerplate.Controllers
{
	using System;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Text;
	using System.Threading.Tasks;
	using Infrastructure.Entities;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.Logging;
	using Microsoft.IdentityModel.Tokens;
	using Models.RequestModels;

	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
		private readonly IConfiguration _config;

		public AuthController(ILogger<AuthController> logger, SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, IConfiguration config)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
			_passwordHasher = passwordHasher;
			_config = config;
		}

		[HttpPost("api/auth/token")]
		public async Task<IActionResult> CreateToken([FromBody] LoginRequest model)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(model.Username);
				if (user != null)
				{
					if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) ==
					    PasswordVerificationResult.Success)
					{
						var userClaims = await _userManager.GetClaimsAsync(user);
						var claims = new[]
						{
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
							new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
							new Claim(JwtRegisteredClaimNames.GivenName, user.UserName ),
							new Claim(JwtRegisteredClaimNames.FamilyName, user.UserName),
							new Claim(JwtRegisteredClaimNames.Email, user.Email),
						}.Union(userClaims);
						var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
						var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
						var token = new JwtSecurityToken(
							_config["Tokens:Issuer"],
							_config["Tokens:Audience"],
							claims,
							expires: DateTime.UtcNow.AddMinutes(15),
							signingCredentials: creds);
						return Ok(new
						{
							token = new JwtSecurityTokenHandler().WriteToken(token),
							expiration = token.ValidTo
						});
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error while creating JWT: {ex}");
			}
			return BadRequest("Failed to create JWT.");
		}

		[HttpPost("api/auth/register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				try
				{
					var result = await _userManager.CreateAsync(user, model.Password);
					var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
					if (result.Succeeded && roleResult.Succeeded)
					{
						_logger.LogInformation(3, "User created a new account with password.");
						return Ok();
					}
					return BadRequest(result);
				}
				catch (Exception e)
				{
					return BadRequest(e);
				}
			}
			return BadRequest();
		}
	}
}
