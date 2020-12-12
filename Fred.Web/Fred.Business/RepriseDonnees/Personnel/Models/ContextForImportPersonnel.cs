using System.Collections.Generic;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Business.RepriseDonnees.Personnel.Models
{
    /// <summary>
    /// Contient les données necessaire pour faire l'import des Personnels
    /// </summary>
    public class ContextForImportPersonnel
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
        /// Les Personnels utilisés pour l'import des Personnels
        /// </summary>
        public List<PersonnelEnt> PersonnelsUsedInExcel { get; set; } = new List<PersonnelEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Pays utilisés pour l'import des Personnels
        /// </summary>
        public List<PaysEnt> PaysUsedInExcel { get; set; } = new List<PaysEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Ressources utilisées pour l'import des Personnels
        /// </summary>
        public List<RessourceEnt> RessourcesUsedInExcel { get; set; } = new List<RessourceEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Societes utilisées pour l'import des Personnels
        /// </summary>
        public List<SocieteEnt> SocietesUsedInExcel { get; set; } = new List<SocieteEnt>(); // init evite les erreur NullReferenceException

        /// <summary>
        /// Les Email utilisés pour l'import des Personnels
        /// </summary>
        public List<string> MailUsedInExcel { get; set; } = new List<string>(); // init evite les erreur NullReferenceException
    }
}
