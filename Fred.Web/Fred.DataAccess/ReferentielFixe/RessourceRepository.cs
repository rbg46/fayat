using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Carburant;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Tool;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ReferentielFixe
{
    /// <summary>
    /// Référentiel de données pour les ressources
    /// </summary>
    public class RessourceRepository : FredRepository<RessourceEnt>, IRessourceRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RessourceRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="context">Le contexte.</param>
        public RessourceRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public RessourceEnt AddRessource(RessourceEnt ressourceEnt)
        {
            if (ressourceEnt.TypeRessource != null)
            {
                Context.TypesRessource.Attach(ressourceEnt.TypeRessource);
            }
            if (ressourceEnt.Carburant != null)
            {
                Context.Carburants.Attach(ressourceEnt.Carburant);
            }
            if (ressourceEnt.SousChapitre != null)
            {
                Context.SousChapitres.Attach(ressourceEnt.SousChapitre);
            }
            Insert(ressourceEnt);

            return ressourceEnt;
        }

        /// <summary>
        /// Met a jour une ressourceEnt
        /// </summary>
        /// <param name="ressourceEnt">ressourceEnt</param>
        /// <returns>retourne le même objet passé en paramètre</returns>
        public RessourceEnt UpdateRessource(RessourceEnt ressourceEnt)
        {
            if (ressourceEnt.TypeRessource != null)
            {
                Context.TypesRessource.Attach(ressourceEnt.TypeRessource);
            }
            if (ressourceEnt.Carburant != null)
            {
                Context.Carburants.Attach(ressourceEnt.Carburant);
            }
            if (ressourceEnt.SousChapitre != null)
            {
                Context.SousChapitres.Attach(ressourceEnt.SousChapitre);
            }

            Update(ressourceEnt);

            return ressourceEnt;
        }

        /// <summary>
        /// AppelTraitementSqlVerificationDesDependances
        /// </summary>
        /// <param name="ressource">RessourceEnt</param>
        /// <returns>true si il y a des dependances</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(RessourceEnt ressource)
        {
            bool isDeletable = false;
            SqlParameter[] parameters =
            {
        new SqlParameter("origTableName", "FRED_RESSOURCE"),
        new SqlParameter("exclusion", string.Empty),
        new SqlParameter("dependance", string.Empty),
        new SqlParameter("origineId", ressource.RessourceId),
        new SqlParameter("delimiter", string.Empty),
        new SqlParameter("resu", SqlDbType.Int) { Direction = ParameterDirection.Output }
      };

            // ReSharper disable once CoVariantArrayConversion
            Context.Database.ExecuteSqlCommand("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUTPUT", parameters);

            // Vérifie s'il y a aucune dépendances (paramètre "resu")
            if (Convert.ToInt32(parameters.First(x => x.ParameterName == "resu").Value) == 0)
            {
                isDeletable = true;
            }

            return isDeletable;
        }

        /// <summary>
        /// Retourne le ressource avec l'identifiant unique indiqué.
        /// </summary>
        /// <param name="ressourceId">Identifiant du ressource à retrouver.</param>
        /// <returns>Le ressource retrouvé, sinon null.</returns>
        public RessourceEnt GetById(int ressourceId)
        {
            return
              Context.Ressources.Include(c => c.Carburant.Unite)
                     .Include(c => c.TypeRessource)
                     .Include(r => r.SousChapitre.Chapitre)
                     .AsNoTracking()
                     .SingleOrDefault(r => r.RessourceId.Equals(ressourceId));
        }

        public RessourceEnt GetRessourceById(int ressourceId)
        {
            return Context.Ressources
                     .AsNoTracking()
                     .SingleOrDefault(r => r.RessourceId == ressourceId);
        }

        /// <summary>
        /// Retourne la liste des ressources.
        /// </summary>
        /// <returns>Liste des ressources.</returns>
        public IEnumerable<RessourceEnt> GetList()
        {
            foreach (RessourceEnt ressource in Context.Ressources.Include(c => c.Carburant.Unite).Where(s => s.Active && !s.DateSuppression.HasValue))
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Retourne une liste en lecture seule de <see cref="RessourceEnt" /> actif
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressources</param>
        /// <returns>Une liste en lecture seul de <see cref="RessourceEnt" /></returns>
        public IReadOnlyList<RessourceEnt> GetList(List<string> ressourceCodes)
        {
            return Context.Ressources.Where(s => s.Active && !s.DateSuppression.HasValue && ressourceCodes.Contains(s.Code)).ToList();
        }

        /// <summary>
        /// Retourne une liste en lecture seule de <see cref="RessourceEnt" /> actif
        /// </summary>
        /// <param name="ressourceCodes">Liste de code ressources</param>
        /// <param name="societeIds">Liste d'identifiant de societe</param>
        /// <returns>Liste de <see cref="RessourceEnt" /></returns>
        public IEnumerable<RessourceEnt> GetList(List<string> ressourceCodes, List<int> societeIds)
        {
            return Context.Ressources.Where(s => s.Active && !s.DateSuppression.HasValue && ressourceCodes.Contains(s.Code) && s.ReferentielEtendus.Any(re => societeIds.Contains(re.SocieteId)));
        }

        /// <summary>
        /// Obtient la collection des ressources (suppprimées inclus)
        /// </summary>
        /// <returns>La collection des ressources</returns>
        public IEnumerable<RessourceEnt> GetAllList()
        {
            foreach (RessourceEnt ressource in Context.Ressources.AsNoTracking())
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Renvoi la liste des ressources actives qui ont été rattachées à une nature
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>retourne la liste des ressources actives qui ont été rattachées à une nature</returns>
        public IEnumerable<RessourceEnt> GetListRessourcesBySocieteId(int societeId)
        {
            foreach (RessourceEnt ressource in Context.ReferentielEtendus.Include(r => r.Ressource)
                                                      .Where(r => r.SocieteId == societeId && r.NatureId != null && r.Ressource.Active && r.Ressource.DateSuppression == null)
                                                      .Select(r => r.Ressource)
                                                      .AsNoTracking())
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Renvoi la liste des ressources
        /// </summary>
        /// <param name="groupId">Identifiant du groupe</param>
        /// <returns>retourne la liste des ressources appartenant à un groupe</returns>
        public IEnumerable<RessourceEnt> GetListByGroupeId(int groupId)
        {
            foreach (RessourceEnt ressource in Context.Ressources.Include(c => c.SousChapitre.Chapitre)
                                                      .Include(c => c.TypeRessource)
                                                      .Include(c => c.Carburant)
                                                      .Where(r => r.SousChapitre.Chapitre.GroupeId.Equals(groupId) && !r.DateSuppression.HasValue && !r.IsRessourceSpecifiqueCi).AsNoTracking())
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Renvoi la liste des ressources en fonction du code et du groupe.
        /// </summary>
        /// <param name="code">Le code de la ressource.</param>
        /// <param name="groupId">L'identifiant du groupe.</param>
        /// <returns>La liste des ressources concernées.</returns>
        public IEnumerable<RessourceEnt> Get(string code, int groupId)
        {
            foreach (RessourceEnt ressource in Context.Ressources
              .Include(r => r.SousChapitre.Chapitre)
              .Include(r => r.ReferentielEtendus)
              .Where(r => r.Code == code && r.SousChapitre.Chapitre.GroupeId.Equals(groupId) && !r.DateSuppression.HasValue).AsNoTracking())
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Obtient la collection des ressources appartenant à un sous-chapitre spécifié
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sous-chapitre.</param>
        /// <returns>La collection des ressources</returns>
        public IEnumerable<RessourceEnt> GetListBySousChapitreId(int sousChapitreId)
        {
            foreach (RessourceEnt ressource in Context.Ressources.Include(c => c.TypeRessource).Include(c => c.Carburant).Where(r => r.SousChapitreId.Equals(sousChapitreId) && !r.DateSuppression.HasValue && !r.IsRessourceSpecifiqueCi).AsNoTracking())
            {
                yield return ressource;
            }
        }

        /// <summary>
        /// Retourne le type de ressource en fonction de son code
        /// </summary>
        /// <param name="code">Code de la ressource</param>
        /// <returns>Le type de ressource</returns>
        public TypeRessourceEnt GetTypeRessourceByCode(string code)
        {
            return Context.TypesRessource.AsNoTracking().SingleOrDefault(t => t.Code == code);
        }

        /// <summary>
        /// Obtient la collection des types de ressources
        /// </summary>
        /// <returns>La collection des types de ressource</returns>
        public IEnumerable<CarburantEnt> GetCarburantList()
        {
            foreach (CarburantEnt carburant in Context.Carburants.Include(x => x.Unite).AsNoTracking())
            {
                yield return carburant;
            }
        }

        /// <summary>
        /// Indique si le code existe déjà pour les ressources d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeRessourceExist(string code, int groupeId)
        {
            return Context.Ressources.Include(r => r.SousChapitre.Chapitre)
                                     .Any(c => c.SousChapitre != null
                                        && c.SousChapitre.Chapitre != null
                                        && c.SousChapitre.Chapitre.GroupeId == groupeId
                                        && c.Code == code);
        }

        /// <summary>
        /// Cherche une liste de Ressource.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Ressources.</param>
        /// <returns>Une liste de Ressource.</returns>
        public IEnumerable<RessourceEnt> SearchRessources(string text)
        {
            var ressources = QueryPagingHelper.ApplyScrollPaging(Context.Ressources.Where(p => p.Code.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || p.Libelle.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).AsQueryable());
            return ressources.ToList();
        }

        public async Task<IEnumerable<TypeRessourceEnt>> SearchRessourceTypesByCodeOrLabelAsync(string text)
        {
            return await Context.TypesRessource
                .AsNoTracking()
                .Where(p => string.IsNullOrEmpty(text) || p.Code.Contains(text) || p.Libelle.Contains(text))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Cherche une liste de Ressource.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des Ressources.</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Une liste de Ressource.</returns>
        public IEnumerable<RessourceEnt> SearchRessources(string text, int societeId)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetListRessourcesBySocieteId(societeId).AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(GetListRessourcesBySocieteId(societeId)
                                                         .Where(p => p.Code.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || p.Libelle.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                                                         .AsQueryable())
                                    .ToList();
        }

        /// <summary>
        /// Détermine si une ressource peut être supprimée ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="ressource">Elément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        public bool IsDeletable(RessourceEnt ressource)
        {
            return AppelTraitementSqlVerificationDesDependances(ressource);
        }

        /// <summary>
        /// Vérifie que le code de la ressource n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la ressource à comparer</param>
        /// <returns>Vrai si le code de la ressource existe, faux sinon</returns>
        public bool IsExistByCode(string code)
        {
            return Context.Ressources.Where(r => string.Equals(r.Code, code, StringComparison.OrdinalIgnoreCase)).AsNoTracking().Any();
        }

        /// <summary>
        /// Retourne toutes les ressources du referentiel étendu de la société ou étant une ressource spécifique du CI donné.
        /// La liste sera chargée avec le sous chapitre et le chapitre associé à la ressource
        /// Cette fonction inclue les sous chapitre et les chapitres pour chaque ressource
        /// </summary>
        /// <param name="societeId">Id de la société</param>
        /// <param name="ciId">Id du CI</param>
        /// <returns>Une liste potentiellement vide jamais null</returns>
        public IEnumerable<RessourceEnt> GetListBySocieteIdWithSousChapitreEtChapitre(int societeId, int? ciId = null)
        {
            return Context.Ressources
                .Include(r => r.SousChapitre.Chapitre)
                .Include(r => r.ReferentielEtendus)
                .Where(r =>
                        (
                            r.ReferentielEtendus.Any(re => re.SocieteId == societeId) || (r.IsRessourceSpecifiqueCi && r.SpecifiqueCiId == ciId)
                        )
                        &&
                       r.Active &&
                       !r.DateSuppression.HasValue);
        }

        /// <summary>
        /// Récupère les ressources spécifiques d'un CI.
        /// </summary>
        /// <typeparam name="TRessource">Type de ressource souhaité.</typeparam>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="selector">Selector permettant de construire un TRessource à partir d'une entité.</param>
        /// <returns>Les ressources spécifiques du CI.</returns>
        public List<TRessource> GetRessourceSpecifiques<TRessource>(int ciId, Expression<Func<RessourceEnt, TRessource>> selector)
        {
            return Context.Ressources
                .Where(r => r.IsRessourceSpecifiqueCi && r.SpecifiqueCiId == ciId)
                .Select(selector)
                .ToList();
        }

        /// <summary>
        /// Récupère les ressources spécifiques d'un CI.
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="paths">Include à utiliser.</param>
        /// <returns>Les ressources spécifiques du CI.</returns>
        public List<RessourceEnt> GetRessourceSpecifiqueEnts(int ciId, params Expression<Func<RessourceEnt, object>>[] paths)
        {
            IQueryable<RessourceEnt> query = Context.Ressources;
            if (paths != null)
            {
                foreach (var path in paths)
                {
                    query = query.Include(path);
                }
            }
            return query
                .Where(r => r.IsRessourceSpecifiqueCi && r.SpecifiqueCiId == ciId)
                .ToList();
        }

        /// <summary>
        /// Récupère l'identifiant correspondant à la nature d'une ressource
        /// </summary>
        /// <param name="ressourceIdNatureFilter">L'identifiant de la ressource dont on veut retrouver la nature</param>
        /// <param name="societeId">La société de la ressource</param>
        /// <returns>Un nullable correspondant</returns>
        public int? GetNatureIdRessource(int? ressourceIdNatureFilter, int? societeId)
        {
            // recherche de la natureId pour une ressource referentiel etendu non spécifique CI
            int? natureIdRessource = Query()
                                        .Filter(r => r.RessourceId == ressourceIdNatureFilter && !r.IsRessourceSpecifiqueCi).Get()
                                        .SelectMany(re => re.ReferentielEtendus)
                                        .Where(re => re.SocieteId == societeId)
                                        .Select(x => x.NatureId).FirstOrDefault();
            // si pas de NatureId de referentiel étendu c'est une ressource specifique CI
            if (natureIdRessource == null)
            {
                // recherche de la natureId pour une ressource spécifique CI
                natureIdRessource = Query()
                                        .Filter(r => r.RessourceId == ressourceIdNatureFilter.Value && r.IsRessourceSpecifiqueCi).Get()
                                        .SelectMany(x => x.RessourceRattachement.ReferentielEtendus)
                                        .Select(x => x.NatureId).FirstOrDefault();
            }
            return natureIdRessource;
        }

        /// <summary>
        /// Récupère les ressources pour la comparaison de budget.
        /// </summary>
        /// <param name="ressourceIds">Les identifiants des ressources concernées.</param>
        /// <returns>Les ressources.</returns>
        public List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> ressourceIds)
        {
            return context.Ressources
                .Where(r => ressourceIds.Contains(r.RessourceId))
                .Select(r => new AxeInfoDao
                {
                    Id = r.RessourceId,
                    Code = r.Code,
                    Libelle = r.Libelle
                })
                .ToList();
        }

        public async Task<List<RessourceEnt>> SearchRessourcesForAchatAsync(SearchRessourcesRequestModel searchRessourcesRequestModel)
        {
            if (searchRessourcesRequestModel is null)
            {
                throw new ArgumentNullException(nameof(searchRessourcesRequestModel));
            }

            bool searchInTexts = !string.IsNullOrEmpty(searchRessourcesRequestModel.Recherche);

            bool searchInKeyWorks = !string.IsNullOrEmpty(searchRessourcesRequestModel.Recherche2);

            bool ressourceRecommandeesOnly = searchRessourcesRequestModel.RessourcesRecommandeesOnly == 1;

            var filterWithNature = searchRessourcesRequestModel.RessourceIdNatureFilter.HasValue;

            var query = context.Ressources
                            .Where(r => r.Active && r.DateSuppression == null)
                            .Where(r => r.ReferentielEtendus.Any(re => re.Achats && re.SocieteId == searchRessourcesRequestModel.SocieteId && re.NatureId != null) || (r.IsRessourceSpecifiqueCi && r.SpecifiqueCiId == searchRessourcesRequestModel.CiId));

            if (searchInTexts)
            {
                query = query.Where(r => (r.Code.Contains(searchRessourcesRequestModel.Recherche) || r.Libelle.Contains(searchRessourcesRequestModel.Recherche)));
            }
            if (searchInKeyWorks)
            {
                string searchKeyWorksFormatted = StringTool.RemoveDiacritics(searchRessourcesRequestModel.Recherche2.ToLower());
                query = query.Where(r => r.Keywords.ToLower().Contains(searchKeyWorksFormatted));
            }
            if (ressourceRecommandeesOnly)
            {
                query = query.Where(r => r.ReferentielEtendus.Any(re => re.RessourcesRecommandees.Any(rr => rr.OrganisationId == searchRessourcesRequestModel.EtablissementOrganisationId)));
            }

            if (filterWithNature)
            {
                int? natureIdRessource = null;

                if (filterWithNature)
                {
                    natureIdRessource = GetNatureIdRessource(searchRessourcesRequestModel.RessourceIdNatureFilter, searchRessourcesRequestModel.SocieteId);
                }
                query = query.Where(r => (r.ReferentielEtendus.Any(re => re.NatureId == natureIdRessource) || r.RessourceRattachement.ReferentielEtendus.Any(re => re.NatureId == natureIdRessource)));
            }

            var ressourcesIds = await query.Skip((searchRessourcesRequestModel.Page - 1) * searchRessourcesRequestModel.PageSize)
                                           .Take(searchRessourcesRequestModel.PageSize)
                                           .OrderBy(r => r.Libelle)
                                           .Select(x => x.RessourceId)
                                           .ToListAsync()
                                           .ConfigureAwait(false);

            var ressources = await context.Ressources
                                    .Include(r => r.TypeRessource)
                                    .Include(r => r.ReferentielEtendus).ThenInclude(re => re.RessourcesRecommandees)
                                    .Where(x => ressourcesIds.Contains(x.RessourceId))
                                    .ToListAsync()
                                    .ConfigureAwait(false);
            return ressources;
        }

        public Dictionary<string, int> GetOperationDiverseDefaultsByGroupe(List<string> codesSousChapitre, string codeGroupe)
        {
            return context.Ressources
                .Where(x => x.SousChapitre.Chapitre.Groupe.Code == codeGroupe && 
                            x.Code.Contains("OD") && 
                            codesSousChapitre.Contains(x.SousChapitre.Code))
                .ToDictionary(x => x.Code, x => x.RessourceId);
        }
    }
}
