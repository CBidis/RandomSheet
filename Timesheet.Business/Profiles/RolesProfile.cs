using AutoMapper;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;

namespace Timesheet.Business.Profiles
{
    public class RolesProfile : Profile
    {
        public RolesProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }
}
