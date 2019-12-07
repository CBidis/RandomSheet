using Microsoft.AspNetCore.Identity;

namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Role Domain Model extending IdentityRole<Tkey> model
    /// </summary>
    public class Role : IdentityRole<int>, IEntity<int>
    {
    }
}
