using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    /// Représente une exception de type métier qui sera renvoyée au front via un message et non via la méthode classique.
    /// </summary>
    [Serializable]
    public class FredBusinessMessageResponseException : FredBusinessException, IFredMessageResponseException
    {

        /// <summary>
        /// FredBusinessMessageResponseException
        /// </summary>
        /// <param name="message">Message</param>
        public FredBusinessMessageResponseException(string message)
          : base(message)
        { }

        /// <summary>
        /// FredBusinessMessageResponseException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessMessageResponseException(string message, Exception innerException)
          : base(message, innerException)
        { }

        /// <summary>
        /// FredBusinessMessageResponseException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredBusinessMessageResponseException(string message, object objectToDump)
          : base(message, objectToDump)
        { }

        /// <summary>
        /// FredBusinessMessageResponseException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBusinessMessageResponseException(string message, object objectToDump, Exception innerException)
      : base(message, objectToDump, innerException)
        { }

        /// <summary>
        /// FredBusinessMessageResponseException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredBusinessMessageResponseException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        { }
    }
}
