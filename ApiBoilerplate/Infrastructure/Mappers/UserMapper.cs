namespace ApiBoilerplate.Infrastructure.Mappers
{
	using AutoMapper;
	using Entities;
	using Models.DomainModels;

	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<User, ApplicationUser>()
				.ReverseMap();
		}
	}
}
