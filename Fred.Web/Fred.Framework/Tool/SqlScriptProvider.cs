using Fred.Framework.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   Classe permettant de récupérer le contenu d'un fichier SQL intégré à un projet (Properties Build action du fichier : Embedded Resource)
  /// </summary>
  public static class SqlScriptProvider
  {
    /// <summary>
    ///   Récupère le contenu d'un fichier SQL intégré à un projet
    ///   Code reference : https://github.com/DbUp/DbUp/tree/master/src/DbUp/ScriptProviders
    /// </summary>
    /// <param name="assembly">Assembly contenant les fichiers</param>
    /// <param name="path">Chemin vers le fichier</param>
    /// <returns>Contenu du fichier</returns>
    /// <exception cref="FredTechnicalException">Erreur de lecture du fichier SQL</exception>
    public static string GetEmbeddedSqlScriptContent(Assembly assembly, string path)
    {
      string sqlContent = null;
      try
      {
        var sqlFiles = assembly.GetManifestResourceNames().Where(x => x.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase)).ToList();
        string filePath = sqlFiles.FirstOrDefault(x => x.EndsWith(path));
        Stream stream = assembly.GetManifestResourceStream(filePath);

        using (var resourceStreamReader = new StreamReader(stream))
        {
          sqlContent = resourceStreamReader.ReadToEnd();
        }
      }
      catch (Exception e)
      {
        throw new FredTechnicalException(e.Message, e);
      }

      return sqlContent;
    }
  }
}
