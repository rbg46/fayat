using Fred.Framework.Exceptions;
using Fred.Framework.Tool;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Fred.Framework.Services.Google
{
  /// <summary>
  /// Helper pour le geocodage
  /// </summary>
  public static class GeocodeHelper
  {
    /// <summary>
    /// Formate l'url
    /// </summary>
    /// <param name="adresse">adresse</param>
    /// <returns>l'url du service google a appelé</returns>
    public static string FormatGeocodeUrl(string adresse)
    {
      ToolManager tool = new ToolManager();
      string uri = tool.GetConfig("Google:Map:Uri:Decode");
      string key = tool.GetConfig("Google:Map:Key");

      CheckParameters(uri, key);

      //Consolidation URI
      uri = string.Format(uri, adresse, key);

      return Uri.EscapeUriString(uri);
    }

    /// <summary>
    /// Formate l'url pour l'inverse du geocodage( lat - long => adresse)
    /// </summary>
    /// <param name="location">location</param>
    /// <returns>l'url du service google a appelé</returns>
    public static string FormatInverseGeocodeUrl(Location location)
    {
      ToolManager tool = new ToolManager();
      string uri = tool.GetConfig("Google:Map:Uri:InverseGeocode");
      string key = tool.GetConfig("Google:Map:Key");

      CheckParameters(uri, key);

      //Consolidation URI
      uri = string.Format(uri, location.Lat.ToString().Replace(",","."), location.Lng.ToString().Replace(",", "."), key);

      return Uri.EscapeUriString(uri);
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
    /// Determine si le resultatat du service ggole est ok
    /// </summary>
    /// <param name="geoResponse">geoResponse</param>
    /// <returns>true si le resultat est ok</returns>
    public static bool GeocodeStatusIsOk(GeocodeResult geoResponse)
    {
      if (geoResponse.Status == "OK")
      {
        return true;
      }
      return false;
    }


    /// <summary>
    /// Appelle le service ggogle
    /// </summary>
    /// <param name="url">url a appeler</param>
    /// <returns>GeocodeResult</returns>
    public static GeocodeResult CallGoogleGeocodeApi(string url)
    {
      GeocodeResult result = null;
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = "GET";
      WebResponse response = request.GetResponse();
      {
        Stream stream = response.GetResponseStream();
        if (stream != null)
        {
          using (StreamReader streamReader = new StreamReader(stream))
          {
            string str = streamReader.ReadToEnd();

            result = JsonConvert.DeserializeObject<GeocodeResult>(str);
          }
        }
      }
      if (result == null)
      {
        throw new FredTechnicalException("L'adresse n'a pas pu être géolocalisée.");
      }
      return result;
    }

    /// <summary>
    /// Format la Location (7 chiffres apres la virgule)
    /// </summary>
    /// <param name="oldLocation">oldLocation</param>
    /// <returns>Location</returns>
    public static Location FormatLocation(Location oldLocation)
    {
      var newLoc = new Location();
      newLoc.Lat = Math.Round(oldLocation.Lat, 7);
      newLoc.Lng = Math.Round(oldLocation.Lng, 7);
      return newLoc;
    }


  }
}
