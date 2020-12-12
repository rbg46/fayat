// Etend les fonctionnalités de DbUp, ces fonctionnalités étendues sont aussi utilisées sur la pic.
// Si vous effectuez des modifications, il faut aussi modifier le package DbUp sur la PIC

using System;
using System.Text;

namespace Fred.Framework.Extensions.DbUpExtension
{
  /// <summary>
  /// The order in which scripts are executed. 
  /// If set to Filename, scripts can be organized in different folders while still controlling execution order by their filenames. 
  /// If set to File Path, scripts are strictly sorted on their complete path. 
  /// If set to Folder Structure, scripts are executed in the order of the file system provider, 
  /// which means one folder at the time starting with the files in each folder and then working its way through each subfolder. 
  /// All scripts are executed in ascending order.
  /// </summary>
  public enum FileSearchOrder
  {
    /// <summary>
    ///  Tri les fichiers par leur nom
    /// </summary>
    Filename = 0,
    /// <summary>
    /// Tri les fichiers par le nom complet
    /// </summary>
    FilePath = 1,
    /// <summary>
    /// Tri les fichiers en suivant la structure des répertoires
    /// </summary>
    FolderStructure = 2
  }



  /// <summary>
  /// Contient les options étendues : ajoute une option pour trier les fichiers
  /// </summary>
  public class FileSystemScriptOptions
  {
    /// <summary>
    /// Ctor
    /// </summary>
    public FileSystemScriptOptions()
    {
      Encoding = Encoding.Default;
    }

    /// <summary>
    /// Si vrais, les sous-répertoires sont utilisés
    /// </summary>
    public bool IncludeSubDirectories { get; set; }

    /// <summary>
    /// Indique l'ordre de tri des fichiers 
    /// </summary>
    public FileSearchOrder Order { get; set; }

    /// <summary>
    /// Fournie une fonction custom pour filtrer les fichiers
    /// Utile par exemple, pour filtrer les fichiers à jouer en fonction des environnements : dev/prod etc
    /// </summary>
    public Func<string, bool> Filter { get; set; }

    /// <summary>
    /// Indique l'encodage des fichiers : UTF8 avec Bom.
    /// </summary>
    public Encoding Encoding { get; set; }
  }

}