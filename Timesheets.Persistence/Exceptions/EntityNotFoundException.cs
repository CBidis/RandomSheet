using System;

namespace Timesheets.Persistence.Exceptions
{
    /// <summary>
    /// Exception that indicates that the requested entity does not exist
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, object keyValue, string exMessage) : base(exMessage)
        {
            EntityName = entityName;
            KeyValue = keyValue;
        }

        /// <summary>
        /// Entity Name of Table
        /// </summary>
        public string EntityName { get; }
        /// <summary>
        /// Primary key value requested that was not found
        /// </summary>
        public object KeyValue { get; }
    }
}
