using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    ///   Exception lancée par un repository lorsqu'il ne trouve pas un objet
    /// </summary>
    [Serializable]
    public class FredRepositoryNotFoundException : FredRepositoryException
    {
        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        public FredRepositoryNotFoundException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredRepositoryNotFoundException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredRepositoryNotFoundException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredRepositoryNotFoundException(string message, object objectToDump, Exception innerException)
          : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredRepositoryNotFoundException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}

