using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Fred.Entities.Rapport;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une ligne de rapport d'un moyen
    /// </summary>
    public class SearchRapportLigneMoyenEnt : SearchBaseMoyenEnt
    {
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date minimale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMin { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la date maximale pour laquelle récupérer les pointages
        /// </summary>
        public DateTime DatePointageMax { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'ordre de tri sur les dates
        /// </summary>
        public bool? DatePointageAsc { get; set; }


#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une affectation moyen.
        /// </summary>
        /// <returns>Retourne la condition de recherche de l'affectation moyen</returns>
        public Expression<Func<RapportLigneEnt, bool>> GetPredicateWhere()
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            return a => (a.AffectationMoyen != null)
                        && (!a.DateSuppression.HasValue)
                        && (!CiId.HasValue || a.CiId == CiId)
                        && (!PersonnelListAffectationMoyenIds.Any() || PersonnelListAffectationMoyenIds.Any(x => x == a.AffectationMoyenId.Value))
                        && (!a.PersonnelId.HasValue || !PersonnelId.HasValue || a.PersonnelId.HasValue && PersonnelId.HasValue && a.PersonnelId.Value == PersonnelId.Value)
                        && (string.IsNullOrEmpty(NumParc) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Code != null && a.AffectationMoyen.Materiel.Code.Equals(NumParc)))
                        && (string.IsNullOrEmpty(NumImmatriculation) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Immatriculation != null && a.AffectationMoyen.Materiel.Immatriculation.Equals(NumImmatriculation)))
                        && (!IsToBringBack.HasValue || IsToBringBack.Value == false || (IsToBringBack.Value == true && a.AffectationMoyen.SiteId != null && a.AffectationMoyen.SiteId != a.AffectationMoyen.Materiel.SiteAppartenanceId && a.AffectationMoyen.IsActive))
                        && (string.IsNullOrEmpty(Societe) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Societe != null && a.AffectationMoyen.Materiel.Societe.Code != null && a.AffectationMoyen.Materiel.Societe.Code.Equals(Societe)))
                        && (!EtablissementComptableId.HasValue || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.EtablissementComptableId == EtablissementComptableId))
                        && (string.IsNullOrEmpty(ModelMoyen) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Ressource != null && a.AffectationMoyen.Materiel.Ressource.Code != null && a.AffectationMoyen.Materiel.Ressource.Code.Equals(ModelMoyen)))
                        && (string.IsNullOrEmpty(SousTypeMoyen) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Ressource != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre.Code != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                        && (string.IsNullOrEmpty(TypeMoyen) || (a.AffectationMoyen.Materiel != null && a.AffectationMoyen.Materiel.Ressource != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre.Chapitre != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre.Chapitre.Code != null && a.AffectationMoyen.Materiel.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                        && (!SiteActuelId.HasValue || (a.AffectationMoyen.SiteId == SiteActuelId))
                        && (!AffectationMoyenTypeId.HasValue || (a.AffectationMoyen.AffectationMoyenTypeId == AffectationMoyenTypeId))
                        // test intervalle de pointage dans l'intervalle de filtre - test du chevauchement des dates (a.start <= b.end && b.start <= a.end) où a est le référentiel et b le filtre
                        && ((!DateFrom.HasValue && !DateTo.HasValue) // cas où pas de dates renseignées dans le filtre
                        || (DateFrom.HasValue && !DateTo.HasValue && a.DatePointage >= DateFrom.Value) // cas où pas de DateTo renseignée dans le filtre
                        || (!DateFrom.HasValue && DateTo.HasValue && a.DatePointage <= DateToEndOfDay) // cas où pas de DateFrom renseignée dans le filtre
                        || (DateFrom.HasValue && DateTo.HasValue && a.DatePointage <= DateToEndOfDay) && DateFrom.Value <= a.DatePointage); // cas où 2 dates renseignées dans le filtre
        }

        /// <summary>
        /// Tri par date de pointage
        /// </summary>
        /// <returns>Chaine de tri</returns>
        public string GetOrderBy()
        {
            var filter = new StringBuilder();

            if (DatePointageAsc.HasValue && DatePointageAsc.Value)
            {
                filter.Append("DatePointage ascending");
            }
            if (DatePointageAsc.HasValue && !DatePointageAsc.Value)
            {
                filter.Append("DatePointage descending");
            }
            if (!DatePointageAsc.HasValue)
            {
                filter.Append("DatePointage");
            }
            return filter.ToString();
        }
    }
}
