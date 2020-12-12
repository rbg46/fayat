using System;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{

    /// <summary>
    /// Excetion pour gérerles BadGateway
    /// </summary>
    [Serializable]
    public class FredBadGatewayException : FredBusinessException
    {
        /// <summary>
        /// FredBadGatewayException
        /// </summary>
        /// <param name="message">Message</param>
        public FredBadGatewayException(string message)
      : base(message)
        {
        }

        /// <summary>
        /// FredBadGatewayException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBadGatewayException(string message, Exception innerException)
      : base(message, innerException)
        {
        }

        /// <summary>
        ///   Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected FredBadGatewayException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

        /// <summary>
        /// FredBadGatewayException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        public FredBadGatewayException(string message, object objectToDump)
      : base(message, objectToDump)
        {
        }


        /// <summary>
        /// FredBadGatewayException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="objectToDump">Object to Dump</param>
        /// <param name="innerException">Inner Exception</param>
        public FredBadGatewayException(string message, object objectToDump, Exception innerException)
      : base(message, objectToDump, innerException)
        {
        }
    }
}
