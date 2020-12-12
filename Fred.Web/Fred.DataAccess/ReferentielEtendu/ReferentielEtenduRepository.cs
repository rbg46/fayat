using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.ReferentielEtendu
{
    /// <summary>
    ///   Référentiel de données pour les ressources
    /// </summary>
    public class ReferentielEtenduRepository : FredRepository<ReferentielEtenduEnt>, IReferentielEtenduRepository
    {
        private readonly IOrganisationRepository organisationRepo;
        private readonly IUniteReferentielEtenduRepository uniteReferentielEtenduRepo;
        private readonly ILogManager logManager;

        public ReferentielEtenduRepository(
            FredDbContext context,
            IOrganisationRepository organisationRepo,
            IUniteReferentielEtenduRepository uniteReferentielEtenduRepo,
            ILogManager logManager)
          : base(context)
        {
            this.organisationRepo = organisationRepo;
            this.uniteReferentielEtenduRepo = uniteReferentielEtenduRepo;
            this.logManager = logManager;
        }

        /// <inheritdoc />
        public ReferentielEtenduEnt GetReferentielEtenduByRessourceAndSociete(int idRessource, int idSociete, bool withInclude = false)
        {
            if (withInclude)
            {
                return Context.ReferentielEtendus
                  .Include(p => p.Nature)
                  .Include(p => p.Ressource.SousChapitre)
                  .Where(p => p.RessourceId.Equals(idRessource) && p.SocieteId.Equals(idSociete))
                  .FirstOrDefault();
            }
            else
            {
                return Context.ReferentielEtendus
                  .Include(p => p.Nature)
                  .Where(p => p.RessourceId.Equals(idRessource) && p.SocieteId.Equals(idSociete))
                  .FirstOrDefault();
            }
        }

        #region Paramétrage Référentiel Etendu

        public IReadOnlyList<ReferentielEtenduEnt> Get(List<int> ressourceIds, int societeId)
        {
            return Context.ReferentielEtendus.Where(q => ressourceIds.Contains(q.RessourceId) && q.SocieteId == societeId).ToList();
        }

        /// <inheritdoc />
        public ReferentielEtenduEnt GetById(int referentielEtenduId, bool withRessourceInclude = false)
        {
            ReferentielEtenduEnt refE;
            if (withRessourceInclude)
            {
                refE = Context.ReferentielEtendus.Include("Nature").Include("Ressource.SousChapitre.Chapitre").Include("UniteReferentielEtendus.Unite").FirstOrDefault(r => r.ReferentielEtenduId.Equals(referentielEtenduId));
            }
            else
            {
                refE = Context.ReferentielEtendus.Find(referentielEtenduId);
                if (refE != null)
                {
                    refE.Societe.Organisation = null;
                }
            }

            return refE;
        }

        /// <inheritdoc />
        public IEnumerable<ReferentielEtenduEnt> GetList(int societeId)
        {
            foreach (ReferentielEtenduEnt referentielEtendu in Context.ReferentielEtendus.Include("Nature").Include("Ressource").Include("UniteReferentielEtendus.Unite").Where(r => r.NatureId != null && r.SocieteId.Equals(societeId)))
            {
                yield return referentielEtendu;
            }
        }

        /// <inheritdoc />
        public IEnumerable<ReferentielEtenduEnt> GetListBySocieteId(int societeId)
        {
            foreach (
              ReferentielEtenduEnt referentielEtendu in
              Context.ReferentielEtendus.Include("Nature")
                     .Include("Ressource.SousChapitre.Chapitre")
                     .Include("UniteReferentielEtendus.Unite")
                     .Where(r => r.Ressource.Active && !r.Ressource.IsRessourceSpecifiqueCi && !r.Ressource.DateSuppression.HasValue && r.SocieteId.Equals(societeId)))
            {
                yield return referentielEtendu;
            }
        }

        /// <summary>
        /// Gets the list recommande by societe identifier.
        /// </summary>
        /// <param name="societeId">The societe identifier.</param>
        /// <returns>IEnumerable of ReferentielEtenduEnt</returns>
        public IEnumerable<ReferentielEtenduEnt> GetListRecommandeBySocieteId(int societeId)
        {
            return Context.ReferentielEtendus.Include(x => x.Nature)
                 .Include(x => x.Ressource.SousChapitre.Chapitre)
                 .Include(x => x.RessourcesRecommandees)
                 .Include(x => x.Ressource)
                 .Where(r => r.Ressource.Active
                 && !r.Ressource.DateSuppression.HasValue
                 && !r.Ressource.IsRessourceSpecifiqueCi
                 && r.NatureId != null
                 && r.SocieteId.Equals(societeId)).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, bool onlyActive = true)
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                                    .Include("Ressource.TypeRessource")
                                    .Include("Ressource.Carburant.Unite")
                                    .Include("Ressource.SousChapitre.Chapitre")
                                    .Include("Ressource.CIRessources")
                                    .Include("UniteReferentielEtendus.Unite")
                                    .Where(r => r.SocieteId.Equals(societeId) &&
                                                (!onlyActive || r.Ressource.Active) &&
                                                !r.Ressource.DateSuppression.HasValue)
                                    .ToList();

            return refEtendus.GroupBy(r => r.Ressource.SousChapitre).GroupBy(c => c.Key.Chapitre).Select(c => c.Key).ToList();
        }

        /// <inheritdoc />
        public List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightBySocieteId(int societeId)
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                                    .Include("Ressource.TypeRessource")
                                    .Include("Ressource.SousChapitre.Chapitre")
                                    .Where(r => r.SocieteId.Equals(societeId) &&
                                                r.Ressource.Active &&
                                                !r.Ressource.DateSuppression.HasValue &&
                                                (r.Ressource.TypeRessource.Code == TypeRessource.CodeTypePersonnel || r.Ressource.TypeRessource.Code == TypeRessource.CodeTypeMateriel))
                                    .ToList();

            return refEtendus.Select(r => r.Ressource.SousChapitre.Chapitre).Distinct().ToList();
        }


        /// <inheritdoc />
        public List<ChapitreEnt> GetReferentielEtenduAsChapitreListLightByRessourceIdList(List<int> ressourceIdList)
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                                    .Include("Ressource.TypeRessource")
                                    .Include("Ressource.SousChapitre.Chapitre")
                                    .Where(r => ressourceIdList.Contains(r.RessourceId) &&
                                                r.Ressource.Active &&
                                                !r.Ressource.DateSuppression.HasValue)
                                    .ToList();

            return refEtendus.Select(r => r.Ressource.SousChapitre.Chapitre).Distinct().ToList();
        }

        /// <inheritdoc />
        public IEnumerable<ChapitreEnt> GetReferentielEtenduAsChapitreList(int societeId, string codeTypeRessource)
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                              .Include("Ressource.TypeRessource")
                              .Include("Ressource.Carburant.Unite")
                              .Include("Ressource.SousChapitre.Chapitre")
                              .Include("Ressource.CIRessources")
                              .Include("Ressource.TypeRessource")
                              .Include("UniteReferentielEtendus.Unite")
                              .Where(r => r.SocieteId == societeId &&
                                          r.Ressource.Active &&
                                          !r.Ressource.DateSuppression.HasValue &&
                                          (r.Ressource.TypeRessource.Code.Equals(codeTypeRessource) || string.IsNullOrEmpty(codeTypeRessource)))
                              .ToList();

            return refEtendus.GroupBy(r => r.Ressource.SousChapitre).GroupBy(c => c.Key.Chapitre).Select(c => c.Key).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<ChapitreEnt> GetChapitresFromReferentielEtendus(int societeId, string filter = "")
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                         .Include("Ressource.RessourcesEnfants")
                         .Include("Ressource.TypeRessource")
                         .Include("ParametrageReferentielEtendus.Devise")
                         .Include("ParametrageReferentielEtendus.Unite")
                         .Include("ParametrageReferentielEtendus.Organisation")
                         .Include("Ressource.SousChapitre.Chapitre")
                         .Where(
                            r => r.SocieteId == societeId
                            && r.Ressource.Active
                            && !r.Ressource.DateSuppression.HasValue
                            && (r.Ressource.Code.Contains(filter) || r.Ressource.Libelle.Contains(filter))
                           ).ToList();

            return refEtendus.GroupBy(r => r.Ressource.SousChapitre).GroupBy(c => c.Key.Chapitre).Select(c => c.Key).ToList();
        }

        /// <summary>
        /// Recupere La liste des natures pour une societe et un code chapitre
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="chapitreCode">chapitreCode</param>
        /// <returns>Liste de nature</returns>
        public IEnumerable<NatureEnt> GetNaturesByChapitreCodeAndSocieteId(int societeId, string chapitreCode)
        {

            List<NatureEnt> natures = Context.ReferentielEtendus
                         .Include("Ressource")
                         .Include("Ressource.TypeRessource")
                         .Include("Ressource.SousChapitre.Chapitre")
                         .Include("Nature")
                         .Where(
                            r => r.SocieteId == societeId
                            && r.Ressource.Active
                            && !r.Ressource.DateSuppression.HasValue
                            && r.Ressource.SousChapitre.Chapitre.Code == chapitreCode
                           ).Select(re => re.Nature).ToList();

            return natures;
        }

        /// <summary>
        /// Recupere La liste des natures pour une societe 
        /// </summary>
        /// <param name="societeId">societeId</param>   
        /// <returns>Liste de nature</returns>
        public IEnumerable<ChapitreEnt> GetAllChapitresWithNatures(int societeId)
        {
            List<ReferentielEtenduEnt> refEtendus = Context.ReferentielEtendus
                         .Include("Ressource")
                         .Include("Ressource.SousChapitre.Chapitre")
                         .Include("Nature")
                         .Where(
                            r => r.SocieteId == societeId
                            && r.Ressource.Active
                            && !r.Ressource.DateSuppression.HasValue
                           )
                           .ToList();

            return refEtendus.GroupBy(r => r.Ressource.SousChapitre).GroupBy(c => c.Key.Chapitre).Select(c => c.Key).ToList();
        }


        /// <inheritdoc />
        public RessourceEnt GetRessourceWithRefEtenduAndParams(int ressourceId, int societeId)
        {
            return Context.Ressources.Include("SousChapitre.Chapitre")
                                     .Include("ReferentielEtendus.ParametrageReferentielEtendus.Devise")
                                     .Include("ReferentielEtendus.ParametrageReferentielEtendus.Unite")
                                     .Include("ReferentielEtendus.ParametrageReferentielEtendus.Organisation")
                                     .Include("RessourcesEnfants")
                                     .Include("RessourcesEnfants.SousChapitre.Chapitre")
                                     .Include("RessourcesEnfants.ReferentielEtendus.ParametrageReferentielEtendus.Devise")
                                     .Include("RessourcesEnfants.ReferentielEtendus.ParametrageReferentielEtendus.Unite")
                                     .Include("RessourcesEnfants.ReferentielEtendus.ParametrageReferentielEtendus.Organisation")
                                     .Where(r => r.RessourceId == ressourceId
                                            && r.ReferentielEtendus.Any(x => x.SocieteId == societeId))
                                     .FirstOrDefault();
        }

        /// <summary>
        /// Récupére une resouce par Code et groupe code.
        /// </summary>
        /// <param name="code">Code de la ressource.</param>
        /// <param name="groupeCode">Le code du groupe.</param>
        /// <returns>La ressource créée.</returns>
        public List<RessourceEnt> GetRessourceByCodeAndGroupeCode(string code, string groupeCode)
        {
            return Context.Ressources.Where(x => x.Code == code && x.SousChapitre.Chapitre.Groupe.Code == groupeCode)?.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<ReferentielEtenduEnt> DifferentialReferentielFixeReferentielEtendu(int societeId, List<ReferentielEtenduEnt> listReferentielEtendu)
        {
            int? groupeId = Context.Societes.Where(s => s.SocieteId.Equals(societeId)).Select(s => s.GroupeId).SingleOrDefault();
            if (groupeId.HasValue)
            {
                List<RessourceEnt> ressourceList = Context.Ressources
                                                          .Include("SousChapitre.Chapitre")
                                                          .Include("ReferentielEtendus.UniteReferentielEtendus.Unite")
                                                          .Where(r => r.Active && !r.DateSuppression.HasValue && !r.IsRessourceSpecifiqueCi && r.SousChapitre.Chapitre.GroupeId.Equals(groupeId.Value) &&
                                                                      !Context.ReferentielEtendus.Where(re => re.SocieteId.Equals(societeId)).Any(e => e.RessourceId == r.RessourceId))
                                                          .ToList();
                foreach (RessourceEnt ressource in ressourceList)
                {
                    ressource.ReferentielEtendus = new List<ReferentielEtenduEnt>() { new ReferentielEtenduEnt { SocieteId = societeId, RessourceId = ressource.RessourceId, Ressource = ressource } };
                    listReferentielEtendu.AddRange(ressource.ReferentielEtendus);
                }
            }

            return listReferentielEtendu;
        }


        /// <summary>
        /// Permet de récuperer les réferentiels etendus a partir d'un ci
        /// </summary>
        /// <param name="ciId">Identifiant de la ci</param>
        /// <returns>liste des réferentiels etendus</returns>
        public IEnumerable<ReferentielEtenduEnt> GetReferentielsEtendusByCi(int ciId)
        {
            int? societeId = Context.CIs.Where(s => s.CiId.Equals(ciId)).Select(s => s.SocieteId).FirstOrDefault();

            if (societeId.HasValue)
            {
                foreach (
                ReferentielEtenduEnt referentielEtendu in
                Context.ReferentielEtendus
                      .Include("Ressource.SousChapitre.Chapitre")
                      .Where(r => r.Ressource.Active && !r.Ressource.DateSuppression.HasValue && r.SocieteId.Equals((int)societeId)))
                {
                    yield return referentielEtendu;
                }
            }
        }

        public IEnumerable<ReferentielEtenduEnt> GetReferentielsEtendusBySocieteId(int societeId)
        {
            return Context.ReferentielEtendus
                        .Include(c => c.Ressource.SousChapitre.Chapitre)
                        .Where(r => r.Ressource.Active && !r.Ressource.DateSuppression.HasValue && r.SocieteId == societeId);
        }

        /// <summary>
        /// Permet d'ajouter les ressources spécifiques CI à la liste des réferenitiel etendus
        /// </summary>
        /// <param name="ciId">Identifiant du ci</param>
        /// <param name="listReferentielEtendu">liste des réferenitiel etendus</param>
        public void AddRessourcesSpecifiqueInReferentielEtendu(int ciId, ref List<ReferentielEtenduEnt> listReferentielEtendu)
        {
            List<RessourceEnt> ressourceList = Context.Ressources
                                                          .Include("SousChapitre.Chapitre")
                                                          .Include("RessourceRattachement")
                                                          .Where(r => !r.DateSuppression.HasValue && ciId == r.SpecifiqueCiId)
                                                          .ToList();

            foreach (RessourceEnt ressource in ressourceList)
            {
                ressource.ReferentielEtendus = new List<ReferentielEtenduEnt>() { new ReferentielEtenduEnt { RessourceId = ressource.RessourceId, Ressource = ressource } };
                listReferentielEtendu.AddRange(ressource.ReferentielEtendus);
            }
        }

        /// <inheritdoc />
        public ReferentielEtenduEnt AddReferentielEtendu(ReferentielEtenduEnt referentielEtendu)
        {
            HandleUniteReferentielEtendus(referentielEtendu);
            DetachDependencies(referentielEtendu); // A faire : tester et voir si c'est utile : c'est nécessaire pour ne pas ajouter de nouvelles entrées dans les tables correspondantes aux dépendances que nous vidons avant enregistrement
            Insert(referentielEtendu);

            return referentielEtendu;
        }

        /// <inheritdoc />
        public ReferentielEtenduEnt UpdateReferentielEtendu(ReferentielEtenduEnt referentielEtendu)
        {
            HandleUniteReferentielEtendus(referentielEtendu);
            DetachDependencies(referentielEtendu);
            Update(referentielEtendu);

            return referentielEtendu;
        }

#pragma warning disable S3776
        /// <summary>
        /// Gère l'ajout, et la suppression d'unité/référentiel étendu
        /// </summary>
        /// <param name="referentielEtendu">le référentiel étenu à traîter</param>
        private void HandleUniteReferentielEtendus(ReferentielEtenduEnt referentielEtendu)
        {
            if (referentielEtendu.UniteReferentielEtendus != null && referentielEtendu.UniteReferentielEtendus.Count > 0)
            {
                foreach (var uniteRefEtendu in referentielEtendu.UniteReferentielEtendus)
                {
                    uniteRefEtendu.Unite = null;
                    if (!Context.UnitesReferentielEtendu.Any(u => u.UniteId == uniteRefEtendu.UniteId && u.ReferentielEtenduId == uniteRefEtendu.ReferentielEtenduId))
                    {
                        if (uniteRefEtendu.ReferentielEtenduId == 0)
                        {
                            var refEtendu = (ReferentielEtenduEnt)referentielEtendu.Clone();
                            DetachDependencies(refEtendu);
                            uniteRefEtendu.ReferentielEtendu = refEtendu;
                            uniteReferentielEtenduRepo.Insert(uniteRefEtendu);
                        }
                        else
                        {
                            uniteReferentielEtenduRepo.Insert(uniteRefEtendu);
                        }
                    }
                    if (uniteRefEtendu.IsDeleted)
                    {
                        uniteReferentielEtenduRepo.Delete(uniteRefEtendu);
                    }
                }
            }
            else
            {
                foreach (var uniteRefEtenduFromDB in Context.UnitesReferentielEtendu.AsNoTracking().Where(r => r.ReferentielEtenduId == referentielEtendu.ReferentielEtenduId).ToList())
                {
                    uniteReferentielEtenduRepo.Delete(uniteRefEtenduFromDB);
                }
            }
        }
#pragma warning restore S3776

        /// <inheritdoc />
        public void DeleteReferentielEtendu(int referentielEtenduId)
        {
            DeleteById(referentielEtenduId);
        }

        /// <summary>
        /// Retourne le référentiel étendu correspondant aux paramètres
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>le référentiel étendu correspondant aux paramètres</returns>
        public ReferentielEtenduEnt GetByRessourceIdAndSocieteId(int ressourceId, int societeId)
        {
            return Context.ReferentielEtendus.FirstOrDefault(r => r.RessourceId == ressourceId && r.SocieteId == societeId);
        }

        /// <summary>
        ///   Détache les dépendances d'un référentiel étendu
        /// </summary>
        /// <param name="referentielEtendu">Référentiel étendu</param>
        private void DetachDependencies(ReferentielEtenduEnt referentielEtendu)
        {
            referentielEtendu.Nature = null;
            referentielEtendu.Ressource = null;
            referentielEtendu.Societe = null;
            referentielEtendu.UniteReferentielEtendus = null;
        }

        #endregion

        #region Paramétrage Tarifs Référentiel

        /// <inheritdoc />
        public ParametrageReferentielEtenduEnt GetParametrageById(int paramReferentielEtenduId)
        {
            ParametrageReferentielEtenduEnt param = Context.ParametragesReferentielEtendu.Where(p => p.ParametrageReferentielEtenduId.Equals(paramReferentielEtenduId)).SingleOrDefault();
            if (param != null)
            {
                InitParametrageParentList(param);
            }

            return param;
        }

        /// <inheritdoc />
        public IEnumerable<ParametrageReferentielEtenduEnt> GetParametrageReferentielEtendu(int societeId, int deviseId, string filter = "")
        {
            try
            {

                return Context.ParametragesReferentielEtendu
                                    .Include(p => p.ReferentielEtendu.Ressource.SousChapitre.Chapitre)
                                    .Include(d => d.Unite)
                                    .Include(d => d.Devise)
                                    .Include(d => d.Organisation)
                                    .Where(p => p.ReferentielEtendu.Ressource.Active &&
                                                !p.ReferentielEtendu.Ressource.DateSuppression.HasValue &&
                                                p.DeviseId.Equals(deviseId) &&
                                                p.ReferentielEtendu.SocieteId == societeId &&
                                                (p.ReferentielEtendu.Ressource.Code.Contains(filter) || p.ReferentielEtendu.Ressource.Libelle.Contains(filter)))
                                    .ToList();

            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <inheritdoc />
        public ParametrageReferentielEtenduEnt GetParametrageReferentielEtendu(int organisationId, int deviseId, int referentielId)
        {
            return
              Context.ParametragesReferentielEtendu.Include(p => p.Organisation.TypeOrganisation)
                     .SingleOrDefault(
                                      p =>
                                        p.OrganisationId.Equals(organisationId) && p.DeviseId.Equals(deviseId) &&
                                        p.ReferentielEtenduId.Equals(referentielId));
        }

        /// <inheritdoc />
        public IEnumerable<ParametrageReferentielEtenduEnt> DifferentialParametrageReferentielFixeReferentielEtendu(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam)
        {
            return DifferentialParametrageReferentielFixeReferentielEtendu(orgaList, deviseId, listParam, string.Empty);
        }

        /// <inheritdoc />
        public IEnumerable<ParametrageReferentielEtenduEnt> DifferentialParametrageReferentielFixeReferentielEtendu(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam, string filter)
        {
            if (orgaList != null)
            {
                var orgaSociete = orgaList.FirstOrDefault(o => o.TypeOrganisation.Code == Constantes.OrganisationType.CodeSociete);

                //On récupère le societeId de notre organisation courante
                if (orgaSociete.Societe == null)
                {
                    this.organisationRepo.PerformEagerLoading(orgaSociete, o => o.Societe);
                }
                int societeId = orgaSociete.Societe.SocieteId;

                //On récupère tous les Référentiels étendus de la société
                var refEtendus = Context.ReferentielEtendus
                                          .Include(r => r.Ressource.SousChapitre.Chapitre)
                                          .Where(r => r.NatureId != null && r.Ressource.Active &&
                                                      !r.Ressource.DateSuppression.HasValue && r.SocieteId.Equals(societeId) &&
                                                      (r.Ressource.Code.Contains(filter) || r.Ressource.Libelle.Contains(filter)))
                                          .ToList();

                var paramRefEtenduList = NewMethod(orgaList, deviseId, listParam, refEtendus);

                return paramRefEtenduList;
            }

            return Enumerable.Empty<ParametrageReferentielEtenduEnt>();
        }

        private static List<ParametrageReferentielEtenduEnt> NewMethod(List<OrganisationEnt> orgaList, int deviseId, List<ParametrageReferentielEtenduEnt> listParam, List<ReferentielEtenduEnt> refEtendus)
        {
            var outputParamRefEtenduList = new List<ParametrageReferentielEtenduEnt>();
            var notFoundList = new List<ParametrageReferentielEtenduEnt>();

            foreach (var refEtendu in refEtendus)
            {
                refEtendu.ParametrageReferentielEtendus = new List<ParametrageReferentielEtenduEnt>();

                foreach (var orga in orgaList)
                {
                    var found = listParam.FirstOrDefault(p => p.ReferentielEtenduId == refEtendu.ReferentielEtenduId && p.OrganisationId == orga.OrganisationId && p.DeviseId.Equals(deviseId));

                    if (found == null)
                    {
                        ParametrageReferentielEtenduEnt param = new ParametrageReferentielEtenduEnt
                        {
                            OrganisationId = orga.OrganisationId,
                            Organisation = orga,
                            ReferentielEtenduId = refEtendu.ReferentielEtenduId,
                            ReferentielEtendu = refEtendu,
                            DeviseId = deviseId
                        };

                        notFoundList.Add(param);
                        refEtendu.ParametrageReferentielEtendus.Add(param);
                    }
                    else
                    {
                        refEtendu.ParametrageReferentielEtendus.Add(found);
                        outputParamRefEtenduList.Add(found);
                    }
                }
            }

            outputParamRefEtenduList.AddRange(notFoundList);
            return outputParamRefEtenduList;
        }

        /// <inheritdoc />
        public IEnumerable<ChapitreEnt> GetParametrageReferentielEtenduAsChapitreList(IEnumerable<ParametrageReferentielEtenduEnt> paramRefEtenduList)
        {
            return paramRefEtenduList.GroupBy(x => x.ReferentielEtendu.Ressource.SousChapitre).GroupBy(x => x.Key.Chapitre).Select(x => x.Key);
        }

        /// <inheritdoc />
        public ParametrageReferentielEtenduEnt InitParametrageParentList(ParametrageReferentielEtenduEnt parametrage)
        {
            if (parametrage?.Organisation.PereId != null)
            {
                OrganisationEnt orga = this.organisationRepo.FindById(parametrage.Organisation.PereId.Value);

                this.organisationRepo.PerformEagerLoading(orga, o => o.TypeOrganisation);
                int deviseId = parametrage.DeviseId;
                int referentialId = parametrage.ReferentielEtenduId;
                var list = new List<ParametrageReferentielEtenduEnt>();

                while (orga != null && orga.TypeOrganisation.Code != Constantes.OrganisationType.CodeGroupe)
                {
                    list.Add(GetParametrageReferentielEtendu(orga.OrganisationId, deviseId, referentialId));
                    if (orga.PereId.HasValue)
                    {
                        orga = this.organisationRepo.FindById(orga.PereId.Value);
                        this.organisationRepo.PerformEagerLoading(orga, o => o.TypeOrganisation);
                    }
                    else
                    {
                        orga = null;
                    }
                }

                parametrage.ParametragesParent = list;
            }

            return parametrage;
        }

        #endregion
    }
}
