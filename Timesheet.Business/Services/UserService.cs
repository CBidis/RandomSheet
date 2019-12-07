using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Timesheet.Business.Contracts;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;
using Timesheets.Persistence.Exceptions;
using Timesheets.Persistence.Repositories;

namespace Timesheet.Business.Services
{
    public class UserService : GenericService<User, int, UserDto>, IUserService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, GenericRepository<User, int> rolesRepo, IMapper mapper) : base(rolesRepo, mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public override async Task<int> CreateAsync(UserDto user)
        {
            User appUser = _mapper.Map<User>(user);

            foreach (var role in user.Roles)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                    throw new EntityNotFoundException(nameof(Role), role, $"There is no Role with name {role}");
            }

            IdentityResult userCreationResult = await _userManager.CreateAsync(appUser, user.Password);
            EnsureIdentityResult(userCreationResult, appUser, "Create");

            IdentityResult userRolesCreationResult = await _userManager.AddToRolesAsync(appUser, user.Roles);
            EnsureIdentityResult(userRolesCreationResult, appUser, "Assign Roles");

            return appUser.Id;
        }

        /// <summary>
        /// Validates Identity result as succesful, or throws an InvalidOperation Exception
        /// </summary>
        /// <param name="actionResult">IdentityResult</param>
        /// <param name="user">AspNetUsers</param>
        /// <param name="action">action Performed</param>
        private void EnsureIdentityResult(IdentityResult identityResult, User user, string action)
        {
            if (!identityResult.Succeeded)
                throw new InvalidOperationException($"Could not {action} user with Id {user.Id}," +
                    $" {string.Join(" ,", identityResult.Errors.Select(c => c.Description))}");
        }
    }
}
