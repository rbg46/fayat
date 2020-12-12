using Fred.Framework.Exceptions;
using System;
using System.Runtime.Serialization;


namespace Fred.ImportExport.Framework.Exceptions
{
    /// <summary>
    ///   Classe de base pour toute exception de type métier
    ///   Si pertinant, dérivez la classe pour créer votre propre exception 
    /// </summary>
    [Serializable]
    public class FredIeBusinessException : FredException
    {

        /// <summary>
        /// FredIeBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        public FredIeBusinessException(string message)
          : base(message)
        {
        }

        /// <summary>
        /// FredIeBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Execption</param>
        public FredIeBusinessException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        /// FredIeBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">ObjectToDump</param>
        public FredIeBusinessException(string message, object objectToDump)
          : base(message, objectToDump)
        {
        }

        /// <summary>
        /// FredIeBusinessException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Execption</param>
        /// <param name="objectToDump">ObjectToDump</param>
        public FredIeBusinessException(string message, object objectToDump, Exception innerException)
          : base(message, objectToDump, innerException)
        {
        }

        /// <summary>
        /// FredIeBusinessException
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FredIeBusinessException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
