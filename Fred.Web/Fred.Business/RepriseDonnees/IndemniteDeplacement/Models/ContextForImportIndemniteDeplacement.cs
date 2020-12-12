using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Models
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des Indemnités de Déplacement
    /// </summary>
    public class ContextForImportIndemniteDeplacement
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
        /// Les Societes utilisées pour l'import des Indemnité de Déplacement
        /// </summary>
        public List<SocieteEnt> SocietesUsedInExcel { get; set; } = new List<SocieteEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Codes Déplacement utilisés pour l'import des Indemnité de Déplacement
        /// </summary>
        public List<CodeDeplacementEnt> CodesDeplacementUsedInExcel { get; set; } = new List<CodeDeplacementEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Codes Zone Déplacement utilisés pour l'import des Indemnité de Déplacement
        /// </summary>
        public List<CodeZoneDeplacementEnt> CodesZoneDeplacementUsedInExcel { get; set; } = new List<CodeZoneDeplacementEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Personnels utilisés pour l'import des Indemnité de Déplacement
        /// </summary>
        public List<PersonnelEnt> PersonnelsUsedInExcel { get; set; } = new List<PersonnelEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les CI utilités pour l'import des Indemnité de Déplacement
        /// </summary>
        public List<CIEnt> CIsUsedInExcel { get; set; } = new List<CIEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Indemnité de déplacements utilisées dans l'import
        /// </summary>
        public List<IndemniteDeplacementEnt> IndemniteDeplacementUsedInExcel { get; set; } = new List<IndemniteDeplacementEnt>(); // init evite les erreur NullReferenceException
    }
}
