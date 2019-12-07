using System.Collections.Generic;
using Timesheet.Business.Dtos;

namespace Timesheets.Web.Data
{
    /// <summary>
    /// Contains Seed Data for Database, used in startup of Application
    /// </summary>
    public static class Seed
    {
        public static List<RoleDto> GetInitialRoles() => new List<RoleDto>
            {
                new RoleDto{ Name = "Admin", NormalizedName = "ADMIN" },
                new RoleDto{ Name = "Manager", NormalizedName = "MANAGER" },
                new RoleDto{ Name = "Employee", NormalizedName = "EMPLOYEE" }
            };

        public static UserDto GetDefaultAdmin() => new UserDto
            {
                Email = "cbidis88@gmail.com",
                Roles = new List<string> { "Admin" },
                Password = "1234cc!@#$",
                UserName = "Chris Bidis"
            };
    }
}
