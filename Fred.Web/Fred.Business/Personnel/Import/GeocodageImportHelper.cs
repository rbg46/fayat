using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Framework.Services.Google;
using System.Linq;

namespace Fred.Business.Personnel.Import
{
  /// <summary>
  /// Helper pour le geocodage lors d'un import de personnel
  /// </summary>
  public static class GeocodageImportHelper
  {

    /// <summary>
    /// Manage le geocodage, si le quota est atteint alors on ne fait plus de geocodage
    /// </summary>
    /// <param name="param">param</param>
    /// <param name="persoInterneFred">persoInterneFred</param>
    /// <param name="geocodeService">geocodeService</param>
    public static void ManageGeocodageAdress(GoogleApiParam param, PersonnelEnt persoInterneFred, IGeocodeService geocodeService)
    {
      if (param.IndexCourant <= param.Quota && CanGeocodeAddress(persoInterneFred))
      {
        ComputeGPSCoordinates(persoInterneFred, geocodeService);
        param.IndexCourant++;
      }
    }

    /// <summary>
    /// Détermine si l'adresse d'un personnel peut-être géolocalisée
    /// </summary>
    /// <param name="personel">le personnel dont on va tester l'adresse</param>
    /// <returns>true si l'adresse peut potentiellement être géolocalisée, autrement false</returns>
    private static bool CanGeocodeAddress(PersonnelEnt personel)
    {
      if (personel == null)
      {
        return false;
      }

      // Pour être géocodable, le personnel doit avoir au moins un code postal et une ville
      if (string.IsNullOrEmpty(personel.CodePostal))
      {
        return false;
      }

      if (string.IsNullOrEmpty(personel.Ville))
      {
        return false;
      }

      return true;
    }

    /// <summary>
    ///   Récupération des coordonnées GPS à partir de l'Adresse du personnel
    /// </summary>
    /// <param name="perso">personnel</param>
    /// <param name="geocodeService">geocodeService</param>
    private static  void ComputeGPSCoordinates(PersonnelEnt perso, IGeocodeService geocodeService)
    {
      var adresse = new Address()
      {
        Adresse1 = perso.Adresse1,
        Adresse2 = perso.Adresse2,
        Adresse3 = perso.Adresse3,
        CodePostal = perso.CodePostal,
        Ville = perso.Ville,
        Pays = perso.Pays != null ? perso.Pays.Libelle : string.Empty
      };
      var georesponse = geocodeService.Geocode(adresse);

      // Potentiellement Google retourne une liste de résultats
      // Le premier est pris en considération, c'est sensé être le plus approprié
      if (georesponse != null && georesponse.Results.Any())
      {
        var firstItem = georesponse.Results.FirstOrDefault();
        perso.LongitudeDomicile = firstItem.Geometry.Location.Lng;
        perso.LatitudeDomicile = firstItem.Geometry.Location.Lat;
      }
    }

  }
}
