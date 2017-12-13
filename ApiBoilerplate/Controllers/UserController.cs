﻿namespace ApiBoilerplate.Controllers
{
	using System;
	using System.Threading.Tasks;
	using AutoMapper;
	using Infrastructure.Entities;
	using Interfaces;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using Models.ViewModels;

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
	public class UserController : BaseController
	{
		private readonly ILogger<AuthController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public UserController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager,
			IMapper mapper, IAuthService authService)
		{
			_logger = logger;
			_userManager = userManager;
			_mapper = mapper;
			_authService = authService;
		}
		// GET api/values
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var claimsUser = User;
				var user = await _authService.GetCurrentUser(claimsUser, _userManager, _mapper);
				if (user != null)
					return Ok(_mapper.Map<UserDto>(user));
				return NotFound();
			}
			catch (Exception e)
			{
				_logger.LogError($"Error retrieving current user: {e.Message}");
			}
			return BadRequest("Error retrieving current user.");
		}
	}
}
