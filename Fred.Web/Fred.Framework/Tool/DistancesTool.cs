using Fred.Framework.Extensions;
using System;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   Classe utilitaire permettant le calcul des distances
  /// </summary>
  public static class DistancesTool
  {
    /// <summary>
    ///   Calcul de la distance orthrodomique entre 2 coordonnées GPS
    /// </summary>
    /// <param name="latitudeDepart">Latitude du premier point GPS</param>
    /// <param name="longitudeDepart">Longitude du premier point GPS</param>
    /// <param name="latitudeArrive">Latitude du second point GPS</param>
    /// <param name="longitudeArrive">Longitude du second point GPS</param>
    /// <returns>La distance calculée</returns>
    public static double GetOrthodromieInKm(double latitudeDepart, double longitudeDepart, double latitudeArrive, double longitudeArrive)
    {
      double resu;
      double lat1InRad;
      double long1InRad;
      double lat2InRad;
      double long2InRad;
      double longitude;
      double latitude;

      double a;
      double c;
      double earthRadius;

      lat1InRad = latitudeDepart * (Math.PI / 180.0);
      long1InRad = longitudeDepart * (Math.PI / 180.0);
      lat2InRad = latitudeArrive * (Math.PI / 180.0);
      long2InRad = longitudeArrive * (Math.PI / 180.0);

      longitude = long2InRad - long1InRad;
      latitude = lat2InRad - lat1InRad;
      /* Intermediate result a. */
      a = Math.Sin(latitude / 2.0).Square() + (Math.Cos(lat1InRad) * Math.Cos(lat2InRad) * Math.Sin(longitude / 2.0).Square());
      /* Intermediate result c (great circle distance in Radians). */
      c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
      /*  kEarthRadius = 3956.0 miles */
      earthRadius = 6376.5; /* kms */

      resu = Math.Round(earthRadius * c, 2);

      return resu;
    }


    /// <summary>
    /// Calcul la distance orthrodomique entre 2 coordonnées géographique.
    /// </summary>
    /// <param name="coordinate1">Coordonnée 1.</param>
    /// <param name="coordinate2">Coordonnée 2.</param>
    /// <returns>La distance calculée</returns>
    /// <remarks>Si les coordonnées sont identiques ou si au moins une des 2 est null alors la fonction retournera 0.</remarks>
    public static double GetOrthodromieInKm(GeographicCoordinate coordinate1, GeographicCoordinate coordinate2)
    {
      if (coordinate1 == null || coordinate2 == null)
      {
        return 0;
      }

      return GetOrthodromieInKm(coordinate1.Latitude, coordinate1.Longitude, coordinate2.Latitude, coordinate2.Longitude);
    }
  }
}
