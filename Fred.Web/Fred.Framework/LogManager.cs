using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;

namespace Fred.Framework
{
  /// <summary>
  ///   Gestionnaire de logs, encapsule l'ensemble des systèmes sous-jacents.
  /// </summary>
  public class LogManager : ILogManager
  {
    /// <summary>
    ///   Transforme un dictionnaire de string/string en texte brute
    ///   Chaque entrée/valeur est écrite sur une nouvelle ligne
    /// </summary>
    /// <param name="properties">Propriétés du sictionnaire</param>
    /// <returns>Retourne un string</returns>
    private static string DictionnaryToString(IDictionary<string, string> properties)
    {
      string propString = string.Empty;
      if (properties != null)
      {
        StringBuilder flatProp = new StringBuilder();
        foreach (string key in properties.Keys)
        {
          flatProp.AppendFormat("\"{0}\": \"{1}\"\n", key, properties[key]);
        }

        propString = flatProp.ToString();
      }

      return propString;
    }

    /// <summary>
    ///   Ecrit une entrée non typée dans les logs.
    /// </summary>
    /// <param name="message">Message à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void WriteLine(string message, IDictionary<string, string> properties = null)
    {
      try
      {
        string propString = DictionnaryToString(properties);
        Trace.WriteLine($"{message}\n{propString}");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Exception dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="ex">L'exception à logguer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceException(string message, Exception ex, IDictionary<string, string> properties = null)
    {
      try
      {
        StringBuilder txt = new StringBuilder();
        txt.AppendLine();
        txt.AppendLine("==== Begin Dumping Exception ====");
        txt.AppendLine(message);
        txt.Append(DumpObject.SerializeException(ex));

        if (properties != null)
        {
          txt.Append("Properties : ").AppendLine(DictionnaryToString(properties));
        }

        txt.AppendLine("==== End Dumping Exception ====");
        TraceError(txt.ToString());
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type FredException dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="ex">L'exception à logguer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceException(string message, FredException ex, IDictionary<string, string> properties = null)
    {
      try
      {
        StringBuilder txt = new StringBuilder();
        txt.AppendLine();
        txt.AppendLine("==== Begin Dumping Exception ====");
        txt.AppendLine(message);
        txt.Append(DumpObject.SerializeException(ex));
        txt.Append("Service     = ").AppendLine(ex.Service);
        txt.Append("Utilisateur = ").AppendLine(ex.UserLogin);

        if (properties != null)
        {
          txt.Append("Properties : ").AppendLine(DictionnaryToString(properties));
        }

        if (ex.ObjectToDump != null)
        {
          txt.AppendLine();
          txt.AppendLine("== Dumping attached Object ==");
          txt.Append(DumpObject.GetObjectString(ex.ObjectToDump));
          txt.AppendLine("== end attached Object ==");
        }

        txt.AppendLine("==== End Dumping Exception ====");
        TraceError(txt.ToString());
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Error dans les logs.
    /// </summary>
    /// <param name="message">Message d'erreur à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceError(string message, IDictionary<string, string> properties = null)
    {
      try
      {
        string propString = DictionnaryToString(properties);
        Trace.TraceError($"{message}\n{propString}");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Warning dans les logs.
    /// </summary>
    /// <param name="message">Message de warning à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceWarning(string message, IDictionary<string, string> properties = null)
    {
      try
      {
        string propString = DictionnaryToString(properties);
        Trace.TraceWarning($"{message}\n{propString}");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Information dans les logs.
    /// </summary>
    /// <param name="message">Message de warning à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceInformation(string message, IDictionary<string, string> properties = null)
    {
      try
      {
        string propString = DictionnaryToString(properties);
        Trace.TraceInformation($"{message}\n{propString}");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Debug dans les logs.
    /// </summary>
    /// <param name="message">Message de warning à indiquer.</param>
    /// <param name="properties">Ensemble des clés/valeurs qui complète le message.</param>
    public void TraceDebug(string message, IDictionary<string, string> properties = null)
    {
      try
      {
        string propString = DictionnaryToString(properties);
        Debug.Print($"{message}\n{propString}");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }

    /// <summary>
    ///   Ecrit une entrée de type Debug contenant toutes les properties de l'object
    /// </summary>
    /// <param name="message">message décrivant objectToDump</param>
    /// <param name="objectToDump">objet dont les properties seront dumpées en string</param>
    public void TraceObject(string message, object objectToDump)
    {
      try
      {
        Debug.Print("=== Dumping Object ===");

        if (!string.IsNullOrWhiteSpace(message))
        {
          Debug.Print(message);
        }

        if (objectToDump != null)
        {
          Debug.Print(DumpObject.GetObjectString(objectToDump));
        }

        Debug.Print("=== End Dumping Object ===");
      }
      catch
      {
        // ne doit pas générer d'erreur sous peine de bloquer l'ensemble de la gestion des erreurs.
      }
    }
  }
}
