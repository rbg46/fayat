using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;

namespace Fred.Business.RepriseDonnees.Commande.Models
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des ci
    /// </summary>
    public class ContextForImportCommande
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
        /// les commandes utilisées pour l'import des commande et receptions
        /// </summary>
        public List<CommandeEnt> CommandesUsedInExcel { get; set; } = new List<CommandeEnt>();// init evite les erreur NullReferenceException

        /// <summary>
        /// les cis utilisées pour l'import des commande et receptions
        /// </summary>
        public List<CIEnt> CisUsedInExcel { get; set; } = new List<CIEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Toutes les types de commandes de Fred(4 ou 5 types maxi)
        /// </summary>
        public List<CommandeTypeEnt> AllCommandesTypes { get; set; } = new List<CommandeTypeEnt>();

        /// <summary>
        /// les fournisseurs utilisées pour l'import des commande et receptions
        /// </summary>
        public List<FournisseurEnt> FournisseurUsedInExcel { get; set; }

        /// <summary>
        /// Le statu commande validée
        /// </summary>
        public StatutCommandeEnt StatutCommandeValidee { get; set; }
        /// <summary>
        /// Retourn les devise utilsé dans le fichier exel
        /// </summary>
        public List<DeviseEnt> DevisesUsedInExcel { get; set; }
        /// <summary>
        /// Retourne le resultats de la recherche des taches
        /// </summary>
        public List<GetT3ByCodesOrDefaultResponse> TachesUsedInExcel { get; set; } = new List<GetT3ByCodesOrDefaultResponse>();

        /// <summary>
        /// Les ressources utilisées dans la fichier excel
        /// </summary>
        public List<RessourceEnt> RessourcesUsedInExcel { get; set; } = new List<RessourceEnt>();

        /// <summary>
        /// Liste d'unités utilisées dans le fichiers excel.
        /// </summary>
        public List<UniteEnt> UnitesUsedInExcel { get; set; }

        /// <summary>
        /// Le type de reception
        /// </summary>
        public DepenseTypeEnt DepenseTypeReception { get; set; }
    }
}
