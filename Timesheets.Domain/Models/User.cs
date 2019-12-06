using Microsoft.AspNetCore.Identity;

namespace Timesheets.Domain.Models
{
    /// <summary>
    /// User Domain Model extending IdentityUser<Tkey> model
    /// </summary>
    public class User : IdentityUser<int>, IEntity<int>
    {
        public Timesheet Timesheet { get; set; }
    }
}
