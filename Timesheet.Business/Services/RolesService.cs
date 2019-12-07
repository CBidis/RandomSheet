using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Timesheet.Business.Contracts;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;
using Timesheets.Persistence.Repositories;

namespace Timesheet.Business.Services
{
    public class RolesService : GenericService<Role, int, RoleDto>, IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public RolesService(RoleManager<Role> roleManager, GenericRepository<Role, int> rolesRepo, IMapper mapper) : base(rolesRepo, mapper) => _roleManager = roleManager;

        public async Task SeedRolesAsync(List<RoleDto> roles)
        {
            foreach(RoleDto role in roles)
            {
                if (await _roleManager.FindByNameAsync(role.Name) == null)
                {
                    Role dbRole = _mapper.Map<RoleDto, Role>(role);
                    await _baseRepo.AddAsync(dbRole);
                }
                    
            }
        }
    }
}
