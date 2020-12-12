using System.Diagnostics;

namespace Fred.Framework.Tool
{
  /// <summary>
  /// Représente une coordonnée géographique.
  /// </summary>
  [DebuggerDisplay("{Latitude}, {Longitude}")]
  public class GeographicCoordinate
  {
    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="latitude">La latitude.</param>
    /// <param name="longitude">La longitude.</param>
    public GeographicCoordinate(double latitude, double longitude)
    {
      Latitude = latitude;
      Longitude = longitude;
    }

    /// <summary>
    /// La latitude.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// La longitude.
    /// </summary>
    public double Longitude { get; private set; }
  }
}
