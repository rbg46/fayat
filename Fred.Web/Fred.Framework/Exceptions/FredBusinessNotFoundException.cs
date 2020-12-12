using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    /// Excetion pour gérer le notfound
    /// </summary>
    [Serializable]
    public class FredBusinessNotFoundException : FredBusinessException
    {

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        public FredBusinessNotFoundException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessNotFoundException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredBusinessNotFoundException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessNotFoundException(string message, object objectToDump, Exception innerException)
      : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredRepositoryException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredBusinessNotFoundException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
