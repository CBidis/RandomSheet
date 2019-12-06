namespace Timesheet.Business.Dtos
{
    /// <summary>
    /// Marker Interfaces for all DTO objects
    /// </summary>
    public interface IDto<TKey> where TKey : struct
    {
        TKey Id { get; set; }
    }
}
