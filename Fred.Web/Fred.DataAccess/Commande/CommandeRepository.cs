using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Commande
{
    /// <summary>
    ///   Référentiel de données pour les commandes.
    /// </summary>
    public class CommandeRepository : FredRepository<CommandeEnt>, ICommandeRepository
    {
        #region Constantes

        private const int CommandTimeout = 3600;
        private readonly string buyerDatabase = "[" + ConfigurationManager.AppSettings["Buyer:Server:Address"] + "]." + ConfigurationManager.AppSettings["Buyer:Server:Database"];

        #endregion

        public CommandeRepository(FredDbContext context)
            : base(context)
        {
        }

        #region Commandes

        /// <summary>
        ///   Appelle la méthode d'exécution de la procédure stockée de recherche des commandes BUYER à insérer
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        /// <returns>Le nombre de commandes trouvées</returns>
        public int GetNombreCommandesBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            int nombreCommandes;

            try
            {
                DbConnection sqlConnection = Context.Database.GetDbConnection();
                DbCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "FRED_countCommande_fromBuyer";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = CommandTimeout;

                sqlCommand.Parameters.Add(new SqlParameter("@ServerBuyer", buyerDatabase));
                sqlCommand.Parameters.Add(new SqlParameter("@EtablID", codeEtab));
                sqlCommand.Parameters.Add(new SqlParameter("@DateMinImport", dateDebut));
                sqlCommand.Parameters.Add(new SqlParameter("@DateMaxImport", dateFin));
                sqlCommand.Parameters.Add(new SqlParameter("@NombreCommandes", SqlDbType.Int) { Direction = ParameterDirection.Output });

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                nombreCommandes = (int)sqlCommand.Parameters["@NombreCommandes"].Value;

            }
            catch (SqlException e)
            {
                throw new FredRepositoryException(e.Message, e);
            }

            return nombreCommandes;
        }

        /// <summary>
        ///   Appelle la méthode d'exécution de la procédure stockée d'insertion des commandes BUYER dans FRED
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        /// <exception cref="SqlException">en cas d'erreur dans l'exécution de la requête</exception>
        public void ImporterCmdsBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                DbConnection sqlConnection = Context.Database.GetDbConnection();
                DbCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "FRED_collecteCommande_fromBuyer";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = CommandTimeout;

                sqlCommand.Parameters.Add(new SqlParameter("@ServerBuyer", buyerDatabase));
                sqlCommand.Parameters.Add(new SqlParameter("@EtablID", codeEtab));
                sqlCommand.Parameters.Add(new SqlParameter("@ForceEnregistrement", 0));
                sqlCommand.Parameters.Add(new SqlParameter("@DateMinImport", dateDebut));
                sqlCommand.Parameters.Add(new SqlParameter("@DateMaxImport", dateFin));
                sqlCommand.Parameters.Add(new SqlParameter("@DebugMode", 0));

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (SqlException e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne l'organisationId pour une commande
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>L organisationId</returns>
        public int? GetOrganisationIdByCommandeId(int commandeId)
        {
            //Query() a virer
            var commande = Query()
                        .Include(c => c.CI.Organisation)
                        .Filter(c => c.CommandeId == commandeId)
                        .Get()
                        .AsNoTracking()
                        .FirstOrDefault();
            return commande?.CI?.Organisation.OrganisationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool DoesCommandeExist(Func<CommandeEnt, bool> condition)
        {
            return Context.Commandes != null && Context.Commandes.Where(condition).Any();
        }

        /// <inheritdoc />            
        public CommandeEnt GetFull(int commandeId)
        {
            var query = Context.Commandes.Where(s => s.CommandeId == commandeId);

            //load Tables enumerables
            Context.StatutsCommande.Load();
            Context.Devise.Load();
            Context.CommandeTypes.Load();
            Context.TypeSocietes.Load();
            Context.TypesRessource.Load();
            Context.Unites.Load();
            Context.DepenseTypes.Load();
            Context.FacturationTypes.Load();

            // Mettre des Include lorsqu'on récupère plusieurs objets
            query.Include(a => a.CI.EtablissementComptable.Societe)
                 .Include(s => s.AuteurCreation.Personnel)
                 .Include(a => a.CI.Societe).Load();

            query.Include(s => s.AuteurModification.Personnel)
                 .Include(v => v.Valideur.Personnel)
                 .Load();

            query.Select(a => a.Contact).Load();
            query.Select(a => a.Suivi).Load();
            query.Select(f => f.Agence).Load();
            query.Select(f => f.Fournisseur).Load();
            query.Select(f => f.OldFournisseur).Load();
            query.Select(lp => lp.LivraisonPays).Load();
            query.Select(fp => fp.FacturationPays).Load();
            query.Select(fp => fp.FournisseurPays).Load();
            query.Select(c => c.SystemeExterne).Load();
            query.Select(t => t.PiecesJointesCommande).Load();

            // Mettre également des Include lorsque l'on récupère des objets dans une liste
            var querylignes = query.SelectMany(x => x.Lignes);

            querylignes.Include(l => l.Tache)
                        .Include(l => l.Ressource).Load();

            querylignes.Include(l => l.AvenantLigne.Avenant).Load();

            var querydepenses = querylignes.Include(x => x.AllDepenses).SelectMany(x => x.AllDepenses);

            querydepenses.Include(x => x.Tache)
                    .Include(x => x.Ressource).Load();

            querydepenses.Include(x => x.AuteurCreation.Personnel)
                    .Include(x => x.AuteurModification.Personnel)
                    .Include(x => x.AuteurVisaReception.Personnel).Load();

            querydepenses.Include(r => r.Depenses)
                        .Include(r => r.FacturationsFar).Load();

            querydepenses.Include(r => r.FacturationsReception).Load();

            return query.Single();
        }

        public CommandeEnt GetFullWithoutDepenses(int commandeId)
        {
            var query = Context.Commandes.Where(s => s.CommandeId == commandeId);

            // load Tables enumerables
            Context.StatutsCommande.Load();
            Context.Devise.Load();
            Context.CommandeTypes.Load();
            Context.TypeSocietes.Load();
            Context.TypesRessource.Load();
            Context.Unites.Load();

            // Mettre des Include lorsqu'on récupère plusieurs objets
            query.Include(a => a.CI.EtablissementComptable.Societe)
                 .Include(s => s.AuteurCreation.Personnel)
                 .Include(a => a.CI.Societe).Load();

            query.Include(s => s.AuteurModification.Personnel)
                 .Include(v => v.Valideur.Personnel)
                 .Load();

            query.Select(a => a.Contact).Load();
            query.Select(a => a.Suivi).Load();
            query.Select(f => f.Agence).Load();
            query.Select(f => f.Fournisseur).Load();
            query.Select(f => f.OldFournisseur).Load();
            query.Select(lp => lp.LivraisonPays).Load();
            query.Select(fp => fp.FacturationPays).Load();
            query.Select(fp => fp.FournisseurPays).Load();
            query.Select(c => c.SystemeExterne).Load();

            // Mettre également des Include lorsque l'on récupère des objets dans une liste
            var querylignes = query.SelectMany(x => x.Lignes);

            querylignes.Include(l => l.Tache)
                        .Include(l => l.Ressource).Load();

            return query.Single();
        }

        /// <summary>
        ///     Récupération de la liste des commandes selon un filtre
        /// </summary>
        /// <param name="filter">Filtre de commande</param>
        /// <param name="totalCount">Nombre total de commande selon le filtre</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste de commandes filtré</returns>
        public IEnumerable<CommandeEnt> GetList(SearchCommandeEnt filter, out int totalCount, int? page, int? pageSize)
        {
            var query = Context.Commandes.AsExpandable().Where(filter.GetPredicateWhere())
                .OrderBy(c => c.CiId)
                .ThenByDescending(x => x.CommandeId);

            var querybased = page.HasValue && pageSize.HasValue
                ? GetPartialcommande(query.Select(x => x.CommandeId)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value).ToList()).OrderBy(c => c.CiId)
                .ThenByDescending(x => x.CommandeId)
                : GetAllCommandeInclude(query);

            //load Tables enumerables sans filtres c'est plus rapide
            Context.DepenseTypes.Load();
            Context.FacturationTypes.Load();
            Context.StatutsCommande.Load();
            Context.CommandeTypes.Load();
            Context.Devise.Load();

            totalCount = query.Count();

            return querybased;
        }

        private IEnumerable<CommandeEnt> GetAllCommandeInclude(IQueryable<CommandeEnt> query)
        {
            query.Include(s => s.AuteurCreation.Personnel)
                .Include(s => s.AuteurModification.Personnel)
                .Include(v => v.Valideur.Personnel)
                .Load();
            //Include lignes Commandes and dépenses
            query.SelectMany(x => x.Lignes)
                  .Include(l => l.AllDepenses).ThenInclude(r => r.FacturationsReception)
                  .Include(l => l.AllDepenses).ThenInclude(r => r.Depenses)
                  .Include(l => l.AvenantLigne.Avenant)
                  .Load();
            //load réferentiels  avec filtres
            query.Select(x => x.PiecesJointesCommande).Load();
            query.Select(a => a.CI).Load();
            query.Select(a => a.Contact).Load();
            query.Select(a => a.Suivi).Load();
            query.Select(f => f.Fournisseur).Load();

            return query;
        }

        private IEnumerable<CommandeEnt> GetPartialcommande(List<int> commandeIds)
        {
            var querypartiel = Context.Commandes.Where(x => commandeIds.Contains(x.CommandeId));
            //include Auteurs Creation et Auteurs Modification

            querypartiel.Include(s => s.AuteurCreation.Personnel)
                .Include(s => s.AuteurModification.Personnel)
                .Include(v => v.Valideur.Personnel)
                .Load();
            //Include lignes Commandes and dépenses
            querypartiel.SelectMany(x => x.Lignes).Where(x => commandeIds.Contains(x.CommandeId))
                  .Include(l => l.AllDepenses).ThenInclude(r => r.FacturationsReception)
                  .Include(l => l.AllDepenses).ThenInclude(r => r.Depenses)
                  .Include(l => l.AvenantLigne.Avenant)
                  .Load();
            //load Pièces jointes commande
            querypartiel.Select(x => x.PiecesJointesCommande).Load();

            //load réferentiels  avec filtres
            querypartiel.Select(a => a.CI).Load();
            querypartiel.Select(a => a.Contact).Load();
            querypartiel.Select(a => a.Suivi).Load();
            querypartiel.Select(f => f.Fournisseur).Load();

            return querypartiel;
        }

        /// <summary>
        ///   Retourner la requête de récupération des commandes
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public IEnumerable<CommandeEnt> Search(List<Expression<Func<CommandeEnt, bool>>> filters,
                                              Func<IQueryable<CommandeEnt>, IOrderedQueryable<CommandeEnt>> orderBy = null,
                                              List<Expression<Func<CommandeEnt, object>>> includeProperties = null,
                                              int? page = null,
                                              int? pageSize = null,
                                              bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                //Get() a virer
                return Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }

            //Get() a virer
            return Get(filters, orderBy, includeProperties, page, pageSize).ToList();
        }

        #endregion

        public (List<CommandeEnt> orders, int total) SearchReceivableOrders(SearchReceivableOrdersFilter filter, int page = 1, int pageSize = 20)
        {
            var ids = Context
                .Commandes
                .Where(filter.GetCommandeForReceptionPredicateWhere())
                .Select(x => x.CommandeId)
                .ToList();

            int total = ids.Count;

            List<CommandeEnt> orders = Context.Commandes
                              .Include(a => a.CI)
                              .Include(f => f.Fournisseur)
                              .Include(t => t.Devise)
                              .Include(c => c.StatutCommande)
                              .Include(c => c.PiecesJointesCommande)
                              .Include(c => c.Lignes).ThenInclude(l => l.Tache)
                              .Include(c => c.Lignes).ThenInclude(l => l.Ressource)
                              .Include(c => c.Lignes).ThenInclude(l => l.Unite)
                              .Include(c => c.Lignes).ThenInclude(r => r.AllDepenses).ThenInclude(x => x.DepenseType)
                              .Include(c => c.Lignes).ThenInclude(r => r.AllDepenses).ThenInclude(x => x.PiecesJointesReception)
                              .Include(c => c.Lignes).ThenInclude(l => l.AvenantLigne.Avenant)
                              .Where(c => ids.Contains(c.CommandeId))
                              .OrderBy(c => c.Numero)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            if (filter.RessourceId.HasValue || filter.TacheId.HasValue)
            {
                orders.ForEach(
                    item => item.Lignes.AsQueryable().Where(filter.GetCommandeLigneForReceptionPredicateWhere()).ToList().ForEach(
                        ligne => item.Lignes.Remove(ligne)
                    )
                );
            }

            return (orders, total);
        }

        /// <summary>
        /// Récupérer une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande trouvée</returns>
        public CommandeEnt GetById(int commandeId)
        {
            return Context
                .Commandes
                .AsNoTracking()
                .FirstOrDefault(x => x.CommandeId == commandeId);
        }

        /// <summary>
        /// Récupérer le statut d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Statut de la commande trouvée</returns>
        public StatutCommandeEnt GetStatutCommandeByCommandeId(int commandeId)
        {
            return Context
                .Commandes
                .Include(c => c.StatutCommande)
                .AsNoTracking()
                .FirstOrDefault(x => x.CommandeId == commandeId)?
                .StatutCommande;
        }

        /// <summary>
        /// Récupérer le statut d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Statut de la commande trouvée</returns>
        public StatutCommandeEnt GetStatutCommandeByStatutCommandeId(int statutCommandeId)
        {
            return Context
                .StatutsCommande
                .FirstOrDefault(x => x.StatutCommandeId == statutCommandeId);
        }

        /// <summary>
        /// Récupérer le type d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Type de la commande trouvée</returns>
        public CommandeTypeEnt GetCommandeTypeByCommandeId(int commandeId)
        {
            return Context
                .Commandes
                .Include(c => c.Type)
                .AsNoTracking()
                .FirstOrDefault(x => x.CommandeId == commandeId)?
                .Type;
        }

        /// <summary>
        /// Récupérer le type d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Type de la commande trouvée</returns>
        public CommandeTypeEnt GetCommandeTypeByCommandeTypeId(int commandeTypeId)
        {
            return Context
                .CommandeTypes
                .FirstOrDefault(x => x.CommandeTypeId == commandeTypeId);
        }

        /// <summary>
        /// Récupérer une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande trouvée</returns>
        public CommandeEnt GetCommandeWithLignes(int commandeId)
        {
            return Context
                .Commandes
                .Include(c => c.Type)
                .Include(c => c.Lignes)
                .Include(c => c.Lignes).ThenInclude(l => l.Ressource)
                .Include(c => c.Lignes).ThenInclude(l => l.Materiel)
                .AsNoTracking()
                .FirstOrDefault(x => x.CommandeId == commandeId);
        }


        /// <summary>
        /// Récupérer une commande avec ses lignes d'avenants
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande trouvée</returns>
        public CommandeEnt GetCommandeWithCommandeLignes(int commandeId)
        {
            var commandeWithAvenantLignes = Context.Commandes
              .Where(c => c.CommandeId == commandeId)
              .Include(c => c.Lignes)
                  .ThenInclude(l => l.AvenantLigne)
              .Include(c => c.StatutCommande)
              .FirstOrDefault();

            return commandeWithAvenantLignes;
        }

        /// <summary>
        /// Retourne les commandes pour une liste de numéro de commande
        /// </summary>
        /// <param name="numerosCommande">Liste de numéro de commande</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de commandes</returns>
        public IEnumerable<CommandeEnt> GetCommandes(List<string> numerosCommande, int ciId)
        {
            return Context.Commandes.Include(l => l.Lignes).Where(q => numerosCommande.Contains(q.Numero) && q.CiId == ciId);
        }

        /// <inheritdoc/>
        public async Task<CommandeEnt> GetCommandeSAPAsync(int commandeId)
        {
            return await GetCommandeQuery()
                .FirstOrDefaultAsync(x => x.CommandeId == commandeId);
        }

        /// <inheritdoc/>
        public async Task<CommandeEnt> GetCommandeAvenantSAPAsync(int commandeId, int numeroAvenant)
        {
            return await GetCommandeQuery()
                .Where(c => c.CommandeId == commandeId
                            && c.Lignes.Any(l => l.AvenantLigne != null)
                            && c.Lignes.Any(l => l.AvenantLigne.Avenant != null)
                            && c.Lignes.Any(l => l.AvenantLigne.Avenant.NumeroAvenant == numeroAvenant))
                .Include(c => c.Lignes).ThenInclude(l => l.AvenantLigne.Avenant)
                .FirstOrDefaultAsync();
        }

        private IQueryable<CommandeEnt> GetCommandeQuery()
        {
            return Context.Commandes
                .Include(c => c.CI.Organisation)
                .Include(c => c.CI.EtablissementComptable)
                .Include(c => c.Contact)
                .Include(c => c.Suivi)
                .Include(c => c.Fournisseur)
                .Include(c => c.Agence.Adresse)
                .Include(c => c.Agence.Fournisseur)
                .Include(c => c.AuteurCreation.Personnel)
                .Include(c => c.AuteurModification.Personnel)
                .Include(c => c.Valideur.Personnel)
                .Include(c => c.StatutCommande)
                .Include(c => c.Type)
                .Include(c => c.Devise)
                .Include(c => c.Lignes).ThenInclude(l => l.Tache)
                .Include(c => c.Lignes).ThenInclude(l => l.Ressource)
                .Include(c => c.Lignes).ThenInclude(l => l.Unite)
                .Include(c => c.Lignes).ThenInclude(l => l.Ressource.ReferentielEtendus).ThenInclude(x => x.Nature)
                .Include(c => c.FacturationPays)
                .Include(c => c.FournisseurPays)
                .Include(c => c.LivraisonPays);
        }

        /// <inheritdoc/>
        public async Task<int> GetOrganisationIdByCommandeIdAsync(int commandeId)
        {
            return await Context.Commandes
                .Where(c => c.CommandeId == commandeId)
                .Select(c => c.CI.Organisation.OrganisationId)
                .FirstOrDefaultAsync();
        }

        public CommandeEnt GetCommandeByNumberOrExternalNumber(string numero)
        {
            return Query()
                  .Include(c => c.CI)
                  .Get().FirstOrDefault(x => x.Numero == numero || x.NumeroCommandeExterne == numero);
        }
    }
}
