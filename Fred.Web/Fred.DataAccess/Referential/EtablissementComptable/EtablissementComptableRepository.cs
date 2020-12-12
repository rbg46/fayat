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
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.EtablissementComptable
{
    /// <summary>
    ///   Référentiel de données pour les établissements comptables.
    /// </summary>
    public class EtablissementComptableRepository : FredRepository<EtablissementComptableEnt>, IEtablissementComptableRepository
    {
        private readonly IOrganisationTreeRepository organisationTreeRepository;
        private readonly ILogManager logManager;

        public EtablissementComptableRepository(FredDbContext context, IOrganisationTreeRepository organisationTreeRepository, ILogManager logManager)
          : base(context)
        {
            this.organisationTreeRepository = organisationTreeRepository;
            this.logManager = logManager;
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <exception cref="System.UnauthorizedAccessException">Utilisateur n'a pas l'autorisation</exception>
        public override void CheckAccessToEntity(EtablissementComptableEnt entity, int userId)
        {
            if (entity.Organisation == null)
            {
                PerformEagerLoading(entity, x => x.Organisation);
            }

            if (entity.Organisation != null)
            {
                var orgaList = organisationTreeRepository.GetOrganisationsAvailable(null, new List<int> { entity.Organisation.TypeOrganisationId }, userId);
                if (orgaList.All(o => o.OrganisationId != entity.Organisation.OrganisationId))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        ///   Appel à une procédure stockée
        /// </summary>
        /// <param name="etablissementComptable">L' établissement comptablet a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(EtablissementComptableEnt etablissementComptable)
        {
            try
            {
                var factureId = GetFactureIdByEtabComptId(etablissementComptable.EtablissementComptableId);

                var ciId = GetCiIdByEtabComptId(etablissementComptable.EtablissementComptableId);

                DbConnection sqlConnection = Context.Database.GetDbConnection();
                DbCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "VerificationDeDependance";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 240;

                sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_ETABLISSEMENT_COMPTABLE"));
                sqlCommand.Parameters.Add(new SqlParameter("@exclusion", "FRED_ORGANISATION|FRED_TYPE_ORGANISATION|FRED_SOCIETE|FRED_SOCIETE_DEVISE"));
                sqlCommand.Parameters.Add(new SqlParameter("@dependance", (factureId.HasValue || ciId.HasValue) ? $"'FRED_FACTURE',{factureId} | 'FRED_CI',{ciId}" : string.Empty));
                sqlCommand.Parameters.Add(new SqlParameter("@origineId", etablissementComptable.EtablissementComptableId));
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
            catch (Exception e)
            {
                throw new FredRepositoryException("Un problème est survenu pendant la vérification des dépendances des établissement comptables.", e);
            }
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes des établissements pour éviter de les prendre en compte dans la sauvegarde
        ///   du contexte.
        /// </summary>
        /// <param name="etablissementComptable">Etablissement dont les dépendances sont à attacher</param>
        private void AttachDependancies(EtablissementComptableEnt etablissementComptable)
        {
            if (etablissementComptable.Societe != null)
            {
                Context.Societes.Attach(etablissementComptable.Societe);
            }
            if (etablissementComptable.Pays != null)
            {
                Context.Pays.Attach(etablissementComptable.Pays);
            }
        }

        /// <summary>
        ///   Permet de récupérer l'id d'une facture lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement comptable</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetFactureIdByEtabComptId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.FactureARs.Where(s => s.EtablissementId == id).Select(s => s.FactureId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'id d'un CI lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement comptable</param>
        /// <returns>Retourne l'identifiant du 1er pointage anticipé</returns>
        private int? GetCiIdByEtabComptId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.CIs.Where(s => s.EtablissementComptableId == id).Select(s => s.CiId).FirstOrDefault();
        }

        /// <summary>
        ///   Retourne la liste des établissements comptables.
        /// </summary>
        /// <returns>La liste des établissements comptables.</returns>
        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptableList()
        {
            return Context.EtablissementsComptables
                          .Include(x => x.Organisation)
                          .Include(x => x.Societe)
                          .Include(x => x.Pays)
                          .AsNoTracking();
        }

        /// <summary>
        ///   Retourne l'établissement comptable portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="etablissementComptableId">Identifiant de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        public EtablissementComptableEnt GetEtablissementComptableById(int etablissementComptableId)
        {
            return Context.EtablissementsComptables.Find(etablissementComptableId);
        }

        /// <summary>
        ///   Retourne l'établissement comptable portant le code indiqué.
        /// </summary>
        /// <param name="etablissementComptableCode"> Code de l'établissement comptable à retrouver.</param>
        /// <returns>L'établissement comptable retrouvé, sinon nulle.</returns>
        public EtablissementComptableEnt GetEtablissementComptableByCode(string etablissementComptableCode)
        {
            return Context.EtablissementsComptables.FirstOrDefault(s => s.Code == etablissementComptableCode);
        }

        /// <summary>
        ///   Retourne l'identifiant de l'établissement comptable portant le code indiqué.
        /// </summary>
        /// <param name="code">Code de l'établissement comptable dont l'identifiant est à retrouver.</param>
        /// <returns>Identifiant retrouvé, sinon nulle.</returns>
        public int? GetEtablissementComptableIdByCode(string code)
        {
            int? etablissementComptableId = null;
            EtablissementComptableEnt etablissementComptable = Context.EtablissementsComptables.FirstOrDefault(s => s.Code == code);

            if (etablissementComptable != null)
            {
                etablissementComptableId = etablissementComptable.EtablissementComptableId;
            }

            return etablissementComptableId;
        }

        /// <summary>
        ///   Ajoute un nouvel établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableEnt">Etablissement comptable à ajouter</param>
        /// <returns>L'identifiant de l'établissement comptable ajouté</returns>
        public int AddEtablissementComptable(EtablissementComptableEnt etablissementComptableEnt)
        {
            try
            {
                if (Context.Entry(etablissementComptableEnt).State == EntityState.Detached)
                {
                    AttachDependancies(etablissementComptableEnt);
                }

                Context.EtablissementsComptables.Add(etablissementComptableEnt);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return etablissementComptableEnt.EtablissementComptableId;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptable">Etablissement comptable à modifier</param>
        public void UpdateEtablissementComptable(EtablissementComptableEnt etablissementComptable)
        {
            try
            {
                if (Context.Entry(etablissementComptable).State == EntityState.Detached /*&& this.Context.Entry<EtablissementComptableEnt>(etablissementComptable).State != EntityState.Unchanged*/)
                {
                    AttachDependancies(etablissementComptable);
                }

                Context.Entry(etablissementComptable).State = EntityState.Modified;

                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Supprime un établissement comptable.
        /// </summary>
        /// <param name="etablissementComptableId">L'identifiant du établissement comptable à supprimer</param>
        public void DeleteEtablissementComptableById(int etablissementComptableId)
        {
            EtablissementComptableEnt etablissement = Context.EtablissementsComptables.Find(etablissementComptableId);
            if (etablissement == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            ////etablissement.IsDeleted = true;
            ////etablissement.DateSuppression = DateTime.UtcNow;
            Context.EtablissementsComptables.Remove(etablissement);

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Vérifie qu'un établissement comptable peut être supprimé
        /// </summary>
        /// <param name="etablissementComptable">L' établissement comptable à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(EtablissementComptableEnt etablissementComptable)
        {
            return AppelTraitementSqlVerificationDesDependances(etablissementComptable);
        }

        /// <summary>
        ///   Permet de récupérer l'id de l'organisation liée à la établissement spécifié.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement</param>
        /// <returns>Retourne l'identifiant de l'organisation</returns>
        public int? GetOrganisationIdByEtablissementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.EtablissementsComptables.Where(e => e.EtablissementComptableId == id).Select(s => s.Organisation.OrganisationId).FirstOrDefault();
        }

        /// <summary>
        ///   Permet de récupérer l'organisation liée à la établissement spécifié.
        /// </summary>
        /// <param name="id">Identifiant de l'établissement</param>
        /// <returns>Retourne l'organisation</returns>
        public OrganisationEnt GetOrganisationByEtablissementId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.EtablissementsComptables.Where(e => e.EtablissementComptableId == id).Select(s => s.Organisation).FirstOrDefault();
        }

        /// <summary>
        ///   Etablissement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de EtablissementCompteble</returns>
        public IEnumerable<EtablissementComptableEnt> GetListBySocieteId(int societeId)
        {
            return Context.EtablissementsComptables.Where(x => x.SocieteId == societeId).OrderBy(x => x.Code);
        }

        /// <summary>
        ///   Permet de connaître l'existence d'une établissement comptable depuis son code.
        /// </summary>
        /// <param name="idCourant"> id courant</param>
        /// <param name="codeEtablissementComptable"> code établissement comptable</param>
        /// <param name="societeId"> Id de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsEtablissementComptableExistsByCode(int idCourant, string codeEtablissementComptable, int societeId)
        {
            if (societeId > 0)
            {
                if (idCourant != 0)
                {
                    return Context.EtablissementsComptables.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeEtablissementComptable && s.EtablissementComptableId != idCourant);
                }

                return Context.EtablissementsComptables.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeEtablissementComptable);
            }

            return false;
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        public IEnumerable<EtablissementComptableEnt> SearchListAllWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate)
        {
            return Context.EtablissementsComptables.Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        ///   Permet de récupérer la liste de tous les établissements compltables en fonction des critères de recherche par
        ///   société.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les établissements compltables</param>
        /// <param name="societeId">Id de la societe</param>
        /// <returns>Retourne la liste filtrée de tous les établissements compltables</returns>
        public IEnumerable<EtablissementComptableEnt> SearchEtablissementComptableAllBySocieteIdWithFilters(Expression<Func<EtablissementComptableEnt, bool>> predicate, int societeId)
        {
            return Context.EtablissementsComptables.Include(x => x.Pays).Where(e => e.SocieteId == societeId).Where(predicate).OrderBy(s => s.Code);
        }

        /// <summary>
        /// Retourne la société parente de l'organisation passée en paramètre.
        /// </summary>
        /// <param name="organisationId">L'identifiant de l'organisation.</param>
        /// <returns>La societé parente de l'organisation passée en paramètre ou null si aucune société trouvée.</returns>
        /// <remarks>La fonction GetSocieteParentByOrgaId semble ne pas fonctionner.</remarks>
        public EtablissementComptableEnt GetEtablissementComptableByOrganisationIdEx(int organisationId)
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
                if (organisation.TypeOrganisation.Code == Constantes.OrganisationType.CodeEtablissement)
                {
                    var etablissements = Context.EtablissementsComptables.Where(s => s.Organisation.OrganisationId == organisation.OrganisationId);
                    return etablissements.Count() == 1
                      ? etablissements.First()
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
        /// Permet d'obtenir la liste des établissements de GFES
        /// </summary>
        /// <returns>liste des étblissements GFES</returns>
        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptableGFESList()
        {
            return Context.EtablissementsComptables.Where(e => Constantes.CodeGroupeFES.Equals(e.Societe.Groupe.Code));
        }

        /// <summary>
        /// Get etablissement comptable list by organisation list id
        /// </summary>
        /// <param name="organisationIdList">Organisations ids list</param>
        /// <returns>List des etablissements comptables</returns>
        public async Task<List<EtablissementComptableEnt>> GetEtabComptableListByOrganisationListId(IEnumerable<int> organisationIdList)
        {
            return await Context.EtablissementsComptables.Where(x => organisationIdList.Contains(x.OrganisationId)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get etablissement comptable list by organisation pere id
        /// </summary>
        /// <param name="organisationPereIdList">Organisation pere id list</param>
        /// <returns>List des etablissements comptables</returns>
        public async Task<List<EtablissementComptableEnt>> GetEtabComptableListByOrganisationPereIdList(IEnumerable<int> organisationPereIdList)
        {
            return await Context.EtablissementsComptables.Where(x => organisationPereIdList.Contains(x.Organisation.PereId.Value)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Permet d'obtenir la liste des établissements par organisation id
        /// </summary>
        /// <param name="organisationIds">Liste des identifiants</param>
        /// <returns>liste des étblissements</returns>
        public IEnumerable<EtablissementComptableEnt> GetEtablissementComptableByOrganisationIds(List<int> organisationIds)
        {
            return Context.EtablissementsComptables
                .Include(x => x.EtablissementsPaie)
                .Where(x => organisationIds.Contains(x.OrganisationId));
        }
    }
}
