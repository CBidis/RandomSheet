namespace Timesheets.Domain.Models
{

    /// <summary>
    /// IEnity interface definition
    /// </summary>
    /// <typeparam name="TKey">type of key</typeparam>
    public interface IEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
