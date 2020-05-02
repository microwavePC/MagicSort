using System;
using System.Runtime.Serialization;

namespace MagicSort
{
    /// <summary>
    /// Represents errors that occur during the sort key's property does not exist.
    /// </summary>
    [Serializable()]
    public class SortTargetPropertyNotExistException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SortTargetPropertyNotExistException()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SortTargetPropertyNotExistException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SortTargetPropertyNotExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected SortTargetPropertyNotExistException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
