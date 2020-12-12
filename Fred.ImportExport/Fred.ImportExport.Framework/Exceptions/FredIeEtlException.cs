using System;
using System.Runtime.Serialization;

namespace Fred.ImportExport.Framework.Exceptions
{
  /// <summary>
  ///   Base class for all custom exception in the ETL
  ///   EtlProcess.OnError sera appelé
  /// </summary>
  [Serializable]
  public abstract class FredIeEtlException : FredIeBusinessException
  {

    /// <summary>
    ///   Initialise une nouvelle instance de FredIeEtlException pour la gestion des erreurs
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    protected FredIeEtlException(string message)
      : base(message)
    {
    }

    /// <summary>
    ///   Initialise une nouvelle instance de classe pour la gestion des erreurs
    ///   message and a reference to the inner exception that is the cause of this exception.
    ///   <see cref="T:Fred.ImportExport.Framework.Etl.Engine.Exceptions.FredIeEtlException`2" />
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    ///   The exception that is the cause of the current exception, or a null reference
    ///   (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    protected FredIeEtlException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    ///   Without this constructor, deserialization will fail
    /// </summary>
    /// <param name="info">info</param>
    /// <param name="context">context</param>
    protected FredIeEtlException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

