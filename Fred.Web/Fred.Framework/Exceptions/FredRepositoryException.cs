using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    ///   Classe de base pour toute exception de type Repository
    ///   Si pertinant, dérivez la classe pour créer votre propre exception
    /// </summary>
    [Serializable]
    public class FredRepositoryException : FredException
    {
        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        public FredRepositoryException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredRepositoryException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredRepositoryException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredRepositoryNotFoundException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredRepositoryException(string message, object objectToDump, Exception innerException)
          : base(message, objectToDump, innerException)
        {
        }
        /// <summary>
        /// FredRepositoryException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredRepositoryException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }

}
