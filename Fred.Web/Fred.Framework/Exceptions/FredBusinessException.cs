using System;
using System.Runtime.Serialization;

#pragma warning disable S3925 // ISerializable is correctly implemented


namespace Fred.Framework.Exceptions
{
    /// <summary>
    ///   Classe de base pour toute exception de type métier
    ///   Si pertinant, dérivez la classe pour créer votre propre exception 
    /// </summary>
    [Serializable]
    public class FredBusinessException : FredException
    {

        /// <summary>
        /// FredBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        public FredBusinessException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">InnerException</param>
        public FredBusinessException(string message, Exception innerException)
      : base(message, innerException)
        {
        }

        /// <summary>
        /// FredBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredBusinessException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessException(string message, object objectToDump, Exception innerException)
          : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredBusinessException
        /// </summary>
        /// <param name="info"><see cref="FredBusinessException"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredBusinessException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}

#pragma warning restore S3925 // ISerializable is correctly implemented
