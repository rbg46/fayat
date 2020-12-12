using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;

namespace Fred.Business.RepriseDonnees.Rapport.ContextProviders
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des ci
    /// </summary>
    public class ContextForImportRapport
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
        /// Societes du groupe selectionné  dans l'interface fred ie
        /// </summary>
        public List<OrganisationBase> SocietesOfGroupe { get; set; }
        /// <summary>
        /// l'arbre des Organisation de fayat
        /// </summary>
        public OrganisationTree OrganisationTree { get; set; }
        /// <summary>
        /// Les pays utilisé pour l'import des cis
        /// </summary>
        public List<CodeDeplacementEnt> CodeDeplacementsUsedInExcel { get; set; }

        /// <summary>
        /// Les pays utilisé pour l'import des cis
        /// </summary>
        public List<CodeZoneDeplacementEnt> CodeZoneDeplacementsUsedInExcel { get; set; }

        /// <summary>
        /// Les pays utilisé pour l'import des cis
        /// </summary>
        public List<GetT3ByCodesOrDefaultResponse> TachesUsedInExcel { get; set; } = new List<GetT3ByCodesOrDefaultResponse>();
        /// <summary>
        /// les ci utiliser pour l'import des cis
        /// </summary>
        public List<CIEnt> CisUsedInExcel { get; set; }
        /// <summary>
        /// les personnels utilisé pour l'import des cis
        /// </summary>
        public List<PersonnelEnt> PersonnelsUsedInExcel { get; set; }
    }
}
