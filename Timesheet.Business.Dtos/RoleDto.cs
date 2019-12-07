namespace Timesheet.Business.Dtos
{
    public class RoleDto : Dto<int>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
