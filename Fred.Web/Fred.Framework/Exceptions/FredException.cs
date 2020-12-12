using System;
using System.Runtime.Serialization;
using System.Security;

namespace Fred.Framework.Exceptions
{
  /// <summary>
  ///   Base class for all custom exception in Fred
  ///   Will be automaticaly logged with
  ///   - the message if not empty
  ///   - the inner exception(s) recursively if not null
  ///   - the objectToDump if not null
  /// </summary>
  /// <example>
  ///   Client code :
  ///   throw new FredBusinessException("Sample", entity, originalException);
  ///   Catch code :
  ///   catch (FredBusinessException ex)
  ///   {
  ///   ex.LogException();
  ///   do some error handling
  ///   }
  /// </example>
  [Serializable]
  public abstract class FredException : Exception
  {
    /// <summary>
    /// Il est interdit de créer une exception sans paramètre
    /// </summary>
    private FredException()
    {

    }

    /// <summary>
    ///   Initialise une nouvelle instance de FredException pour la gestion des erreurs
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    protected FredException(string message)
      : base(message)
    {
    }

    /// <summary>
    ///   Initialise une nouvelle instance de classe pour la gestion des erreurs
    ///   message and a reference to the inner exception that is the cause of this exception. <see cref="FredException" />
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    ///   The exception that is the cause of the current exception, or a null reference
    ///   (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    protected FredException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    ///   Initialise une nouvelle instance de classe pour la gestion des erreurs
    ///   message and a reference to an object witch properties will be dump to log. <see cref="FredException" />
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="objectToDump">
    ///   Properties of this object will be dump to log
    ///   Usefull if you want to log informations about an object for debugging purpose
    /// </param>
    protected FredException(string message, object objectToDump)
      : base(message)
    {
      ObjectToDump = objectToDump;
    }

    /// <summary>
    ///   Initialise une nouvelle instance de classe pour la gestion des erreurs
    ///   message and a reference to an object witch properties will be dump to log. <see cref="FredException" />
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="objectToDump">
    ///   Properties of this object will be dump to log
    ///   Usefull if you want to log informations about an object for debugging purpose
    /// </param>
    /// <param name="innerException">
    ///   The exception that is the cause of the current exception, or a null reference
    ///   (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    protected FredException(string message, object objectToDump, Exception innerException)
      : base(message, innerException)
    {
      ObjectToDump = objectToDump;
    }


    /// <summary>
    ///   Without this constructor, deserialization will fail
    /// </summary>
    /// <param name="info">info</param>
    /// <param name="context">context</param>
    protected FredException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }



    /// <summary>
    ///   All the properties of this object will be dump
    /// </summary>
    /// <value>
    ///   The object to dump.
    /// </value>
    public object ObjectToDump { get; set; }

    /// <summary>
    ///   Service dans lequel est généré l'exception
    /// </summary>
    /// <value>
    ///   The service.
    /// </value>
    public string Service { get; set; }

    /// <summary>
    ///   Utilisateur
    /// </summary>
    /// <value>
    ///   The user login.
    /// </value>
    public string UserLogin { get; set; }

    /// <summary>
    ///   Override GetObjectData() and make sure you call through to base.GetObjectData(info, context) at the end,
    ///   in order to let the base class save its own state.
    /// </summary>
    /// <param name="info">info</param>
    /// <param name="context">context</param>
    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException(nameof(info));
      }

      info.AddValue("Service", Service);
      info.AddValue("UserLogin", UserLogin);
      info.AddValue("Object to dump", ObjectToDump);
      base.GetObjectData(info, context);
    }
  }
}