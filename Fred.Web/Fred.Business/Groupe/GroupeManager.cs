using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Organisation;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Organisation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Security;

namespace Fred.Business.Groupe
{
    /// <summary>
    ///   Gestionnaire des groupes.
    /// </summary>
    public class GroupeManager : Manager<GroupeEnt, IGroupeRepository>, IGroupeManager
    {
        private readonly ISecurityManager securityManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IOrganisationManager organisationManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="GroupeManager" />.
        /// </summary>
        /// <param name="securityManager">Gestionnaire de sécurité</param>
        /// <param name="uow">Repository des Unit of Work</param>
        /// <param name="groupeRepository">groupeRepository</param>
        /// <param name="utilisateurManager">Le gestionnaire des Utilisateur</param>
        /// <param name="affectationSeuilUtilisateurManager">affectationSeuilUtilisateurManager</param>
        /// <param name="organisationManager">organisationManager</param>      
        public GroupeManager(ISecurityManager securityManager,
            IUnitOfWork uow,
            IGroupeRepository groupeRepository,
            IUtilisateurManager utilisateurManager,
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IOrganisationManager organisationManager)
          : base(uow, groupeRepository)
        {
            this.securityManager = securityManager;
            this.utilisateurManager = utilisateurManager;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.organisationManager = organisationManager;
        }

        /// <summary>
        /// Obtient l'identifiant du job d'import (Code du flux d'import).
        /// </summary>
        public static NameValueCollection SectionHelp { get; } = (NameValueCollection)ConfigurationManager.GetSection("help");

        /// <summary>
        /// Obtient l'identifiant du job d'import (Code du flux d'import).
        /// </summary>
        public string TutoGRZB { get; } = SectionHelp != null ? SectionHelp["HelpGRZB"] : null;

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <exception cref="System.UnauthorizedAccessException">Informe la non-autorisation d'accéder</exception>
        public override void CheckAccessToEntity(GroupeEnt entity)
        {
            int userId = this.securityManager.GetUtilisateurId();
            Repository.CheckAccessToEntity(entity, userId);
        }

        /// <summary>
        /// Get groupe entité by code
        /// </summary>
        /// <param name="code">Groupe code</param>
        /// <returns>Groupe entité</returns>
        public GroupeEnt GetGroupeByCode(string code)
        {
            return this.Repository.GetGroupeByCode(code);
        }

        /// <summary>
        /// Get groupe by code include societes
        /// </summary>
        /// <param name="code">groupe code</param>
        /// <returns>GroupeEnt</returns>
        public GroupeEnt GetGroupeByCodeIncludeSocietes(string code)
        {
            return this.Repository.GetGroupeByCodeIncludeSocietes(code);

        }

        public async Task<int> GetGroupeIdByCodeAsync(string code)
        {
            return await Repository.GetGroupeIdByCodeAsync(code);

        }

        /// <summary>
        /// Get groupe by code include societes
        /// </summary>
        /// <returns>GroupeEnt</returns>
        public string GetUrlTutoByGroupe()
        {
            UtilisateurEnt utilisateurCourant = utilisateurManager.GetContextUtilisateur();
            IEnumerable<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurList = affectationSeuilUtilisateurManager.GetListByUtilisateurId(utilisateurCourant.UtilisateurId);
            int typeOrganisationGroupeId = organisationManager.GetTypeOrganisationIdByCode(TypeOrganisationEnt.CodeGroupe);
            foreach (var affectation in affectationSeuilUtilisateurList)
            {
                List<OrganisationEnt> listOrgaPere = organisationManager.GetOrganisationParentByOrganisationId(affectation.OrganisationId, typeOrganisationGroupeId);

                OrganisationEnt organisationGroupeRZB = listOrgaPere.FirstOrDefault(o => o.Code == Constantes.CodeGroupeRZB);

                if (organisationGroupeRZB != null)
                {
                    return TutoGRZB;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Tous les groupes
        /// </summary>
        /// <returns>Liste des groupes</returns>
        public IEnumerable<GroupeEnt> GetAll()
        {
            return Repository.Get();
        }

        /// <summary>
        /// Recuperer Tous les groupes qui sont dans le perimetre de l'utilisateur connecte
        /// </summary>
        /// <returns>Liste des groupes</returns>
        public List<GroupeEnt> GetAllGroupForUser()
        {
            List<GroupeEnt> listGroup = new List<GroupeEnt>();
            UtilisateurEnt utilisateurConnecte = utilisateurManager.GetContextUtilisateur();
            listGroup.Add(utilisateurConnecte.Personnel?.Societe?.Groupe);
            return listGroup;
        }

        /// <summary>
        /// Recupre le groupe a partir d'un codeSocieteComptable d'une sosiete.
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable</param>
        /// <returns>Le groupe de la societe</returns>
        public GroupeEnt GetGroupeByCodeSocieteComptableOfSociete(string codeSocieteComptable)
        {
            return Repository.GetGroupeByCodeSocieteComptableOfSociete(codeSocieteComptable);
        }
    }
}
