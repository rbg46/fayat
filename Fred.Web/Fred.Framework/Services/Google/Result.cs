using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming


namespace Fred.Framework.Services.Google
{

  /// <summary>
  /// Classe Result du geocodage
  /// </summary>
  public class Result
  {
    /// <summary>
    ///   Obtient ou définit la liste address_components
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public List<AddressComponent> Address_components { get; set; }

    /// <summary>
    ///   Obtient ou définit l'address
    /// </summary>
    public Address Adresse { get; set; }

    /// <summary>
    ///   Obtient ou définit le Formatted_address
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public string Formatted_address { get; set; }

    /// <summary>
    ///   Obtient ou définit Geometry
    /// </summary>
    public Geometry Geometry { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si Partial_match
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public bool Partial_match { get; set; }

    /// <summary>
    ///   Obtient ou définit la Place_id
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public string Place_id { get; set; }

    /// <summary>
    ///   Obtient ou définit les Types
    /// </summary>
    public List<string> Types { get; set; }
  }
}
