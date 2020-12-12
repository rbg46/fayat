using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Tool;

namespace Fred.Framework.Services.Google
{
    /// <summary>
    ///   La classe Map
    /// </summary>
    public class GeocodeService : IGeocodeService
    {
        /// <summary>
        ///   Retourne le nombre de kilometre routier entre 2 points GPS
        /// </summary>
        /// <param name="latOrigine">Latitude du point d'origine</param>
        /// <param name="lngOrigine">Longitude du point d'origine</param>
        /// <param name="latDestination">Latitude du point de destination</param>
        /// <param name="lngDestination">Longitude du point de destination</param>
        /// <returns>Retourne une distance en kilometre entre 2 points</returns>
        public static string GetDrivingDistanceInKm(string latOrigine, string lngOrigine, string latDestination, string lngDestination)
        {
            double latO, lngO, latD, lngD;
            bool parseLatO = double.TryParse(latOrigine, out latO);
            bool parseLngO = double.TryParse(lngOrigine, out lngO);
            bool parseLatD = double.TryParse(latDestination, out latD);
            bool parseLngD = double.TryParse(lngDestination, out lngD);

            if (parseLatO && parseLngO && parseLatD && parseLngD)
            {
                return GetDrivingDistanceInKm(latO, lngO, latD, lngD).ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }
        private static void CheckParameters(string uri, string key)
        {
            // Controle URI
            if (uri == null)
            {
                throw new FredTechnicalException("L'URI Google Map n'est pas renseigné dans le fichier Web.config de la solution. Merci de contacter votre administrateur technique Fayat pour corriger ce point.");
            }

            // Controle Clé d'authentification
            if (key == null)
            {
                throw new FredTechnicalException("La clé d'authentification Google Map n'est pas renseignée dans le fichier Web.config de la solution. Merci de contacter votre administrateur technique Fayat pour corriger ce point.");
            }
        }

        /// <summary>
        ///   Retourne le nombre de kilometre routier entre 2 points GPS
        /// </summary>
        /// <param name="latOrigine">Latitude du point d'origine</param>
        /// <param name="lngOrigine">Longitude du point d'origine</param>
        /// <param name="latDestination">Latitude du point de destination</param>
        /// <param name="lngDestination">Longitude du point de destination</param>
        /// <returns>Retourne une distance en kilometre entre 2 points</returns>
        public static double GetDrivingDistanceInKm(double latOrigine, double lngOrigine, double latDestination, double lngDestination)
        {
            double resultat = double.MaxValue;
            ToolManager tool = new ToolManager();
            string url = tool.GetConfig("Google:map:uri:ui:DistanceRT");
            string key = tool.GetConfig("Google:Map:Key");

            CheckParameters(url, key);

            ////string gpsFormat = tool.GetConfig("Google:map:GPSFormat");
            url = string.Format(url, latOrigine.ToEnglishString(), lngOrigine.ToEnglishString(), latDestination.ToEnglishString(), lngDestination.ToEnglishString(), '&', key);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            if (dataStream != null)
            {
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);
                string status = xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText;
                if (status == "OK")
                {
                    XmlNodeList distances = xmldoc.GetElementsByTagName("distance");
                    foreach (XmlNode distance in distances)
                    {
                        if (distance.ChildNodes[0] != null)
                        {
                            resultat = Math.Min(resultat, Math.Round(Convert.ToDouble(distance.ChildNodes[0].InnerText) / 1000, 2));
                        }
                    }
                }
                else
                {
                    throw new FredTechnicalException($"Google API returns error : {status}");
                }
            }
            return resultat.Equals(double.MaxValue) ? 0 : resultat;
        }

        /// <summary>
        /// Retourne le nombre de kilometre routier entre 2 coordonnées géographique.
        /// </summary>
        /// <param name="origine">La coordonnée d'origine.</param>
        /// <param name="destination">La coordonnée de destination.</param>
        /// <returns>Le nombre de kilometre routier entre les 2 coordonnées ou 0 si au moins une des coordonnées est null.</returns>
        public double GetDrivingDistanceInKm(GeographicCoordinate origine, GeographicCoordinate destination)
        {
            if (origine == null || destination == null)
            {
                return 0;
            }

            return GetDrivingDistanceInKm(origine.Latitude, origine.Longitude, destination.Latitude, destination.Longitude);
        }

        /// <summary>
        /// Geocode une adresse
        /// </summary>
        /// <param name="adresse">adresse</param>
        /// <returns>Entry</returns>
        public GeocodeResult Geocode(Address adresse)
        {

            GeocodeResult geoResponse = null;

            try
            {
                var stringAddress = AddressHelper.ConvertAddressToString(adresse);

                var url = GeocodeHelper.FormatGeocodeUrl(stringAddress);

                geoResponse = GeocodeHelper.CallGoogleGeocodeApi(url);

                //Deuxieme tentative sans les champs 'adresse'
                if (!GeocodeHelper.GeocodeStatusIsOk(geoResponse))
                {
                    stringAddress = AddressHelper.ConvertAddressToString(adresse, withoutAddresses: true);

                    url = GeocodeHelper.FormatGeocodeUrl(stringAddress);

                    geoResponse = GeocodeHelper.CallGoogleGeocodeApi(url);
                }

            }
            catch (FredTechnicalException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException("Erreur lors de la géolocalisation de l'adresse : " + adresse, ex);
            }
            if (geoResponse != null)
            {
                foreach (var item in geoResponse.Results)
                {
                    item.Adresse = AddressHelper.ConvertAddressComponentsToAddress(item.Address_components);
                    item.Geometry.Location = GeocodeHelper.FormatLocation(item.Geometry.Location);
                }
            }
            //Format le resultat

            return geoResponse;
        }

        /// <summary>
        /// Donne une adresse en fonction d'une location(lat et lng)
        /// </summary>
        /// <param name="location">location</param>
        /// <returns>GeocodeResult</returns>
        public GeocodeResult InverseGeocode(Location location)
        {

            GeocodeResult geoResponse = null;

            try
            {

                var url = GeocodeHelper.FormatInverseGeocodeUrl(location);

                geoResponse = GeocodeHelper.CallGoogleGeocodeApi(url);

            }
            catch (FredTechnicalException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException("Erreur lors de la géolocalisation des coordonnées ", ex);
            }

            if (geoResponse != null)
            {
                //Format le resultat
                foreach (var item in geoResponse.Results)
                {
                    item.Adresse = AddressHelper.ConvertAddressComponentsToAddress(item.Address_components);
                    item.Geometry.Location = GeocodeHelper.FormatLocation(item.Geometry.Location);
                }
            }

            return geoResponse;
        }
    }
}
