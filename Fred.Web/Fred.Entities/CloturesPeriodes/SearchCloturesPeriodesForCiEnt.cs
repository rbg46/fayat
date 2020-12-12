using Fred.Entities.Organisation;
using System;
using System.Linq.Expressions;

namespace Fred.Entities.CloturesPeriodes
{
    /// <summary>
    /// Représente les critéres de recherche des periodes de clôture pour un centre d'imputation
    /// </summary>
    public class SearchCloturesPeriodesForCiEnt
    {
        /// <summary>
        /// Fait
        /// </summary>
        public const string Fait = "{\"value\":\"Fait\",\"libelle\":\"Fait\"}";

        /// <summary>
        /// NonFait
        /// </summary>
        public const string NonFait = "{\"value\":\"Non fait\",\"libelle\":\"Non fait\"}";

        /// <summary>
        /// Tous
        /// </summary>
        public const string Tous = "{\"value\":\" - Tous\",\"libelle\":\" - Tous\"}";
        /// <summary>
        /// Année de la période
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Mois de la période
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Identifiant de l'organisation si présent
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        /// Type de l'organisation si présent
        /// </summary>
        public int? TypeOrganisationId { get; set; }

        /// <summary>
        /// Text de la recherche du centre d'imputation
        /// </summary>
        public string CentreImputation { get; set; }

        /// <summary>
        /// Obtient ou définit le transfert Far
        /// </summary>
        public string TransfertFar { get; set; }

        /// <summary>
        /// Obtient ou définit les dépenses clôture
        /// </summary>
        public string ClotureDepenses { get; set; }

        /// <summary>
        /// Obtient ou définit la validation avancement
        /// </summary>
        public string ValidationAvancement { get; set; }

        /// <summary>
        /// Obtient ou définit la validation contrôle budgétaire
        /// </summary>
        public string ValidationControleBudgetaire { get; set; }

        /// <summary>
        /// Obtient ou définit la clôture sur la période
        /// </summary>
        public string ClotureSurLaPeriode { get; set; }

        /// <summary>
        /// Obtient ou définit les centres d'imputation déjà terminé
        /// </summary>
        public string DejaTermine { get; set; }

        /// <summary>
        ///   Permet de filtrer sur le centre d'imputation
        /// </summary>
        /// <returns>Retourne la condition de filtre sur le centre d'imputation</returns>
        public Expression<Func<CiDateClotureComptableDto, bool>> FiltreParCentreImputation()
        {
            return f => (string.IsNullOrEmpty(CentreImputation) || (f.Libelle.Contains(CentreImputation)) || (f.Code.Contains(CentreImputation)) || (f.Societe != null && f.Societe.Code.Contains(CentreImputation)));
        }

        /// <summary>
        ///   Permet de filtrer sur le centre d'imputation
        /// </summary>
        /// <returns>Retourne la condition de filtre sur le centre d'imputation</returns>
        public Expression<Func<CiDateClotureComptableNavigableEnt, bool>> FiltreNavigableParCentreImputation()
        {
            return f => (string.IsNullOrEmpty(CentreImputation) || (f.Ci.Libelle.Contains(CentreImputation)) || (f.Ci.Code.Contains(CentreImputation)) || (f.Ci.Societe != null && f.Ci.Societe.Code.Contains(CentreImputation)));
        }

        /// <summary>
        ///   Permet de filtrer sur l'organisation
        /// </summary>
        /// <returns>Retourne la condition de filtre sur l'organisation</returns>
        public Expression<Func<CiDateClotureComptableDto, bool>> FiltreParOrganisation()
        {
            return f => (!OrganisationId.HasValue || (
                            ((TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationUo && f.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationEtablissement && f.PereId.HasValue && f.PereId.Value == OrganisationId.Value) ||
                            (TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationEtablissement && f.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationCi && f.PereId.HasValue && f.PereId == OrganisationId.Value) ||
                            (TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationCi && f.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationCi && f.OrganisationId == OrganisationId.Value))));
        }

        /// <summary>
        ///   Permet de filtrer sur l'organisation
        /// </summary>
        /// <returns>Retourne la condition de filtre sur l'organisation</returns>
        public Expression<Func<CiDateClotureComptableNavigableEnt, bool>> FiltreNavigableParOrganisation()
        {
            return f => (!OrganisationId.HasValue || (
                        ((TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationUo && f.Ci.Organisation.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationEtablissement && f.Ci.Organisation.PereId.HasValue && f.Ci.Organisation.PereId.Value == OrganisationId.Value) ||
                         (TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationEtablissement && f.Ci.Organisation.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationCi && f.Ci.Organisation.PereId.HasValue && f.Ci.Organisation.PereId == OrganisationId.Value) ||
                         (TypeOrganisationId.Value == TypeOrganisationEnt.TypeOrganisationCi && f.Ci.Organisation.TypeOrganisationId == TypeOrganisationEnt.TypeOrganisationCi && f.Ci.Organisation.OrganisationId == OrganisationId.Value))));
        }

        /// <summary>
        ///   Permet de filtrer sur le transfert FAR
        /// </summary>
        /// <returns>Retourne la condition de filtre sur le transfert FAR</returns>
        public Expression<Func<CiDateClotureComptableNavigableEnt, bool>> FiltreNavigableParTransfertFar()
        {
            return f => (string.IsNullOrEmpty(TransfertFar) || TransfertFar == Tous || (TransfertFar == Fait && f.DatesClotureComptable.DateTransfertFAR.HasValue) || (TransfertFar == NonFait && (!f.DatesClotureComptable.DateTransfertFAR.HasValue)));
        }

        /// <summary>
        ///   Permet de filtrer sur le transfert FAR
        /// </summary>
        /// <returns>Retourne la condition de filtre sur le transfert FAR</returns>
        public Expression<Func<CiDateClotureComptableDto, bool>> FiltreParTransfertFar()
        {
            return f => (string.IsNullOrEmpty(TransfertFar) || TransfertFar == Tous || (TransfertFar == Fait && f.DateTransfertFAR.HasValue) || (TransfertFar == NonFait && (!f.DateTransfertFAR.HasValue)));
        }

        /// <summary>
        ///   Permet de filtrer sur la clôture des dépenses
        /// </summary>
        /// <returns>Retourne la condition de filtre sur la clôture des dépenses</returns>
        public Expression<Func<CiDateClotureComptableDto, bool>> FiltreParClotureDepenses()
        {
            return f => (string.IsNullOrEmpty(ClotureDepenses) || ClotureDepenses == Tous || (ClotureDepenses == Fait && f.DateCloture.HasValue) || (ClotureDepenses == NonFait && (!f.DateCloture.HasValue)));
        }


        /// <summary>
        ///   Permet de filtrer sur la clôture des dépenses
        /// </summary>
        /// <returns>Retourne la condition de filtre sur la clôture des dépenses</returns>
        public Expression<Func<CiDateClotureComptableNavigableEnt, bool>> FiltreNavigableParClotureDepenses()
        {
            return f => (string.IsNullOrEmpty(ClotureDepenses) || ClotureDepenses == Tous || (ClotureDepenses == Fait && f.DatesClotureComptable.DateCloture.HasValue) || (ClotureDepenses == NonFait && (!f.DatesClotureComptable.DateCloture.HasValue)));
        }
    }
}
