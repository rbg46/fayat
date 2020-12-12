using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    /// Excetion pour gérer le conflict d'une donnée déjà existante
    /// </summary>
    [Serializable]
    public class FredBusinessConflictException : FredBusinessException
    {

        /// <summary>
        /// FredBusinessConflictException
        /// </summary>
        /// <param name="message">Message</param>
        public FredBusinessConflictException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredBusinessConflictException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessConflictException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredBusinessConflictException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredBusinessConflictException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredBusinessConflictException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessConflictException(string message, object objectToDump, Exception innerException)
      : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredBusinessConflictException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredBusinessConflictException(SerializationInfo info, StreamingContext context)
      : base(info, context)
        {
        }

    }

}

