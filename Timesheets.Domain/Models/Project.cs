using System.Collections.Generic;

namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Project Domain Model
    /// </summary>
    public class Project : BaseEntity<int>
    {
        public string ProjectName { get; set; }
        // one project can be done by many deparments
        public IList<ProjectDepartment> RelatedDeparments { get; set; }

        // one project is owned by one deparment
        public long OwnerDeparmentId { get; set; }
        public Department OwnerDeparment { get; set; }
    }
}
