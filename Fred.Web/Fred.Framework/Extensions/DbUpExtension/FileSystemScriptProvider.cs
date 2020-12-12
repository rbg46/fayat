// Etend les fonctionnalités de DbUp, ces fonctionnalités étendues sont aussi utilisées sur la pic.
// Si vous effectuez des modifications, il faut aussi modifier le package DbUp sur la PIC

using DbUp.Engine;
using DbUp.Engine.Transactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fred.Framework.Extensions.DbUpExtension
{
  /// <summary>
  /// Alternate <see cref="IScriptProvider"/> implementation which retrieves upgrade scripts via a directory
  /// This is a FileSystemScriptProvider inspired of that is implemented in  DbUp 4.0, with added ordering feature.
  /// Permet d'ajouter des fonctionnalités de tri mais aussi d'afficher le nom complet du fichier dans la base DbUp
  /// Credit : https://github.com/DbUp/DbUp/blob/release/4.0.0/src/dbup-core/ScriptProviders/FileSystemScriptProvider.cs
  /// Credit : https://github.com/johanclasson/vso-agent-tasks/blob/master/DbUpMigration/task/Update-DatabaseWithDbUp.ps1
  /// </summary>
  public class FileSystemScriptProvider : IScriptProvider
  {
    private readonly string directoryPath;
    private readonly Func<string, bool> filter;
    private readonly Encoding encoding;
    private readonly FileSystemScriptOptions options;




    /// <summary>
    /// Create a new instance of FileSystemScriptProvider
    /// </summary>
    /// <param name="directoryPath">Path to SQL upgrade scripts</param>
    public FileSystemScriptProvider(string directoryPath)
      : this(directoryPath, new FileSystemScriptOptions())
    {
    }


    /// <summary>
    /// Create a new instance of FileSystemScriptProvider
    /// </summary>
    /// <param name="directoryPath">Path to SQL upgrade scripts</param>
    /// <param name="options">Fred custom option pour filtrer les fichiers.</param>
    public FileSystemScriptProvider(string directoryPath, FileSystemScriptOptions options)
    {
      if (options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }
      this.directoryPath = directoryPath.Replace("/", "\\").EndsWith("\\") ? directoryPath.Substring(0, directoryPath.Length - 1) : directoryPath;
      this.filter = options.Filter;
      this.encoding = options.Encoding;
      this.options = options;
    }





    /// <summary>
    /// Gets all scripts that should be executed.
    /// Options are applied
    /// </summary>
    /// <param name="connectionManager">DbUp connexion manager</param>
    /// <returns>Liste de fichier sql</returns>
    public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
    {
      var files = Directory.GetFiles(this.directoryPath, "*.sql", ShouldSearchSubDirectories()).AsEnumerable();
      if (this.filter != null)
      {
        files = files.Where(this.filter);
      }
      var infos = files.Select(f => new FileInfo(f));
      if (this.options.Order == FileSearchOrder.Filename)
      {
        infos = infos.OrderBy(i => i.Name);
      }
      if (this.options.Order == FileSearchOrder.FilePath)
      {
        infos = infos.OrderBy(i => i.FullName);
      }
      return infos.Select(i => SqlScriptFromFile(i)).ToArray();
    }




    /// <summary>
    /// Dbup retourne normalement le nom du fichier uniquement
    /// Ici on rajoute le chemin à partir duquel on fait la recherche
    /// Permet d'écrire le nom complet du fichier dans la base DbUp
    /// </summary>
    /// <param name="file">information sur la fichier</param>
    /// <returns>Fichier de type sql contenant le script</returns>
    private SqlScript SqlScriptFromFile(FileInfo file)
    {
      using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
      {
        var fileName = file.FullName.Substring(this.directoryPath.Length + 1);
        return SqlScript.FromStream(fileName, fileStream, this.encoding);
      }
    }





    /// <summary>
    /// Converti FileSystemScriptOptions.IncludeSubDirectories en System.Io.SearchOption
    /// </summary>
    /// <returns>System.Io.SearchOption</returns>
    private SearchOption ShouldSearchSubDirectories()
    {
      return this.options.IncludeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    }
  }

}