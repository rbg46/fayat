using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming


namespace Fred.Framework.Services.Google
{
  /// <summary>
  ///   La classe AddressComponent
  /// </summary>
  public class AddressComponent
  {
    /// <summary>
    ///   Obtient ou définit Long_name
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public string Long_name { get; set; }

    /// <summary>
    ///   Obtient ou définit Short_name
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public string Short_name { get; set; }

    /// <summary>
    ///   Obtient ou définit le Type
    /// </summary>
    public List<string> Types { get; set; }
   
  }
}
