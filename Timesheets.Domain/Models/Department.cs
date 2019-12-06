using System.Collections.Generic;

namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Department Domain Model
    /// </summary>
    public class Department : BaseEntity<int>
    {
        public string Name { get; set; }
        // one department works on many projects
        public IList<ProjectDepartment> RelatedProjects { get; set; }

        // one deparment onws many projects
        public IList<Project> OwnedProjects { get; set; }
    }
}
