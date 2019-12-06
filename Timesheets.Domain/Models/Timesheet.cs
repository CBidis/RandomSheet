using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Timesheet Domain Model
    /// </summary>
    public class Timesheet : BaseEntity<int>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public int HoursWorked { get; set; }
    }
}
