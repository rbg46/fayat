using System;
using System.Linq.Expressions;
using System.Text;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une affectation d'un moyen
    /// </summary>
    public class SearchAffectationMoyenEnt : SearchBaseMoyenEnt
    {
        /// <summary>
        /// Booléan indique si l'affectation est active
        /// </summary>
        public bool? IsActive { get; set; }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Permet de récupérer le prédicat de recherche d'une affectation moyen.
        /// </summary>
        /// <returns>Retourne la condition de recherche de l'affectation moyen</returns>
        public Expression<Func<AffectationMoyenEnt, bool>> GetPredicateWhere()
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            return a => (!CiId.HasValue || a.CiId == CiId)
                        && (!PersonnelId.HasValue || a.PersonnelId == PersonnelId)
                        && (string.IsNullOrEmpty(NumParc) || (a.Materiel.Code != null && a.Materiel.Code.Equals(NumParc)))
                        && (string.IsNullOrEmpty(NumImmatriculation) || (
                           !a.MaterielLocationId.HasValue ?
                           a.Materiel.Immatriculation != null && a.Materiel.Immatriculation.Equals(NumImmatriculation)
                           : a.MaterielLocation.Immatriculation != null && a.MaterielLocation.Immatriculation == NumImmatriculation)
                           )
                        && (!IsDateFinPredictedOutdated.HasValue
                                        || IsDateFinPredictedOutdated.Value == false
                                        || (IsDateFinPredictedOutdated.Value == true && a.DateFin < DateTime.Today && a.IsActive))
                        && (!IsToBringBack.HasValue || IsToBringBack.Value == false || (IsToBringBack.Value == true && a.SiteId != null && a.SiteId != a.Materiel.SiteAppartenanceId && a.IsActive))
                        && (!IsActive.HasValue || !IsActive.Value || (IsActive.Value && a.IsActive))
                        && (string.IsNullOrEmpty(Societe) || (a.Materiel.Societe != null && a.Materiel.Societe.Code != null && a.Materiel.Societe.Code.Equals(Societe)))
                        && (!EtablissementComptableId.HasValue || (a.Materiel.EtablissementComptableId == EtablissementComptableId.Value))
                        && (string.IsNullOrEmpty(ModelMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.Code != null && a.Materiel.Ressource.Code.Equals(ModelMoyen)))
                        && (string.IsNullOrEmpty(SousTypeMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.SousChapitre != null && a.Materiel.Ressource.SousChapitre.Code != null && a.Materiel.Ressource.SousChapitre.Code.Equals(SousTypeMoyen)))
                        && (string.IsNullOrEmpty(TypeMoyen) || (a.Materiel.Ressource != null && a.Materiel.Ressource.SousChapitre != null && a.Materiel.Ressource.SousChapitre.Chapitre != null && a.Materiel.Ressource.SousChapitre.Chapitre.Code != null && a.Materiel.Ressource.SousChapitre.Chapitre.Code.Equals(TypeMoyen)))
                        && (!SiteActuelId.HasValue || a.SiteId == SiteActuelId)
                        && (!AffectationMoyenTypeId.HasValue || a.AffectationMoyenTypeId == AffectationMoyenTypeId)
                        && (!a.Materiel.IsImported || !a.Materiel.IsLocation || a.MaterielLocationId.HasValue)
                        // test du chevauchement des dates (a.start <= b.end && b.start <= a.end) où a est le référentiel et b le filtre
                        && ((!DateFrom.HasValue && !DateTo.HasValue) // cas où pas de dates renseignées dans le filtre
                        || (DateFrom.HasValue && !DateTo.HasValue && a.DateDebut <= DateFrom.Value && (!a.DateFin.HasValue || DateFrom.Value <= a.DateFin)) // cas où DateTo équivaut à DateFrom renseigné
                        || (!DateFrom.HasValue && DateTo.HasValue && a.DateDebut <= DateTo.Value && (!a.DateFin.HasValue || DateTo.Value <= a.DateFin)) // cas où DateFrom équivaut à DateTo renseigné
                        || (DateFrom.HasValue && DateTo.HasValue && a.DateDebut <= DateTo.Value && (!a.DateFin.HasValue || DateFrom.Value <= a.DateFin))); // cas où 2 dates renseignées dans le filtre
        }


    }
}
