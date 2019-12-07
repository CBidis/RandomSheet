namespace Timesheets.Domain.Models
{
    /// <summary>
    /// Project Department Domain Model
    /// </summary>
    public class ProjectDepartment : BaseEntity<int>
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
