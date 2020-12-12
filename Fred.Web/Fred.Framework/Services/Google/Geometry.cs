using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming


namespace Fred.Framework.Services.Google
{

  /// <summary>
  ///   La classe Geometry
  /// </summary>
  public class Geometry
  {
    /// <summary>
    ///   Obtient ou définit la location
    /// </summary>
    public Location Location { get; set; }

    /// <summary>
    ///   Obtient ou définit le location_type
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "JSON Relation")]
    public string Location_type { get; set; }

    /// <summary>
    ///   Obtient ou définit leviewport
    /// </summary>
    public Viewport Viewport { get; set; }
  }
}
