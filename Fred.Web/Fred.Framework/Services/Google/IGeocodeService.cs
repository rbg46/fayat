using Fred.Framework.Tool;

namespace Fred.Framework.Services.Google
{
    /// <summary>
    ///   Interface IGeocodeService
    /// </summary>
    public interface IGeocodeService
    {

        /// <summary>
        /// Permet de geocoder une adresse
        /// </summary>
        /// <param name="adresse">adresse a géocoder</param>
        /// <returns>GeocodeResult</returns>
        GeocodeResult Geocode(Address adresse);


        /// <summary>
        /// Donne une adresse en fonction d'une location(lat et lng)
        /// </summary>
        /// <param name="location">location</param>
        /// <returns>GeocodeResult</returns>
        GeocodeResult InverseGeocode(Location location);

        /// <summary>
        /// Retourne le nombre de kilometre routier entre 2 coordonnées géographique.
        /// </summary>
        /// <param name="origine">La coordonnée d'origine.</param>
        /// <param name="destination">La coordonnée de destination.</param>
        /// <returns>Le nombre de kilometre routier entre les 2 coordonnées ou 0 si au moins une des coordonnées est null.</returns>
        double GetDrivingDistanceInKm(GeographicCoordinate origine, GeographicCoordinate destination);
    }
}
