using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;

namespace Timesheet.Business.Contracts
{
    public interface IUserService : IGenericService<User, int, UserDto>
    {
    }
}
