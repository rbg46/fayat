using System;
using System.Runtime.Serialization;

namespace Fred.ImportExport.Framework.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    ///   Exception lancée lorsque la configuration du workflow de l'ETL est invalide
    /// </summary>
    [Serializable]
    public class FredIeEtlConfigException : FredIeEtlException
    {
        /// <summary>
        /// FredIeEtlConfigException
        /// </summary>
        /// <param name="message">Message</param>
        public FredIeEtlConfigException(string message)
      : base(message)
        {
        }

        /// <summary>
        /// FredIeEtlConfigException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">InnerException</param>
        public FredIeEtlConfigException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        ///   Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected FredIeEtlConfigException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
