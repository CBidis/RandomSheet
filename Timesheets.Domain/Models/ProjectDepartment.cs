namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Project Department Domain Model
    /// </summary>
    public class ProjectDepartment : BaseEntity<int>
    {
        public long ProjectId { get; set; }
        public Project Project { get; set; }

        public long DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
