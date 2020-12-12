using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Societe
{
    public class SocieteRepository : FredRepository<SocieteEnt>, ISocieteRepository
    {
        private readonly IOrganisationRepository userRepo;
        private readonly ILogManager logManager;

        public SocieteRepository(FredDbContext context, IOrganisationRepository userRepo, ILogManager logManager)
          : base(context)
        {
            this.userRepo = userRepo;
            this.logManager = logManager;
        }

        /// <summary>
        /// Appel à une procédure stockée
        /// </summary>
        /// <param name="societeId">Identifiant Societe a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int societeId)
        {
            int? organisationId = GetOrganisationIdBySocieteId(societeId);

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_SOCIETE"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", "FRED_SOCIETE_DEVISE|FRED_CODE_ABSENCE|FRED_ORGANISATION|FRED_TYPE_ORGANISATION|FRED_SOCIETE"));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", organisationId.HasValue ? $"'FRED_ORGANISATION',{organisationId}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", societeId));
            sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
            sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            int nbcmd = (int)sqlCommand.Parameters["@resu"].Value;

            if (nbcmd == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Vérifie si un groupe ne possède pas déjà une société intérimaire
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Vrai si le groupe possède une société intérimaire, sinon faux</returns>
        public bool FindSocieteInterimaire(int groupeId)
        {
            return Context.Societes != null && Context.Societes.Any(x => x.GroupeId == groupeId && x.IsInterimaire);
        }

        /// <summary>
        /// Appel à une procédure stockée
        /// </summary>
        /// <param name="societeId">Societe a supprimer</param>
        protected void AppelTraitementSuppressionSociete(int societeId)
        {
            if (societeId <= 0)
            {
                throw new ArgumentNullException("societeId");
            }

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            sqlConnection.Open();

            using (DbTransaction transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    DbCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "deleteSociete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 240;

                    sqlCommand.Transaction = transaction;
                    sqlCommand.Parameters.Add(new SqlParameter("@societeId", societeId));

                    sqlCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    this.logManager.TraceException(exception.Message, exception);
                    throw;
                }
            }

            sqlConnection.Close();
        }

        /// <summary>
        /// Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <exception cref="System.UnauthorizedAccessException">Utilisateur n'a pas l'autorisation</exception>
        public override void CheckAccessToEntity(SocieteEnt entity, int userId)
        {
            if (entity.Organisation == null)
            {
                PerformEagerLoading(entity, x => x.Organisation);
            }
            if (entity.Organisation != null)
            {
                var orgaList = userRepo.GetOrganisationsAvailable(null, new List<int> { entity.Organisation.TypeOrganisationId }, userId);
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        /// Retourne la liste de toutes les sociétés.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<SocieteEnt> GetSocieteListAll()
        {
            foreach (SocieteEnt societe in Context.Societes.OrderBy(s => s.Code).Include(o => o.Organisation).Include(g => g.Groupe).AsNoTracking())
            {
                societe.Organisation.Societe = null;
                yield return societe;
            }
        }

        /// <summary>
        /// Retourne la liste de toutes les sociétés pour la synchronisation mobile.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        public IEnumerable<SocieteEnt> GetSocieteListAllSync()
        {
            return Context.Societes.AsNoTracking();
        }

        /// <summary>
        /// Retourne la liste des sociétés actives.
        /// </summary>
        /// <returns>Liste des sociétés.</returns>
        public IEnumerable<SocieteEnt> GetSocieteList()
        {
            IEnumerable<SocieteEnt> listSociete = Context.Societes
                                                         .Include(g => g.Groupe)
                                                         .Include(o => o.Organisation)
                                                         .Where(s => s.Active)
                                                         .OrderBy(s => s.Code)
                                                         .AsNoTracking();

            foreach (SocieteEnt societe in listSociete)
            {
                societe.Organisation.Societe = null;
                yield return societe;
            }
        }

        /// <summary>
        /// Permet de connaître l'existence d'une société depuis son code ou libelle.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeSociete"> code societe</param>
        /// <param name="libelle">libelle</param>
        /// <returns>Retourne la SocieteEnt qui est identique</returns>
        public SocieteEnt GetSocieteWithSameCodeOrLibelle(int idCourant, string codeSociete, string libelle)
        {

            if (idCourant != 0)
            {
                return Context.Societes.FirstOrDefault(s => s.SocieteId != idCourant && (s.Code.ToLower() == codeSociete || s.Libelle.ToLower() == libelle));
            }

            return Context.Societes.FirstOrDefault(s => s.Code.ToLower() == codeSociete || s.Libelle.ToLower() == libelle);
        }

        /// <summary>
        /// Retourne la société portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <param name="includeGroupe">Si vrai, on charge le groupe de la société</param>
        /// <returns>la société retrouvée, sinon nulle.</returns>
        public SocieteEnt GetSocieteById(int societeId, bool includeGroupe)
        {
            if (includeGroupe)
            {
                return Context.Societes.Include(s => s.Groupe).SingleOrDefault(s => s.SocieteId == societeId);
            }
            else
            {
                return Context.Societes.Find(societeId);
            }
        }

        /// <summary>
        /// Retourne la société portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="societeId">Identifiant de la société à retrouver.</param>
        /// <returns>la société retrouvée, sinon nulle.</returns>
        public SocieteEnt GetSocieteWithOrganisation(int societeId)
        {
            return Context.Societes.Include("Organisation").FirstOrDefault(s => s.SocieteId == societeId);
        }

        /// <summary>
        /// Retourne l'identifiant de la société portant le code société comptable indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon null.</returns>
        public int? GetSocieteIdByCodeSocieteComptable(string code)
        {
            int? societeId = null;
            SocieteEnt societe = Context.Societes.FirstOrDefault(s => s.CodeSocieteComptable == code);

            if (societe != null)
            {
                societeId = societe.SocieteId;
            }

            return societeId;
        }

        /// <summary>
        /// Retourne la liste des indentifiant des société pour un identifiant de groupe
        /// </summary>
        /// <param name="groupeId">identifiant de groupe</param>
        /// <returns> la liste des indentifiant des société</returns>
        public IReadOnlyList<int> GetSocieteIdsByGroupeId(int groupeId)
        {
            return Context.Societes.Where(s => s.GroupeId == groupeId).Select(s => s.SocieteId).ToList();
        }

        public async Task<int?> GetSocieteImageLogoIdByOrganisationIdAsync(int organisationId)
        {
            return await Context.Societes
                .Where(s => s.OrganisationId == organisationId)
                .Select(s => s.ImageLogoId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        public SocieteEnt GetSocieteByCodeSocieteComptable(string code)
        {
            return Context.Societes.Include(s => s.Organisation).FirstOrDefault(s => s.CodeSocieteComptable == code);
        }

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        public SocieteEnt GetSocieteByCodeSocietePaye(string code)
        {
            return Context.Societes.FirstOrDefault(s => s.CodeSocietePaye == code);
        }

        /// <summary>
        /// Retourne l'identifiant de la société portant le code indiqué.
        /// </summary>
        /// <param name="id">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        public string GetCodeSocietePayeById(int id)
        {
            string codeSociete = string.Empty;
            SocieteEnt societe = Context.Societes.FirstOrDefault(s => s.SocieteId == id);

            if (societe != null)
            {
                codeSociete = societe.CodeSocietePaye;
            }

            return codeSociete;
        }

        /// <summary>
        /// Ajout une nouvelle société
        /// </summary>
        /// <param name="societe"> société à ajouter</param>
        /// <returns>Société ajoutée</returns>
        public SocieteEnt AddSociete(SocieteEnt societe)
        {
            Insert(societe);

            return societe;
        }

        /// <summary>
        /// Sauvegarde des modifications d'un société
        /// </summary>
        /// <param name="societe">société à modifier</param>
        /// <returns>Société mise à jour</returns>
        public SocieteEnt UpdateSociete(SocieteEnt societe)
        {
            if (societe.MoisDebutExercice.HasValue && societe.MoisDebutExercice.Value == 0)
            {
                societe.MoisDebutExercice = null;
            }
            if (societe.MoisFinExercice.HasValue && societe.MoisFinExercice.Value == 0)
            {
                societe.MoisFinExercice = null;
            }

            Update(societe);

            return societe;
        }

        /// <summary>
        /// Supprime une société
        /// </summary>
        /// <param name="id">L'identifiant de la société à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        public bool DeleteSocieteById(int id)
        {
            try
            {
                AppelTraitementSuppressionSociete(id);
                return true;
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                return false;
            }
        }

        /// <summary>
        /// Vérifie qu'une societe peut être supprimée
        /// </summary>
        /// <param name="societeId">L'identifiant de la société à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(int societeId)
        {
            return AppelTraitementSqlVerificationDesDependances(societeId);
        }

        /// <summary>
        /// Retourne l'identifiant de la société portant le code indiqué.
        /// </summary>
        /// <param name="codeSociete">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>identifiant retrouvé, sinon nulle.</returns>
        public int? GetSocieteIdByCodeSocietePaye(string codeSociete)
        {
            int? societeId = null;
            SocieteEnt societe = Context.Societes.FirstOrDefault(s => s.CodeSocietePaye == codeSociete);

            if (societe != null)
            {
                societeId = societe.SocieteId;
            }

            return societeId;
        }

        /// <summary>
        /// Permet de récupérer la liste de toutes les sociétés en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur toutes les sociétés</param>
        /// <returns>Retourne la liste filtré de toutes les sociétés</returns>
        public IEnumerable<SocieteEnt> SearchSocieteAllWithFilters(Expression<Func<SocieteEnt, bool>> predicate)
        {
            return Query()
                   .Include(x => x.TypeSociete)
                   .Include(x => x.Classification)
                   .Filter(predicate)
                   .OrderBy(list => list.OrderBy(s => s.Code))
                   .Get();
        }

        /// <summary>
        /// Permet de récupérer l'id de l'organisation liée à la société spécifiée.
        /// </summary>
        /// <param name="id">Identifiant de la société</param>
        /// <returns>Retourne l'identifiant de l'organisation</returns>
        public int? GetOrganisationIdBySocieteId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.Societes.Where(s => s.SocieteId == id).Select(s => s.Organisation.OrganisationId).FirstOrDefault();
        }

        /// <summary>
        /// Retourne la liste des sociétés dont les factures doivent être importées
        /// </summary>
        /// <returns>La liste des sociétés dontles factures doivent être importées.</returns>
        public IEnumerable<SocieteEnt> GetListSocieteToImportFacture()
        {
            return Context.Societes.Where(s => s.ImportFacture);
        }

        /// <summary>
        /// Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        public IEnumerable<SocieteEnt> SearchLight(string text)
        {
            var query = Query()
              .Filter(c => string.IsNullOrEmpty(text) || c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()))
              .OrderBy(list => list.OrderBy(s => s.Code))
              .Get();

            return query;
        }

        /// <summary>
        /// Retourne la société parent de l'organisation passé en paramètre
        /// </summary>
        /// <param name="organisationId">ID du de l'organisation</param>
        /// <returns>societe parent de l'organisation passé en paramètre</returns>
        public SocieteEnt GetSocieteParentByOrgaId(int organisationId)
        {
            OrganisationEnt organisation = Context.Organisations
                      .Where(o => o.OrganisationId == organisationId)
                      .Include(o => o.TypeOrganisation)
                      .SingleOrDefault();

            if (organisation != null)
            {
                if (organisation.TypeOrganisation.Code != Constantes.OrganisationType.CodeSociete)
                {
                    if (organisation.PereId != null)
                    {
                        return GetSocieteParentByOrgaId(organisation.PereId.Value);
                    }
                }
                else
                {
                    return
                      Query()
                        .Include(o => o.Organisation)
                        .Include(s => s.AssocieSeps.Select(a => a.TypeParticipationSep))
                        .Include(s => s.AssocieSeps.Select(a => a.SocieteAssociee))
                        .Filter(s => s.Organisation.OrganisationId.Equals(organisation.OrganisationId))
                        .Get()
                        .SingleOrDefault();
                }
            }

            return null;
        }

        /// <summary>
        /// Retourne la société parente de l'organisation passée en paramètre.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <param name="withIncludeTypeSociete">Indique si on veux charger le type de la société</param>
        /// <returns>La societé parente de l'organisation passée en paramètre ou null si aucune société trouvée.</returns>
        /// <remarks>La fonction GetSocieteParentByOrgaId semble ne pas fonctionner.</remarks>
        public SocieteEnt GetSocieteParentByOrgaIdEx(int organisationId, bool withIncludeTypeSociete)
        {
            var processed = new List<int>();
            while (organisationId != 0)
            {
                if (processed.Contains(organisationId))
                {
                    // Evite les boucles infines
                    return null;
                }
                processed.Add(organisationId);

                var organisations = Context.Organisations
                  .Include(o => o.TypeOrganisation)
                  .Where(o => o.OrganisationId == organisationId);

                if (organisations.Count() != 1)
                {
                    return null;
                }
                var organisation = organisations.First();
                if (organisation.TypeOrganisation.Code == Constantes.OrganisationType.CodeSociete)
                {
                    var societes = Context.Societes.Where(s => s.Organisation.OrganisationId == organisation.OrganisationId);
                    if (withIncludeTypeSociete)
                    {
                        societes = societes.Include(s => s.TypeSociete);
                    }
                    return societes.Count() == 1
                      ? societes.First()
                      : null;
                }
                else if (organisation.PereId != null)
                {
                    organisationId = organisation.PereId.Value;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Retourne la liste des devises de la societe
        /// </summary>
        /// <param name="societeId">La société</param>
        /// <returns>la liste des devises de la societe</returns>
        public IEnumerable<DeviseEnt> GetListDeviseBySocieteId(int societeId)
        {
            var socDevises = Context.SocietesDevises.Where(s => s.SocieteId.Equals(societeId)).Include(s => s.Devise).ToList();

            foreach (SocieteDeviseEnt socDevise in socDevises)
            {
                socDevise.Devise.Reference = socDevise.DeviseDeReference;
            }

            foreach (DeviseEnt devise in socDevises.Select(d => d.Devise))
            {
                yield return devise;
            }
        }

        /// <summary>
        /// Retourne la devise de référence de la societe
        /// </summary>
        /// <param name="societeId">La société</param>
        /// <returns>la devise de référence de la société</returns>
        public DeviseEnt GetDeviseRefBySocieteId(int societeId)
        {
            return Context.SocietesDevises.Where(s => s.SocieteId.Equals(societeId) && s.DeviseDeReference).Select(d => d.Devise).SingleOrDefault();
        }

        /// <summary>
        /// Récupère la société en fonction de l'identifiant de son organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <returns>Société</returns>
        public SocieteEnt GetSocieteByOrganisationId(int organisationId)
        {
            var societe = this.Query()
                       .Include(x => x.Organisation)
                       .Get()
                       .FirstOrDefault(x => x.Organisation.OrganisationId == organisationId);

            if (societe?.Organisation != null)
            {
                societe.Organisation.Societe = null;
            }
            return societe;
        }

        /// <summary>
        /// Retourne La société en fonction de son code et de son identifiant unique de groupe 
        /// </summary>
        /// <param name="code">Code de la societe</param>
        /// <param name="groupeId">Indentifiant unique du groupe</param>
        /// <returns>la société</returns>
        public SocieteEnt GetSocieteByCodeAndGroupeId(string code, int groupeId)
        {
            return Query()
                .Filter(s => s.Code == code && s.GroupeId == groupeId)
                .Get()
                .FirstOrDefault();
        }

        /// <summary>
        /// Retourner la requête de récupération sociétés
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public List<SocieteEnt> Search(List<Expression<Func<SocieteEnt, bool>>> filters,
                                                Func<IQueryable<SocieteEnt>, IOrderedQueryable<SocieteEnt>> orderBy = null,
                                                List<Expression<Func<SocieteEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).AsNoTracking().ToList();
            }
            else
            {
                return Get(filters, orderBy, includeProperties, page, pageSize).ToList();
            }

        }

        /// <summary>
        /// Retourne vrai si la société appartient au groupe passé en paramètre
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="codeGroupe">Code du groupe</param>
        /// <returns>Vrai si la société appartient au groupe</returns>
        public bool IsSocieteIdInGroupe(int societeId, string codeGroupe)
        {
            return Query()
                    .Filter(s => s.SocieteId == societeId && s.Groupe.Code == codeGroupe)
                    .Get()
                    .Any();
        }

        public async Task<Dictionary<string, int>> GetCompanyIdsByPayrollCompanyCodesAsync()
        {
            return await Context.Societes.Where(t => !string.IsNullOrEmpty(t.CodeSocietePaye)).ToDictionaryAsync(t => t.CodeSocietePaye, t => t.SocieteId);
        }

        /// <summary>
        /// Retourne l'objet société portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de la société dont l'identifiant est à retrouver.</param>
        /// <returns>société retrouvé, sinon nulle.</returns>
        public SocieteEnt GetSocieteByCodeSocieteComptables(string code)
        {
            return Context.Societes
                .Include(s => s.Organisation)
                .FirstOrDefault(s => s.CodeSocieteComptable == code && s.Active);
        }

        /// <summary>
        /// Récupére la liste des Sociétés par SIREN  et coupe code
        /// </summary>
        /// <param name="siren">SIREN société</param>
        /// <param name="groupeCode">Groupe code</param>
        /// <returns>List de société</returns>
        public List<SocieteEnt> GetSocieteBySirenAndGroupeCode(string siren, string groupeCode)
        {
            return Context.Societes.Where(x => x.SIREN == siren && x.Groupe.Code == groupeCode).ToList();
        }

        /// <summary>
        /// Obtenir la liste des sociétés par organisation
        /// </summary>
        /// <param name="organisationIds">List des organisations ids</param>
        /// <returns>Liste des sociétés</returns>
        public IEnumerable<SocieteEnt> GetSocieteListByOrganisationIds(List<int> organisationIds)
        {
            return Context.Societes.Where(x => organisationIds.Contains(x.OrganisationId));
        }

        public async Task<string> GetCodeSocieteComptableByCodeAsync(string code)
        {
            return await Context.Societes
                .Where(s => s.Code == code)
                .Select(x => x.CodeSocieteComptable)
                .SingleAsync()
                .ConfigureAwait(false);
        }

        public async Task<string> GetCodeSocieteComptableByIdAsync(int id)
        {
            return await Context.Societes
                .Where(s => s.SocieteId == id)
                .Select(x => x.CodeSocieteComptable)
                .SingleAsync()
                .ConfigureAwait(false);
        }

        public int GetSepGerantSocieteId(int ciId)
        {
            return Context.CIs
                .Where(ci => ci.CiId == ciId && ci.Societe.TypeSociete.Code == TypeSociete.Sep)
                .SelectMany(ci => ci.Societe.AssocieSeps)
                .Where(x => x.TypeParticipationSep.Code == TypeParticipationSep.Gerant && !x.AssocieSepParentId.HasValue)
                .Select(s => s.SocieteAssocieeId)
                .FirstOrDefault();
        }
    }
}
