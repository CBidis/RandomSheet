using System.Collections.Generic;
using System.Threading.Tasks;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;

namespace Timesheet.Business.Contracts
{
    public interface IRoleService : IGenericService<Role, int, RoleDto>
    {
        /// <summary>
        /// Add roles if they dont exist
        /// </summary>
        /// <param name="roles">roles to add</param>
        Task SeedRolesAsync(List<RoleDto> roles);
    }
}
