using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.Business.Depense
{
    public class DepenseManager : Manager<DepenseAchatEnt, IDepenseRepository>, IDepenseManager
    {
        private readonly ICommandeManager commandeManager;
        private readonly ICIManager ciManager;
        private readonly IUtilisateurManager userManager;
        private readonly IDepenseTypeManager depenseTypeManager;
        private readonly IDepenseAchatService dpenseAchatService;

        public DepenseManager(
            IUnitOfWork uow,
            IDepenseValidator validator,
            IDepenseRepository depenseRepository,
            IUtilisateurManager userManager,
            ICommandeManager commandeManager,
            ICIManager ciManager,
            IDepenseTypeManager depenseTypeManager,
            IDepenseAchatService dpenseAchatService)
            : base(uow, depenseRepository, validator)
        {
            this.ciManager = ciManager;
            this.userManager = userManager;
            this.commandeManager = commandeManager;
            this.depenseTypeManager = depenseTypeManager;
            this.dpenseAchatService = dpenseAchatService;
        }

        /// <summary>
        /// Retourne la liste des dépenses.
        /// </summary>
        /// <returns>La liste des dépenses.</returns>
        public IEnumerable<DepenseAchatEnt> GetDepenseList()
        {
            IEnumerable<DepenseAchatEnt> depenses = Repository.GetDepenseList().ComputeAll();

            dpenseAchatService.ComputeNature(depenses);

            return depenses;
        }

        /// <summary>
        /// Retourne la liste des dépenses selon un CI choisit
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepenseListAsync(int ciId)
        {
            return await Repository.GetDepenseListAsync(ciId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des dépenses en incluant les tahces et les ressources liées
        /// ainsi que toutes les factuartions
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetDepensesListWithMinimumIncludesAsync(int ciId)
        {
            return await Repository.GetDepensesListWithMinimumIncludesAsync(ciId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des dépenses filtrée, triée (avec pagination)
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des dépense</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Retourne la liste des dépenses filtrée, triée et paginée</returns>
        public IEnumerable<DepenseAchatEnt> SearchDepenseListWithFilter(SearchDepenseEnt filter, int page, int pageSize)
        {
            IEnumerable<DepenseAchatEnt> depenses = Repository.SearchDepenseListWithFilter(filter, page, pageSize).ComputeAll();

            dpenseAchatService.ComputeNature(depenses);

            return depenses;
        }

        /// <summary>
        /// Retourne le montant total de la liste des dépenses filtrée, triée
        /// </summary>
        /// <param name="filter">Objet de recherche et de tri des dépense</param>
        /// <returns>Retourne le montant total de la liste des dépenses filtrée, triée et paginée</returns>
        public double GetMontantTotal(SearchDepenseEnt filter)
        {
            return Repository.GetMontantTotal(filter.GetPredicateWhere());
        }

        /// <summary>
        /// Retourne la dépense portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="depenseID">Identifiant de la dépense à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        public DepenseAchatEnt GetDepenseById(int depenseID)
        {
            return Repository.GetDepenseById(depenseID).ComputeAll();
        }

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns>Dépense ajoutée</returns>
        public DepenseAchatEnt AddDepense(DepenseAchatEnt depense)
        {
            return AddDepense(depense, userManager.GetContextUtilisateurId()).ComputeAll();
        }

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <param name="utilisateurId">L'identifant de l'auteur créateur.</param>
        /// <returns>Dépense ajoutée</returns>
        public DepenseAchatEnt AddDepense(DepenseAchatEnt depense, int? utilisateurId)
        {
            depense.DateCreation = DateTime.UtcNow;
            depense.AuteurCreationId = utilisateurId;

            // RG US 2853
            depense.DateComptable = depense.Date;
            depense.QuantiteDepense = depense.Quantite;

            BusinessValidation(depense);

            DepenseAchatEnt depenseAchat = Repository.AddDepense(depense);
            Save();

            return depenseAchat.ComputeAll();
        }

        /// <summary>
        /// Sauvegarde les modifications d'une dépense.
        /// </summary>
        /// <param name="depense">dépense à modifier</param>
        /// <returns>Dépense modifiée</returns>
        public DepenseAchatEnt UpdateDepense(DepenseAchatEnt depense)
        {
            depense.DateModification = DateTime.UtcNow;
            depense.AuteurModificationId = userManager.GetContextUtilisateurId();

            BusinessValidation(depense);

            DepenseAchatEnt depenseAchat = Repository.UpdateDepense(depense);
            Save();

            return depenseAchat.ComputeAll();
        }

        /// <summary>
        /// Sauvegarde les modifications d'une liste de dépenses
        /// </summary>
        /// <param name="depenses">Liste de dépenses à modifier</param>
        /// <param name="auteurModificationId">Identifiant de l'utilisateur ayant effectué la modification</param>
        /// <returns>Liste de Dépenses modifiées</returns>
        public List<DepenseAchatEnt> UpdateDepense(List<DepenseAchatEnt> depenses, int auteurModificationId)
        {
            foreach (DepenseAchatEnt depense in depenses.ToList())
            {
                depense.DateModification = DateTime.UtcNow;
                depense.AuteurModification = null;
                depense.AuteurModificationId = auteurModificationId;
                BusinessValidation(depense);
                Repository.Update(depense);
            }

            depenses.ComputeAll();

            dpenseAchatService.ComputeNature(depenses);

            Save();

            return depenses;
        }

        /// <summary>
        /// Supprime une dépense.
        /// </summary>
        /// <param name="depenseId">Identifiant unique de la dépense à supprimer</param>
        public void DeleteDepenseById(int depenseId)
        {
            DepenseAchatEnt depense = GetDepenseById(depenseId);
            depense.DateSuppression = DateTime.UtcNow;
            depense.AuteurSuppressionId = userManager.GetContextUtilisateurId();
            Repository.UpdateDepense(depense).ComputeAll();
        }

        /// <summary>
        /// Initialise une nouvelle instance de dépense <see cref="DepenseAchatEnt" /> selon les règles de gestion établies.
        /// </summary>
        /// <param name="commandeLigneId">La ligne de commande à dépense</param>
        /// <returns>Retourne une instance de dépense.</returns>
        public DepenseAchatEnt GetNewDepense(int commandeLigneId)
        {
            CommandeLigneEnt commandeLigne = commandeManager.GetCommandeLigneById(commandeLigneId);
            CIEnt ci = null;

            if (commandeLigne.Commande.CiId.HasValue)
            {
                ci = ciManager.GetCIById(commandeLigne.Commande.CiId.Value);
            }

            DepenseAchatEnt depense = new DepenseAchatEnt
            {
                CommandeLigneId = commandeLigne.CommandeLigneId,
                CiId = commandeLigne.Commande.CiId,
                CI = ci,
                FournisseurId = commandeLigne.Commande.FournisseurId,
                Libelle = commandeLigne.Libelle,
                TacheId = commandeLigne.TacheId,
                Tache = commandeLigne.Tache,
                RessourceId = commandeLigne.RessourceId,
                Ressource = commandeLigne.Ressource,
                Quantite = commandeLigne.Quantite - commandeLigne.QuantiteReceptionnee,
                PUHT = commandeLigne.PUHT,
                UniteId = commandeLigne.UniteId,
                Unite = commandeLigne.Unite,
                Devise = commandeLigne.Commande.Devise,
                DeviseId = commandeLigne.Commande.DeviseId,
                Date = DateTime.Today.ToUniversalTime(),
                AuteurCreationId = userManager.GetContextUtilisateurId(),
                DateCreation = DateTime.UtcNow,
                DepenseTypeId = depenseTypeManager.Get(DepenseType.Reception.ToIntValue()).DepenseTypeId // Par défaut, type de dépense Réception
            };

            return depense.ComputeAll();
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe de recherche des dépenses
        /// </summary>
        /// <returns>Objet de filtrage + tri des dépenses initialisé</returns>
        public SearchDepenseEnt GetNewFilter()
        {
            return new SearchDepenseEnt
            {
                ValueText = string.Empty,

                CICodeLibelle = true,
                Libelle = true,
                CommandeNumeroLibelle = true,
                FournisseurCodeLibelle = true,
                NumeroBL = true,
                RessourceCodeLibelle = true,
                TacheCodeLibelle = true,
                Commentaire = true,
                AuteurCreationNomPrenom = true,
                AuteurModificationNomPrenom = true,
                Unite = true,

                DateAsc = true // Initialisation business : tri sur Date
            };
        }

        /// <summary>
        /// Renvoie la dépense associée au groupe de remplacement
        /// </summary>
        /// <param name="groupRemplacementId">L'id du groupe de remplacement</param>
        /// <returns>Dépense</returns>
        public DepenseAchatEnt GetByGroupRemplacementId(int groupRemplacementId)
        {
            return Repository.GetByGroupRemplacementId(groupRemplacementId).FirstOrDefault();
        }

        /// <summary>
        /// Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une requête</returns>
        public IEnumerable<DepenseAchatEnt> Search(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                                   Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                                   List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                                   int? page = null,
                                                   int? pageSize = null
            )
        {
            return Repository.Get(filters, orderBy, includeProperties, page, pageSize, asNoTracking: false);
        }
    }
}
