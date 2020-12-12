using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Business.RepriseDonnees.PlanTaches.Models
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des Plan de taches
    /// </summary>
    public class ContextForImportPlanTaches
    {
        /// <summary>
        /// Le groupeId courrant
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// L'utilisateur fred ie
        /// </summary>
        public UtilisateurEnt FredIeUser { get; set; }

        /// <summary>
        /// l'arbre des Organisation de fayat
        /// </summary>
        public OrganisationTree OrganisationTree { get; set; }

        /// <summary>
        /// les cis utilisées pour l'import des commande et receptions
        /// </summary>
        public List<CIEnt> CisUsedInExcel { get; set; } = new List<CIEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les tâches utilisées pour l'import du plan de tâches
        /// </summary>
        public List<TacheEnt> TachesUsedInExcel { get; set; } = new List<TacheEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les tâches parents utilisées pour l'import du plan de tâches
        /// </summary>
        public List<TacheEnt> TachesParentsUsedInExcel { get; set; } = new List<TacheEnt>(); // init evite les erreur NullReferenceException
    }
}
