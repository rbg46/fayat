using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Organisation
{
    /// <summary>
    ///   Référentiel de données pour les organisations.
    /// </summary>
    public class OrganisationRepository : FredRepository<OrganisationEnt>, IOrganisationRepository
    {
        private readonly IOrganisationTreeRepository organisationTreeRepository;
        private readonly ILogManager logManager;

        public OrganisationRepository(FredDbContext context, IOrganisationTreeRepository organisationTreeRepository, ILogManager logManager)
          : base(context)
        {
            this.organisationTreeRepository = organisationTreeRepository;
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne la liste des organisations.
        /// </summary>
        /// <returns>Liste des organisations.</returns>
        public IEnumerable<OrganisationEnt> GetList()
        {
            foreach (OrganisationEnt organisation in Context.Organisations)
            {
                organisation.ClearCircularReference();

                yield return organisation;
            }
        }


        /// <summary>
        ///   Retourne le organisation dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation à retrouver.</param>
        /// <returns>le organisation retrouvé, sinon nulle.</returns>
        public OrganisationEnt GetOrganisationById(int organisationId)
        {
            OrganisationEnt organisation = Context.Organisations
                                                  .Include(a => a.AffectationsSeuilRoleOrga).ThenInclude(r => r.Role)
                                                  .Include(a => a.AffectationsSeuilRoleOrga).ThenInclude(d => d.Devise)
                                                  .Include(o => o.CI)
                                                  .Include(o => o.Etablissement)
                                                  .Include(o => o.Societe)
                                                  .Include(o => o.OrganisationGenerique)
                                                  .Include(o => o.Groupe)
                                                  .Include(o => o.Pole)
                                                  .Include(o => o.Holding)
                                                  .Include(o => o.TypeOrganisation)
                                                  .Include(o => o.Pere.TypeOrganisation)
                                                  .AsNoTracking()
                                                  .FirstOrDefault(o => o.OrganisationId == organisationId);
            if (organisation == null)
            {
                return null;
            }
            organisation.ClearCircularReference();

            return organisation;
        }

        /// <summary>
        ///   Retourne les seuils de commande définit pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation à retrouver.</param>
        /// <param name="roleId">Identifiant du rôle à retrouver.</param>
        /// <returns>liste des seuils de l'organisation choisi, sinon nulle.</returns>
        public IEnumerable<AffectationSeuilOrgaEnt> GetSeuilByOrganisationId(int organisationId, int? roleId)
        {
            var query = Context.SeuilRoleOrgas
                                  .Include(d => d.Devise)
                                  .Include(r => r.Role)
                                  .Where(o => o.OrganisationId == organisationId);

            if (roleId.HasValue)
            {
                query = query.Where(r => r.RoleId == roleId);
            }

            return query.ToList();
        }

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pereId"> identifiant de l'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        public OrganisationEnt GenerateOrganisation(string codeOrganisation, int? pereId)
        {
            try
            {
                int id = GetTypeOrganisationIdByCode(codeOrganisation);
                return new OrganisationEnt { TypeOrganisationId = id, PereId = pereId };
            }
            catch (Exception ex)
            {
                this.logManager.TraceException(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        ///   Retourne une nouvelle organisation
        /// </summary>
        /// <param name="codeOrganisation"> code du type d'organisation</param>
        /// <param name="pere">l'organisation du parent</param>
        /// <returns> L'organisation générée</returns>
        public OrganisationEnt GenerateOrganisation(string codeOrganisation, OrganisationEnt pere)
        {
            try
            {
                int id = GetTypeOrganisationIdByCode(codeOrganisation);
                return new OrganisationEnt { TypeOrganisationId = id, Pere = pere };
            }
            catch (Exception ex)
            {
                this.logManager.TraceException(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        ///   Sauvegarde les modifications d'une organisation.
        /// </summary>
        /// <param name="organisationEnt">l'organisation à modifier</param>
        /// <param name="pereId">identifiant du père à modifier</param>
        /// <returns>Organisation crée</returns>
        public OrganisationEnt UpdateOrganisation(OrganisationEnt organisationEnt, int? pereId)
        {
            try
            {
                organisationEnt.PereId = pereId;

                Context.Entry(organisationEnt).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            return organisationEnt;
        }

        /// <summary>
        ///   Supprime une organisation
        /// </summary>
        /// <param name="id">L'identifiant de l'organisation à supprimer</param>
        public void DeleteOrganisationById(int id)
        {
            OrganisationEnt organisation = Context.Organisations.Find(id);
            if (organisation == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Organisations.Remove(organisation);

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

        /// <inheritdoc />
        public AffectationSeuilOrgaEnt AddOrganisationThreshold(AffectationSeuilOrgaEnt threshold)
        {
            if (threshold.Devise != null)
            {
                var devise = Context.Devise.Find(threshold.DeviseId);
                threshold.Devise = devise;
            }
            if (threshold.Role != null)
            {
                var role = Context.Roles.Find(threshold.RoleId);
                threshold.Role = role;
            }

            Context.SeuilRoleOrgas.Add(threshold);

            return threshold;
        }

        /// <inheritdoc />
        public AffectationSeuilOrgaEnt UpdateThresholdOrganisation(AffectationSeuilOrgaEnt threshold)
        {
            if (threshold.Devise != null)
            {
                Context.Devise.Attach(threshold.Devise);
            }
            if (threshold.Role != null)
            {
                Context.Roles.Attach(threshold.Role);
            }
            Context.SeuilRoleOrgas.Update(threshold);

            return threshold;
        }

        /// <summary>
        ///   Supprimer une surcharge de devise
        /// </summary>
        /// <param name="thresholdOrganisationId">Identifiant de la surcharge à supprimer</param>
        public void DeleteThresholdOrganisationById(int thresholdOrganisationId)
        {
            try
            {
                AffectationSeuilOrgaEnt tmp = Context.SeuilRoleOrgas.First(x => x.SeuilRoleOrgaId == thresholdOrganisationId);
                Context.SeuilRoleOrgas.Remove(tmp);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Retourne la liste des Types d'organisation.
        /// </summary>
        /// <returns>Liste des types d'organisation.</returns>
        public IEnumerable<TypeOrganisationEnt> GetOrganisationTypesList()
        {
            foreach (TypeOrganisationEnt typeOrganisation in Context.OrganisationTypes)
            {
                yield return typeOrganisation;
            }
        }

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué.
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        [Obsolete("Prefer to use " + nameof(GetTypeOrganisationIdByCodeAsync) + " instead")]
        public int GetTypeOrganisationIdByCode(string codeTypeOrganisation)
        {
            return Context.OrganisationTypes.AsNoTracking().Single(o => o.Code == codeTypeOrganisation).TypeOrganisationId;
        }

        /// <summary>
        ///   Retourne l'identifiant du type d'organisation portant le code indiqué de manière async.
        /// </summary>
        /// <param name="codeTypeOrganisation">Code du type d'organisation.</param>
        /// <returns>l'id du type d'organisation retrouvé</returns>
        public async Task<int> GetTypeOrganisationIdByCodeAsync(string codeTypeOrganisation)
        {
            return await Context.OrganisationTypes
                .Where(o => o.Code == codeTypeOrganisation)
                .AsNoTracking()
                .Select(o => o.TypeOrganisationId)
                .SingleAsync();
        }



        /// <summary>
        ///   Charger des propriétés liées de l'entité.
        /// </summary>
        /// <param name="entity">l'entité à compléter.</param>
        /// <param name="expressions">Expressions des propriétés à inclure.</param>
        public override void PerformEagerLoading(OrganisationEnt entity, params Expression<Func<OrganisationEnt, object>>[] expressions)
        {
            base.PerformEagerLoading(entity, expressions);

            if (entity != null)
            {
                entity.ClearCircularReference();
            }
        }

        /// <summary>
        ///   Récupère la liste des organisations parentes
        /// </summary>
        /// <param name="organisationEnfantId">Identifiant de l'organisation fille</param>
        /// <param name="orgaTypeIdToStop">Identifiant du type d'organisation parent où l'on doit stopper la recherche</param>
        /// <param name="organisationParentListe">Liste des organisations fille</param>
        /// <returns>Une liste d'organisation</returns>
        [Obsolete("Methode recursive, ne plus utiliser. Utiliser le OrganisationTreeService ou OrganisationTreeRepository à la place")]
        public List<OrganisationEnt> GetOrganisationParentList(int organisationEnfantId, int? orgaTypeIdToStop = null, List<OrganisationEnt> organisationParentListe = null)
        {
            if (organisationParentListe == null)
            {
                organisationParentListe = new List<OrganisationEnt>();
            }

            var organisation = this.GetOrganisationById(organisationEnfantId);
            if (organisation != null)
            {
                organisationParentListe.Add(organisation);

                if (organisation.PereId.HasValue && organisation.TypeOrganisationId != orgaTypeIdToStop)
                {
                    GetOrganisationParentList(organisation.PereId.Value, orgaTypeIdToStop, organisationParentListe);
                }
            }
            return organisationParentListe;
        }

        /// <summary>
        /// Recupere la liste des organisation parentes avec l'organisation passé en parametre.
        /// La liste retournée commence par l'organisation dont on passe l'id en parametre et continue en remontant l'arbre.
        /// </summary>
        /// <param name="organisationEnfantId">organisationEnfantId</param>
        /// <returns>Liste d'organisation</returns>
        public List<OrganisationEnt> GetOrganisationParentsWithCurrent(int organisationEnfantId)
        {
            List<OrganisationEnt> organisationParentListe = new List<OrganisationEnt>();
            OrganisationEnt organisation = this.FindById(organisationEnfantId);
            organisationParentListe.Add(organisation);
            while (organisation.PereId.HasValue)
            {
                organisation = this.FindById(organisation.PereId);
                organisationParentListe.Add(organisation);
            }
            return organisationParentListe;
        }

        /// <summary>
        ///   Renvoi la liste des organisations d'un Utilisateur
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="types">Types d'organisation</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="organisationIdPere">Identifiant de l'organisation parente</param>
        /// <returns>Une liste d'organisations</returns>
        [Obsolete("Utiliser le OrganisationTreeService ou OrganisationTreeRepository à la place")]
        public IEnumerable<OrganisationLightEnt> GetOrganisationsAvailable(string text = null, List<int> types = null, int? utilisateurId = null, int? organisationIdPere = null)
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var date = DateTimeOffset.Now;

            var list = this.organisationTreeRepository.GetOrganisationsAvailable(text, types, utilisateurId, organisationIdPere).OrderBy(x => x.OrganisationId);

            timer.Stop();

            // Sending elapsed duration to AppInsight
            string filter = $"Text : {text}, utilisateurId : {utilisateurId}, organsationIdPere : {organisationIdPere}";
            new TelemetryClient().TrackDependency("SQL", "GetOrganisationWithTree", filter, date, timer.Elapsed.Duration(), true);

            return list;
        }

        /// <summary>
        /// Retourne l'identifiant de l'organisation parente demandée.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <param name="organisationParentType">Le type d'organisation parente.</param>
        /// <returns>L'identifiant de l'organisation parente.</returns>
        public int? GetParentId(int organisationId, OrganisationType organisationParentType)
        {
            var organisation = Get()
                .Where(o => o.OrganisationId == organisationId)
                .Select(o => new { o.PereId, o.TypeOrganisationId })
                .FirstOrDefault();

            if (organisation == null)
            {
                return null;
            }
            else if (organisation.TypeOrganisationId == (int)organisationParentType)
            {
                return organisationId;
            }
            else if (organisation.PereId.HasValue)
            {
                return GetParentId(organisation.PereId.Value, organisationParentType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retourne l'identifiant d'une société en fonction de l'identifiant de l'organisation d'un de ses enfants.
        /// </summary>
        /// <param name="childOrganisationId">L'identifiant de l'organisation d'un enfant de la société.</param>
        /// <returns>L'identifiant de la société ou null si non trouvé.</returns>
        public int? GetSocieteId(int childOrganisationId)
        {
            var societeOrganisationId = GetParentId(childOrganisationId, OrganisationType.Societe);
            var organisation = Get()
                .Where(o => o.OrganisationId == societeOrganisationId)
                .Select(o => new
                {
                    o.Societe.SocieteId,
                    AssocieSeps = o.Societe.AssocieSeps.Select(a => new { a.SocieteAssocieeId, a.AssocieSepParentId, a.TypeParticipationSep })
                })
                .FirstOrDefault();
            if (organisation.AssocieSeps.Any())
            {
                return organisation.AssocieSeps.SingleOrDefault(x => x.TypeParticipationSep.Code == Constantes.TypeParticipationSep.Gerant && x.AssocieSepParentId == null).SocieteAssocieeId;
            }
            return organisation?.SocieteId;
        }
    }
}
