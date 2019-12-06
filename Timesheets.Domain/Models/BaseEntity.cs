namespace Timesheets.Domain.Models
{
    /// <summary>
    /// IEntity Implementation
    /// </summary>
    /// <typeparam name="TKey">type of key</typeparam>
    public class BaseEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// Primary Key Value
        /// </summary>
        public TKey Id { get; set; }
    }
}
