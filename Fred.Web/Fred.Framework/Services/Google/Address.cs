using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming


namespace Fred.Framework.Services.Google
{


  /// <summary>
  ///   La classe Address
  /// </summary>
  public class Address
  {
    /// <summary>
    ///   Obtient ou définit Adresse1
    /// </summary>
    public string Adresse1 { get; set; }

    /// <summary>
    ///   Obtient ou définit Adresse2
    /// </summary>
    public string Adresse2 { get; set; }

    /// <summary>
    ///   Obtient ou définit Adresse3
    /// </summary>
    public string Adresse3 { get; set; }

    /// <summary>
    ///   Obtient ou définit le code postal
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    ///   Obtient ou définit la ville
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    ///   Obtient ou définit le pays
    /// </summary>
    public string Pays { get; set; }
  }
}
