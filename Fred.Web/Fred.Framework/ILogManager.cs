using System;
using System.Collections.Generic;
using Fred.Framework.Exceptions;

namespace Fred.Framework
{
  /// <summary>
  ///   Gestionnaire de logs, encapsule l'ensemble des systèmes sous-jacents.
  /// </summary>
  public interface ILogManager
  {
    /// <summary>
    ///   Ecrit une entrée de type Debug dans les logs.
    /// </summary>
    /// <param name="message">Message de warning à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceDebug(string message, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type Exception dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="ex">L'exception à logguer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceException(string message, Exception ex, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type FredException dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="ex">L'exception à logguer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceException(string message, FredException ex, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type Error dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceError(string message, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type Information dans les logs.
    /// </summary>
    /// <param name="message">Message d'information à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceInformation(string message, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type Warning dans les logs.
    /// </summary>
    /// <param name="message">Message de warning à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void TraceWarning(string message, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée non typée dans les logs.
    /// </summary>
    /// <param name="message">Message à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    void WriteLine(string message, IDictionary<string, string> properties = null);

    /// <summary>
    ///   Ecrit une entrée de type Debug contenant toutes les properties de l'object
    /// </summary>
    /// <param name="message">message décrivant objectToDump</param>
    /// <param name="objectToDump">objet dont les properties seront dumpées en string</param>
    void TraceObject(string message, object objectToDump);
  }
}