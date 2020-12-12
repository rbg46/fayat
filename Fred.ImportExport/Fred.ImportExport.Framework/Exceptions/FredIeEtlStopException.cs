using System;
using System.Runtime.Serialization;

namespace Fred.ImportExport.Framework.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    ///   Exception lancée pour arrêter le workflow de l'ETL
    ///   EtlProcess.OnError sera appelé
    /// </summary>
    [Serializable]
    public class FredIeEtlStopException : FredIeEtlException
    {
        /// <summary>
        /// FredIeEtlStopException
        /// </summary>
        /// <param name="message">Message</param>
        public FredIeEtlStopException(string message)
      : base(message)
        {
        }

        /// <summary>
        /// FredIeEtlStopException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredIeEtlStopException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        /// <summary>
        ///   Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected FredIeEtlStopException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

    }
}

