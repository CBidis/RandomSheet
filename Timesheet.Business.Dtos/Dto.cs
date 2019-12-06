namespace Timesheet.Business.Dtos
{
    /// <summary>
    /// Marker Interfaces for all DTO objects
    /// </summary>
    public class Dto<TKey> : IDto<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
