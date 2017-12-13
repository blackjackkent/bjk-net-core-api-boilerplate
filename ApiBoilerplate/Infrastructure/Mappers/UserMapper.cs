namespace ApiBoilerplate.Infrastructure.Mappers
{
	using AutoMapper;
	using Entities;
	using Models.DomainModels;
	using Models.ViewModels;

	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<User, ApplicationUser>()
				.ReverseMap();
			CreateMap<User, UserDto>()
				.ReverseMap();
		}
	}
}
