using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    /// <summary>
    ///   Classe de base pour toute exception de type technique
    ///   Si pertinant, dérivez la classe pour créer votre propre exception
    /// </summary>
    /// <seealso cref="Fred.Framework.Exceptions.FredException" />
    [Serializable]
    public class FredTechnicalException : FredException
    {

        /// <summary>
        /// FredTechnicalException
        /// </summary>
        /// <param name="message">Message</param>
        public FredTechnicalException(string message)
          : base(message)
        {
        }


        /// <summary>
        /// FredTechnicalException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">InnerException</param>
        public FredTechnicalException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredTechnicalException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">objectToDump</param>
        public FredTechnicalException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }


        /// <summary>
        /// FredTechnicalException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">objectToDump</param>
        /// <param name="innerException">innerException</param>
        public FredTechnicalException(string message, object objectToDump, Exception innerException)
          : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredTechnicalException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredTechnicalException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }


}
