using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.DatesClotureComptable.Services.Valorisations;
using Fred.Business.Depense;
using Fred.Business.ExplorateurDepense;
using Fred.Business.ExplorateurDepense.Models;
using Fred.Business.FeatureFlipping;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Valorisation.Services;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Bareme;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Societe;
using Fred.Entities.Valorisation;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models.Bareme;
using Fred.Web.Shared.Models.Valorisation;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.Business.Valorisation
{
    public class ValorisationManager : Manager<ValorisationEnt, IValorisationRepository>, IValorisationManager
    {
        private readonly Semaphore semaphore = new Semaphore(1, 1);
        private static List<BaremeExploitationCIEnt> listNewBareme = new List<BaremeExploitationCIEnt>();
        private static List<BaremeExploitationOrganisationEnt> listNewBaremeStorm = new List<BaremeExploitationOrganisationEnt>();

        private readonly ISepService sepService;
        private readonly ICIManager ciManager;
        private readonly IBaremeExploitationCIRepository baremeExploitationCIRepository;
        private readonly IBaremeExploitationCISurchargeRepository baremeExploitationCISurchargeRepository;
        private readonly IBaremeExploitationOrganisationRepository baremeExploitationOrganisationRepository;
        private readonly IReferentielEtenduRepository referentielEtenduRepository;
        private readonly IPointageRepository pointageRepository;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IMaterielRepository materielRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly IDeviseManager deviseManager;
        private readonly IUniteManager uniteManager;
        private readonly IOrganisationManager organisationManager;
        private readonly ISocieteManager societeManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IValorisationVerrouPeriodesService valorisationVerrouPeriodesService;
        private readonly IPremierePeriodeComptableNonClotureeService premierePeriodeComptableNonClotureeService;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly ISocieteRepository societeRepository;

        public ValorisationManager(
            IUnitOfWork uow,
            IValorisationRepository valorisationRepository,
            ISepService sepService,
            ICIManager ciManager,
            IBaremeExploitationCIRepository baremeExploitationCIRepository,
            IBaremeExploitationCISurchargeRepository baremeExploitationCISurchargeRepository,
            IBaremeExploitationOrganisationRepository baremeExploitationOrganisationRepository,
            IReferentielEtenduRepository referentielEtenduRepository,
            IPointageRepository pointageRepository,
            IDatesClotureComptableManager datesClotureComptableManager,
            IMaterielRepository materielRepository,
            IPersonnelRepository personnelRepository,
            IDeviseManager deviseManager,
            IUniteManager uniteManager,
            IOrganisationManager organisationManager,
            ISocieteManager societeManager,
            IFeatureFlippingManager featureFlippingManager,
            IValorisationVerrouPeriodesService valorisationVerrouPeriodesService,
            IPremierePeriodeComptableNonClotureeService premierePeriodeComptableNonClotureeService,
            IOrganisationTreeService organisationTreeService,
            ISocieteRepository societeRepository)
            : base(uow, valorisationRepository)
        {
            this.sepService = sepService;
            this.ciManager = ciManager;
            this.baremeExploitationCIRepository = baremeExploitationCIRepository;
            this.baremeExploitationCISurchargeRepository = baremeExploitationCISurchargeRepository;
            this.baremeExploitationOrganisationRepository = baremeExploitationOrganisationRepository;
            this.referentielEtenduRepository = referentielEtenduRepository;
            this.pointageRepository = pointageRepository;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.materielRepository = materielRepository;
            this.personnelRepository = personnelRepository;
            this.deviseManager = deviseManager;
            this.uniteManager = uniteManager;
            this.organisationManager = organisationManager;
            this.societeManager = societeManager;
            this.featureFlippingManager = featureFlippingManager;
            this.valorisationVerrouPeriodesService = valorisationVerrouPeriodesService;
            this.premierePeriodeComptableNonClotureeService = premierePeriodeComptableNonClotureeService;
            this.organisationTreeService = organisationTreeService;
            this.societeRepository = societeRepository;
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="periode">Période</param>
        /// <returns>Liste de valorisations</returns>
        public IEnumerable<ValorisationEnt> GetByCiAndPeriod(int ciId, DateTime periode)
        {
            return Repository.GetByCiAndPeriod(ciId, periode);
        }

        /// <summary>
        /// Récupération de la valorisation (explorateur des dépenses)
        /// </summary>
        /// <param name="groupRemplacementId">Identifiant du groupe de remplacement</param>
        /// <returns>Liste de valorisations</returns>
        public ValorisationEnt GetByGroupRemplacementId(int groupRemplacementId)
        {
            return Repository.GetByGroupRemplacementId(groupRemplacementId).FirstOrDefault();
        }

        public async Task<List<ValorisationEnt>> GetValorisationListAsync(int ciId, MainOeuvreAndMaterielsFilter mainOeuvreAndMaterielsFilter)
        {
            if (mainOeuvreAndMaterielsFilter == null)
                throw new ArgumentNullException(nameof(mainOeuvreAndMaterielsFilter));

            IEnumerable<ValorisationEnt> query = await Repository.GetValorisationsListAsync(ciId).ConfigureAwait(false);

            List<ValorisationEnt> queryAggregate = ApplyPersonnelAndMaterielFiltersForValorisation(query,
                                                                                                  mainOeuvreAndMaterielsFilter.TakeMOInt,
                                                                                                    mainOeuvreAndMaterielsFilter.TakeMOInterim,
                                                                                                    mainOeuvreAndMaterielsFilter.TakeMaterielInt,
                                                                                                    mainOeuvreAndMaterielsFilter.TakeMaterielExt);

            return queryAggregate.Count > 0 ? queryAggregate : query.ToList();

        }

        /// <summary>
        /// Récupération des valorisations (explorateur des dépenses)
        /// </summary>
        /// <param name="filtre">Filtre de l'explorateur de dépenses</param>
        /// <returns>Liste de valorisations</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ValorisationEnt> query = await Repository.GetValorisationsListAsync(filtre.CiId).ConfigureAwait(false);

            List<ValorisationEnt> queryAggregate = ApplyPersonnelAndMaterielFiltersForValorisation(query, filtre.TakeMOInt, filtre.TakeMOInterim, filtre.TakeMaterielInt, filtre.TakeMaterielExt);

            return queryAggregate.Count > 0 ? queryAggregate : query;
        }



        /// <summary>
        /// Récupération des valorisations (explorateur des dépenses)
        /// </summary>
        /// <param name="filtre">Filtre de l'explorateur de dépenses</param>
        /// <returns>Liste de valorisations</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(SearchDepense filtre)
        {
            IEnumerable<ValorisationEnt> query = await Repository.GetValorisationsListAsync(filtre.CiId).ConfigureAwait(false);

            List<ValorisationEnt> queryAggregate = ApplyPersonnelAndMaterielFiltersForValorisation(query, filtre.TakeMOInt, filtre.TakeMOInterim, filtre.TakeMaterielInt, filtre.TakeMaterielExt);

            return queryAggregate.Count > 0 ? queryAggregate : query;
        }

        /// <summary>
        /// Application des filtres concernant la valorisation
        /// </summary>
        /// <param name="filtre">Filtre de l'explorateur de dépenses</param>
        /// <param name="query">Liste de toutes les valorisations</param>
        /// <returns>Liste filtrée de valorisations</returns>
        private List<ValorisationEnt> ApplyPersonnelAndMaterielFiltersForValorisation(IEnumerable<ValorisationEnt> query, bool TakeMOInt, bool TakeMOInterim, bool TakeMaterielInt, bool TakeMaterielExt)
        {
            List<ValorisationEnt> queryAggregate = new List<ValorisationEnt>();

            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ExplorateurDepensesFiltresMOMateriels))
            {
                if (TakeMOInt)
                {
                    queryAggregate.AddRange(query.Where(x => x.Personnel?.IsInterimaire == false));
                }
                if (TakeMOInterim)
                {
                    queryAggregate.AddRange(query.Where(x => x.Personnel?.IsInterimaire == true));
                }
                if (TakeMaterielInt)
                {
                    queryAggregate.AddRange(query.Where(x => x.Materiel?.MaterielLocation == false));
                }
                if (TakeMaterielExt)
                {
                    queryAggregate.AddRange(query.Where(x => x.Materiel?.MaterielLocation == true));
                }
            }
            return queryAggregate;
        }


        /// <summary>
        /// Récupère une valo
        /// </summary>
        /// <param name="valoId">Id valorisation</param>
        /// <returns>Valorisation</returns>
        public ValorisationEnt GetValorisationById(int valoId)
        {
            return Repository.Query()
                               .Include(x => x.ReferentielEtendu.Ressource.SousChapitre.Chapitre)
                               .Include(x => x.ReferentielEtendu.Nature)
                               .Include(c => c.CI)
                               .Include(c => c.Tache.Parent.Parent)
                               .Include(d => d.Unite)
                               .Include(d => d.Devise)
                               .Include(d => d.Personnel.Societe)
                               .Include(d => d.RapportLigne)
                               .Include(d => d.Materiel)
                               .Filter(c => c.ValorisationId == valoId)
                               .Get()
                               .AsNoTracking()
                               .FirstOrDefault();
        }

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <returns>Liste de valorisations</returns>
        private IEnumerable<ValorisationEnt> GetByCiAndYearAndMonth(int ciId, int annee, int mois)
        {
            return Repository.GetByCiAndYearAndMonth(ciId, annee, mois);
        }

        private IEnumerable<ValorisationEnt> GetByCiAndYearAndMonth(List<int> ciIds, int annee, int mois)
        {
            return Repository.GetByCiAndYearAndMonth(ciIds, annee, mois);
        }

        /// <summary>
        /// Execute une procédure avec les paramètres
        /// </summary>
        /// <param name="objectId">Identifiant de l'objet appelant</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="procedureToExecute">Procédure à executé</param>
        /// <param name="periode">Période optionnelle</param>
        public void NewValorisationJob(int objectId, int userId, Action<int, int, DateTime?> procedureToExecute, DateTime? periode = null)
        {
            StartTask(() => procedureToExecute(objectId, userId, periode));
        }

        private void StartTask(System.Action procedureToExecute)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    semaphore.WaitOne();
                    listNewBareme.Clear();
                    listNewBaremeStorm.Clear();
                    procedureToExecute();
                }
                catch (Exception e)
                {
                    throw new FredBusinessException(e.Message, e);
                }
                finally
                {
                    semaphore.Release();
                    listNewBareme.Clear();
                    listNewBaremeStorm.Clear();
                }
            });
        }


        /// <summary>
        /// Déverrouille l'ensemble des lignes de valorisation correspondantes au Ci et à la période en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <param name="verrou">Indique si on verrouille ou déverrouille</param>
        public void UpdateVerrouPeriodeValorisation(int ciId, int annee, int mois, bool verrou)
        {
            IEnumerable<ValorisationEnt> valos = GetByCiAndYearAndMonth(ciId, annee, mois);
            foreach (ValorisationEnt valo in valos)
            {
                valo.VerrouPeriode = verrou;
                Repository.Update(valo);
            }
            Save();
        }

        public void UpdateVerrouPeriodeValorisation(List<int> ciIds, int annee, int mois, bool verrou)
        {
            List<ValorisationEnt> valorisations = GetByCiAndYearAndMonth(ciIds, annee, mois).ToList();

            foreach (ValorisationEnt valorisation in valorisations)
            {
                valorisation.VerrouPeriode = verrou;
            }

            Repository.UpdateRange(valorisations);
            Save();
        }

        /// <summary>
        /// Met à jour une ligne de valorisation avec le groupe
        /// </summary>
        /// <param name="valorisation">Identifiant du CI</param>
        public void UpdateValorisationGroupeRemplacement(ValorisationEnt valorisation)
        {
            CleanDependencies(valorisation);
            Repository.Update(valorisation);
            Save();
        }

        /// <summary>
        /// Retourne la liste des valorisations selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            DateTime? startDate = periodeDebut?.GetFirstDayOfMonth();
            DateTime endDate = periodeFin.Value.GetLastDayOfMonth();

            return await Repository.GetValorisationsListAsync(ciId, ressourceId, startDate, endDate, deviseId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des valorisations sans reception intérimaire
        /// </summary>
        /// <param name="ciIdList">Liste d'Iidentifiants de CI</param>
        /// <param name="datedebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <returns>Liste de <see cref="ValorisationEnt" /></returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsWithoutReceptionInterimaireAsync(List<int> ciIdList, DateTime datedebut, DateTime dateFin)
        {
            return await Repository.GetValorisationsWithoutReceptionInterimaireAsync(ciIdList, datedebut, dateFin).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des valorisations
        /// </summary>
        /// <param name="ciIdList">Liste d'Iidentifiants de CI</param>
        /// <param name="datedebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <returns>Liste de <see cref="ValorisationEnt" /></returns>
        public async Task<IEnumerable<ValorisationEnt>> GetValorisationsAsync(IEnumerable<int> ciIdList, DateTime datedebut, DateTime dateFin)
        {
            return await Repository.GetValorisationsAsync(ciIdList, datedebut, dateFin).ConfigureAwait(false);
        }

        #region FromPointage

        /// <summary>
        /// Retourne la liste des valorisations associées à un rapport
        /// </summary>
        /// <param name="rapportId">identifiant du rapport</param>
        /// <returns>valorisations</returns>
        public IList<ValorisationEnt> GetValorisationByRapport(int rapportId)
        {
            return Repository.GetByRapportId(rapportId).ToList();
        }

        /// <summary>
        /// Crée des lignes de valorisations à partir de lignes de pointages
        /// </summary>
        /// <param name="listPointages">Une liste de pointages</param>
        /// <param name="source">Source de l'execution de la valorisation</param>
        /// <param name="periode">période où le barème est mis à jour sinon null</param>
        public void CreateValorisation(ICollection<RapportLigneEnt> listPointages, string source, DateTime? periode = null)
        {
            listNewBareme.Clear();
            listNewBaremeStorm.Clear();
            listPointages.ToList().ForEach(p => InsertValorisationFromPointage(p, source, periode));
        }

        public void CreateValorisationForPersonnel(ICollection<RapportLigneEnt> listPointages, string source, DateTime? periode = null)
        {
            listNewBareme.Clear();
            listNewBaremeStorm.Clear();
            listPointages.ToList().ForEach(p => InsertValorisationForPersonnel(p, source, periode));
        }

        /// <summary>
        /// Insère des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        /// <param name="source">Source de l'execution de la valorisation</param>
        /// <param name="periode">période où le barème est mis à jour sinon null</param>
        public void InsertValorisationFromPointage(RapportLigneEnt pointage, string source, DateTime? periode = null)
        {
            if (!pointage.Cloture)
            {
                pointage.ListRapportLigneTaches.Where(lt => !lt.HeureTache.Equals(0)).ForEach(t => InsertValorisationFromPointageTache(pointage, t, source, periode));
            }
        }

        private void InsertValorisationForPersonnel(RapportLigneEnt pointage, string source, DateTime? periode = null)
        {
            if (!pointage.Cloture)
            {
                pointage.ListRapportLigneTaches.Where(lt => !lt.HeureTache.Equals(0)).ForEach(t => InsertValorisationPersonnelAndSave(pointage, t, source, periode));
            }
        }

        /// <summary>
        /// Met à jour des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        public void UpdateValorisationFromPointage(RapportLigneEnt pointage)
        {
            if (!pointage.Cloture)
            {
                foreach (RapportLigneTacheEnt pointageTache in pointage.ListRapportLigneTaches)
                {
                    if (pointageTache.IsDeleted)
                    {
                        DeleteValorisation(pointage, pointageTache);
                    }
                    else if (pointageTache.IsCreated)
                    {
                        if (!pointageTache.HeureTache.Equals(0))
                        {
                            InsertValorisationFromPointageTache(pointage, pointageTache, "Rapport");
                        }
                    }
                    else
                    {
                        UpdateValorisationFromPointageTache(pointage, pointageTache);
                    }
                }
            }
        }

        /// <summary>
        /// Supprime des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        public void DeleteValorisationFromPointage(RapportLigneEnt pointage)
        {
            if (!pointage.Cloture)
            {
                foreach (RapportLigneTacheEnt pointageTache in pointage.ListRapportLigneTaches)
                {
                    DeleteValorisation(pointage, pointageTache);
                }
            }
        }

        private void InsertValorisationFromPointageTache(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            InsertValorisationPersonnel(pointage, pointageTache, source, periode);

            if (pointage.MaterielId.HasValue)
            {
                ValorisationRapportLigneEtTache valoMaterielRapportLigneEtTache = new ValorisationRapportLigneEtTache { RapportLigne = pointage, RapportLigneTache = pointageTache };
                List<ValorisationRapportLigneEtTache> valoMaterielRapportLigneEtTacheList = new List<ValorisationRapportLigneEtTache>();
                valoMaterielRapportLigneEtTacheList.Add(valoMaterielRapportLigneEtTache);

                InsertValorisationMaterielFromPointage(valoMaterielRapportLigneEtTacheList, source);
            }

            Save();
        }

        private void InsertValorisationPersonnelAndSave(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            InsertValorisationPersonnel(pointage, pointageTache, source, periode);
            Save();
        }

        public void InsertValorisationPersonnel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            if (pointage.PersonnelId.HasValue)
            {
                bool isVerrouPeriode = GetVerrouPeriodeTrueValorisation(pointage.RapportId, pointage.RapportLigneId);

                if (!isVerrouPeriode)
                {
                    InsertValorisationPersonnelInterneOrInterimaire(pointage, pointageTache, source, periode);
                }
            }
            Save();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////// BAREME ORGANISATION //////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void UpdateValorisationFromListBaremeStorm(int orgaId, DateTime periode, List<int> ressourcesIds)
        {
            listNewBareme.Clear();

            listNewBaremeStorm.Clear();

            List<CIEnt> cis = GetCis(orgaId);

            List<int> cisIdsNotClosed = GetCisNotClosedAfterPeriod(cis, periode);

            List<CiDernierePeriodeComptableNonCloturee> ciDernierePeriodeComptableNonCloturees = premierePeriodeComptableNonClotureeService.GetPremierePeriodeComptableNonCloturees(cisIdsNotClosed, periode);

            // RECUPERATION DES RAPPORTS LIGNES CONCERNEE PAR LA MISE A JOURS
            List<RapportLigneEnt> rapportsLignes = pointageRepository.GetRapportLignesToUpdateValorisationFromListBaremeStorm(ressourcesIds, cisIdsNotClosed, ciDernierePeriodeComptableNonCloturees);

            // SUPRESSION DES VALOS
            DeleteValorisationByRapportsLignes(rapportsLignes);

            // CALCUL OU RECALCUL DES VALOS
            List<RapportLigneTacheEnt> rapportLignesTachesNotVerrouillees = GetRapportsLignesTachesNotVerrous(rapportsLignes);

            InsertValorisationPersonnelInterneOrInterimaires(periode, rapportLignesTachesNotVerrouillees);

            InsertValorisationMaterielsFromBaremeOrganisation(periode, rapportLignesTachesNotVerrouillees);

            Save();

            listNewBareme.Clear();
            listNewBaremeStorm.Clear();
        }

        private List<CIEnt> GetCis(int orgaId)
        {
            OrganisationEnt orga = organisationManager.GetOrganisationById(orgaId);
            List<CIEnt> ciList = ciManager.GetCIList(orgaId).ToList();
            List<CIEnt> result = new List<CIEnt>();
            if (orga.Societe != null)
            {
                result = ciList.Where(ci => ci.Sep).ToList();

                if (result.Count() == 0)
                {
                    result = ciList.Where(ci => !ci.Sep).ToList();
                }
            }
            else if (orga.Etablissement != null)
            {
                result = ciList.Where(ci => !ci.Sep).ToList();
            }

            return result;
        }

        private List<int> GetCisNotClosedAfterPeriod(List<CIEnt> ciList, DateTime periode)
        {
            var result = new List<int>();
            foreach (var ci in ciList)
            {
                if (ci.DateFermeture.HasValue && ci.DateFermeture < periode)
                {
                    continue;
                }
                result.Add(ci.CiId);
            }
            return result;
        }

        private void InsertValorisationMaterielsFromBaremeOrganisation(DateTime periode, List<RapportLigneTacheEnt> rapportLignesTachesNotVerrouillees)
        {
            var rapportLignesTacheMateriels = rapportLignesTachesNotVerrouillees.Where(x => x.RapportLigne.MaterielId.HasValue).ToList();

            foreach (var rapportLigneTache in rapportLignesTacheMateriels)
            {
                InsertValorisationMaterielFromBaremeOrganisation(rapportLigneTache.RapportLigne, rapportLigneTache, "Bareme", periode);
            }
        }

        private void InsertValorisationPersonnelInterneOrInterimaires(DateTime periode, List<RapportLigneTacheEnt> rapportLignesTachesNotVerrouillees)
        {
            List<RapportLigneTacheEnt> rapportLignesTachesPersonnels = rapportLignesTachesNotVerrouillees.Where(x => x.RapportLigne.PersonnelId.HasValue).ToList();

            foreach (var rapportLigneTache in rapportLignesTachesPersonnels)
            {
                InsertValorisationPersonnelInterneOrInterimaire(rapportLigneTache.RapportLigne, rapportLigneTache, "Bareme", periode);
            }
        }

        private List<RapportLigneTacheEnt> GetRapportsLignesTachesNotVerrous(List<RapportLigneEnt> rapportsLignes)
        {
            var result = new List<RapportLigneTacheEnt>();

            //RECUPERATION DE  RAPPORT LIGNES TACHES QUI ONT DES HEURES DE POINTEES
            var rapportLignesTaches = rapportsLignes.SelectMany(x => x.ListRapportLigneTaches).Where(x => !x.HeureTache.Equals(0)).ToList();

            //RECUPERE INFOS POUR SAVOIR SI RAPPORT LIGNE VERROUILLE
            List<RapportRapportLigneVerrouPeriode> rapportRapportLigneVerrouPeriodes = valorisationVerrouPeriodesService.GetVerrouPeriodesList(rapportsLignes);

            foreach (var rapportLignesTache in rapportLignesTaches)
            {
                if (!valorisationVerrouPeriodesService.GetVerrouPeriodeTrueValorisation(rapportRapportLigneVerrouPeriodes, rapportLignesTache.RapportLigne.RapportId, rapportLignesTache.RapportLigne.RapportLigneId))
                {
                    result.Add(rapportLignesTache);
                }
            }

            return result;
        }

        private void DeleteValorisationByRapportsLignes(List<RapportLigneEnt> rapportsLignes)
        {
            var rapportlignesIds = rapportsLignes.Select(x => x.RapportLigneId).ToList();
            var valorisationsToDeletes = Repository.GetValorisationsByRapporLignesIds(rapportlignesIds);
            Repository.DeleteValorisations(valorisationsToDeletes);
        }


        private void InsertValorisationMaterielFromBaremeOrganisation(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime periode)
        {
            ValorisationMaterielModel materiel = GetValorisationMaterielByMaterielId(pointage.MaterielId.Value);

            int societeId = GetSocieteId(pointage.Ci.Societe, materiel, null);

            ReferentielEtenduEnt referentielEtenduMatos = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(materiel.RessourceId, societeId, true);
            if (referentielEtenduMatos != null)
            {
                ValorisationEnt newValoMatos = null;
                if (!materiel.IsMaterielLocation)
                {
                    newValoMatos = NewValorisationMateriel(pointage, pointageTache, referentielEtenduMatos, materiel, source, periode);
                }

                if (newValoMatos != null)
                {
                    Repository.Insert(newValoMatos);
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool GetVerrouPeriodeTrueValorisation(int rapportId, int rapportLigneId)
        {
            List<ValorisationEnt> valorisationList = Repository.GetByRapportIdAndRapportLigneId(rapportId, rapportLigneId).ToList();

            return valorisationList.Any(v => v.VerrouPeriode);
        }

        private void InsertValorisationPersonnelInterneOrInterimaire(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            PersonnelEnt perso = personnelRepository.GetPersonnel(pointage.PersonnelId.Value);
            if (perso.IsInterimaire)
            {
                InsertValorisationInterimaireFromPointage(perso, pointage, pointageTache, source);
            }
            else
            {
                InsertValorisationPersonnelFromPointage(perso, pointage, pointageTache, source, periode);
            }
        }

        private void InsertValorisationPersonnelFromPointage(PersonnelEnt personnel, RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            ValorisationEnt valoPerso = GetNewValorisationPersoFromPointage(personnel, pointage, pointageTache, source, periode);

            if (valoPerso != null)
            {
                Repository.Insert(valoPerso);
            }
        }

        private ValorisationEnt GetNewValorisationPersoFromPointage(PersonnelEnt personnel, RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            ObjetValorise objetValorise = GetObjetValorise(pointage, personnel, null, periode);
            if (objetValorise != null)
            {
                return NewValorisationPersonnel(pointage, pointageTache, objetValorise.RefEtendu, objetValorise.UniteId, objetValorise.Prix, personnel, source, objetValorise.Bareme, periode);
            }
            else
            {
                return null;
            }
        }

        private ObjetValorise GetObjetValorise(RapportLigneEnt pointage, PersonnelEnt personnel, MaterielEnt materiel, DateTime? periode = null)
        {
            int societeId = GetSocieteIdByPointage(pointage, null, personnel);
            ReferentielEtenduEnt referentielEtenduPerso = (personnel.RessourceId.HasValue) ? referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(personnel.RessourceId.Value, societeId, true) : null;
            if (referentielEtenduPerso != null)
            {
                return ComputeObjetValorise(referentielEtenduPerso, pointage, personnel, materiel, periode);
            }
            else
            {
                return null;
            }
        }

        private ObjetValorise ComputeObjetValorise(ReferentielEtenduEnt referentielEtenduPerso, RapportLigneEnt pointage, PersonnelEnt personnel, MaterielEnt materiel, DateTime? periode = null)
        {
            DateTime periodeValorisation = periode ?? pointage.DatePointage;

            SurchargeBaremeExploitationCIEnt surcharge = baremeExploitationCISurchargeRepository.GetSurcharge(pointage.CiId, periodeValorisation, personnel?.PersonnelId, materiel?.MaterielId);
            if (surcharge != null && surcharge.Prix.HasValue)
            {
                // Cas de l'exception dans le barème (barème personnel défini sur une ressource différente de la ressource du personnel)
                if (referentielEtenduPerso.ReferentielEtenduId != surcharge.ReferentielEtenduId)
                {
                    return new ObjetValorise(referentielEtenduRepository.GetById(surcharge.ReferentielEtenduId, true), surcharge.UniteId, surcharge.Prix.Value, null);
                }
                // Cas de la surcharge du personnel sur la ressource défini dans la fiche du personnel
                else
                {
                    return new ObjetValorise(referentielEtenduPerso, surcharge.UniteId, surcharge.Prix.Value, null);
                }
            }
            // Cas du bareme classique
            else
            {
                BaremeExploitationCIEnt bareme = baremeExploitationCIRepository.Get(pointage.CiId, periodeValorisation, referentielEtenduPerso.ReferentielEtenduId);
                if (bareme == null)
                {
                    // Si le barème n'est pas défini on valorise avec les valeurs par défaut
                    bareme = GetOrInsertNewDefaultBareme(GetDefaultBaremeCI(pointage.CiId, referentielEtenduPerso.ReferentielEtenduId, pointage.DatePointage.GetPeriode()));
                }
                if (bareme.Prix.HasValue)
                {
                    return new ObjetValorise(referentielEtenduPerso, bareme.UniteId, bareme.Prix.Value, bareme);
                }
                else
                {
                    return null;
                }
            }
        }

        private void InsertValorisationInterimaireFromPointage(PersonnelEnt interim, RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source)
        {
            ContratInterimaireEnt contratActif = personnelRepository.GetContratInterimaireActive(interim.PersonnelId, pointage.DatePointage);
            if (contratActif != null)
            {
                int societeId = contratActif.SocieteId.Value;
                int ciId = contratActif.CiId.Value;

                if (sepService.IsSep(ciId))
                {
                    societeId = GetSocieteGeranteIdForSep(societeId, ciId);
                }

                ReferentielEtenduEnt refEtendu = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(contratActif.RessourceId.Value, societeId, true);
                if (refEtendu != null)
                {
                    ValorisationEnt newValoInterim = NewValorisationInterimaire(pointage, pointageTache, refEtendu, interim, contratActif, source);
                    if (newValoInterim != null)
                    {
                        Repository.Insert(newValoInterim);
                    }
                }
            }
        }

        private void InsertValorisationMaterielExterneFromPointage(ValorisationMaterielModel materielExterne, RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source)
        {
            ReferentielEtenduEnt refEtendu = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(materielExterne.RessourceId, materielExterne.SocieteId, true);
            if (refEtendu != null)
            {
                ValorisationEnt newValoMaterielExterne = NewValorisationMaterielExterne(pointage, pointageTache, refEtendu, materielExterne, source);
                if (newValoMaterielExterne != null)
                {
                    Repository.Insert(newValoMaterielExterne);
                }
            }
        }

        private void InsertValorisationMaterielFromPointage(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null)
        {
            ValorisationMaterielModel materiel = GetValorisationMaterielByMaterielId(pointage.MaterielId.Value);

            int societeId = GetSepGerantSocieteId();

            ReferentielEtenduEnt referentielEtenduMatos = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(materiel.RessourceId, societeId, true);
            if (referentielEtenduMatos != null)
            {
                ValorisationEnt newValoMatos = null;
                if (!materiel.IsMaterielLocation)
                {
                    newValoMatos = NewValorisationMateriel(pointage, pointageTache, referentielEtenduMatos, materiel, source, periode);
                }
                else
                {
                    newValoMatos = NewValorisationMaterielExterne(pointage, pointageTache, referentielEtenduMatos, materiel, source);
                }

                if (newValoMatos != null)
                {
                    Repository.Insert(newValoMatos);
                }
            }

            int GetSepGerantSocieteId()
            {
                var gerantSepId = societeRepository.GetSepGerantSocieteId(pointage.CiId);

                return gerantSepId == 0 ? materiel.SocieteId : gerantSepId;
            }
        }

        public int GetSocieteIdByPointage(RapportLigneEnt pointage, ValorisationMaterielModel materiel, PersonnelEnt personnel)
        {
            CIEnt ci = pointage.Ci ?? ciManager.GetCI(pointage.CiId);
            SocieteEnt societeCI = societeManager.GetSocieteById(ci.SocieteId.Value, new List<Expression<Func<SocieteEnt, object>>> { s => s.TypeSociete, s => s.AssocieSeps.Select(a => a.TypeParticipationSep) });
            return GetSocieteId(societeCI, materiel, personnel);
        }

        public int GetSocieteId(SocieteEnt societe, ValorisationMaterielModel materiel, PersonnelEnt personnel)
        {
            if (societe.TypeSociete.Code == TypeSociete.Sep)
            {
                return societe.AssocieSeps.SingleOrDefault(x => x.TypeParticipationSep.Code == TypeParticipationSep.Gerant && x.AssocieSepParentId == null).SocieteAssocieeId;
            }
            else if (materiel != null)
            {
                return materiel.SocieteId;
            }
            else if (personnel != null)
            {
                return personnel.SocieteId ?? 0;
            }
            else
            {
                return 0;
            }
        }

        private void UpdateValorisationFromPointageTache(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            if (pointage.PersonnelId.HasValue)
            {
                PersonnelEnt perso = personnelRepository.GetPersonnel(pointage.PersonnelId.Value);
                if (perso.IsInterimaire)
                {
                    UpdateValorisationInterimaireFromPointage(pointage, pointageTache, perso);
                }
                else
                {
                    UpdateValorisationPersonnelFromPointage(pointage, pointageTache);
                }
            }
            else
            {
                int? personnelIdBDD = pointageRepository.GetById(pointage.RapportLigneId).PersonnelId;
                if (personnelIdBDD.HasValue)
                {
                    DeleteValorisationPersonnel(pointage, pointageTache, personnelIdBDD.Value);
                }
            }

            if (pointage.MaterielId.HasValue)
            {
                ValorisationMaterielModel matos = GetValorisationMaterielByMaterielId(pointage.MaterielId.Value);
                if (matos.IsMaterielLocation)
                {
                    UpdateValorisationMaterielExterneFromPointage(pointage, pointageTache, matos);
                }
                else
                {
                    UpdateValorisationMaterielFromPointage(pointage, pointageTache);
                }
            }
            else
            {
                int? materielIdBDD = pointageRepository.GetById(pointage.RapportLigneId).MaterielId;
                if (materielIdBDD.HasValue)
                {
                    DeleteValorisationMateriel(pointage, pointageTache, materielIdBDD.Value);
                }
            }
        }

        private void UpdateValorisationPersonnelFromPointage(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            PersonnelEnt perso = personnelRepository.GetPersonnel(pointage.PersonnelId.Value);
            RapportLigneEnt pointageBdd = pointageRepository.GetById(pointage.RapportLigneId);
            ValorisationEnt valoPerso = GetValorisation(pointage.RapportLigneId, pointageTache.TacheId, perso.PersonnelId, null);
            if (valoPerso != null && perso.PersonnelId == pointageBdd.PersonnelId)
            {
                if (perso?.RessourceId.HasValue == true && perso.SocieteId.HasValue)
                {
                    int societeId = GetSocieteIdByPointage(pointage, null, perso);
                    ReferentielEtenduEnt referentielEtenduPerso = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(perso.RessourceId.Value, societeId, true);
                    if (referentielEtenduPerso != null)
                    {
                        UpdateValorisationPersonnel(valoPerso, pointage, pointageBdd, pointageTache, perso, referentielEtenduPerso);
                    }
                }
            }
            else
            {
                if (pointageBdd != null)
                {
                    DeleteValorisationOnPersonnelOrTacheUpdate(pointageBdd, pointageTache, perso);
                }
                if (Math.Abs(pointageTache.HeureTache) > 0)
                {
                    InsertValorisationPersonnelFromPointage(perso, pointage, pointageTache, "Rapport");
                }
            }
        }

        private void DeleteValorisationOnPersonnelOrTacheUpdate(RapportLigneEnt pointageBdd, RapportLigneTacheEnt pointageTache, PersonnelEnt perso)
        {
            RapportLigneTacheEnt pointageTacheBDD = pointageRepository.GetRapportLigneTacheById(pointageTache.RapportLigneTacheId);
            if (pointageBdd.PersonnelId.HasValue && perso.PersonnelId != pointageBdd.PersonnelId)
            {
                DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
            }
            if (pointageTacheBDD.TacheId != pointageTache.TacheId)
            {
                DeleteValorisationForPersonnelOnly(pointageBdd, pointageTacheBDD);
            }
        }

        private void UpdateValorisationPersonnel(ValorisationEnt valoPerso, RapportLigneEnt pointage, RapportLigneEnt pointageBdd, RapportLigneTacheEnt pointageTache, PersonnelEnt perso, ReferentielEtenduEnt referentielEtenduPerso)
        {
            if (!valoPerso.VerrouPeriode)
            {
                valoPerso.Date = pointage.DatePointage;
                valoPerso.Quantite = (decimal)pointageTache.HeureTache;
                if (valoPerso.Quantite > 0)
                {
                    CalculMontantPersonnelForUpdate(valoPerso, pointage, perso, referentielEtenduPerso.ReferentielEtenduId);
                    Repository.Update(valoPerso);
                }
                else
                {
                    DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
                }
            }
        }

        private void UpdateValorisationMaterielFromPointage(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            ValorisationMaterielModel materiel = GetValorisationMaterielByMaterielId(pointage.MaterielId.Value);

            if (materiel?.IsMaterielLocation == false)
            {
                RapportLigneEnt pointageBdd = pointageRepository.GetById(pointage.RapportLigneId);
                ValorisationEnt valoMatos = GetValorisation(pointage.RapportLigneId, pointageTache.TacheId, null, materiel.MaterielId);
                if (valoMatos != null && materiel.MaterielId == pointageBdd.MaterielId)
                {
                    int societeId = GetSocieteIdByPointage(pointage, materiel, null);
                    ReferentielEtenduEnt referentielEtenduMatos = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(materiel.RessourceId, societeId, true);
                    if (referentielEtenduMatos != null)
                    {
                        BaremeExploitationComposanteMaterielModel composanteMateriel = GetComposantesBaremeMateriel(pointage, referentielEtenduMatos, materiel);
                        UpdateValorisationMateriel(valoMatos, pointage, pointageBdd, pointageTache, composanteMateriel);
                    }
                }
                else
                {
                    if (pointageBdd.MaterielId.HasValue && materiel.MaterielId != pointageBdd.MaterielId)
                    {
                        DeleteValorisationForMaterielOnly(pointageBdd, pointageTache);
                    }

                    if (Math.Abs(pointageTache.HeureTache) > 0)
                    {
                        InsertValorisationMaterielFromPointage(pointage, pointageTache, "Rapport");
                    }
                }
            }
        }

        private void UpdateValorisationMateriel(ValorisationEnt valoMatos, RapportLigneEnt pointage, RapportLigneEnt pointageBdd, RapportLigneTacheEnt pointageTache, BaremeExploitationComposanteMaterielModel composanteMateriel)
        {
            if (!valoMatos.VerrouPeriode)
            {
                valoMatos.Date = pointage.DatePointage;
                valoMatos.Quantite = (decimal)pointageTache.HeureTache;
                if (valoMatos.Quantite > 0)
                {
                    CalculMontantMateriel(valoMatos, pointage, composanteMateriel.Composante.Prix, composanteMateriel.Composante.PrixChauffeur);
                    Repository.Update(valoMatos);
                }
                else
                {
                    DeleteValorisationForMaterielOnly(pointageBdd, pointageTache);
                }
            }
        }

        private void UpdateValorisationMaterielExterne(ValorisationEnt valoMatos, RapportLigneEnt pointage, RapportLigneEnt pointageBdd, RapportLigneTacheEnt pointageTache, ValorisationMaterielModel materielExterne)
        {
            if (pointage.MaterielId.HasValue && !pointageBdd.MaterielId.HasValue)
            {
                DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
            }
            else
            {
                valoMatos.Date = pointage.DatePointage;
                valoMatos.Quantite = (decimal)pointageTache.HeureTache;
                if (!valoMatos.Quantite.Equals(0))
                {
                    CalculMontantMaterielExterne(materielExterne, valoMatos);
                    Repository.Update(valoMatos);
                }
                else
                {
                    DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
                }
            }
        }

        private void UpdateValorisationMaterielExterneFromPointage(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, ValorisationMaterielModel materielExterne)
        {
            RapportLigneEnt pointageBdd = pointageRepository.GetById(pointage.RapportLigneId);
            ValorisationEnt valoMatos = GetValorisation(pointage.RapportLigneId, pointageTache.TacheId, null, materielExterne.MaterielId);

            if (valoMatos != null && materielExterne.MaterielId == pointageBdd.MaterielId)
            {
                UpdateValorisationMaterielExterne(valoMatos, pointage, pointageBdd, pointageTache, materielExterne);
            }
            else
            {
                if (pointageBdd.MaterielId.HasValue && materielExterne.MaterielId != pointageBdd.MaterielId)
                {
                    DeleteValorisationForMaterielOnly(pointageBdd, pointageTache);
                }

                if (Math.Abs(pointageTache.HeureTache) > 0)
                {
                    InsertValorisationMaterielExterneFromPointage(materielExterne, pointage, pointageTache, "Rapport");
                }
            }
        }

        private void UpdateValorisationInterimaireFromPointage(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, PersonnelEnt interim)
        {
            ContratInterimaireEnt contratActif = personnelRepository.GetContratInterimaireActive(interim.PersonnelId, pointage.DatePointage);
            if (contratActif != null)
            {
                RapportLigneEnt pointageBdd = pointageRepository.GetById(pointage.RapportLigneId);
                ValorisationEnt valoInterim = GetValorisation(pointage.RapportLigneId, pointageTache.TacheId, interim.PersonnelId, null);
                if (valoInterim != null && interim.PersonnelId == pointageBdd.PersonnelId)
                {
                    UpdateValorisationInterimaire(valoInterim, pointage, pointageBdd, pointageTache, contratActif);
                }
                else
                {
                    if (pointageBdd != null)
                    {
                        DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
                    }
                    if (Math.Abs(pointageTache.HeureTache) > 0)
                    {
                        InsertValorisationInterimaireFromPointage(interim, pointage, pointageTache, "Rapport");
                    }
                }
            }
        }

        private void UpdateValorisationInterimaire(ValorisationEnt valoInterim, RapportLigneEnt pointage, RapportLigneEnt pointageBdd, RapportLigneTacheEnt pointageTache, ContratInterimaireEnt contratActif)
        {
            if (!valoInterim.VerrouPeriode)
            {
                valoInterim.Date = pointage.DatePointage;
                valoInterim.Quantite = (decimal)pointageTache.HeureTache;
                if (!valoInterim.Quantite.Equals(0))
                {
                    CalculMontantInterimaire(valoInterim, pointage, contratActif);
                    Repository.Update(valoInterim);
                }
                else
                {
                    DeleteValorisationForPersonnelOnly(pointageBdd, pointageTache);
                }
            }
        }

        private void DeleteValorisation(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            if (pointage.PersonnelId.HasValue)
            {
                DeleteValorisationPersonnel(pointage, pointageTache, pointage.PersonnelId.Value);
            }

            if (pointage.MaterielId.HasValue)
            {
                DeleteValorisationMateriel(pointage, pointageTache, pointage.MaterielId.Value);
            }
        }

        private void DeleteValorisationForPersonnelOnly(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            if (pointage.PersonnelId.HasValue)
            {
                DeleteValorisationPersonnel(pointage, pointageTache, pointage.PersonnelId.Value);
            }
        }

        private void DeleteValorisationForMaterielOnly(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache)
        {
            if (pointage.MaterielId.HasValue)
            {
                DeleteValorisationMateriel(pointage, pointageTache, pointage.MaterielId.Value);
            }
        }

        private void DeleteValorisationPersonnel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, int personnelId)
        {
            Repository.DeleteValorisations(Repository.Get().Where(v => v.RapportLigneId == pointage.RapportLigneId && v.TacheId == pointageTache.TacheId && v.PersonnelId == personnelId && !v.VerrouPeriode).ToList());
        }

        private void DeleteValorisationMateriel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, int materielId)
        {
            Repository.DeleteValorisations(Repository.Get().Where(v => v.RapportLigneId == pointage.RapportLigneId && v.TacheId == pointageTache.TacheId && v.MaterielId == materielId && !v.VerrouPeriode).ToList());
        }

        /// <summary>
        /// Supprime les valorisations en fonction des paramètres
        /// </summary>
        /// <param name="ciId">identifiant du Ci</param>
        /// <param name="periode">Période</param>
        private void DeleteValorisationByCiIdFromPeriode(int ciId, DateTime periode)
        {
            IReadOnlyList<ValorisationEnt> valorisations = Repository.GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(ciId, periode);
            Repository.DeleteValorisations(valorisations.ToList());
        }

        private void DeleteValorisationByCiIdsFromPeriode(List<int> ciIds, DateTime periode)
        {
            Task<IReadOnlyList<ValorisationEnt>> valorisations = Repository.GetValorisationWithoutInterimaireOrMaterielExterneFromCiIdsAfterPeriodeWithQuantityGreaterThanZeroAndWithoutLockPeriod(ciIds, periode);
            Repository.DeleteValorisations(valorisations.Result.ToList());
        }

        /// <summary>
        /// Supprime les valorisations en fonction des paramètres
        /// </summary>
        /// <param name="ciId">identifiant du Ci</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="periode">Période</param>
        private void DeleteValorisationPersonnelByCiIdFromPeriode(int ciId, int personnelId, DateTime periode)
        {
            List<ValorisationEnt> valorisationList = Repository.Get().Where(v => v.CiId == ciId && v.PersonnelId == personnelId && v.Date >= periode && !v.VerrouPeriode).ToList();
            Repository.DeleteValorisations(valorisationList);
        }

        /// <summary>
        /// Supprime les valorisations en fonction des paramètres
        /// </summary>
        /// <param name="ciId">identifiant du Ci</param>
        /// <param name="materielId">Identifiant du matériel</param>
        /// <param name="periode">Période</param>
        private void DeleteValorisationMateriel(int ciId, int materielId, DateTime periode)
        {
            List<ValorisationEnt> valorisationList = Repository.Get().Where(v => v.CiId == ciId && v.MaterielId == materielId && v.Date >= periode && !v.VerrouPeriode).ToList();
            Repository.DeleteValorisations(valorisationList);
        }
        #endregion

        #region FromBareme
        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période comptable</param>
        public void UpdateValorisationFromListBareme(int ciId, int userId, DateTime? periode = null)
        {
            if (periode.HasValue)
            {
                // Suppresion des lignes de valorisations existantes pour la période
                DeleteValorisationByCiIdFromPeriode(ciId, periode.Value);

                // Insertion des lignes en fonction des pointages de la période
                RefreshValorisationFromPointages(ciId, periode.Value, "Bareme");
            }
        }

        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes Strom
        /// </summary>
        /// <param name="orgaId">Identifiant de l'organisation</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période comptable</param>
        public void UpdateValorisationFromListBaremeStrom(int orgaId, int userId, DateTime? periode = null)
        {
            if (periode.HasValue)
            {
                OrganisationEnt orga = organisationManager.GetOrganisationById(orgaId);
                List<CIEnt> ciList = ciManager.GetCIList(orgaId).ToList();
                List<int> ciIdList = new List<int>();

                if (orga.Societe != null)
                {
                    ciIdList = ciList.Where(ci => ci.Sep).Select(ci => ci.CiId).ToList();

                    if (ciIdList.Count() == 0)
                    {
                        ciIdList = ciList.Where(ci => !ci.Sep).Select(ci => ci.CiId).ToList();
                    }
                }
                else if (orga.Etablissement != null)
                {
                    ciIdList = ciList.Where(ci => !ci.Sep).Select(ci => ci.CiId).ToList();
                }

                // Suppresion des lignes de valorisations existantes pour la période
                DeleteValorisationByCiIdsFromPeriode(ciIdList, periode.Value);

                // Insertion des lignes en fonction des pointages de la période
                RefreshValorisationFromPointagesAndCiIds(ciIdList, periode.Value, "Bareme");
            }
        }

        private void RefreshValorisationFromPointages(int ciId, DateTime periode, string source)
        {
            List<RapportLigneEnt> pointages = pointageRepository.GetListPointagesReelsByCiIdFromPeriode(ciId, periode).Where(o =>
           !(o.PersonnelId.HasValue && o.Personnel.IsInterimaire)
           && !(o.MaterielId.HasValue && o.Materiel.MaterielLocation)).ToList();
            CreateValorisation(pointages, source);
            Save();
        }

        private void RefreshValorisationFromPointagesAndCiIds(List<int> ciIds, DateTime periode, string source)
        {
            List<RapportLigneEnt> pointages = pointageRepository.GetListPointagesReelsByCiIdListFromPeriode(ciIds, periode).Where(o =>
           (o.PersonnelId.HasValue && !o.Personnel.IsInterimaire) || (o.MaterielId.HasValue && !o.Materiel.MaterielLocation)).ToList();
            CreateValorisation(pointages, source);
            Save();
        }

        private void RefreshValorisationFromPersonnel(int ciId, DateTime periode, int personnelId, string source, DateTime? periodeDebutBaremeCourant = null)
        {
            IEnumerable<RapportLigneEnt> pointages = pointageRepository.GetListPointagesReelsByCiIdFromPeriode(ciId, periode);
            List<RapportLigneEnt> pointagesPersonnel = pointages.Where(p => p.PersonnelId.HasValue && p.PersonnelId == personnelId).ToList();

            CreateValorisationForPersonnel(pointagesPersonnel, source, periodeDebutBaremeCourant);
            Save();
        }

        private void RecreatedValorisationFromMaterielId(int ciId, DateTime periode, int materielId, string source, DateTime? periodeDebutBaremeCourant = null)
        {
            IEnumerable<RapportLigneEnt> pointages = pointageRepository.GetListPointagesReelsByCiIdFromPeriode(ciId, periode);
            List<RapportLigneEnt> pointagesMateriel = pointages.Where(p => p.MaterielId.HasValue && p.MaterielId == materielId).ToList();

            CreateValorisationForMateriel(pointagesMateriel, source, periodeDebutBaremeCourant);
            Save();
        }

        private void CreateValorisationForMateriel(List<RapportLigneEnt> pointages, string source, DateTime? periode = null)
        {
            listNewBareme.Clear();
            listNewBaremeStorm.Clear();
            InsertValorisationForMateriel(pointages, source, periode);
        }

        private void InsertValorisationForMateriel(List<RapportLigneEnt> pointages, string source, DateTime? periode = null)
        {
            IEnumerable<RapportLigneEnt> rapportLigneList = pointages.Where(q => !q.Cloture && q.MaterielId.HasValue);
            IEnumerable<RapportLigneTacheEnt> rapportLigneTacheList = rapportLigneList.SelectMany(p => p.ListRapportLigneTaches).Where(q => q.HeureTache != 0);

            List<ValorisationRapportLigneEtTache> valoMaterielRapportLigneEtTache = (from pointage in rapportLigneList
                                                                                     join ligne in rapportLigneTacheList on pointage.RapportLigneId equals ligne.RapportLigneId
                                                                                     select new ValorisationRapportLigneEtTache
                                                                                     {
                                                                                         RapportLigne = pointage,
                                                                                         RapportLigneTache = ligne
                                                                                     }).ToList();

            InsertValorisationMaterielFromPointage(valoMaterielRapportLigneEtTache, source, periode);
            Save();
        }

        private void InsertValorisationMaterielFromPointage(List<ValorisationRapportLigneEtTache> valoMaterielRapportLigneEtTache, string source, DateTime? periode = null)
        {
            foreach (ValorisationRapportLigneEtTache valoMateriel in valoMaterielRapportLigneEtTache)
            {
                bool isVerrouPeriode = GetVerrouPeriodeTrueValorisation(valoMateriel.RapportLigne.RapportId, valoMateriel.RapportLigne.RapportLigneId);

                if (!isVerrouPeriode)
                {
                    InsertValorisationMaterielFromPointage(valoMateriel.RapportLigne, valoMateriel.RapportLigneTache, source, periode);
                }
            }
        }

        #endregion

        #region FromPersonnel
        /// <summary>
        /// Met à jour des lignes de valo lors d'un changement de ressource sur un personnel
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période toujours null dans cette méthode</param>
        public void UpdateValorisationFromPersonnel(int personnelId, int userId, DateTime? periode = null)
        {
            List<int> ciList = (from RapportLigneEnt rapportLigne in pointageRepository.Get().Where(rl => rl.PersonnelId == personnelId).ToList()
                                where !datesClotureComptableManager.IsPeriodClosed(rapportLigne.CiId, rapportLigne.DatePointage.Year, rapportLigne.DatePointage.Month)
                                select rapportLigne.CiId).Distinct().ToList();


            foreach (int ciId in ciList)
            {
                DateTime currentPeriod = datesClotureComptableManager.GetLastDateClotureByCiID(ciId);

                DateTime? periodeDebutBaremeCourant = baremeExploitationCIRepository.GetPeriodeDebutBaremeCourant(ciId);

                // Suppresion des lignes de valorisations existantes pour la période
                DeleteValorisationPersonnelByCiIdFromPeriode(ciId, personnelId, currentPeriod);

                // Insertion des lignes en fonction des pointages de la période
                RefreshValorisationFromPersonnel(ciId, currentPeriod, personnelId, "Personnel", periodeDebutBaremeCourant);
            }
        }
        #endregion

        #region FromMateriel
        /// <summary>
        /// Met à jour des lignes de valo lors d'un changement de ressource sur un matériel
        /// </summary>
        /// <param name="materielId">Identifiant du materiel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période toujours null dans cette méthode</param>
        public void UpdateValorisationFromMateriel(int materielId, int userId, DateTime? periode = null)
        {
            List<int> ciIds = (from RapportLigneEnt rapportLigne in pointageRepository.Get().Where(rl => rl.MaterielId == materielId).ToList()
                               where !datesClotureComptableManager.IsPeriodClosed(rapportLigne.CiId, rapportLigne.DatePointage.Year, rapportLigne.DatePointage.Month)
                               select rapportLigne.CiId).Distinct().ToList();

            foreach (int ciId in ciIds)
            {
                DateTime currentPeriod = datesClotureComptableManager.GetLastDateClotureByCiID(ciId);

                DateTime? periodeDebutBaremeCourant = GetPeriodeDebutBaremeOrgaCourant(ciId);

                // Suppresion des lignes de valorisations existantes pour la période
                DeleteValorisationMateriel(ciId, materielId, currentPeriod);

                // Insertion des lignes en fonction des pointages de la période
                RecreatedValorisationFromMaterielId(ciId, currentPeriod, materielId, "Materiel", periodeDebutBaremeCourant);
            }
        }

        private DateTime? GetPeriodeDebutBaremeOrgaCourant(int ciId)
        {
            int orgaId = 0;
            SocieteEnt societe = ciManager.GetSocieteByCIId(ciId);
            EtablissementComptableEnt etabl = ciManager.GetEtablissementComptableByCIId(ciId);
            if (etabl != null)
            {
                orgaId = etabl.Organisation.OrganisationId;
            }
            else if (societe != null)
            {
                orgaId = societe.Organisation.OrganisationId;
            }

            DateTime? periodeDebutBaremeCourant = baremeExploitationOrganisationRepository.GetPeriodeDebutBaremeCourant(orgaId);
            return periodeDebutBaremeCourant;
        }

        #endregion

        #region Private

        private ValorisationEnt GetValorisation(int rapportLigneId, int tacheId, int? persoId, int? matosId)
        {
            if (persoId.HasValue)
            {
                return Repository.Get().FirstOrDefault(v => v.RapportLigneId == rapportLigneId && v.PersonnelId == persoId && v.TacheId == tacheId);
            }
            if (matosId.HasValue)
            {
                return Repository.Get().FirstOrDefault(v => v.RapportLigneId == rapportLigneId && v.MaterielId == matosId && v.TacheId == tacheId);
            }

            return Repository.Get().FirstOrDefault(v => v.RapportLigneId == rapportLigneId && v.TacheId == tacheId);
        }

#pragma warning disable S107 // Methods should not have too many parameters
        private ValorisationEnt NewValorisationPersonnel(RapportLigneEnt pointage,
                                                         RapportLigneTacheEnt pointageTache,
                                                         ReferentielEtenduEnt referentielEtendu,
                                                         int uniteId,
                                                         decimal? prix,
                                                         PersonnelEnt personnel,
                                                         string source,
                                                         BaremeExploitationCIEnt bareme = null,
                                                         DateTime? periode = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            ValorisationEnt valorisation;
            if (pointage.RapportLigneId != 0)
            {
                valorisation = new ValorisationEnt()
                {
                    PersonnelId = personnel.PersonnelId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigneId = pointage.RapportLigneId,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            else
            {
                valorisation = new ValorisationEnt()
                {
                    PersonnelId = personnel.PersonnelId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigne = pointage,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            SetValorisationPUHTForPersonnel(valorisation, pointage, prix, periode);

            valorisation.UniteId = uniteId;

            SetValorisationMontant(valorisation);

            if (bareme != null)
            {
                valorisation.Bareme = bareme;
            }

            return valorisation;
        }

        private void SetValorisationMontant(ValorisationEnt valorisation)
        {
            valorisation.Montant = decimal.Round(valorisation.PUHT * valorisation.Quantite, 2, MidpointRounding.AwayFromZero);
        }

        private ValorisationEnt NewValorisationInterimaire(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, ReferentielEtenduEnt referentielEtendu, PersonnelEnt personnel, ContratInterimaireEnt contrat, string source)
        {
            ValorisationEnt valorisation;
            if (pointage.RapportLigneId != 0)
            {
                valorisation = new ValorisationEnt()
                {
                    PersonnelId = personnel.PersonnelId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigneId = pointage.RapportLigneId,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            else
            {
                valorisation = new ValorisationEnt()
                {
                    PersonnelId = personnel.PersonnelId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigne = pointage,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }

            CalculMontantInterimaire(valorisation, pointage, contrat);

            return valorisation;
        }
        public ValorisationEnt NewValorisationMateriel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel, string source, DateTime? periode = null)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel = GetComposantesBaremeMateriel(pointage, referentielEtendu, materiel, periode);

            ValorisationEnt valorisation;
            if (pointage.RapportLigneId != 0)
            {
                valorisation = new ValorisationEnt()
                {
                    MaterielId = materiel.MaterielId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigneId = pointage.RapportLigneId,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    UniteId = composanteMateriel.Composante.UniteId,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            else
            {
                valorisation = new ValorisationEnt()
                {
                    MaterielId = materiel.MaterielId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigne = pointage,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    UniteId = composanteMateriel.Composante.UniteId,
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            if (composanteMateriel.IsStorm)
            {
                valorisation.BaremeStorm = composanteMateriel.BaremeOrganisation;
            }
            else
            {
                valorisation.Bareme = composanteMateriel.BaremeCI;
            }
            CalculMontantMateriel(valorisation, pointage, composanteMateriel.Composante.Prix, composanteMateriel.Composante.PrixChauffeur);

            return valorisation;
        }

        private ValorisationEnt NewValorisationMaterielExterne(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel, string source)
        {
            ValorisationEnt valorisation;
            if (pointage.RapportLigneId != 0)
            {
                valorisation = new ValorisationEnt()
                {
                    MaterielId = materiel.MaterielId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigneId = pointage.RapportLigneId,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    Quantite = (decimal)pointageTache.HeureTache,
                    PUHT = materiel.CommandeLignes.Select(c => c.PUHT).FirstOrDefault(),
                    UniteId = (int)materiel.CommandeLignes.Select(c => c.UniteId).FirstOrDefault(),
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }
            else
            {
                valorisation = new ValorisationEnt()
                {
                    MaterielId = materiel.MaterielId,
                    CiId = pointage.CiId,
                    RapportId = pointage.RapportId,
                    RapportLigne = pointage,
                    TacheId = pointageTache.TacheId,
                    ReferentielEtenduId = referentielEtendu.ReferentielEtenduId,
                    SousChapitreId = referentielEtendu.Ressource.SousChapitreId,
                    ChapitreId = referentielEtendu.Ressource.SousChapitre.ChapitreId,
                    Date = pointage.DatePointage,
                    PUHT = materiel.CommandeLignes.Select(c => c.PUHT).FirstOrDefault(),
                    Quantite = (decimal)pointageTache.HeureTache,
                    UniteId = (int)materiel.CommandeLignes.Select(c => c.UniteId).FirstOrDefault(),
                    DeviseId = deviseManager.GetDeviseIdByCode(CodeDevise.Euro).Value,
                    DateCreation = DateTime.UtcNow,
                    Source = source
                };
            }

            CalculMontantMaterielExterne(materiel, valorisation);

            return valorisation;
        }

        private BaremeExploitationOrganisationEnt GetBaremeMaterielStormFromOrga(RapportLigneEnt pointage, ValorisationMaterielModel materiel, DateTime periode)
        {
            if (materiel == null)
            {
                return null;
            }
            var organisationTree = organisationTreeService.GetOrganisationTree();
            var ciId = pointage.CiId;
            var ciOrganisation = organisationTree.GetCi(ciId);
            if (ciOrganisation == null)
            {
                return null;
            }
            var etablissementOrganisation = organisationTree.GetEtablissementComptableOfCi(ciId);
            if (etablissementOrganisation != null)
            {
                return baremeExploitationOrganisationRepository.Get(etablissementOrganisation.OrganisationId, periode, materiel.RessourceId);
            }
            var societeOrganisation = organisationTree.GetSocieteParentOfCi(ciId);
            if (societeOrganisation != null)
            {
                return baremeExploitationOrganisationRepository.Get(societeOrganisation.OrganisationId, periode, materiel.RessourceId);
            }
            return null;
        }

        private void SetValorisationPUHTForPersonnel(ValorisationEnt valorisation, RapportLigneEnt pointage, decimal? prix, DateTime? periode = null)
        {
            valorisation.PUHT = prix.Value;

            if (!pointage.MaterielId.HasValue)
            {
                return;
            }

            ValorisationMaterielModel valorisationMateriel = pointage.Materiel == null ?
                GetValorisationMaterielByMaterielId(pointage.MaterielId.Value) :
                GetValorisationMaterielByMateriel(pointage.Materiel);
            ReferentielEtenduEnt referentielEtenduMateriel = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(valorisationMateriel.RessourceId, valorisationMateriel.SocieteId, true);

            if (valorisationMateriel.IsStorm)
            {
                DateTime periodeValorisation = periode ?? pointage.DatePointage;
                BaremeExploitationOrganisationEnt baremeStorm = GetBaremeMaterielStormFromOrga(pointage, valorisationMateriel, periodeValorisation);

                if (baremeStorm == null)
                {
                    return;
                }

                SetValorisationPUHTWithBaremeStorm(valorisation, baremeStorm);
            }
            else
            {
                SurchargeBaremeExploitationCIEnt surcharge = baremeExploitationCISurchargeRepository.GetSurcharge(pointage.CiId, pointage.DatePointage, null, pointage.MaterielId.Value);
                BaremeExploitationCIEnt baremeCi = baremeExploitationCIRepository.GetUnclosed(pointage.CiId, pointage.DatePointage, referentielEtenduMateriel?.ReferentielEtenduId ?? 0);

                if (baremeCi == null && surcharge == null)
                {
                    return;
                }

                SetValorisationPUHTWithBaremeCi(valorisation, surcharge, baremeCi);
            }
        }

        private void SetValorisationPUHTWithBaremeCi(ValorisationEnt valorisation, SurchargeBaremeExploitationCIEnt surcharge, BaremeExploitationCIEnt baremeCi)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel = GenerateComposanteMateriel(baremeCi, surcharge);

            if (composanteMateriel.BaremeCI != null && composanteMateriel.BaremeCI.PrixConduite.HasValue)
            {
                valorisation.PUHT = composanteMateriel.BaremeCI.PrixConduite.Value;
            }
        }

        private void SetValorisationPUHTWithBaremeStorm(ValorisationEnt valorisation, BaremeExploitationOrganisationEnt baremeStorm)
        {
            BaremeExploitationComposanteMaterielModel composanteMaterielStorm = GenerateComposanteMaterielStorm(baremeStorm);

            if (composanteMaterielStorm.BaremeOrganisation != null && composanteMaterielStorm.BaremeOrganisation.PrixConduite.HasValue)
            {
                valorisation.PUHT = composanteMaterielStorm.BaremeOrganisation.PrixConduite.Value;
            }
        }

        private void CalculMontantPersonnelForUpdate(ValorisationEnt valorisation, RapportLigneEnt pointage, PersonnelEnt personnel, int referentielEtenduId, BaremeExploitationCIEnt bareme = null, SurchargeBaremeExploitationCIEnt surcharge = null)
        {
            if (surcharge == null) { surcharge = baremeExploitationCISurchargeRepository.GetSurcharge(pointage.CiId, pointage.DatePointage, personnel.PersonnelId, null); }
            if (bareme == null) { bareme = baremeExploitationCIRepository.GetUnclosed(pointage.CiId, pointage.DatePointage, referentielEtenduId); }
            if (bareme != null || surcharge != null)
            {
                int uniteId = surcharge != null ? surcharge.UniteId : bareme.UniteId;
                decimal? prix = surcharge != null ? surcharge.Prix : bareme.Prix;
                SetValorisationPUHTForPersonnel(valorisation, pointage, prix);

                valorisation.UniteId = uniteId;
                SetValorisationMontant(valorisation);
                if (bareme != null)
                {
                    valorisation.Bareme = bareme;
                }
            }
        }

        private void CalculMontantInterimaire(ValorisationEnt valorisation, RapportLigneEnt pointage, ContratInterimaireEnt contrat)
        {
            if (pointage.MaterielId.HasValue)
            {
                MaterielEnt materiel = materielRepository.GetMaterielById(pointage.MaterielId.Value);
                ReferentielEtenduEnt referentielEtenduMatos = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete(materiel.RessourceId, materiel.SocieteId, true);
                SurchargeBaremeExploitationCIEnt surchargeMatos = baremeExploitationCISurchargeRepository.GetSurcharge(pointage.CiId, pointage.DatePointage, null, pointage.MaterielId.Value);
                BaremeExploitationCIEnt baremeMatos = baremeExploitationCIRepository.GetUnclosed(pointage.CiId, pointage.DatePointage, referentielEtenduMatos?.ReferentielEtenduId ?? 0);
                if (baremeMatos != null || surchargeMatos != null)
                {
                    decimal? prixConduite = surchargeMatos != null ? surchargeMatos.PrixConduite : baremeMatos.PrixConduite;
                    if (prixConduite.HasValue)
                    {
                        valorisation.PUHT = prixConduite.Value;
                    }
                    else
                    {
                        valorisation.PUHT = contrat.Valorisation;
                    }
                }
                else
                {
                    valorisation.PUHT = contrat.Valorisation;
                }
            }
            else
            {
                valorisation.PUHT = contrat.Valorisation;
            }
            valorisation.UniteId = contrat.UniteId;
            SetValorisationMontant(valorisation);
        }

        private void CalculMontantMateriel(ValorisationEnt valorisation, RapportLigneEnt pointage, decimal? prix, decimal? prixChauffeur)
        {
            if (prix.HasValue)
            {
                if (pointage.AvecChauffeur && prixChauffeur.HasValue)
                {
                    valorisation.PUHT = prix.Value + prixChauffeur.Value;
                }
                else
                {
                    valorisation.PUHT = prix.Value;
                }
                SetValorisationMontant(valorisation);
            }
        }

        private void CalculMontantMaterielExterne(ValorisationMaterielModel materiel, ValorisationEnt valorisation)
        {
            CommandeLigneModel commandeLigneModel = materiel.CommandeLignes.FirstOrDefault();

            if (commandeLigneModel.Unite.Code == CodeUnite.Jour)
            {
                valorisation.PUHT = commandeLigneModel.PUHT / 8;
            }
            else if (commandeLigneModel.Unite.Code == CodeUnite.Semaine)
            {
                valorisation.PUHT = commandeLigneModel.PUHT / 40;
            }

            valorisation.PUHT = decimal.Round(valorisation.PUHT, 2, MidpointRounding.AwayFromZero);
            SetValorisationMontant(valorisation);
        }

        private BaremeExploitationCIEnt GetDefaultBaremeCI(int ciId, int referentielEtenduId, DateTime periode)
        {
            UniteEnt defaultUnite = uniteManager.GetUnite(CodeUnite.Heure);
            int? defaultDevise = deviseManager.GetDeviseIdByCode(CodeDevise.Euro);
            if (defaultUnite != null && defaultDevise.HasValue)
            {
                return new BaremeExploitationCIEnt()
                {
                    CIId = ciId,
                    ReferentielEtenduId = referentielEtenduId,
                    UniteId = defaultUnite.UniteId,
                    DeviseId = defaultDevise.Value,
                    DateCreation = DateTime.UtcNow,
                    AuteurCreationId = 1,
                    PeriodeDebut = periode,
                    Prix = PrixBaremeParDefaut,
                    PrixChauffeur = 30,
                    PrixConduite = null
                };
            }
            else
            {
                return null;
            }
        }

        private BaremeExploitationCIEnt GetOrInsertNewDefaultBareme(BaremeExploitationCIEnt defaultBareme)
        {
            if (!listNewBareme.Any(b => b.CIId == defaultBareme.CIId && b.PeriodeDebut == defaultBareme.PeriodeDebut && b.ReferentielEtenduId == defaultBareme.ReferentielEtenduId))
            {
                listNewBareme.Add(defaultBareme);
                baremeExploitationCIRepository.Insert(defaultBareme);
            }
            else
            {
                defaultBareme = listNewBareme.FirstOrDefault(b => b.CIId == defaultBareme.CIId && b.PeriodeDebut == defaultBareme.PeriodeDebut && b.ReferentielEtenduId == defaultBareme.ReferentielEtenduId);
            }
            return defaultBareme;
        }

        private BaremeExploitationOrganisationEnt GetDefaultBaremeOrganisation(int ciId, int ressourceId, DateTime periode)
        {
            int orgaId = 0;
            CIEnt ci = ciManager.GetCIById(ciId);
            if (ci != null)
            {
                SocieteEnt societe = ciManager.GetSocieteByCIId(ci.CiId);
                EtablissementComptableEnt etabl = ciManager.GetEtablissementComptableByCIId(ci.CiId);
                if (etabl != null)
                {
                    orgaId = etabl.Organisation.OrganisationId;
                }
                else
                {
                    orgaId = societe.Organisation.OrganisationId;
                }
            }

            UniteEnt defaultUnite = uniteManager.GetUnite(CodeUnite.Heure);
            int? defaultDevise = deviseManager.GetDeviseIdByCode(CodeDevise.Euro);
            if (defaultUnite != null && defaultDevise.HasValue && orgaId != 0)
            {
                return new BaremeExploitationOrganisationEnt()
                {
                    OrganisationId = orgaId,
                    RessourceId = ressourceId,
                    UniteId = defaultUnite.UniteId,
                    DeviseId = defaultDevise.Value,
                    DateCreation = DateTime.UtcNow,
                    AuteurCreationId = 1,
                    Statut = 1,
                    PeriodeDebut = periode,
                    Prix = PrixBaremeParDefaut,
                    PrixChauffeur = 30,
                    PrixConduite = null
                };
            }
            else
            {
                return null;
            }
        }

        private BaremeExploitationComposanteMaterielModel GetComposantesBaremeMateriel(RapportLigneEnt pointage, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel, DateTime? periode = null)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel;
            if (materiel.IsStorm)
            {
                composanteMateriel = GetComposantesMaterielSTORM(pointage, referentielEtendu, materiel, periode);
            }
            else
            {
                composanteMateriel = GetComposantesMateriel(pointage, referentielEtendu, materiel);
            }
            return composanteMateriel;
        }

        private BaremeExploitationComposanteMaterielModel GetComposantesMaterielSTORM(RapportLigneEnt pointage, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel, DateTime? periode = null)
        {
            DateTime periodeValorisation = periode ?? pointage.DatePointage;

            BaremeExploitationOrganisationEnt baremeStorm = GetBaremeMaterielStormFromOrga(pointage, materiel, periodeValorisation);
            if (baremeStorm == null)
            {
                baremeStorm = GetDefaultBaremeOrganisation(pointage.CiId, referentielEtendu.RessourceId, periodeValorisation.GetPeriode());
                if (!listNewBaremeStorm.Any(b => b.OrganisationId == baremeStorm.OrganisationId && b.PeriodeDebut == baremeStorm.PeriodeDebut && b.RessourceId == baremeStorm.RessourceId))
                {
                    listNewBaremeStorm.Add(baremeStorm);
                    baremeExploitationOrganisationRepository.Insert(baremeStorm);
                }
                else
                {
                    baremeStorm = listNewBaremeStorm.FirstOrDefault(b => b.OrganisationId == baremeStorm.OrganisationId && b.PeriodeDebut == baremeStorm.PeriodeDebut && b.RessourceId == baremeStorm.RessourceId);
                }
            }
            if (baremeStorm != null)
            {
                return GenerateComposanteMaterielStorm(baremeStorm);
            }
            return null;
        }

        private BaremeExploitationComposanteMaterielModel GetComposantesMateriel(RapportLigneEnt pointage, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel = new BaremeExploitationComposanteMaterielModel();
            composanteMateriel.Composante = new BaremeExploitationComposantesModel();
            SurchargeBaremeExploitationCIEnt surcharge = baremeExploitationCISurchargeRepository.GetSurcharge(pointage.CiId, pointage.DatePointage, null, materiel.MaterielId);
            BaremeExploitationCIEnt bareme = baremeExploitationCIRepository.Get(pointage.CiId, pointage.DatePointage, referentielEtendu.ReferentielEtenduId);
            if (bareme == null)
            {
                bareme = GetDefaultBaremeCI(pointage.CiId, referentielEtendu.ReferentielEtenduId, pointage.DatePointage.GetPeriode());
                if (bareme != null)
                {
                    if (!listNewBareme.Any(b => b.CIId == bareme.CIId && b.PeriodeDebut == bareme.PeriodeDebut && b.ReferentielEtenduId == bareme.ReferentielEtenduId))
                    {
                        listNewBareme.Add(bareme);
                        baremeExploitationCIRepository.Insert(bareme);
                    }
                    else
                    {
                        bareme = listNewBareme.FirstOrDefault(b => b.CIId == bareme.CIId && b.PeriodeDebut == bareme.PeriodeDebut && b.ReferentielEtenduId == bareme.ReferentielEtenduId);
                    }
                }
            }
            if (bareme != null)
            {
                return GenerateComposanteMateriel(bareme, surcharge);
            }
            return null;
        }

        private BaremeExploitationComposanteMaterielModel GenerateComposanteMateriel(BaremeExploitationCIEnt bareme, SurchargeBaremeExploitationCIEnt surcharge)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel = new BaremeExploitationComposanteMaterielModel()
            {
                Composante = new BaremeExploitationComposantesModel()
                {
                    PrixConduite = surcharge != null ? surcharge.PrixConduite : bareme.PrixConduite,
                    PrixChauffeur = surcharge != null ? surcharge.PrixChauffeur : bareme.PrixChauffeur,
                    UniteId = surcharge != null ? surcharge.UniteId : bareme.UniteId,
                    Prix = surcharge != null ? surcharge.Prix : bareme.Prix
                },
                BaremeCI = bareme
            };
            return composanteMateriel;
        }

        private BaremeExploitationComposanteMaterielModel GenerateComposanteMaterielStorm(BaremeExploitationOrganisationEnt baremeStorm)
        {
            BaremeExploitationComposanteMaterielModel composanteMateriel = new BaremeExploitationComposanteMaterielModel()
            {
                Composante = new BaremeExploitationComposantesModel()
                {
                    PrixConduite = baremeStorm.PrixConduite,
                    PrixChauffeur = baremeStorm.PrixChauffeur,
                    UniteId = baremeStorm.UniteId,
                    Prix = baremeStorm.Prix
                },
                BaremeOrganisation = baremeStorm
            };
            return composanteMateriel;
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes des Valorisations pour éviter de les prendre en compte dans la sauvegarde du
        ///   contexte.
        /// </summary>
        /// <param name="valo">Valorisation dont les dépendances sont à détacher</param>
        private void CleanDependencies(ValorisationEnt valo)
        {
            valo.Tache = null;
            valo.CI = null;
            valo.Devise = null;
            valo.Unite = null;
            valo.Rapport = null;
            valo.RapportLigne = null;
            valo.Chapitre = null;
            valo.SousChapitre = null;
            valo.ReferentielEtendu = null;
            valo.Bareme = null;
            valo.BaremeStorm = null;
            valo.Personnel = null;
            valo.Materiel = null;
        }

        public ValorisationMaterielModel GetValorisationMaterielByMaterielId(int materielId)
        {
            MaterielEnt materiel = materielRepository.GetMaterielByIdWithCommandes(materielId);

            return GetValorisationMaterielByMateriel(materiel);
        }

        public ValorisationMaterielModel GetValorisationMaterielByMateriel(MaterielEnt materiel)
        {
            ValorisationMaterielModel valorisationMaterielModel = new ValorisationMaterielModel
            {
                MaterielId = materiel.MaterielId,
                IsMaterielLocation = materiel.MaterielLocation,
                IsStorm = materiel.IsStorm,
                RessourceId = materiel.RessourceId,
                SocieteId = materiel.SocieteId,
                CommandeLignes = new List<CommandeLigneModel>
                {
                    GetCommandeLigneByMateriel(materiel)
                }
            };

            return valorisationMaterielModel;
        }

        private CommandeLigneModel GetCommandeLigneByMateriel(MaterielEnt materiel)
        {
            CommandeLigneModel commandeLigneModel = new CommandeLigneModel();
            if (materiel.CommandeLignes != null && materiel.CommandeLignes.Any())
            {
                CommandeLigneEnt commandeLigne = materiel.CommandeLignes.FirstOrDefault();

                if (commandeLigne != null)
                {
                    commandeLigneModel.UniteId = commandeLigne.UniteId;
                    commandeLigneModel.PUHT = commandeLigne.PUHT;
                    commandeLigneModel.Unite = new UniteModel
                    {
                        Code = commandeLigne.Unite.Code
                    };
                }
            }

            return commandeLigneModel;
        }

        #endregion

        #region Totaux

        /// <summary>
        /// Retourne le total valorisé
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="periode">Période</param>
        /// <returns>total valorisé</returns>
        public decimal Total(int ciId, DateTime periode)
        {
            return Repository.Total(ciId, periode.GetPeriode(), periode.GetNextPeriode());
        }

        public decimal Total(int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return Repository.Total(ciId, dateDebut, dateFin);
        }

        public decimal Total(int ciId, DateTime dateDebut, DateTime dateFin, int? ressourceId, int? tacheId)
        {
            return Repository.Total(ciId, dateDebut, dateFin, ressourceId, tacheId);
        }

        public decimal TotalByChapitreId(int ciId, DateTime dateDebut, DateTime dateFin, int chapitreId)
        {
            return Repository.TotalByChapitreId(ciId, dateDebut, dateFin, chapitreId);
        }

        /// <summary>
        /// Renvoi le total de valorisation pour une liste de chapitre codes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreCodes">liste de chapitre codes</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        public decimal TotalByChapitreCodes(int ciId, DateTime dateDebut, DateTime dateFin, List<string> chapitreCodes)
        {
            return Repository.TotalByChapitreCodes(ciId, dateDebut, dateFin, chapitreCodes);
        }

        #endregion

        /// <summary>
        /// Ajoute une valorisation négative suite à la réception intérimaire
        /// </summary>
        /// <param name="firstPointage">premier pointage concenant la reception intérimaire</param>
        /// <param name="receptionInterimaire">réception intérimaire</param>
        public void InsertValorisationNegativeForInterimaire(RapportLigneEnt firstPointage, DepenseAchatEnt receptionInterimaire)
        {
            int societeId = (int)receptionInterimaire.CI.SocieteId;
            int ciId = (int)receptionInterimaire.CI.CiId;

            if (sepService.IsSep(ciId))
            {
                societeId = GetSocieteGeranteIdForSep(societeId, ciId);
            }

            ReferentielEtenduEnt refEtendu = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete((int)receptionInterimaire.RessourceId, societeId, true);
            if (refEtendu != null)
            {
                ValorisationEnt valorisationInterimaire = new ValorisationEnt()
                {
                    CiId = (int)receptionInterimaire.CiId,
                    RapportId = firstPointage.RapportId,
                    TacheId = (int)receptionInterimaire.TacheId,
                    ChapitreId = refEtendu.Ressource.SousChapitre.ChapitreId,
                    SousChapitreId = refEtendu.Ressource.SousChapitreId,
                    ReferentielEtenduId = refEtendu.ReferentielEtenduId,
                    UniteId = (int)receptionInterimaire.UniteId,
                    DeviseId = (int)receptionInterimaire.DeviseId,
                    PersonnelId = firstPointage.PersonnelId,
                    Date = (DateTime)receptionInterimaire.Date,
                    DateCreation = (DateTime)receptionInterimaire.DateCreation,
                    Source = "Annulation Intérimaire",
                    PUHT = receptionInterimaire.PUHT,
                    Quantite = receptionInterimaire.Quantite * -1,
                    RapportLigneId = firstPointage.RapportLigneId
                };

                SetValorisationMontant(valorisationInterimaire);

                Repository.AddValorisation(valorisationInterimaire);
                Save();
            }
        }

        private int GetSocieteGeranteIdForSep(int societeId, int ciId)
        {
            SocieteEnt societeGerante = sepService.GetSocieteGeranteForSep(ciId);

            if (societeGerante != null)
            {
                societeId = societeGerante.SocieteId;
            }

            return societeId;
        }

        /// <summary>
        /// Ajoute une valorisation négative suite à la réception materiel externe
        /// </summary>
        /// <param name="firstPointage">premier pointage concenant la reception materiel externe</param>
        /// <param name="receptionMaterielExterne">réception materiel externe</param>
        public void InsertValorisationNegativeForMaterielExterne(RapportLigneEnt firstPointage, DepenseAchatEnt receptionMaterielExterne)
        {
            int idSociete = receptionMaterielExterne.CI.SocieteId.Equals(DBNull.Value) ? 0 : Convert.ToInt32(receptionMaterielExterne.CI.SocieteId);

            if (sepService.IsSep(receptionMaterielExterne.CI.CiId.Equals(DBNull.Value) ? 0 : (int)receptionMaterielExterne.CI.CiId))
            {
                idSociete = sepService.GetSocieteGerante(idSociete).SocieteId;
            }

            ReferentielEtenduEnt refEtendu = referentielEtenduRepository.GetReferentielEtenduByRessourceAndSociete((int)receptionMaterielExterne.RessourceId, idSociete, true);
            if (refEtendu != null)
            {
                ValorisationEnt valorisationMaterielExterne = new ValorisationEnt()
                {
                    CiId = (int)receptionMaterielExterne.CiId,
                    RapportId = firstPointage.RapportId,
                    TacheId = (int)receptionMaterielExterne.TacheId,
                    ChapitreId = refEtendu.Ressource.SousChapitre.ChapitreId,
                    SousChapitreId = refEtendu.Ressource.SousChapitreId,
                    ReferentielEtenduId = refEtendu.ReferentielEtenduId,
                    UniteId = (int)receptionMaterielExterne.UniteId,
                    DeviseId = (int)receptionMaterielExterne.DeviseId,
                    MaterielId = firstPointage.MaterielId,
                    Date = (DateTime)receptionMaterielExterne.Date,
                    DateCreation = (DateTime)receptionMaterielExterne.DateCreation,
                    Source = "Annulation Matériel Externe",
                    PUHT = receptionMaterielExterne.PUHT,
                    Quantite = receptionMaterielExterne.Quantite * -1,
                    RapportLigneId = firstPointage.RapportLigneId
                };

                SetValorisationMontant(valorisationMaterielExterne);

                Repository.AddValorisation(valorisationMaterielExterne);
                Save();
            }
        }

        /// <summary>
        /// Retourne la liste des valorisations pour une liste d'identifiant de rapport de ligne
        /// </summary>
        /// <param name="rapportLigneIds">Liste de rapport de ligne</param>
        /// <returns><see cref="ValorisationEcritureComptableODModel" /></returns>
        public IReadOnlyList<ValorisationEcritureComptableODModel> GetValorisationByRapportLigneIds(List<int> rapportLigneIds)
        {
            List<ValorisationEcritureComptableODModel> valorisationEcritureComptableODModels = new List<ValorisationEcritureComptableODModel>();
            Repository.GetByListRapportLigneId(rapportLigneIds).ForEach(valo => valorisationEcritureComptableODModels.Add(
                new ValorisationEcritureComptableODModel
                {
                    ValorisationId = valo.ValorisationId,
                    Montant = valo.Montant,
                    Quantite = valo.Quantite,
                    UniteId = valo.UniteId,
                    PUHT = valo.PUHT,
                    RapportLigneId = valo.RapportLigneId,
                    PersonnelId = valo.PersonnelId,
                    MaterielId = valo.MaterielId
                }));

            IReadOnlyList<UniteEnt> uniteCodes = uniteManager.GetUnites(valorisationEcritureComptableODModels.Select(valo => valo.UniteId).ToList());

            valorisationEcritureComptableODModels.Join(uniteCodes, valo => valo.UniteId, unite => unite.UniteId, (valo, unite) => new { valo, unite }).ForEach(item => item.valo.Unite = item.unite.Code);

            return valorisationEcritureComptableODModels;
        }

        /// <summary>
        ///  mise à jour du prix et du montant
        /// </summary>
        /// <param name="idRapportLignes">id repport ligne </param>
        /// <param name="valo"> la nouvelle valorisation</param>
        public void UpdateValorisationMontant(List<int> idRapportLignes, decimal valo)
        {
            List<ValorisationEnt> valos = Repository.GetByListRapportLigneId(idRapportLignes).ToList();
            foreach (ValorisationEnt v in valos)
            {
                v.PUHT = valo;
                v.Montant = decimal.Round(valo * v.Quantite, 2);
                Repository.Update(v);
            }
            Save();
        }

        // Représente un objet contenant les informations relatives à sa valorisation
        private class ObjetValorise
        {
            public readonly ReferentielEtenduEnt RefEtendu;
            public readonly int UniteId;
            public readonly decimal Prix;
            public readonly BaremeExploitationCIEnt Bareme;

            public ObjetValorise(ReferentielEtenduEnt refEtendu, int uniteId, decimal prix, BaremeExploitationCIEnt bareme)
            {
                RefEtendu = refEtendu;
                UniteId = uniteId;
                Prix = prix;
                Bareme = bareme;
            }
        }
    }
}