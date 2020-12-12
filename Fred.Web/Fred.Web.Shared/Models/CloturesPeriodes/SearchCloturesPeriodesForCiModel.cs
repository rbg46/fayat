using System.Collections.Generic;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Web.Models.CloturesPeriodes
{
    public class SearchCloturesPeriodesForCiModel
    {
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

        public List<object> TransfertFarItems => new List<object>()
            {
                new { Libelle = FeatureCloturesPeriodes.Filtre_Tiret_Tous_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Fait_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Non_Fait_cb }
            };
        public List<object> ClotureDepensesItems => new List<object>()
            {
                new { Libelle = FeatureCloturesPeriodes.Filtre_Tiret_Tous_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Fait_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Non_Fait_cb }
            };
        public List<object> ValidationAvancementItems => new List<object>()
            {
                new { Libelle = FeatureCloturesPeriodes.Filtre_Tiret_Tous_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Fait_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Non_Fait_cb }
            };
        public List<object> ValidationControleBudgetaireItems => new List<object>()
            {
                new { Libelle = FeatureCloturesPeriodes.Filtre_Tiret_Tous_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Fait_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Non_Fait_cb }
            };
        public List<object> ValidationClotureSurLaPeriodeItems => new List<object>()
            {
                new { Libelle = FeatureCloturesPeriodes.Filtre_Tiret_Tous_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Fait_cb },
                new { Libelle = FeatureCloturesPeriodes.Filtre_Non_Fait_cb }
            };
    }
}
