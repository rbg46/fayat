using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Services.Google;
using Fred.Framework.Tool;

namespace Fred.Business.Referential.IndemniteDeplacement.Features
{
    /// <summary>
    /// Permet de calculer les zones pour les indemnités de déplacement.
    /// </summary>
    public class IndemniteDeplacementCalculator : ManagersAccess
    {
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;

        public IndemniteDeplacementCalculator(ICodeZoneDeplacementManager codeZoneDeplacementManager)
        {
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
        }
        #region Membres

        private readonly Dictionary<int, IEnumerable<CodeZoneDeplacementEnt>> societeZones = new Dictionary<int, IEnumerable<CodeZoneDeplacementEnt>>();

        #endregion
        #region Fonctions publiques

        /// <summary>
        /// Calcul le code zone de déplacement.
        /// </summary>
        /// <param name="societe">La société concernée.</param>
        /// <param name="origine">Coordonnées d'origine.</param>
        /// <param name="destination">Coordonnées de destination.</param>
        /// <returns>La zone concernée ou null si aucune zone ne correspond au nombre de kilomètre.</returns>
        public CodeZoneDeplacementEnt GetZone(SocieteEnt societe, GeographicCoordinate origine, GeographicCoordinate destination)
        {
            if (origine == null || destination == null || societe.IndemniteDeplacementCalculTypeId == null)
            {
                return null;
            }
            var calculType = (IndemniteDeplacementCalculType)societe.IndemniteDeplacementCalculTypeId;
            var kilometre = GetKilometre(calculType, origine, destination);
            return GetZone(societe.SocieteId, kilometre);
        }

        /// <summary>
        /// Retourne les coordonnées géographique d'un CI.
        /// </summary>
        /// <param name="ci">Le CI concerné.</param>
        /// <returns>Les coordonnées géographique du CI.</returns>
        public static GeographicCoordinate GetGeographicCoordinate(CIEnt ci)
        {
            if (ci != null && ci.LatitudeLocalisation.HasValue && ci.LongitudeLocalisation.HasValue)
            {
                return new GeographicCoordinate(ci.LatitudeLocalisation.Value, ci.LongitudeLocalisation.Value);
            }
            return null;
        }

        /// <summary>
        /// Retourne les coordonnées géographique d'un établissement de paie.
        /// </summary>
        /// <param name="etablissementPaie">L'établissement de paie concerné.</param>
        /// <returns>Les coordonnées géographique de létablissement de paie.</returns>
        public static GeographicCoordinate GetGeographicCoordinate(EtablissementPaieEnt etablissementPaie)
        {
            if (etablissementPaie != null && etablissementPaie.Latitude.HasValue && etablissementPaie.Longitude.HasValue)
            {
                return new GeographicCoordinate(etablissementPaie.Latitude.Value, etablissementPaie.Longitude.Value);
            }
            return null;
        }

        /// <summary>
        /// Retourne les coordonnées géographique du domicile d'un personnel.
        /// </summary>
        /// <param name="personnel">Le personnel concerné.</param>
        /// <returns>Les coordonnées géographique du domicile du personnel.</returns>
        public static GeographicCoordinate GetGeographicCoordinate(PersonnelEnt personnel)
        {
            if (personnel != null && personnel.LatitudeDomicile.HasValue && personnel.LongitudeDomicile.HasValue)
            {
                return new GeographicCoordinate(personnel.LatitudeDomicile.Value, personnel.LongitudeDomicile.Value);
            }
            return null;
        }

        /// <summary>
        /// Retourne la coordonnée géographique de l'origine du déplacement d'un personnel en fonction de son type de rattachement.
        /// </summary>
        /// <param name="personnel">Le personnel concerné.</param>
        /// <returns>La coordonnée géographique de l'origine du déplacement du personnel </returns>
        public static GeographicCoordinate GetOrigine(PersonnelEnt personnel)
        {
            switch (personnel.TypeRattachement)
            {
                case TypeRattachement.TypeRattachement.Agence:
                case TypeRattachement.TypeRattachement.Secteur:
                    return GetGeographicCoordinate(personnel.EtablissementRattachement);

                case TypeRattachement.TypeRattachement.Domicile:
                    return GetGeographicCoordinate(personnel);

                default:
                    return null;
            }
        }

        #endregion
        #region Fonctions privées

        /// <summary>
        /// Calcule la distance en kilomètre entre 2 coordonnées géographique.
        /// </summary>
        /// <param name="calculType">Type de calcul.</param>
        /// <param name="origine">Coordonnées d'origine.</param>
        /// <param name="destination">Coordonnées de destination.</param>
        /// <returns>La distance en kilomètre entre les 2 coordonnées géographique.</returns>
        private static double GetKilometre(IndemniteDeplacementCalculType calculType, GeographicCoordinate origine, GeographicCoordinate destination)
        {
            switch (calculType)
            {
                case IndemniteDeplacementCalculType.KilometreVolOiseau:
                    return DistancesTool.GetOrthodromieInKm(origine, destination);

                case IndemniteDeplacementCalculType.KilometreRoutier:
                case IndemniteDeplacementCalculType.KilometreReel:
                    if (origine == null || destination == null)
                    {
                        return 0;
                    }
                    return GeocodeService.GetDrivingDistanceInKm(origine.Latitude, origine.Longitude, destination.Latitude, destination.Longitude);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calcul le code zone de déplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la société concernée.</param>
        /// <param name="kilometre">Nombre de kilomètre effectué.</param>
        /// <returns>La zone concernée ou null si aucune zone ne correspond au nombre de kilomètre.</returns>
        private CodeZoneDeplacementEnt GetZone(int societeId, double kilometre)
        {
            var zones = societeZones.FirstOrDefault(kvp => kvp.Key == societeId).Value;
            if (zones == null)
            {
                zones = codeZoneDeplacementManager.GetCodeZoneDeplacementBySocieteId(societeId, true);
                societeZones.Add(societeId, zones);
            }

            return zones.FirstOrDefault(z => kilometre > z.KmMini && kilometre <= z.KmMaxi);
        }

        #endregion
    }
}
