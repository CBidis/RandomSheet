using AutoMapper;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;

namespace Timesheet.Business.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                        .ForMember(entity => entity.NormalizedEmail, opt => opt.MapFrom(c => c.Email.ToUpper()))
                        .ForMember(entity => entity.Email, opt => opt.MapFrom(c => c.Email))
                        .ForMember(entity => entity.UserName, opt => opt.MapFrom(c => c.Email))
                        .ForMember(entity => entity.NormalizedUserName, opt => opt.MapFrom(c => c.Email.ToUpper()))
                        .ForMember(entity => entity.EmailConfirmed, opt => opt.MapFrom(c => true))
                        .ForMember(entity => entity.PhoneNumberConfirmed, opt => opt.MapFrom(c => true));
        }
    }
}
