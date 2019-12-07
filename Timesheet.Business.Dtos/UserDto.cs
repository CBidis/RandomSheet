using System.Collections.Generic;

namespace Timesheet.Business.Dtos
{
    public class UserDto : Dto<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual string UserName { get; set; }
        public string Mobile { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
