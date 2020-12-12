using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.CI.Services;
using Fred.Business.Common;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Images;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage.Duplication;
using Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces;
using Fred.Business.Rapport.Pointage.Validation;
using Fred.Business.Rapport.Pointage.Validation.Interfaces;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.Rapport.Reporting;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.StatutAbsence;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Moyen;
using Fred.Entities.Organisation;
using Fred.Entities.Permission;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Framework.Tool;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.EtatPaie;
using Fred.Web.Shared.Models.Personnel;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Rapport.Pointage
{
    public class PointageManager : Manager<RapportLigneEnt, IPointageRepository>, IPointageManager
    {
        private const string CongePayeAbsenceCode = "23";
        private const int MaxPrimesCount = 4;

        private readonly ICIRepository ciRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly IRapportLignePrimeRepository rapportLignePrimeRepo;
        private readonly IRapportLigneAstreinteRepository rapportLigneAstreinteRepo;
        private readonly IRapportLigneTacheRepository rapportLigneTacheRepo;
        private readonly IRapportRepository repoRapport;
        private readonly IPointageRepository pointageRepository;
        private readonly IRapportLigneMajorationRepository rapportLigneMajorationRepo;
        private readonly IToolManager toolManager;
        private readonly IAffectationMoyenRepository affectationMoyenRepository;
        private readonly IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository;
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly IPrimeManager primeManager;
        private readonly ICodeDeplacementManager codeDeplacementManager;
        private readonly ICisAccessiblesService cisAccessiblesService;
        private readonly IPointageValidator validator;
        private readonly ICodeAbsenceManager codeAbsenceManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IIndemniteDeplacementManager indemniteDeplacementManager;
        private readonly ITacheManager tacheManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly ICIManager ciManager;
        private readonly IPointagePersonnelTransformerForViewService pointagePersonnelService;
        private readonly IImageManager imageManager;
        private readonly IRapportLignesValidationDataProvider rapportLignesValidationDataProvider;
        private readonly IRapportLigneErrorBuilder rapportLigneErrorBuilder;
        private readonly IPointageSamediCongePayeService pointageSamediCongePayeService;
        private readonly IPointageDuplicatorService pointageDuplicatorService;
        private readonly ISocieteManager societeManager;
        private readonly IStatutAbsenceManager statutAbsenceManager;

        public PointageManager(
            IUnitOfWork uow,
            IPointageValidator validator,
            IPointageRepository pointageRepository,
            ICisAccessiblesService cisAccessiblesService,
            ICodeAbsenceManager codeAbsenceManager,
            IValorisationManager valorisationManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IIndemniteDeplacementManager indemniteDeplacementManager,
            ITacheManager tacheManager,
            IUtilisateurManager utilisateurManager,
            IPersonnelManager personnelManager,
            IContratInterimaireManager contratInterimaireManager,
            ICIManager ciManager,
            IPointagePersonnelTransformerForViewService pointagePersonnelService,
            IImageManager imageManager,
            IRapportLignesValidationDataProvider rapportLignesValidationDataProvider,
            IRapportLigneErrorBuilder rapportLigneErrorBuilder,
            IPointageSamediCongePayeService pointageSamediCongePayeService,
            IPointageDuplicatorService pointageDuplicatorService,
            ISocieteManager societeManager,
            IStatutAbsenceManager statutAbsenceManager,
            IPersonnelRepository personnelRepository,
            ICIRepository ciRepository,
            IRapportRepository repoRapport,
            IRapportLigneTacheRepository rapportLigneTacheRepo,
            IRapportLignePrimeRepository rapportLignePrimeRepo,
            IRapportLigneMajorationRepository rapportLigneMajorationRepo,
            IRapportLigneAstreinteRepository rapportLigneAstreinteRepo,
            IAffectationMoyenRepository affectationMoyenRepository,
            IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository,
            ICodeMajorationManager codeMajorationManager,
            ICodeZoneDeplacementManager codeZoneDeplacementManager,
            IPrimeManager primeManager,
            ICodeDeplacementManager codeDeplacementManager)
          : base(uow, pointageRepository, validator)
        {
            this.validator = validator;
            this.pointageRepository = pointageRepository;
            this.cisAccessiblesService = cisAccessiblesService;
            this.codeAbsenceManager = codeAbsenceManager;
            this.valorisationManager = valorisationManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.indemniteDeplacementManager = indemniteDeplacementManager;
            this.tacheManager = tacheManager;
            this.utilisateurManager = utilisateurManager;
            this.personnelManager = personnelManager;
            this.contratInterimaireManager = contratInterimaireManager;
            this.ciManager = ciManager;
            this.pointagePersonnelService = pointagePersonnelService;
            this.imageManager = imageManager;
            this.rapportLignesValidationDataProvider = rapportLignesValidationDataProvider;
            this.rapportLigneErrorBuilder = rapportLigneErrorBuilder;
            this.pointageSamediCongePayeService = pointageSamediCongePayeService;
            this.pointageDuplicatorService = pointageDuplicatorService;
            this.societeManager = societeManager;
            this.statutAbsenceManager = statutAbsenceManager;
            this.personnelRepository = personnelRepository;
            this.ciRepository = ciRepository;
            this.repoRapport = repoRapport;
            this.rapportLigneTacheRepo = rapportLigneTacheRepo;
            this.rapportLignePrimeRepo = rapportLignePrimeRepo;
            this.rapportLigneMajorationRepo = rapportLigneMajorationRepo;
            this.rapportLigneAstreinteRepo = rapportLigneAstreinteRepo;
            this.affectationMoyenRepository = affectationMoyenRepository;
            this.rapportLigneCodeAstreinteRepository = rapportLigneCodeAstreinteRepository;
            this.codeMajorationManager = codeMajorationManager;
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.primeManager = primeManager;
            this.codeDeplacementManager = codeDeplacementManager;
            toolManager = new ToolManager();
        }

        /// <summary>
        /// Récupère un RapportLigne en fonction de son Identifiant
        /// </summary>
        /// <param name="rapportLigneId">Identifiant du RapportLigneId</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Un pointage</returns>
        public RapportLigneEnt Get(int rapportLigneId, List<Expression<Func<RapportLigneEnt, object>>> includes)
        {
            return Repository.Get(rapportLigneId, includes);
        }

        /// <summary>
        /// Récupère la liste des rapports ligne.
        /// </summary>
        /// <returns>La liste des rapports ligne</returns>
        public IEnumerable<RapportLigneEnt> GetRapportLigneAllSync()
        {
            return Repository.GetRapportLigneAllSync();
        }

        /// <summary>
        /// Récupère la liste des rapports ligne prime.
        /// </summary>
        /// <returns>La liste des rapports ligne prime</returns>
        public IEnumerable<RapportLignePrimeEnt> GetRapportLignePrimeAllSync()
        {
            return Repository.GetRapportLignePrimeAllSync();
        }

        /// <summary>
        /// Récupère la liste des rapports ligne tache.
        /// </summary>
        /// <returns>La liste des rapports ligne tache</returns>
        public IEnumerable<RapportLigneTacheEnt> GetRapportLigneTacheAllSync()
        {
            return Repository.GetRapportLigneTacheAllSync();
        }

        /// <summary>
        /// Récupérer la liste des rapport statut.
        /// </summary>
        /// <returns>La liste des rapports statut</returns>
        public IEnumerable<RapportStatutEnt> GetRapportStatutAllSync()
        {
            return Repository.GetRapportStatutAllSync();
        }

        /// <summary>
        /// Récupérer la liste des rapport tache.
        /// </summary>
        /// <returns>La liste des rapports tache.</returns>
        public IEnumerable<RapportTacheEnt> GetRapportTacheAllSync()
        {
            return Repository.GetRapportTacheAllSync();
        }

        /// <summary>
        ///   Retourne la liste de personnel du profil paie
        /// </summary>
        /// <param name="userid">L'identifiant du user</param>
        /// <param name="annee">Année de la période</param>
        /// <param name="mois">Mois de la période</param>
        /// <returns>Liste de nombre de personnels</returns>
        private IEnumerable<PointageBase> GetPointageVerrouillesByUserId(int userid, int annee, int mois)
        {
            return Repository.GetPointageVerrouillesByUserId(userid, annee, mois);
        }

        /// <summary>
        ///   Retourne la liste de personnel du profil paie
        /// </summary>
        /// <param name="userid">L'identifiant du user</param>
        /// <param name="datemin">Date Début</param>
        /// <param name="datemax">Date fin</param>
        /// <returns>Liste de nombre de personnels</returns>
        private IEnumerable<PointageBase> GetPointageVerrouillesByUserIdByPeriode(int userid, DateTime datemin, DateTime datemax)
        {
            return Repository.GetPointageVerrouillesByUserIdByPeriode(userid, datemin, datemax);
        }

        #region ApplyReadOnlyRGPointageBase

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>Un pointage de base</returns>
        public PointageBase ApplyReadOnlyRGPointageBase(PointageBase pointage)
        {
            // Test date de clôture comptable
            pointage.ReadOnly = this.datesClotureComptableManager.IsTodayInPeriodeCloture(pointage.CiId, pointage.DatePointage.Year, pointage.DatePointage.Month);

            //// CodeAbsence

            if (pointage.CodeAbsence != null)
            {
                // Si Samedi et code absence CP alors on ne saisit pas d'heure d'absence
                if (pointage.DatePointage.DayOfWeek == DayOfWeek.Saturday && pointage.CodeAbsence.Code.Equals(CongePayeAbsenceCode))
                {
                    pointage.HeureAbsenceReadOnly = true;
                }
                else
                {
                    pointage.HeureAbsenceReadOnly = false;
                }
            }

            //// CodeDeplacement

            if (pointage.Ci != null && pointage.Personnel != null && pointage.Personnel.EtablissementPaie != null && pointage.Personnel.EtablissementPaie.GestionIndemnites && pointage.CodeDeplacement != null)
            {
                ////bool isARH = false;
                ////bool isGSP = false;

                if (pointage.Ci.Organisation == null)
                {
                    this.ciRepository.PerformEagerLoading(pointage.Ci, ci => ci.Organisation);
                }

                if (pointage.Ci.Organisation != null)
                {
                    //// A FAIRE : JNE : Refactoring nécessaire HasAtLeastThisRoleByPersonnelIdAndOrganisationId écrase la valeur de pointage.Ci.Organisation
                    OrganisationEnt orga = pointage.Ci.Organisation;
                    ////isARH = UtilisateurManager.HasAtLeastThisRoleByPersonnelIdAndOrganisationId(orga.OrganisationId, RoleEnt.CodeRoleARH);
                    ////isGSP = UtilisateurManager.HasAtLeastThisRoleByPersonnelIdAndOrganisationId(orga.OrganisationId, RoleEnt.CodeRoleGSP);
                    //// Réaffectation de l'orga
                    pointage.Ci.Organisation = orga;
                }
            }
            return pointage;
        }

        #endregion

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <param name="domaine">Le domaine à vérifier</param>
        /// <returns>Le pointage mis à jour</returns>
        public RapportLigneEnt ApplyValuesRGPointageReel(RapportLigneEnt pointage, string domaine)
        {
            //// CI
            if (domaine.Equals(Constantes.EntityType.CI))
            {
                if (pointage.Ci.Organisation == null)
                {
                    pointage.Ci = this.ciRepository.Get(pointage.CiId);
                }

                if (this.datesClotureComptableManager.IsTodayInPeriodeCloture(pointage.CiId, pointage.DatePointage.Year, pointage.DatePointage.Month))
                {
                    pointage.ListErreurs.Add(FeatureRapport.Pointage_Date_Periode_Cloture_Erreur);
                    return pointage;
                }

                // Epuration des données
                // Primes (Si une prime est privée (!Publique) alors IsDeleted = true, sinon false)        
                pointage.ListRapportLignePrimes.ForEach(x => x.IsDeleted = !x.Prime.Publique);

                // Taches        
                pointage.ListRapportLigneTaches.ForEach(x => x.IsDeleted = true);

                //Récupération de la tâche par défaut du CI et ajout si elle n'est pas déjà dans la liste.
                TacheEnt tachedefaut = this.tacheManager.GetTacheParDefaut(pointage.CiId);

                if (tachedefaut != null)
                {
                    RapportLigneTacheEnt rlTache = pointage.ListRapportLigneTaches.FirstOrDefault(t => t.TacheId == tachedefaut.TacheId);

                    if (rlTache != null)
                    {
                        rlTache.IsDeleted = false;
                    }
                    else
                    {
                        pointage.ListRapportLigneTaches.Add(GetNewPointageReelTache(pointage, tachedefaut));
                    }
                }

                //Suppression des associations inutiles
                pointage.ListRapportLigneTaches = pointage.ListRapportLigneTaches.Where(r => r.RapportLigneTacheId != 0 || r.RapportLigneTacheId == 0 && !r.IsDeleted).ToList();

                ApplyValuesRGPointageReelMajoration(pointage);
            }

            //// CodeAbsence
            if (domaine.Equals(Constantes.EntityType.Absence) && pointage.CodeAbsence != null)
            {
                pointage.NumSemaineIntemperieAbsence = pointage.CodeAbsence.Intemperie ? toolManager.GetWeekOfYear(DateTime.Today) : null;

                // Si Samedi et code absence CP alors on ne saisit pas d'heure d'absence
                if (pointage.DatePointage.DayOfWeek == DayOfWeek.Saturday && pointage.CodeAbsence.Code.Equals(CongePayeAbsenceCode))
                {
                    pointage.HeureAbsence = 0;
                }
            }

            if (domaine.Equals(Constantes.EntityType.Deplacement) || domaine.Equals(Constantes.EntityType.CI)
              && (pointage.Personnel != null && pointage.Personnel.EtablissementRattachement != null && pointage.Personnel.EtablissementRattachement.GestionIndemnites))
            {
                //// Pour le calcul automatique des codes déplacement et zone,
                //// Il faut que le personnel appartienne à un établissement de paie qui est paramétré dans la gestion des déplacements RZB
                GetOrCreateIndemniteDeplacementForRapportLigne(pointage);
            }

            return pointage;
        }

        /// <summary>
        ///   Retourne un pointage avec indemnité de déplacement selon le CI et le personnel associé
        /// </summary>
        /// <param name="pointage">Ligne d'un rapport</param>
        /// <returns>Ligne d'un rapport avec indemnité déplacement</returns>
        public RapportLigneEnt GetOrCreateIndemniteDeplacementForRapportLigne(RapportLigneEnt pointage)
        {
            List<string> warnings;
            return GetOrCreateIndemniteDeplacementForRapportLigne(pointage, out warnings, false);
        }

#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high

        /// <summary>
        ///   Retourne un pointage avec indemnité de déplacement selon le CI et le personnel associé
        /// </summary>
        /// <param name="pointage">Ligne d'un rapport</param>
        /// <param name="warnings">Les avertissements. Peut-être null.</param>
        /// <param name="refresh">Indique s'il s'agit d'un rafraichissement.</param>
        /// <returns>Ligne d'un rapport avec indemnité déplacement</returns>
        public RapportLigneEnt GetOrCreateIndemniteDeplacementForRapportLigne(RapportLigneEnt pointage, out List<string> warnings, bool refresh)
        {
            RapportHebdoDeplacement deplacement = new RapportHebdoDeplacement(codeMajorationManager, codeZoneDeplacementManager, primeManager, codeDeplacementManager);
            if (deplacement.UtilisateurConnecteIsGFES)
            {
                deplacement.UpdateCodeEtZone(pointage, true);
                Save();
                warnings = deplacement.Warnings;
            }
            else
            {
                warnings = null;

                // Déplacements et calcul des indemnités
                IndemniteDeplacementEnt indemniteDeplacement = this.indemniteDeplacementManager.GetOrCreateIndemniteDeplacementByPersonnelAndCi(pointage.Personnel, pointage.Ci, refresh);

                pointage.CodeDeplacement = null;
                pointage.CodeDeplacementId = null;
                pointage.CodeZoneDeplacement = null;
                pointage.CodeZoneDeplacementId = null;
                pointage.DeplacementIV = false;
                if (indemniteDeplacement != null)
                {
                    pointage.CodeDeplacement = indemniteDeplacement.CodeDeplacement;
                    pointage.CodeDeplacementId = indemniteDeplacement.CodeDeplacementId;
                    pointage.CodeZoneDeplacement = indemniteDeplacement.CodeZoneDeplacement;
                    pointage.CodeZoneDeplacementId = indemniteDeplacement.CodeZoneDeplacementId;
                    pointage.DeplacementIV = indemniteDeplacement.IVD;
                }
            }
            return pointage;
        }

        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <param name="errors">Les erreurs qui font que les indemnités de déplacement ne peuvent pas être calculées.</param>
        /// <returns>True s'il est possible de faire le calcul, sinon false.</returns>
        public bool CanCalculateIndemniteDeplacement(RapportLigneEnt pointage, out List<string> errors)
        {
            RapportHebdoDeplacement deplacement = new RapportHebdoDeplacement(codeMajorationManager, codeZoneDeplacementManager, primeManager, codeDeplacementManager);
            if (deplacement.UtilisateurConnecteIsGFES)
            {
                if (deplacement.CanCalculateIndemniteDeplacement(pointage))
                {
                    errors = new List<string>();
                    return true;
                }
                else
                {
                    errors = deplacement.Warnings;
                    return false;
                }
            }
            else
            {
                var tupleControle = indemniteDeplacementManager.CanCalculateIndemniteDeplacement(pointage.Personnel, pointage.Ci);
                errors = tupleControle.Item2;
                return tupleControle.Item1;
            }
        }

        /// <summary>
        ///   Vérification du type de ligne de rapport saisi. false : personnel, null : personnel + matériel, true : matériel
        /// </summary>
        /// <param name="pointageReel">Pointage pour lequel effectuer le traitement</param>
        public void SetPointageReelType(RapportLigneEnt pointageReel)
        {
            bool? checkTypeRapport = null;

            if (!string.IsNullOrWhiteSpace(pointageReel.PrenomNomTemporaire) || pointageReel.PersonnelId != null)
            {
                checkTypeRapport = false;
            }

            if (!string.IsNullOrWhiteSpace(pointageReel.MaterielNomTemporaire) || pointageReel.MaterielId != null)
            {
                if (checkTypeRapport.HasValue)
                {
                    checkTypeRapport = null;
                }
                else
                {
                    checkTypeRapport = true;
                }
            }

            pointageReel.RapportLigneType = checkTypeRapport;
        }

        /// <summary>
        ///   Teste une liste de pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="lstPointages">La liste des pointages à tester</param>
        /// <returns>Une liste de pointages</returns>       
        public IEnumerable<RapportLigneEnt> CheckListPointages(IEnumerable<RapportLigneEnt> lstPointages)
        {
            GlobalDataForValidationPointage dataForValidation = rapportLignesValidationDataProvider.GetDataForValidateRapportLignes(lstPointages);

            rapportLigneErrorBuilder.IncludeErrorsMessages(dataForValidation, lstPointages);

            return lstPointages;
        }

        /// <summary>
        ///   Teste une liste de pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="lstPointages">La liste des pointages à tester</param>
        /// <returns>Une liste de pointages</returns>       
        public IEnumerable<RapportLigneEnt> CheckListPointagesMaterielOnly(IEnumerable<RapportLigneEnt> lstPointages)
        {
            rapportLigneErrorBuilder.IncludeErrorsMessagesMaterielOnly(lstPointages);

            return lstPointages;
        }

        /// <summary>
        ///   Teste un pointage quel que soit son type (personnel et/ou materiel)
        /// </summary>
        /// <param name="pointage">Le pointage que l'on vient de vérifier</param>
        /// <returns>Le pointage</returns>
        public RapportLigneEnt CheckPointage(RapportLigneEnt pointage)
        {
            GlobalDataForValidationPointage dataForValidation = rapportLignesValidationDataProvider.GetDataForValidateRapportLignes(pointage);

            rapportLigneErrorBuilder.IncludeErrorsMessages(dataForValidation, pointage);

            return pointage;
        }

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/mois/annee/organisation d'un rapport.
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPere">Identifiant de l'organisation pére</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        public IEnumerable<PointageBase> GetListePointageMensuel(EtatPaieExportModel etatPaieExportModel, int? organisationPere = null)
        {
            SearchRapportLigneEnt search = new SearchRapportLigneEnt();
            search.DatePointageMin = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1);
            search.DatePointageMax = search.DatePointageMin.AddMonths(1);
            search.PersonnelId = etatPaieExportModel.PersonnelId;

            IEnumerable<PointageBase> listePointageMensuel = null;
            if (etatPaieExportModel.PersonnelId == null)
            {
                switch (etatPaieExportModel.Filtre)
                {
                    case TypeFiltreEtatPaie.Population:
                        int userId = utilisateurManager.GetContextUtilisateur().UtilisateurId;
                        listePointageMensuel = GetPointageVerrouillesByUserId(userId, etatPaieExportModel.Annee, etatPaieExportModel.Mois);
                        break;
                    case TypeFiltreEtatPaie.Perimetre:
                        userId = utilisateurManager.GetContextUtilisateur().UtilisateurId;
                        IEnumerable<int> allCisByUser = utilisateurManager.GetAllCIbyUser(userId, false, organisationPere).ToList();
                        listePointageMensuel = SearchPointageReelWithFilter(search, false).Where(p => allCisByUser.Contains(p.CiId));
                        break;
                    case TypeFiltreEtatPaie.Autre:
                        IEnumerable<RapportLigneEnt> pointages = SearchPointageReelWithFilter(search, false);
                        bool isUtilisateurConnectedFes = utilisateurManager.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);
                        if (isUtilisateurConnectedFes)
                        {
                            // filtre sur le personnel de l'établissement pour FES.
                            listePointageMensuel = GetPointagesByEtablissementPaiePersonnel(etatPaieExportModel, pointages);
                        }
                        else
                        {
                            // filtre sur les CI de l'établissement pour les autres.
                            listePointageMensuel = GetPointagesByEtablissementPaieCis(etatPaieExportModel, pointages);
                        }
                        break;
                }
            }
            else
            {
                listePointageMensuel = SearchPointageReelWithFilter(search, false);
            }

            return etatPaieExportModel.Tri
                ? listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Matricule : string.Empty)
                : listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Nom : string.Empty);
        }

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/mois/annee/organisation d'un rapport(fes).
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPere">Identifiant de l'organisation pére</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        public IEnumerable<RapportLigneEnt> GetListePointageMensuelfes(EtatPaieExportModel etatPaieExportModel, int? organisationPere = null)
        {
            SearchRapportLigneEnt search = new SearchRapportLigneEnt();
            search.DatePointageMin = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1);
            search.DatePointageMax = search.DatePointageMin.AddMonths(1);
            search.PersonnelId = etatPaieExportModel.PersonnelId;

            IEnumerable<RapportLigneEnt> listePointageMensuel = SearchPointageReelWithFilter(search, false);
            if (etatPaieExportModel.PersonnelId == null)
            {
                switch (etatPaieExportModel.Filtre)
                {
                    case TypeFiltreEtatPaie.Population:
                        listePointageMensuel = ((IEnumerable<RapportLigneEnt>)GetPointageVerrouillesByUserId(this.utilisateurManager.GetContextUtilisateur().UtilisateurId, etatPaieExportModel.Annee, etatPaieExportModel.Mois));
                        break;
                    case TypeFiltreEtatPaie.Perimetre:
                        IEnumerable<int> allCisByUser = this.utilisateurManager.GetAllCIbyUser(this.utilisateurManager.GetContextUtilisateur().UtilisateurId, false, organisationPere).ToList();
                        listePointageMensuel = listePointageMensuel.Where(p => allCisByUser.Contains(p.CiId));
                        break;
                    case TypeFiltreEtatPaie.Autre:
                        listePointageMensuel = GetPointagesByEtablissementPaiePersonnel(etatPaieExportModel, listePointageMensuel);
                        break;
                }
            }

            if (etatPaieExportModel.Tri)
            {
                return listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Matricule : string.Empty);
            }

            return listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Nom : string.Empty);
        }

        /// <summary>
        /// Retourne les pointages filtrés sur le personnel de l'établissement de paie
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="listePointageMensuel">liste des pointages personnels</param>
        /// <returns>liste des pointages mensueles</returns>
        public IEnumerable<RapportLigneEnt> GetPointagesByEtablissementPaiePersonnel(EtatPaieExportModel etatPaieExportModel, IEnumerable<RapportLigneEnt> listePointageMensuel)
        {
            if (etatPaieExportModel.EtablissementPaieIdList.Any())
            {
                listePointageMensuel = listePointageMensuel.Where(p => p.Personnel != null && etatPaieExportModel.EtablissementPaieIdList.Contains(p.Personnel.EtablissementPaieId));
            }
            else
            {
                IEnumerable<int> listAffectation = Managers.Affectation.GetPersonnelIdAffectedEtablissementByOrganisationId(etatPaieExportModel);
                listePointageMensuel = listePointageMensuel.Where(p => p.Personnel != null && listAffectation.Contains(p.Personnel.PersonnelId));
            }

            listePointageMensuel = listePointageMensuel.Where(l => !etatPaieExportModel.StatutPersonnelList.Any() || etatPaieExportModel.StatutPersonnelList.Contains(l.Personnel.Statut));

            return listePointageMensuel;
        }

        /// <summary>
        /// Retourne les pointages filtrés sur les CIs de l'établissement de paie
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="listePointageMensuel">liste des pointages personnels</param>
        /// <returns>liste des pointages mensueles</returns>
        public IEnumerable<RapportLigneEnt> GetPointagesByEtablissementPaieCis(EtatPaieExportModel etatPaieExportModel, IEnumerable<RapportLigneEnt> listePointageMensuel)
        {
            IEnumerable<int> allCisByOrga = utilisateurManager.GetAllCIIdbyOrganisation(etatPaieExportModel.OrganisationId).ToList();
            listePointageMensuel = listePointageMensuel.Where(p => allCisByOrga.Contains(p.CiId));
            if (etatPaieExportModel.EtablissementPaieIdList.Any())
            {
                listePointageMensuel = listePointageMensuel.Where(p => p.Personnel != null && etatPaieExportModel.EtablissementPaieIdList.Contains(p.Personnel.EtablissementPaieId));
            }
            return listePointageMensuel;
        }

        /// <summary>
        ///   Retourne la liste ligne de rapport par user/jour/mois/annee/organisation d'un rapport.
        /// </summary>
        /// <param name="year">annee du rapport </param>
        /// <param name="month">mois du rapport </param>
        /// <param name="day">jour du rapport</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Renvoie la liste des lignes de rapport.</returns>
        public IEnumerable<RapportLigneEnt> GetListePointageHebdomadaire(int year, int month, int day, EtatPaieExportModel etatPaieExportModel)
        {
            SearchRapportLigneEnt search = new SearchRapportLigneEnt();
            search.DatePointageMin = new DateTime(year, month, day);
            search.DatePointageMax = search.DatePointageMin.AddDays(7);
            search.PersonnelId = etatPaieExportModel.PersonnelId;

            IEnumerable<RapportLigneEnt> listePointageHebdomadaire = SearchPointageReelWithFilter(search, false, true);
            if (etatPaieExportModel.PersonnelId == null)
            {
                int userId = this.utilisateurManager.GetContextUtilisateur().UtilisateurId;
                switch (etatPaieExportModel.Filtre)
                {
                    case TypeFiltreEtatPaie.Population:
                        listePointageHebdomadaire = ((IEnumerable<RapportLigneEnt>)GetPointageVerrouillesByUserIdByPeriode(userId, search.DatePointageMin, search.DatePointageMax));
                        break;
                    case TypeFiltreEtatPaie.Perimetre:
                        IEnumerable<int> allCisByUser = this.utilisateurManager.GetAllCIbyUser(userId).ToList();
                        listePointageHebdomadaire = listePointageHebdomadaire.Where(p => allCisByUser.Contains(p.CiId));
                        break;
                    case TypeFiltreEtatPaie.Autre:
                        listePointageHebdomadaire = GetPointagesByEtablissementPaiePersonnel(etatPaieExportModel, listePointageHebdomadaire);
                        break;
                }
            }

            if (etatPaieExportModel.Tri)
            {
                listePointageHebdomadaire = listePointageHebdomadaire.OrderBy(x => x.Personnel != null ? x.Personnel.Matricule : string.Empty);
            }
            else
            {
                listePointageHebdomadaire = listePointageHebdomadaire.OrderBy(x => x.Personnel != null ? x.Personnel.Nom : string.Empty);
            }

            return listePointageHebdomadaire;
        }
        #region Traitement

        /// <summary>
        ///   Traite les données d'un pointage
        /// </summary>
        /// <param name="pointage">Pointage pour lequel les données doivent être vérifiées</param>
        public void TraiteDonneesPointage(RapportLigneEnt pointage)
        {
            if (!pointage.ReadOnly)
            {
                // Si on a un personnel
                PointageClearPersonnelAnMaterielHelper.ClearPersonnel(pointage);

                // Si on a un materiel
                PointageClearPersonnelAnMaterielHelper.ClearMateriel(pointage);

                pointageSamediCongePayeService.GenerationPointageSamediCP(pointage);

                if (pointage.ListRapportLignePrimes != null && pointage.ListRapportLignePrimes.Any())
                {
                    pointage.ListRapportLignePrimes.ToList()
                    .ForEach(rlp =>
                    {
                        if (rlp.Prime != null && rlp.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire)
                        {
                            rlp.IsChecked = rlp.HeurePrime > 0;
                        }
                        rlp.RapportLigneId = pointage.RapportLigneId;
                    });
                }

                if (pointage.ListRapportLigneTaches != null && pointage.ListRapportLigneTaches.Any())
                {
                    pointage.ListRapportLigneTaches.ToList().ForEach(rlt => rlt.RapportLigneId = pointage.RapportLigneId);
                }
            }
        }

        /// <summary>
        ///   Traite l'état d'un pointage dans le context Entity Framework
        /// </summary>
        /// <param name="pointage">pointage que l'on souhaite traiter en base de donnée</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur étant intervenu sur le pointage</param>
        /// <param name="codeGroupe">Code du groupe</param>
        public void TraiteEtatPointage(RapportLigneEnt pointage, int utilisateurId, string codeGroupe)
        {
            bool updatePointage = false;
            if (!pointage.ReadOnly)
            {
                // Traitement des Lignes
                if (pointage.IsCreated)
                {
                    pointage.DateCreation = DateTime.UtcNow;
                    pointage.AuteurCreationId = utilisateurId;
                    Repository.Insert(pointage);
                    valorisationManager.InsertValorisationFromPointage(pointage, "Rapport");
                }
                else if (pointage.IsDeleted)
                {
                    // si les lignes taches n'ont pas encore été crées, il ne faut pas les supprimer de la base de données
                    // suppression des ligne taches sans ID de la collection à mettre à jour
                    List<RapportLigneTacheEnt> ligneTacheNotInDb = pointage.ListRapportLigneTaches.Where(x => x.RapportLigneTacheId == 0).ToList();
                    ligneTacheNotInDb.ForEach(x => pointage.ListRapportLigneTaches.Remove(x));
                    // Nécessaire à la gestion de la valorisation
                    // On recharge l'identifiant du personnel et du matériel au cas l'utlisateur aurait supprimé ces derniers du pointage en cours de suppression
                    pointage.PersonnelId = pointageRepository.GetPersonnelId(pointage.RapportLigneId);
                    pointage.MaterielId = pointageRepository.GetMaterielId(pointage.RapportLigneId);
                    DeletePointage(pointage, false);
                }
                else if (pointage.IsUpdated)
                {
                    pointage.DateModification = DateTime.UtcNow;
                    pointage.AuteurModificationId = utilisateurId;
                    updatePointage = true;
                    valorisationManager.UpdateValorisationFromPointage(pointage);
                }
                else if (pointage.IsLotPointageIdUpdated)
                {
                    pointage.DateModification = DateTime.UtcNow;
                    pointage.AuteurModificationId = utilisateurId;
                    updatePointage = true;
                }

                Save();

                HandleRapportLignePrime(pointage);
                HandleRapportLigneTache(pointage);

                if (codeGroupe.Equals(FeatureRapport.Code_Groupe_FES))
                {
                    HandleRapportLigneMajoration(pointage);
                    HandleRapportLigneAstreinte(pointage);
                }

                if (updatePointage)
                {
                    pointage.Ci = null;
                    Repository.Update(pointage);
                }

                Save();
            }
        }

        /// <summary>
        ///   Insère une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLignePrime">entité à insére</param>
        public void InsertRapportLignePrime(RapportLignePrimeEnt rapportLignePrime)
        {
            this.rapportLignePrimeRepo.Insert(rapportLignePrime);
        }

        /// <summary>
        ///   Insère une entité de liaison RapportLigneAstreinteEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneAstreinte">entité à insére</param>
        public void InsertRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            this.rapportLigneAstreinteRepo.Insert(rapportLigneAstreinte);
            this.Save();
        }

        /// <summary>
        ///   Update une entité de liaison RapportLigneAstreinteEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneAstreinte">entité à insére</param>
        public void UpdateRapportLigneAstreinte(RapportLigneAstreinteEnt rapportLigneAstreinte)
        {
            this.rapportLigneAstreinteRepo.Update(rapportLigneAstreinte);
        }

        /// <summary>
        ///   Insère une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneTache">entité à insére</param>
        public void InsertRapportLigneTache(RapportLigneTacheEnt rapportLigneTache)
        {
            this.rapportLigneTacheRepo.Insert(rapportLigneTache);
        }

        /// <summary>
        ///   Supprime une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLignePrime">entité à insére</param>
        public void DeleteRapportLignePrime(RapportLignePrimeEnt rapportLignePrime)
        {
            this.rapportLignePrimeRepo.Delete(rapportLignePrime);
        }

        /// <summary>
        ///   Supprime une entité de liaison RapportLignePrimeEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneTache">entité à insére</param>
        public void DeleteRapportLigneTache(RapportLigneTacheEnt rapportLigneTache)
        {
            this.rapportLigneTacheRepo.Delete(rapportLigneTache);
        }

        /// <summary>
        ///   Traitement des primes
        /// </summary>
        /// <param name="pointage">Pointage</param>
        private void HandleRapportLignePrime(RapportLigneEnt pointage)
        {
            List<RapportLignePrimeEnt> listPrimeDeleted = new List<RapportLignePrimeEnt>();
            if (pointage.ListRapportLignePrimes != null)
            {
                foreach (RapportLignePrimeEnt pointagePrime in pointage.ListRapportLignePrimes)
                {
                    if (pointagePrime.IsDeleted || pointage.IsDeleted)
                    {
                        listPrimeDeleted.Add(pointagePrime);
                    }
                    else if (pointagePrime.IsCreated)
                    {
                        rapportLignePrimeRepo.Insert(pointagePrime);
                    }
                    else
                    {
                        rapportLignePrimeRepo.Update(pointagePrime);
                    }
                }
            }
            for (int i = listPrimeDeleted.Count - 1; i >= 0; i--)
            {
                RapportLignePrimeEnt lignePrime = listPrimeDeleted[i];
                pointage.ListRapportLignePrimes.Remove(lignePrime);
                rapportLignePrimeRepo.Delete(lignePrime);
            }
        }

        /// <summary>
        ///   Traitement des Astreintes
        /// </summary>
        /// <param name="pointage">Pointage</param>
        private void HandleRapportLigneAstreinte(RapportLigneEnt pointage)
        {
            if (pointage.ListRapportLigneAstreintes != null)
            {
                List<RapportLigneAstreinteEnt> listAstreintesDeleted = pointage.ListRapportLigneAstreintes.Where(x => x.IsDeleted).ToList();
                foreach (RapportLigneAstreinteEnt pointageAstreinte in pointage.ListRapportLigneAstreintes.Where(x => !x.IsDeleted).ToList())
                {
                    if (pointageAstreinte.RapportLigneAstreinteId == 0)
                    {
                        rapportLigneAstreinteRepo.Insert(pointageAstreinte);
                    }
                    else
                    {
                        rapportLigneAstreinteRepo.Update(pointageAstreinte);
                    }
                }
                foreach (RapportLigneAstreinteEnt pointageAstreinte in listAstreintesDeleted)
                {
                    rapportLigneCodeAstreinteRepository.DeletePrimesAstreinteByLigneAstreinteId(pointageAstreinte.RapportLigneAstreinteId);
                    pointage.ListRapportLigneAstreintes.Remove(pointageAstreinte);
                    rapportLigneAstreinteRepo.DeleteAstreintesById(pointageAstreinte.RapportLigneAstreinteId);
                }
            }
        }

        /// <summary>
        ///   Traitement des tâches
        /// </summary>
        /// <param name="pointage">Pointage</param>
        private void HandleRapportLigneTache(RapportLigneEnt pointage)
        {
            List<RapportLigneTacheEnt> listTacheDeleted = new List<RapportLigneTacheEnt>();
            if (pointage.ListRapportLigneTaches != null)
            {
                foreach (RapportLigneTacheEnt pointageTache in pointage.ListRapportLigneTaches)
                {
                    if (pointageTache.IsDeleted || pointage.IsDeleted)
                    {
                        listTacheDeleted.Add(pointageTache);
                    }
                    else if (pointageTache.IsCreated)
                    {
                        rapportLigneTacheRepo.Insert(pointageTache);
                    }
                    else
                    {
                        rapportLigneTacheRepo.Update(pointageTache);
                    }
                }
            }
            for (int i = listTacheDeleted.Count - 1; i >= 0; i--)
            {
                RapportLigneTacheEnt ligneTache = listTacheDeleted[i];
                pointage.ListRapportLigneTaches.Remove(ligneTache);
                rapportLigneTacheRepo.Delete(ligneTache);
            }
        }

        #endregion

        #region Add

        /// <summary>
        ///   Ajoute une prime à un pointage réel
        /// </summary>
        /// <param name="pointage">La ligne de rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>La ligne de rapport contenant la nouvelle prime</returns>
        public RapportLigneEnt AddPrimeToPointage(RapportLigneEnt pointage, PrimeEnt prime)
        {
            if (pointage == null || prime == null)
            {
                throw new ArgumentException(FeatureRapport.Pointage_Ajout_Prime_Nul_Erreur);
            }
            RapportLignePrimeEnt ptgPrime = new RapportLignePrimeEnt
            {
                PrimeId = prime.PrimeId,
                Prime = prime,
                RapportLigne = pointage
            };

            pointage.ListRapportLignePrimes.Add(ptgPrime);
            return pointage;
        }

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="pointageReel">Le ligne de rapport à laquelle ajouter une tâche</param>
        /// <param name="tache">La tache</param>
        /// <returns>La ligne de rapport avec la nouvelle tâche</returns>
        public RapportLigneEnt AddTacheToPointageReel(RapportLigneEnt pointageReel, TacheEnt tache)
        {
            if (pointageReel == null || tache == null)
            {
                throw new ArgumentException(FeatureRapport.Pointage_Ajout_Tache_Nul_Erreur);
            }

            pointageReel.ListRapportLigneTaches.Add(GetNewPointageReelTache(pointageReel, tache));
            return pointageReel;
        }

        /// <inheritdoc/>
        public void AddPointage(RapportLigneEnt pointage)
        {
            if (pointage.AuteurCreationId == 0)
            {
                pointage.AuteurCreationId = this.utilisateurManager.GetContextUtilisateurId();
            }

            pointage.DateCreation = DateTime.UtcNow;
            valorisationManager.InsertValorisationFromPointage(pointage, "Rapport");
            Repository.Insert(pointage);
        }


        /// <inheritdoc/>
        public void AddPointageWithSave(RapportLigneEnt pointage)
        {
            AddPointage(pointage);
            Save();
        }

        #endregion

        /// <inheritdoc/>
        public void UpdatePointage(RapportLigneEnt pointage)
        {
            pointage.AuteurModificationId = this.utilisateurManager.GetContextUtilisateurId();

            pointage.DateModification = DateTime.UtcNow;
            valorisationManager.UpdateValorisationFromPointage(pointage);
            Repository.Update(pointage);
            this.Save();
        }

        /// <inheritdoc/>
        public void UpdatePointageForReceptionInterimaire(RapportLigneEnt pointage, int utilisateurIdFredIE)
        {
            pointage.ReceptionInterimaire = true;
            pointage.AuteurModificationId = utilisateurIdFredIE;
            pointage.DateModification = DateTime.UtcNow;
            Repository.Update(pointage);
        }

        /// <summary>
        ///   Met à jour une ligne de rapport pour la réception materiel externe
        /// </summary>
        /// <param name="pointage">pointage réel à mettre à jour</param>
        public void UpdatePointageForReceptionMaterielExterne(RapportLigneEnt pointage)
        {
            pointage.ReceptionMaterielExterne = true;
            pointage.AuteurModificationId = utilisateurManager.GetByLogin("fred_ie").UtilisateurId;
            pointage.DateModification = DateTime.UtcNow;
            Repository.Update(pointage);
        }

        /// <inheritdoc/>
        public void DeletePointage(RapportLigneEnt pointage, bool saveUow = true, int? utilisateurId = null)
        {
            if (pointage == null)
            {
                throw new ArgumentException(FeatureRapport.Pointage_Suppression_Pointage_Nul_Erreur);
            }

            if (!utilisateurId.HasValue)
            {
                utilisateurId = this.utilisateurManager.GetContextUtilisateurId();
            }

            if (pointage.AffectationMoyenId != null)
            {
                IReadOnlyList<RapportLigneEnt> listpointage = repoRapport.GetAllRapportLigneBasedOnPersonnelAffectation(pointage.PersonnelId.Value, pointage.DatePointage);

                foreach (RapportLigneEnt rl in listpointage)
                {
                    if (rl.RapportLigneId == pointage.RapportLigneId)
                    {
                        UpdateDateSuppressionRapportLigne(pointage, utilisateurId.Value);
                    }
                    else if (!rl.PersonnelId.HasValue && rl.MaterielId.HasValue && rl.RapportId == pointage.RapportId)
                    {
                        UpdateDateSuppressionRapportLigne(rl, utilisateurId.Value);
                    }
                }
            }
            else
            {
                UpdateDateSuppressionRapportLigne(pointage, utilisateurId.Value);
            }
            valorisationManager.DeleteValorisationFromPointage(pointage);

            if (saveUow)
                Save();
        }

        /// <inheritdoc/>
        public void DeletePointageList(IEnumerable<RapportLigneEnt> pointageList, bool saveUow = true, int? utilisateurId = null)
        {
            if (pointageList == null || !pointageList.Any())
            {
                return;
            }

            pointageList.ForEach(p => DeletePointage(p, false, utilisateurId));
            if (saveUow)
            {
                Save();
            }
        }

        private void UpdateDateSuppressionRapportLigne(RapportLigneEnt rapport, int utilisateurId)
        {
            DateTime currentUtcDate = DateTime.UtcNow;
            rapport.IsDeleted = true;
            rapport.DateSuppression = currentUtcDate;
            rapport.AuteurSuppressionId = utilisateurId;
            rapport.DateModification = currentUtcDate;
            rapport.AuteurModificationId = utilisateurId;
            Repository.Update(rapport);
        }

        #region Get

        /// <summary>
        ///   Créer une ligne de tache vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="tache">La tache</param>
        /// <returns>Une tache correspondant à une ligne de rapport</returns>
        public RapportLigneTacheEnt GetNewPointageReelTache(RapportLigneEnt pointageReel, TacheEnt tache)
        {
            return new RapportLigneTacheEnt
            {
                TacheId = tache.TacheId,
                Tache = tache,
                RapportLigne = pointageReel,
                HeureTache = 0
            };
        }

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de pointage mensuel.
        /// </summary>
        /// <returns>Nouvelle instance de pointage mensuel intialisée</returns>
        public PointageMensuelEnt GetNewPointageMensuel()
        {
            return new PointageMensuelEnt { DateComptable = DateTime.Now, Domaine = false, Tri = false };
        }

        /// <inheritdoc />
        public int CountPointage(int rapportId)
        {
            return Repository.GetAllLight(rapportId).Count(x => !x.DateSuppression.HasValue);
        }

        /// <inheritdoc/>
        public IEnumerable<RapportLigneEnt> GetAllLockedPointages(DateTime periode)
        {
            UtilisateurEnt currentUser = this.utilisateurManager.GetContextUtilisateur();
            IEnumerable<int> allCisByUser = this.utilisateurManager.GetAllCIbyUser(currentUser.UtilisateurId).ToList();

            return this.pointageRepository
                        .Query()
                        .Include(x => x.ListRapportLignePrimes.Select(p => p.Prime))
                        .Include(x => x.LotPointage)
                        .Include(x => x.Personnel)
                        .Filter(x => allCisByUser.Contains(x.CiId) &&
                                     x.LotPointageId.HasValue &&
                                     x.LotPointage.Periode.Month == periode.Month &&
                                     x.LotPointage.Periode.Year == periode.Year &&
                                     !x.DateSuppression.HasValue)
                        .Get();
        }

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissement(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList)
        {
            return this.pointageRepository
                        .Query()
                        .Include(rl => rl.Rapport)
                        .Include(rl => rl.Ci.Societe.Groupe)
                        .Include(rl => rl.Ci.CompteInterneSep)
                        .Include(rl => rl.Ci.EtablissementComptable.Societe)
                        .Include(rl => rl.Personnel.Societe)
                        .Include(rl => rl.Personnel.EtablissementPaie)
                        .Include(rl => rl.CodeAbsence)
                        .Include(rl => rl.CodeMajoration)
                        .Include(rl => rl.CodeDeplacement)
                        .Include(rl => rl.CodeZoneDeplacement)
                        .Include(rl => rl.Materiel)
                        .Include(rl => rl.ListRapportLignePrimes.Select(rlp => rlp.Prime))
                        .Include(rl => rl.ListRapportLigneTaches.Select(rlt => rlt.Tache))
                        .Filter(x => (societeId.HasValue && x.Ci.SocieteId == societeId && !etablissementPaieIdList.Any()
                                            || x.Personnel.EtablissementPaieId.HasValue && etablissementPaieIdList.Contains(x.Personnel.EtablissementPaieId.Value)) &&
                                        x.LotPointageId.HasValue &&
                                        x.LotPointage.Periode.Month == periode.Month &&
                                        x.LotPointage.Periode.Year == periode.Year &&
                                        (!x.Personnel.DateSortie.HasValue || x.Personnel.DateSortie.Value.Year >= periode.Year && x.Personnel.DateSortie.Value.Month >= periode.Month))
                        .Get()
                        .AsNoTracking();
        }

        /// <summary>
        ///   Récupère la liste des pointages, dans le périmètre de l'utilisateur connecté, des rapports verrouillés d'une période donnée
        /// </summary>
        /// <param name="periode">Période choisie</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="etablissementPaieIdList">Liste des indentifiants d'établissements de paie</param>
        /// <returns>Liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep(DateTime periode, int? societeId, IEnumerable<int> etablissementPaieIdList)
        {
            return pointageRepository.GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep(periode, societeId, etablissementPaieIdList);
        }

        /// <inheritdoc/>
        public SearchRapportLigneEnt GetFiltersList()
        {
            SearchRapportLigneEnt searchPointage = new SearchRapportLigneEnt();
            searchPointage.DatePointageMin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            searchPointage.DatePointageMax = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            searchPointage.DatePointageAsc = false;
            searchPointage.IsReel = true;

            return searchPointage;
        }

        /// <inheritdoc/>
        public RapportLigneEnt GetNewPointageReelLight()
        {
            return new RapportLigneEnt
            {
                PointageId = 0,
                RapportId = 0,
                NbMaxPrimes = MaxPrimesCount,
                ListRapportLignePrimes = new List<RapportLignePrimeEnt>(),
                ListRapportLigneTaches = new List<RapportLigneTacheEnt>()
            };
        }

        /// <inheritdoc/>
        public PointagePrimeBase GetNewPointagePrime(RapportLigneEnt pointage, PrimeEnt prime)
        {
            return new RapportLignePrimeEnt
            {
                PrimeId = prime.PrimeId,
                Prime = prime,
                RapportLigne = pointage
            };
        }

        /// <summary>
        ///   Récupération des pointages dans le périmètre de l'utilisateur connecté (Minimum habilité Gestionnaire)
        /// </summary>
        /// <param name="period">Période choisie</param>
        /// <returns>Liste de pointages</returns>
        public async Task<IEnumerable<RapportLigneEnt>> GetPointagesAsync(DateTime period)
        {
            return await pointageRepository.GetPointagesAsync(period);
        }

        /// <summary>
        /// Récupère la tache par défaut d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la tache par défaut pour un CI donné</returns>
        public TacheEnt GetTacheParDefaut(int ciId)
        {
            TacheEnt tache = tacheManager.GetTacheParDefaut(ciId);
            if (tache != null)
            {
                return tache;
            }
            else
            {
                throw new ArgumentException(FeatureRapport.Pointage_Aucune_Tache_Defaut_Erreur);
            }
        }

        /// <summary>
        /// Intialise un nouveau pointage avec la tache par défaut du Ci
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="personnelId">Identifinat du perosnnel</param>
        /// <returns>Retourne le pointage initialisé</returns>
        public RapportLigneEnt InitTacheParDefautPointagePersonnel(int ciId, int personnelId)
        {
            RapportLigneEnt pointage = new RapportLigneEnt();
            pointage.CiId = ciId;
            pointage.Ci = ciRepository.Get(ciId);
            if (pointage.Ci.Societe.Groupe.Code.Equals(Constantes.CodeGroupeFES) && pointage.Ci.CIType != null && pointage.Ci.CIType.Code.Equals(Constantes.CiType.Etude))
            {
                return pointage;
            }
            AddTacheToPointageReel(pointage, GetTacheParDefaut(ciId));
            // Chargement du personnel pour le calcul des indemnités de déplacement
            pointage.Personnel = personnelRepository.GetPersonnel(personnelId);
            GetOrCreateIndemniteDeplacementForRapportLigne(pointage);
            return pointage;
        }

        #endregion

        #region SearchPointageReelWithFilter

        private string GetOrderBy(SearchRapportLigneEnt searchRapportLigne, bool isHebdomadaire = false)
        {
            //Bouchon dans l'attente de gestion d'une date max dans la récupération des pointages
            if (!isHebdomadaire) { searchRapportLigne.DatePointageMax = searchRapportLigne.DatePointageMin.AddMonths(1); }

            StringBuilder filter = new StringBuilder();

            if (searchRapportLigne.DatePointageAsc.HasValue && searchRapportLigne.DatePointageAsc.Value)
            {
                filter.Append("DatePointage ascending");
            }
            if (searchRapportLigne.DatePointageAsc.HasValue && !searchRapportLigne.DatePointageAsc.Value)
            {
                filter.Append("DatePointage descending");
            }
            if (!searchRapportLigne.DatePointageAsc.HasValue)
            {
                filter.Append("DatePointage");
            }

            return filter.ToString();
        }

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <param name="applyReadOnly">Variable permettant d'appliquer les règles de gestion au pointage</param>
        /// <param name="isHebdomadaire">si le cas est Hebdomadaire</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        public IEnumerable<RapportLigneEnt> SearchPointageReelWithFilter(SearchRapportLigneEnt searchRapportLigne, bool applyReadOnly, bool isHebdomadaire = false)
        {
            IEnumerable<RapportLigneEnt> pointages = Repository.SearchPointageWithFilter(searchRapportLigne.GetExpressionWhere(), GetOrderBy(searchRapportLigne, isHebdomadaire));
            if (pointages != null)
            {
                Parallel.ForEach(pointages, (pointage) => HandleSearchPointageReelWithFilter(pointage, applyReadOnly));
                return pointages;
            }

            return new RapportLigneEnt[] { };
        }

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        public IEnumerable<RapportLigneEnt> SearchPointageReelWithFilterByPage(SearchRapportLigneMoyenEnt searchRapportLigne, int page, int pageSize)
        {
            if (searchRapportLigne.PersonnelId.HasValue)
            {
                searchRapportLigne.PersonnelListAffectationMoyenIds = affectationMoyenRepository.GetAffectationMoyenIdListByPersonnelId(searchRapportLigne.PersonnelId.Value);
            }

            return Repository.SearchPointageWithFilterByPage(searchRapportLigne.GetPredicateWhere(), searchRapportLigne.GetOrderBy(), page, pageSize);
        }

        /// <summary>
        ///   Récupère la liste des pointages correspondant aux critères de recherche
        /// </summary>
        /// <param name="searchRapportLigne">Critère de recherche</param>
        /// <returns>IEnumerable contenant la liste des pointages correspondants aux critères</returns>
        public List<RapportLigneEnt> SearchPointageReelWithMoyenFilter(SearchRapportLigneMoyenEnt searchRapportLigne)
        {
            if (searchRapportLigne.PersonnelId.HasValue)
            {
                searchRapportLigne.PersonnelListAffectationMoyenIds = affectationMoyenRepository.GetAffectationMoyenIdListByPersonnelId(searchRapportLigne.PersonnelId.Value);
            }

            return Repository.SearchPointageReelWithMoyenFilter(searchRapportLigne.GetPredicateWhere(), searchRapportLigne.GetOrderBy());
        }

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="pointage">Un pointage</param>
        /// <returns>Le pointage mis à jour</returns>
        public RapportLigneEnt ApplyReadOnlyRGPointageReel(RapportLigneEnt pointage)
        {
            ApplyReadOnlyRGPointageBase(pointage);
            return pointage;
        }

        #endregion

        #region SearchPointageWithFilter

        /// <summary>
        ///   Génération des samedi en CP
        /// </summary>
        /// <param name="pointage">La base du pointage</param>
        public void GenerationPointageSamediCP(RapportLigneEnt pointage)
        {
            pointageSamediCongePayeService.GenerationPointageSamediCP(pointage);
        }

        /// <summary>
        /// Indique si le pointage est un samedi en congés payé.
        /// </summary>
        /// <param name="pointage">Le pointage concerné.</param>
        /// <returns>True si le pointage est un samedi en congés payé, sinon false.</returns>
        public bool IsSamediCP(RapportLigneEnt pointage)
        {
            return pointageSamediCongePayeService.IsSamediCP(pointage);
        }

        /// <summary>
        ///   Retourne les informations de pointage d'un personnel en fonction de ce personnel et d'une période
        ///   Les pointages retournés seront filtrés en fonction des CI accessibles à l'utilisateur connecté
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="periode">La période du pointage</param>
        /// <returns>Les informations de pointage du personnel</returns>
        public async Task<PointagePersonnelInfo> GetListPointagesByPersonnelIdAndPeriodeAsync(int personnelId, DateTime periode)
        {
            PointagePersonnelInfo pointagePersonnelInfo = await pointagePersonnelService.GetListPointagesForViewAsync(personnelId, periode);

            //Potentiellement l'utilisateur dont on va récupérer les pointages a pointé sur des CI auxquels l'utilisateur connecté n'a pas droit
            //On filtre donc les pointages récupérés en retirant de notre liste les pointages sur des CI innaccessibles à l'utilisateur connecté
            int utilisateurConnecteId = utilisateurManager.GetContextUtilisateurId();
            List<int> listCiAccessible = cisAccessiblesService.GetCisAccessiblesForUserAndPermission(utilisateurConnecteId, PermissionKeys.AffichageMenuCIIndex).Select(ci => ci.Id).ToList();
            IEnumerable<RapportLigneEnt> pointagesSurCiAccessible = pointagePersonnelInfo.Pointages.Where(p => listCiAccessible.Contains(p.CiId));

            GlobalDataForValidationPointage dataForValidation = rapportLignesValidationDataProvider.GetDataForValidateRapportLignes(pointagesSurCiAccessible);
            rapportLigneErrorBuilder.IncludeErrorsMessages(dataForValidation, pointagePersonnelInfo.Pointages);

            foreach (RapportLigneEnt p in pointagePersonnelInfo.Pointages
             .Where(p => !listCiAccessible.Contains(p.CiId)))
            {
                p.IsLocked = true;
            }

            return pointagePersonnelInfo;
        }

        /// <summary>
        /// Duplique un pointage
        /// </summary>
        /// <param name="rapportLigneId">ID du rapportLigne</param>
        /// <param name="ciId">ciId</param>
        /// <param name="startDate">startDate</param>
        /// <param name="endDate">endDate</param>
        /// <returns>DuplicatePointageResult</returns>
        public DuplicatePointageResult DuplicatePointage(int rapportLigneId, int ciId, DateTime startDate, DateTime endDate)
        {
            RapportLigneEnt rapportLigneToDuplicate = pointageDuplicatorService.GetPointageForDuplication(rapportLigneId);

            return pointageDuplicatorService.DuplicatePointage(rapportLigneToDuplicate, ciId, startDate, endDate);
        }

        #endregion

        #region Export

        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="typeExport">Type d'export (Excel ou Pdf)</param>
        /// <param name="isFes">Indique si l'édition est à destination de FES</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        public async Task<byte[]> GetPointagePersonnelExportAsync(int personnelId, DateTime periode, int typeExport, bool isFes, string templateFolderPath)
        {
            PointagePersonnelInfo pointagePersonnelInfo = await GetListPointagesByPersonnelIdAndPeriodeAsync(personnelId, periode).ConfigureAwait(false);
            List<RapportLigneEnt> pointages = pointagePersonnelInfo.Pointages;
            PersonnelEnt editeur = personnelManager.GetPersonnel(utilisateurManager.GetContextUtilisateur().UtilisateurId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;

            if (typeExport == TypeExport.Excel.ToIntValue())
            {
                return PointagePersonnelExport.ToExcel(pointages, editeur.CodeNomPrenom, periode, pathLogo, isFes, templateFolderPath);
            }
            else if (typeExport == TypeExport.Pdf.ToIntValue())
            {
                return PointagePersonnelExport.ToPdf(pointages, editeur.CodeNomPrenom, periode, pathLogo, isFes, templateFolderPath);
            }
            else
            {
                throw new FredBusinessException(BusinessResources.TypeExport_Error);
            }
        }

        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        public byte[] GetPointageInterimaireExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            IEnumerable<IGrouping<string, RapportLigneEnt>> contrats = new List<IGrouping<string, RapportLigneEnt>>();
            IEnumerable<RapportLigneEnt> pointages = GetListPointageInterimaireExport(pointagePersonnelExportModel);

            if (pointages.Any() && pointages != null)
            {
                contrats = TraitementPointageInterimaireExport(pointages, contrats);

                if (contrats != null)
                {
                    string pathImage = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(pointagePersonnelExportModel.Utilisateur.SocieteId.Value).Path;
                    if (pointagePersonnelExportModel.TypeExport == TypeExport.Excel.ToIntValue())
                    {
                        return PointageInterimaireExport.ToExcel(pointagePersonnelExportModel, contrats, pathImage);
                    }
                    else if (pointagePersonnelExportModel.TypeExport == TypeExport.Pdf.ToIntValue())
                    {
                        return PointageInterimaireExport.ToPdf(pointagePersonnelExportModel, contrats, pathImage);
                    }
                    else
                    {
                        throw new FredBusinessException(BusinessResources.TypeExport_Error);
                    }
                }
                else
                {
                    return new byte[0];
                }
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        ///   Retourne la liste de pointage en fonction du rapport demandé 
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Liste de pointages</returns>
        public IEnumerable<RapportLigneEnt> GetListPointageInterimaireExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            List<int> ciIdList = ciManager.GetCIList(pointagePersonnelExportModel.Organisation.OrganisationId).Select(c => c.CiId).ToList();

            return Repository.GetListPointagesInterimaireVerrouillesByPeriode(pointagePersonnelExportModel, ciIdList);
        }

        /// <summary>
        ///   Retourne la liste de pointages triées par contrat
        /// </summary>
        /// <param name="pointages">Liste de pointages</param>
        /// <param name="contrats">Container De pointages trié par contrats</param>
        /// <returns>Liste de pointages triées par contrat</returns>
        public IEnumerable<IGrouping<string, RapportLigneEnt>> TraitementPointageInterimaireExport(IEnumerable<RapportLigneEnt> pointages, IEnumerable<IGrouping<string, RapportLigneEnt>> contrats)
        {
            foreach (RapportLigneEnt pointage in pointages)
            {
                //Si l’intérimaire a travaillé pour plusieurs ETT dans la période choisie, ils a donc plusieurs contrats actifs
                PersonnelEnt clone = (PersonnelEnt)pointage.Personnel.Clone();
                clone.ContratActif = contratInterimaireManager.GetContratInterimaireByDatePointage(pointage.PersonnelId, pointage.DatePointage)
                                                   ?? contratInterimaireManager.GetContratInterimaireByDatePointageAndSouplesse(pointage.PersonnelId, pointage.DatePointage);
                pointage.Personnel = clone;
            }

            pointages = pointages.Where(rl => rl.Personnel.ContratActif != null).OrderBy(rl => rl.Personnel.NomPrenom);
            IEnumerable<IGrouping<string, RapportLigneEnt>> nomPrenoms = pointages.GroupBy(c => c.Personnel.NomPrenom);

            foreach (IGrouping<string, RapportLigneEnt> nomPrenom in nomPrenoms)
            {
                contrats = contrats.Concat(nomPrenom.GroupBy(c => string.Concat(c.CiId, " - ", c.Personnel.ContratActif.ContratInterimaireId)));
            }

            return contrats;
        }

        /// <summary>
        /// Vérifie si le personnel a des pointages dans une semaines
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <param name="mondayDate">premier jour de la semaine</param>
        /// <returns>true or false</returns>
        public bool CheckPointageByPersonnelAndCi(int personnelId, int ciId, DateTime mondayDate)
        {
            List<RapportLigneEnt> rapportLignes = Repository.GetPointageByPersonnelAndCiByDate(personnelId, ciId, mondayDate).ToList();
            return rapportLignes.IsNullOrEmpty() ? false : true;
        }

        /// <summary>
        /// Récupére les rapport lignes et determine si le personnel a des pointages
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <returns>true or false</returns>
        public bool GetpointageByPersonnelAndCi(int personnelId, int ciId)
        {
            AffectationEnt affectation = Managers.Affectation.GetAffectationByCiAndPersonnel(ciId, personnelId);
            List<RapportLigneEnt> rapportLignes = Repository.GetpointageByPersonnelAndCi(personnelId, ciId).ToList();
            if (rapportLignes.IsNullOrEmpty())
            {
                return false;
            }

            if (affectation.IsDelete)
            {
                affectation.IsDelete = false;
            }
            else
            {
                affectation.IsDelete = true;
                affectation.IsDelegue = false;
            }

            Save();

            List<int> personnelList = new List<int>();
            personnelList.Add(personnelId);
            int? organisationId = Managers.CI.GetOrganisationIdByCiId(ciId);
            Managers.Utilisateur.ManageRoleDelegueForCiPersonnel(organisationId.Value, utilisateurIdLisToAdd: null, utilisateurIdListToRemove: personnelList);
            return true;
        }

        /// <summary>
        /// Récupére les rapport lignes et determine si le personnel a des pointages
        /// </summary>
        /// <param name="ciId">identifiant de l'affaire</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>true or false</returns>
        public bool GetpointageByPersonnelAndCiForReadOnly(int ciId, int personnelId)
        {
            AffectationEnt affectation = Managers.Affectation.GetAffectationByCiAndPersonnel(ciId, personnelId);
            return affectation != null && affectation.IsDelete ? true : false;
        }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        public byte[] GetPointagePersonnelHebdomadaireExport(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            try
            {
                List<IEnumerable<IGrouping<int?, RapportLigneEnt>>> pointagesApresTri = new List<IEnumerable<IGrouping<int?, RapportLigneEnt>>>();

                pointagePersonnelExportModel = ConvertDateComptableToPeriode(pointagePersonnelExportModel);

                List<int> ciIdList = ciManager.GetCIList(pointagePersonnelExportModel.Organisation.OrganisationId).Select(c => c.CiId).ToList();
                IEnumerable<RapportLigneEnt> pointages = Repository.GetListPointagePersonnelHebdomadaire(pointagePersonnelExportModel, ciIdList);

                if (pointages.Any())
                {
                    IEnumerable<IGrouping<string, RapportLigneEnt>> personnels = pointages.GroupBy(p => p.Personnel.Matricule).OrderBy(p => p.Key);
                    foreach (IGrouping<string, RapportLigneEnt> personnel in personnels)
                    {
                        IEnumerable<IGrouping<int, RapportLigneEnt>> semaines = personnel.GroupBy(p => CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(p.DatePointage, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).OrderBy(s => s.Key);
                        foreach (IGrouping<int, RapportLigneEnt> semaine in semaines)
                        {
                            IEnumerable<IGrouping<int?, RapportLigneEnt>> evenements = semaine.GroupBy(s => s.CodeAbsenceId != null ? s.CodeAbsenceId : 0).OrderBy(e => e.Key);
                            pointagesApresTri.Add(evenements);
                        }
                    }

                    if (pointagePersonnelExportModel.TypeExport == TypeExport.Excel.ToIntValue())
                    {
                        string pathImage = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(pointagePersonnelExportModel.Utilisateur.SocieteId.Value).Path;
                        return PointagePersonnelHebdomadaireExport.ToExcel(pointagePersonnelExportModel, pointagesApresTri, pathImage);
                    }
                    else
                    {
                        throw new FredBusinessException(BusinessResources.TypeExport_Error);
                    }
                }
                else
                {
                    return new byte[0];
                }
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high

        /// <summary>
        ///   Création le fichier Excel ou Pdf contenant les pointages d'un personnel pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        public PointagePersonnelExportModel ConvertDateComptableToPeriode(PointagePersonnelExportModel pointagePersonnelExportModel)
        {
            if (pointagePersonnelExportModel.DateDebut == null && pointagePersonnelExportModel.DateFin == null)
            {
                pointagePersonnelExportModel.DateDebut = new DateTime(pointagePersonnelExportModel.DateComptable.Value.Year, pointagePersonnelExportModel.DateComptable.Value.Month, 1);
                int lastDayInMonth = DateTime.DaysInMonth(pointagePersonnelExportModel.DateComptable.Value.Year, pointagePersonnelExportModel.DateComptable.Value.Month);
                pointagePersonnelExportModel.DateFin = new DateTime(pointagePersonnelExportModel.DateComptable.Value.Year, pointagePersonnelExportModel.DateComptable.Value.Month, lastDayInMonth);
            }

            return pointagePersonnelExportModel;
        }

        /// <inheritdoc />
        public string GetPointagePersonnelExportFilename(int personnelId, DateTime periode)
        {
            PersonnelEnt perso = this.personnelRepository.GetPersonnel(personnelId);
            return string.Format(FeatureRapport.Rapport_PointagePersonnel_Filename,
                                               perso.Nom,
                                               perso.Matricule,
                                               string.Format("{0:yyyyMM}", periode));
        }

        /// <inheritdoc />
        public string GetPointageInterimaireExportFilename(DateTime dateDebut, DateTime dateFin)
        {
            return string.Format(FeatureRapport.Pointage_Export_Filename_Interimaire,
                                               string.Format("{0:ddMMyyyy}", dateDebut),
                                               string.Format("{0:ddMMyyyy}", dateFin));
        }

        /// <inheritdoc />
        public string GetPointagePersonnelHebdomadaireExportFilename(DateTime dateComptable, DateTime dateDebut, DateTime dateFin)
        {
            PointagePersonnelExportModel pointagePersonnelExportModel = new PointagePersonnelExportModel()
            {
                DateComptable = dateComptable,
                DateDebut = dateDebut,
                DateFin = dateFin
            };

            pointagePersonnelExportModel = ConvertDateComptableToPeriode(pointagePersonnelExportModel);

            return string.Format(FeatureRapport.Pointage_Export_Filename_Personnel_Hebdomadaire,
                                               string.Format("{0:ddMMyyyy}", pointagePersonnelExportModel.DateDebut),
                                               string.Format("{0:ddMMyyyy}", pointagePersonnelExportModel.DateFin));
        }

        /// <summary>
        /// Récupére les personnels les pointages pour challenge sécurité pour une période donnée (au format byte[])
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <returns>Fichier d'export au format byte[]</returns>
        public byte[] GetPointageChallengeSecuriteExport(PointagePersonnelExportModel pointagePersonnelExportModel, string templateFolderPath)
        {
            pointagePersonnelExportModel = ConvertDateComptableToPeriode(pointagePersonnelExportModel);

            ExcelFormat excelFormat = new ExcelFormat();
            string pathName = Path.Combine(templateFolderPath, "PointagePersonnel/TemplatePointageChallengeSecurite.xlsx").ToString();

            IEnumerable<CIEnt> ciIdList = ciManager.GetCIList(pointagePersonnelExportModel.Organisation.OrganisationId);

            IEnumerable<PersonnelRapportSummaryEnt> personnelsSummary = pointageRepository.GetPointageChallengeSecurite(pointagePersonnelExportModel, ciIdList.Select(c => c.CiId).ToList());

            List<PersonnelSummaryPointageModel> result = new List<PersonnelSummaryPointageModel>();

            foreach (PersonnelRapportSummaryEnt personnelRapportSummary in personnelsSummary)
            {
                PersonnelEnt personnel = personnelRepository.GetPersonnel(personnelRapportSummary.PersonnelId);
                result.Add(new PersonnelSummaryPointageModel
                {
                    PersonnelId = personnelRapportSummary.PersonnelId,
                    Nom = personnel.Nom,
                    Prenom = personnel.Prenom,
                    Matricule = personnel.Matricule,
                    Statut = PersonnelUtils.GetStatutCodeFromStatutId(personnel.Statut),
                    Fonction = personnel.Ressource?.Libelle,
                    EtablissementComptable = ciIdList.Where(ci => ci.CiId.Equals(personnelRapportSummary.CiId)).Select(ci => string.Format("{0} {1}", ci.EtablissementComptable?.Code, ci.EtablissementComptable?.Libelle)).FirstOrDefault(),
                    EtablissementPaie = string.Format("{0} {1}", personnel.EtablissementPaie?.Code, personnel.EtablissementPaie?.Libelle),
                    TotalHeure = personnelRapportSummary.TotalHeures,
                    TotalHeureAbsence = personnelRapportSummary.TotalHeuresAbsence.HasValue ? personnelRapportSummary.TotalHeuresAbsence : 0
                });
            }
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + pointagePersonnelExportModel.Utilisateur.MatriculeNomPrenom;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Personnel_Hebdomadaire_Export_A + DateTime.Now.ToShortTimeString();
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(pointagePersonnelExportModel.Utilisateur.SocieteId.Value).Path;
            BuildHeaderExcelModel buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.Edition_Challenge_Securite_Titre, pointagePersonnelExportModel.Organisation.CodeLibelle, dateEdition, editePar, null, pathLogo, new IndexHeaderExcelModel(2, 7, 8, 8));
            return result.Any() ? excelFormat.GenerateExcel<PersonnelSummaryPointageModel>(pathName, result, null, buildHeaderModel) : null;
        }

        /// <summary>
        ///   Création du nom de fichier d'export des pointages challenge securite
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="dateDebut">Date début</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Nom du fichier</returns>
        public string GetPointageChallengeSecuriteExportFilename(DateTime dateComptable, DateTime dateDebut, DateTime dateFin)
        {
            PointagePersonnelExportModel pointagePersonnelExportModel = new PointagePersonnelExportModel()
            {
                DateComptable = dateComptable,
                DateDebut = dateDebut,
                DateFin = dateFin
            };

            pointagePersonnelExportModel = ConvertDateComptableToPeriode(pointagePersonnelExportModel);

            return string.Format(FeatureRapport.Pointage_Export_Filename_Challenge_Securite,
                                               string.Format("{0:ddMMyyyy}", pointagePersonnelExportModel.DateDebut),
                                               string.Format("{0:ddMMyyyy}", pointagePersonnelExportModel.DateFin));
        }

        #endregion

        /// <inheritdoc/>
        public List<RapportLigneEnt> GetAllPointagesForMaterielStorm(int rapportId)
        {
            return this.Repository
                        .Query()
                        .Include(x => x.Ci)
                        .Include(x => x.AuteurCreation.Personnel.Societe)
                        .Include(x => x.Rapport)
                        .Include(x => x.Rapport.AuteurCreation.Personnel.Societe)
                        .Include(x => x.Rapport.ValideurCDC.Personnel.Societe)
                        .Include(x => x.Rapport.ValideurCDT.Personnel.Societe)
                        .Include(x => x.Rapport.ValideurDRC.Personnel.Societe)
                        .Include(x => x.Rapport.AuteurVerrou.Personnel.Societe)
                        .Include(x => x.Materiel.Societe)
                        .Include(x => x.Personnel)
                        .Include(x => x.ListRapportLigneTaches)
                        .Include(x => x.CodeDeplacement)
                        .Filter(x => x.RapportId == rapportId)
                        .Get()
                        .AsNoTracking()
                        .ToList();
        }

        /// <summary>
        /// Permet de récupérer toutes les lignes de pointage d'un rapport
        /// associétés à un personnel .
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <returns>List de RapportLigneEnt</returns>
        public List<RapportLigneEnt> GetPointagesPersonnelsForSap(int rapportId)
        {
            return this.Repository
                        .Query()
                        .Include(x => x.Ci)
                        .Include(x => x.Rapport)
                        .Include(x => x.Personnel)
                        .Include(x => x.ListRapportLignePrimes.Select(p => p.Prime))
                        .Include(x => x.ListRapportLigneAstreintes)
                        .Include(x => x.ListRapportLigneTaches.Select(t => t.Tache))
                        .Include(x => x.CodeDeplacement)
                        .Include(x => x.CodeMajoration)
                        .Include(x => x.CodeAbsence)
                        .Filter(x => x.RapportId == rapportId && x.PersonnelId != null)
                        .Get()
                        .AsNoTracking()
                        .ToList();
        }

        /// <summary>
        /// Supprimer la liste des lignes de rapport sauf celle spécifié
        /// </summary>
        /// <param name="rapportId">L'identifiant du rapport </param>
        /// <param name="rapportLigneIdList">Les identifiants des rapports ligne à garder</param>
        /// <returns>Liste des RapportLigne qui n'existe pas dans la liste rapportLigneIdList</returns>
        public IEnumerable<RapportLigneEnt> GetOtherRapportLignes(int rapportId, List<int> rapportLigneIdList)
        {
            return this.Repository.GetOtherRapportLignes(rapportId, rapportLigneIdList);
        }

        /// <summary>
        ///   Insère une entité de liaison RapportLigneMajorationEnt dans le contexte
        /// </summary>
        /// <param name="rapportLigneMajoration">entité à insére</param>
        public void InsertRapportLigneMajoration(RapportLigneMajorationEnt rapportLigneMajoration)
        {
            this.rapportLigneMajorationRepo.Insert(rapportLigneMajoration);
        }

        /// <summary>
        /// Get personnel summary 
        /// </summary>
        /// <param name="personnelIdList">Personnel id list</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>IEnumerable of Personnel rapport summary</returns>
        public IEnumerable<PersonnelRapportSummaryEnt> GetPersonnelPointageSummary(List<int> personnelIdList, DateTime mondayDate)
        {
            if (personnelIdList.IsNullOrEmpty())
            {
                return new List<PersonnelRapportSummaryEnt>();
            }

            return Repository.GetPersonnelPointageSummary(personnelIdList, mondayDate);
        }

        /// <summary>
        /// Get Ci pointage summary
        /// </summary>
        /// <param name="ciIdList">Ci id list</param>
        /// <param name="personnelStatut">Personnel statut</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Enumerable de CiPointage Summary</returns>
        public IEnumerable<CiPointageSummaryEnt> GetCiPointageSummary(List<int> ciIdList, string personnelStatut, DateTime mondayDate)
        {
            List<CiPointageSummaryEnt> ciPointageSummaryList = new List<CiPointageSummaryEnt>();
            if (ciIdList.IsNullOrEmpty())
            {
                return ciPointageSummaryList;
            }

            List<RapportLigneEnt> listRapportLignesQuery = Repository.GetCiPointageSummary(ciIdList, personnelStatut, mondayDate).ToList();
            List<CiHeuresSupSummaryEnt> ciHeuresSupSummaryList = GetTotalHeuresSupForCi(listRapportLignesQuery);
            List<IGrouping<int, RapportLigneEnt>> pointageListGroupByCi = listRapportLignesQuery.GroupBy(x => x.CiId).ToList();
            foreach (IGrouping<int, RapportLigneEnt> pointageGroupByCi in pointageListGroupByCi)
            {
                ciPointageSummaryList.Add(new CiPointageSummaryEnt
                {
                    CiId = pointageGroupByCi.FirstOrDefault().CiId,
                    TotalHeuresAbsence = pointageGroupByCi.Sum(o => o.HeureAbsence),
                    TotalHeuresMajorations = pointageGroupByCi.Sum(s => s.ListRapportLigneMajorations?.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration)),
                    TotalHeuresNormale = pointageGroupByCi.Sum(o => o.ListRapportLigneTaches?.Sum(x => x.HeureTache)),
                    TotalHeuresNormalesSup = ciHeuresSupSummaryList.FirstOrDefault(x => x.CiId == pointageGroupByCi.FirstOrDefault().CiId) != null ?
                                             ciHeuresSupSummaryList.FirstOrDefault(x => x.CiId == pointageGroupByCi.FirstOrDefault().CiId).HeuresSup : 0
                });
            }

            return ciPointageSummaryList;
        }

        /// <summary>
        ///   Créer une ligne de majoration vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="majoration">La majoration</param>
        /// <returns>Une majoration correspondant à une ligne de rapport</returns>
        public RapportLigneMajorationEnt GetNewPointageReelMajoration(RapportLigneEnt pointageReel, CodeMajorationEnt majoration)
        {
            return new RapportLigneMajorationEnt
            {
                CodeMajorationId = majoration.CodeMajorationId,
                CodeMajoration = majoration,
                RapportLigne = pointageReel,
                HeureMajoration = 0
            };
        }

        /// <summary>
        /// Duplique une ligne de majoartion de rapport
        /// </summary>
        /// <param name="pointageMajoration">La ligne de majoration de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une ligne de majoration de rapport</returns>
        public RapportLigneMajorationEnt DuplicatePointageReelMajoration(RapportLigneMajorationEnt pointageMajoration, bool emptyValues = false)
        {
            RapportLigneMajorationEnt duplicatedPointageMajoration = pointageMajoration.Duplicate();

            if (emptyValues)
            {
                duplicatedPointageMajoration.HeureMajoration = 0;
                duplicatedPointageMajoration.IsDeleted = false;
            }

            return duplicatedPointageMajoration;
        }

        /// <summary>
        /// Duplique une liste de ligne de majoration de rapport
        /// </summary>
        /// <param name="listePointageReelMajoration">La liste de ligne de majoration de rapport à dupliquer</param>
        /// <param name="emptyValues">Valeurs vides</param>
        /// <returns>Une liste de ligne de majoration de rapport</returns>
        public IEnumerable<RapportLigneMajorationEnt> DuplicateListPointageReelMajoration(IEnumerable<RapportLigneMajorationEnt> listePointageReelMajoration, bool emptyValues = false)
        {
            List<RapportLigneMajorationEnt> newListPointageReelMajoration = new List<RapportLigneMajorationEnt>();

            foreach (RapportLigneMajorationEnt pointageReelMajoration in listePointageReelMajoration)
            {
                if (!pointageReelMajoration.IsDeleted)
                {
                    newListPointageReelMajoration.Add(DuplicatePointageReelMajoration(pointageReelMajoration, emptyValues));
                }
            }

            return newListPointageReelMajoration;
        }

        /// <summary>
        /// Get total hours work and absence and majoration for validation
        /// </summary>
        /// <param name="personnelId">Personneel identifier</param>
        /// <param name="datePointage">Date du chantier</param>
        /// <returns>Total des heures</returns>
        public double GetTotalHoursWorkAndAbsenceWithMajoration(int personnelId, DateTime datePointage)
        {
            return repoRapport.GetTotalHoursWorkAndAbsenceWithMajoration(personnelId, datePointage);
        }

        /// <summary>
        /// Get prime
        /// </summary>
        /// <param name="rapportLigneId">Rapport ligne identifier</param>
        /// <param name="primeId">Prime identifier</param>
        /// <returns>Rapport Ligne prime</returns>
        public RapportLignePrimeEnt FindPrime(int rapportLigneId, int primeId)
        {
            return this.rapportLignePrimeRepo.FindPrime(rapportLigneId, primeId);
        }

        /// <summary>
        /// Check if rapport ligne existance
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <returns>RapportLigne Identifier if exist</returns>
        public int CheckRapportLigneExistance(int rapportId, int personnelId)
        {
            return pointageRepository.CheckRapportLigneExistance(rapportId, personnelId);
        }

        /// <summary>
        /// Handle majoration list
        /// </summary>
        /// <param name="listRapportLigneMajorations">list rapport ligne majoration</param>
        private void ApplyValuesRGPointageMajorationReel(ICollection<RapportLigneMajorationEnt> listRapportLigneMajorations)
        {
            if (listRapportLigneMajorations != null && listRapportLigneMajorations.Any())
            {
                foreach (RapportLigneMajorationEnt rapportLigneMajoration in listRapportLigneMajorations)
                {
                    if (rapportLigneMajoration.CodeMajoration != null && !rapportLigneMajoration.CodeMajoration.EtatPublic)
                    {
                        rapportLigneMajoration.CodeMajoration = null;
                    }
                }
            }
        }

        /// <summary>
        ///   Traitement des majorations
        /// </summary>
        /// <param name="pointage">Pointage</param>
        private void HandleRapportLigneMajoration(RapportLigneEnt pointage)
        {
            List<RapportLigneMajorationEnt> listMajorationDeleted = new List<RapportLigneMajorationEnt>();
            if (pointage.ListRapportLigneMajorations != null)
            {
                foreach (RapportLigneMajorationEnt pointageMajoration in pointage.ListRapportLigneMajorations)
                {
                    if (pointageMajoration.IsDeleted || pointage.IsDeleted)
                    {
                        listMajorationDeleted.Add(pointageMajoration);
                    }
                    else if (pointageMajoration.IsCreated)
                    {
                        this.rapportLigneMajorationRepo.Insert(pointageMajoration);
                    }
                    else
                    {
                        this.rapportLigneMajorationRepo.Update(pointageMajoration);
                    }
                }
            }

            for (int i = listMajorationDeleted.Count - 1; i >= 0; i--)
            {
                RapportLigneMajorationEnt ligneMajoration = listMajorationDeleted[i];
                pointage.ListRapportLigneMajorations.Remove(ligneMajoration);
                rapportLigneMajorationRepo.Delete(ligneMajoration);
            }
        }

        /// <summary>
        /// Apply Values RGPointageReel pour les majorations
        /// </summary>
        /// <param name="pointage">pointage</param>
        private void ApplyValuesRGPointageReelMajoration(RapportLigneEnt pointage)
        {
            // Majoration
            if (pointage.Ci?.Societe?.Groupe != null && pointage.Ci.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
            {
                ApplyValuesRGPointageMajorationReel(pointage.ListRapportLigneMajorations);
            }

            if (pointage.CodeMajoration != null && !pointage.CodeMajoration.EtatPublic)
            {
                pointage.CodeMajoration = null;
                pointage.CodeMajorationId = null;
            }
        }

        /// <summary>
        ///   Retourne le nombre de pointage sur un contrat interimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Interimaire</param>
        /// <returns>Un nombre de pointage</returns>
        public int GetPointageForContratInterimaire(ContratInterimaireEnt contratInterimaireEnt)
        {
            try
            {
                return Repository.GetPointageForContratInterimaire(contratInterimaireEnt);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Get list des primes affected to list of personnel
        /// </summary>
        /// <param name="primePersonnelModel">prime Personnel get model</param>
        /// <returns>List des primes affected</returns>
        public List<PrimePersonnelAffectationEnt> PrimePersonnelAffected(PrimesPersonnelsGetEnt primePersonnelModel)
        {
            List<PrimePersonnelAffectationEnt> primeAffectationList = new List<PrimePersonnelAffectationEnt>();

            if (primePersonnelModel != null && !primePersonnelModel.PersonnelIdList.IsNullOrEmpty())
            {
                foreach (int perseonnelId in primePersonnelModel.PersonnelIdList)
                {
                    PrimePersonnelAffectationEnt primePersonnel = new PrimePersonnelAffectationEnt();
                    primePersonnel.PersonnelId = perseonnelId;
                    primePersonnel.PrimeList = HandlePersonnelPrimeAffectation(perseonnelId, primePersonnelModel.DatePointage);
                    primeAffectationList.Add(primePersonnel);
                }
            }

            return primeAffectationList;
        }

        /// <summary>
        ///   Retourne les pointages vérouiller par rapport à personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique d'un personnel</param>
        /// <returns>Liste de pointage vérouiller</returns>
        public IEnumerable<RapportLigneEnt> GetPointageVerrouillerByPersonnelId(int personnelId)
        {
            try
            {
                return Repository.GetPointageVerrouillerByPersonnelId(personnelId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Handle prime affectation list 
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date pointage</param>
        /// <returns>Prime affectation List for personnel</returns>
        private List<PrimeAffectationEnt> HandlePersonnelPrimeAffectation(int personnelId, DateTime datePointage)
        {
            List<PrimeAffectationEnt> personnelprimeAffectationList = new List<PrimeAffectationEnt>();
            List<RapportLigneEnt> rapportLignes = Repository.PrimePersonnelAffected(personnelId, datePointage, datePointage.AddDays(6));
            if (!rapportLignes.IsNullOrEmpty())
            {
                foreach (RapportLigneEnt pointage in rapportLignes)
                {
                    if (!pointage.ListRapportLignePrimes.IsNullOrEmpty())
                    {
                        foreach (RapportLignePrimeEnt pointagePrime in pointage.ListRapportLignePrimes)
                        {
                            personnelprimeAffectationList.Add(
                              new PrimeAffectationEnt
                              {
                                  CodePrime = pointagePrime.Prime.Code,
                                  AffectationDay = (int)pointage.DatePointage.DayOfWeek > 0 ? (int)pointage.DatePointage.DayOfWeek - 1 : 6,
                                  CiId = pointage.CiId,
                                  IsAffected = pointagePrime.IsChecked,
                              });
                        }
                    }
                }
            }

            return personnelprimeAffectationList;
        }

        /// <summary>
        /// Ajout ou mise à jour en masse
        /// </summary>
        /// <param name="rapportLignes">Liste de lignes de rapports</param>
        public void AddOrUpdateRapportLigneList(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            Repository.AddOrUpdateRapportLigneList(rapportLignes);
        }

        /// <summary>
        /// Verifier si le rapport comporte des pointages
        /// </summary>
        /// <param name="listRapportLigne">listes des rapports ligne</param>
        /// <returns>le rapport a enregistré</returns>
        public List<RapportLigneEnt> CheckPointageForSave(IEnumerable<RapportLigneEnt> listRapportLigne)
        {
            List<RapportLigneEnt> result = new List<RapportLigneEnt>();
            bool isUtilisateurConnectedFes = utilisateurManager.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);

            foreach (RapportLigneEnt rapportLigne in listRapportLigne)
            {
                if (!isUtilisateurConnectedFes)
                {
                    result.Add(rapportLigne);
                }
                else if (CheckRapportLigneBeforeSave(rapportLigne))
                {
                    result.Add(rapportLigne);
                }
                else if (rapportLigne.RapportId > 0)
                {
                    rapportLigne.IsDeleted = true;
                    result.Add(rapportLigne);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks the rapport ligne before save.
        /// </summary>
        /// <param name="rapportLigne">The rapport ligne.</param>
        /// <returns>Boolean</returns>
        /// <remarks>J'ai laissé l'entête, parce qu'elle est collector ... (ironie inside)</remarks>
        public bool CheckRapportLigneBeforeSave(RapportLigneEnt rapportLigne)
        {
            bool ligneValide = false;

            ligneValide = CheckSaveAbsences(rapportLigne, ligneValide);
            ligneValide = CheckSaveTaches(rapportLigne, ligneValide);
            ligneValide = CheckSaveAstreintes(rapportLigne, ligneValide);
            ligneValide = CheckSaveMajoration(rapportLigne, ligneValide);
            ligneValide = CheckSavePrimes(rapportLigne, ligneValide) || (rapportLigne.CodeAbsenceId != 0 && !rapportLigne.HeureAbsence.Equals(0));
            return ligneValide;
        }

        /// <summary>
        /// récupére les pointages des personnels passé en paramétre
        /// </summary>
        /// <param name="personnelModel">model contenant la liste des personnels et la date </param>
        /// <returns>liste des personnels avec leur pointages de la semaine</returns>
        public List<RapportHebdoPersonnelWithTotalHourEnt> GetPointageByPersonnelIDAndInterval(RapportHebdoPersonnelWithAllCiEnt personnelModel)
        {
            List<RapportHebdoPersonnelWithTotalHourEnt> personnelPointageList = new List<RapportHebdoPersonnelWithTotalHourEnt>();

            if (personnelModel != null && !personnelModel.PersonnelIds.IsNullOrEmpty())
            {
                foreach (int personnelId in personnelModel.PersonnelIds)
                {
                    RapportHebdoPersonnelWithTotalHourEnt personnel = new RapportHebdoPersonnelWithTotalHourEnt();
                    personnel.PersonnelId = personnelId;
                    personnel.ListTotalHours = HandlePersonnelTotalHour(personnelId, personnelModel.Mondaydate, personnelModel.IsForMonth);
                    personnelPointageList.Add(personnel);
                    personnel.PointageStatutCode = this.CalculateRapportHebdoStatutForWeek(personnel.ListTotalHours);
                    if (!string.IsNullOrEmpty(personnel.PointageStatutCode))
                    {
                        RapportStatutEnt statut = Repository.GetRapportStatutAllSync().FirstOrDefault(x => x.Code == personnel.PointageStatutCode);
                        personnel.PointageStatutLibelle = statut?.Libelle;
                    }
                }
            }

            return personnelPointageList;
        }

        /// <summary>
        /// Compareer le statut du rapport ligne a statut donné
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date Pointage</param>
        /// <param name="statutCode">Statut code a comparer</param>
        /// <returns>True si le rapport statut egal au statut passer en param</returns>
        public bool CheckRapportLigneStatut(int ciId, int personnelId, DateTime datePointage, string statutCode)
        {
            string rapportStatutCode = this.Repository.GetRapportLigneStatutCode(personnelId, ciId, datePointage);
            if (!rapportStatutCode.IsNullOrEmpty() && !statutCode.IsNullOrEmpty())
            {
                return rapportStatutCode.Equals(statutCode);
            }

            return false;
        }

        /// <summary>
        /// Verifier si les heures d'absences si elles sont égales à zéro
        /// </summary>
        /// <param name="rapportLigne">rapport Ligne</param>
        /// <param name="ligneValide">Indique s'il faut enregister la ligne ou non</param>
        /// <returns>return true or false</returns>
        private bool CheckSaveAbsences(RapportLigneEnt rapportLigne, bool ligneValide)
        {
            if (rapportLigne.HeureAbsence.Equals(0))
            {
                rapportLigne.CodeAbsence = null;
                rapportLigne.CodeAbsenceId = null;
            }

            return ligneValide;
        }

        /// <summary>
        /// Verifier si les heure des taches est supérieure a zero
        /// </summary>
        /// <param name="rapportLigne">rapport Ligne</param>
        /// <param name="ligneValide">Check tache is ok for save </param>
        /// <returns>return true or false</returns>
        private bool CheckSaveTaches(RapportLigneEnt rapportLigne, bool ligneValide)
        {
            if (!rapportLigne.ListRapportLigneTaches.IsNullOrEmpty())
            {
                rapportLigne.ListRapportLigneTaches.ToList().ForEach(ligneTache =>
                {
                    if (ligneTache.HeureTache.Equals(0))
                    {
                        rapportLigne.IsUpdated = true;
                        ligneTache.IsDeleted = true;
                    }
                    else
                    {
                        ligneValide = true;
                    }
                });
            }

            return ligneValide;
        }

        /// <summary>
        /// Verifier si les heure des Astreintes est supérieure a zero
        /// </summary>
        /// <param name="rapportLigne">rapport Ligne</param>
        /// <param name="ligneValide">ligne Valide</param>
        /// <returns>True si la ligne peut être sauvegardée</returns>
        private bool CheckSaveAstreintes(RapportLigneEnt rapportLigne, bool ligneValide)
        {
            if (!rapportLigne.ListRapportLigneAstreintes.IsNullOrEmpty())
            {
                rapportLigne.ListRapportLigneAstreintes.ToList().ForEach(ligneAstreinte =>
                {
                    if (ligneAstreinte.DateDebutAstreinte != ligneAstreinte.DateFinAstreinte)
                    {
                        ligneValide = true;
                    }
                });
            }

            return ligneValide;
        }

        /// <summary>
        /// Verifier si les heure des Majorations est supérieure a zero
        /// </summary>
        /// <param name="rapportLigne">rapport Ligne</param>
        /// <param name="ligneValide">ligne Valide</param>
        /// <returns>True si la ligne peut être sauvée</returns>
        private bool CheckSaveMajoration(RapportLigneEnt rapportLigne, bool ligneValide)
        {
            if (!rapportLigne.ListRapportLigneMajorations.IsNullOrEmpty())
            {
                rapportLigne.ListRapportLigneMajorations.ToList().ForEach(ligneMajoration =>
                {
                    if (ligneMajoration.HeureMajoration.Equals(0) && ligneMajoration.RapportLigneId > 0)
                    {
                        rapportLigne.IsUpdated = true;
                        ligneMajoration.IsDeleted = true;
                    }
                    else if (ligneMajoration.HeureMajoration.Equals(0) && ligneMajoration.RapportLigneId == 0)
                    {
                        rapportLigne.ListRapportLigneMajorations.Remove(ligneMajoration);
                    }
                    else
                    {
                        ligneValide = true;
                    }
                });
            }
            return ligneValide;
        }

        /// <summary>
        /// Verifier si les heure des primes est supérieure a zero
        /// </summary>
        /// <param name="rapportLigne">rapport Ligne</param>
        /// <param name="ligneValide">ligne Valide</param>
        /// <returns>True si la ligne peut être sauvegardée</returns>
        private bool CheckSavePrimes(RapportLigneEnt rapportLigne, bool ligneValide)
        {
            if (!rapportLigne.ListRapportLignePrimes.IsNullOrEmpty())
            {
                rapportLigne.ListRapportLignePrimes.ToList().ForEach(lignePrime =>
                {
                    if (!lignePrime.IsChecked)
                    {
                        rapportLigne.IsUpdated = true;
                        lignePrime.IsDeleted = true;
                    }
                    else
                    {
                        ligneValide = true;
                    }
                });
            }

            return ligneValide;
        }

        /// <summary>
        /// récupére pour un personnel ses pointages de la semaine
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="datePointage">premier jour de la semaine</param>
        /// <param name="isForMonth">si c'est pour un pointage hebdomadaire ou Mensuelle</param>
        /// <returns>Personnel avec sa liste de pointage de la semaine</returns>
        private List<PersonnelTotalHourByDayEnt> HandlePersonnelTotalHour(int personnelId, DateTime datePointage, bool isForMonth)
        {
            List<PersonnelTotalHourByDayEnt> personnelist = new List<PersonnelTotalHourByDayEnt>();
            if (!isForMonth)
            {
                for (int y = 0; y < 7; y++)
                {
                    PersonnelTotalHourByDayEnt personnelTotalHourByDay = new PersonnelTotalHourByDayEnt
                    {
                        DayNumber = y,
                        ListTotalHourByCi = HandlePersonnelTotalHourByCi(personnelId, datePointage, y, isForMonth),
                    };

                    personnelTotalHourByDay.PointageStatutCode = CalculateRapportHebdoStatutForDay(personnelTotalHourByDay.ListTotalHourByCi);
                    personnelist.Add(personnelTotalHourByDay);
                }
                if (personnelist.Count < 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        PersonnelTotalHourByDayEnt personnelTotalHourByDay = personnelist.FirstOrDefault(x => x.DayNumber == i);
                        if (personnelTotalHourByDay == null)
                        {
                            personnelist.Add(
                              new PersonnelTotalHourByDayEnt
                              {
                                  DayNumber = i,
                              });
                        }
                    }
                }
            }
            return personnelist;
        }

        #region statut rapport hebdo

        /// <summary>
        /// Calcul du statut par jour et par CI
        /// </summary>
        /// <param name="rapportLigneStatutId">id de statut de rapport ligne</param>
        /// <returns>Statut</returns>
        private string CalculateRapportHebdoStatutForDayAndCI(int? rapportLigneStatutId)
        {
            int statut = rapportLigneStatutId ?? RapportStatutEnt.RapportStatutEnCours.Key;
            if (statut == RapportStatutEnt.RapportStatutVerrouille.Key)
            {
                return RapportStatutEnt.RapportStatutVerrouille.Value;
            }
            else if (statut == RapportStatutEnt.RapportStatutValide2.Key || statut == RapportStatutEnt.RapportStatutValide3.Key)
            {
                return RapportStatutEnt.RapportStatutValide2.Value;
            }
            else
            {
                return RapportStatutEnt.RapportStatutEnCours.Value;
            }
        }

        /// <summary>
        /// Calcul du statut par jour 
        /// </summary>
        /// <param name="listTotalHourByDayAndByCi">liste des heures par jour et par CI</param>
        /// <returns>Statut</returns>
        private string CalculateRapportHebdoStatutForDay(List<PersonnelTotalHourByDayAndByCiEnt> listTotalHourByDayAndByCi)
        {
            if ((listTotalHourByDayAndByCi.Sum(x => x.TotalHours) + listTotalHourByDayAndByCi.Sum(x => x.TotalAbsence) + listTotalHourByDayAndByCi.Sum(x => x.TotalMajoration) < 7)
                || listTotalHourByDayAndByCi.Any(x => x.PointageStatutCode == RapportStatutEnt.RapportStatutEnCours.Value))
            {
                return RapportStatutEnt.RapportStatutEnCours.Value;
            }
            else if (listTotalHourByDayAndByCi.Any(x => x.PointageStatutCode == RapportStatutEnt.RapportStatutValide2.Value))
            {
                return RapportStatutEnt.RapportStatutValide2.Value;
            }
            else
            {
                return RapportStatutEnt.RapportStatutVerrouille.Value;
            }
        }

        /// <summary>
        /// Calcul du statut par semaine 
        /// </summary>
        /// <param name="listTotalHourByDay">liste des heures par jour</param>
        /// <returns>Statut</returns>
        private string CalculateRapportHebdoStatutForWeek(List<PersonnelTotalHourByDayEnt> listTotalHourByDay)
        {
            // liste des jours indexés du dimanche au samedi => DayOfWeek 
            // exclusion du lundi et du dimanche.
            IEnumerable<PersonnelTotalHourByDayEnt> workingDays = listTotalHourByDay.Skip(1).Take(5);
            if (workingDays.Any(x => x.PointageStatutCode == RapportStatutEnt.RapportStatutEnCours.Value))
            {
                return RapportStatutEnt.RapportStatutEnCours.Value;
            }
            else if (workingDays.Any(x => x.PointageStatutCode == RapportStatutEnt.RapportStatutValide2.Value))
            {
                return RapportStatutEnt.RapportStatutValide2.Value;
            }
            else
            {
                return RapportStatutEnt.RapportStatutVerrouille.Value;
            }
        }

        #endregion statut rapport hebdo

        /// <summary>
        /// récupére pour un personnel ses pointages de chaque journée
        /// </summary>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <param name="datePointage">premier jour de la semaine</param>
        /// <param name="day">le jour exacte </param>
        /// <param name="isForMonth">si c'est pour un pointage hebdomadaire ou Mensuelle</param>
        /// <returns>Personnel avec sa liste de pointage de la journée</returns>
        private List<PersonnelTotalHourByDayAndByCiEnt> HandlePersonnelTotalHourByCi(int personnelId, DateTime datePointage, int day, bool isForMonth)
        {
            List<PersonnelTotalHourByDayAndByCiEnt> personnelist = new List<PersonnelTotalHourByDayAndByCiEnt>();
            if (!isForMonth)
            {
                List<RapportLigneEnt> rapportLignes = Repository.GetRapportLigneByPersonnelIdAndWeek(personnelId, datePointage, datePointage.AddDays(6)).ToList();
                foreach (RapportLigneEnt rapportLigne in rapportLignes.Where(rl => (int)rl.DatePointage.DayOfWeek == day))
                {
                    PersonnelTotalHourByDayAndByCiEnt personnelTotalHour = new PersonnelTotalHourByDayAndByCiEnt
                    {
                        CiId = rapportLigne.CiId,
                        TotalHours = rapportLigne.HeureNormale,
                        TotalAbsence = rapportLigne.HeureAbsence,
                        TotalMajoration = rapportLigne.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration),
                    };

                    personnelTotalHour.PointageStatutCode = CalculateRapportHebdoStatutForDayAndCI(rapportLigne.RapportLigneStatutId);
                    personnelist.Add(personnelTotalHour);
                }
            }
            return personnelist;
        }


        /// <summary>
        /// Retourne la liste des pointage pour les personnels et les Cis envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciIdList">La liste des id des Cis</param>
        /// <param name="personnelIdList">La liste des id des personnels</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        public IEnumerable<RapportLigneEnt> GetPointageByCisPersonnelsAndDates(
            IEnumerable<int> ciIdList,
            IEnumerable<int> personnelIdList,
            DateTime startDate,
            DateTime endDate)
        {
            if (ciIdList.IsNullOrEmpty() || personnelIdList.IsNullOrEmpty())
            {
                return new List<RapportLigneEnt>();
            }

            return Repository.GetPointageByCisPersonnelsAndDates(ciIdList, personnelIdList, startDate, endDate);
        }

        /// <summary>
        /// Handle search pointage reel with filter
        /// </summary>
        /// <param name="pointage">Rapport ligne</param>
        /// <param name="applyReadOnly">apply read only</param>
        private void HandleSearchPointageReelWithFilter(RapportLigneEnt pointage, bool applyReadOnly)
        {
            // Permet de récuperer PrenomNomTemporaire à partir de Personnel.PrenomNom
            if (pointage.Personnel != null && pointage.PersonnelId != null)
            {
                pointage.PrenomNomTemporaire = pointage.Personnel.PrenomNom;
            }

            // Permet de récuperer MaterielNomTemporaire à partir de Materiel.LibelleRef
            if (pointage.Materiel != null && pointage.MaterielId != null)
            {
                pointage.MaterielNomTemporaire = pointage.Materiel.LibelleLong;
            }

            if (applyReadOnly)
            {
                ApplyReadOnlyRGPointageReel(pointage);
            }

            SetPointageReelType(pointage);
        }

        /// <summary>
        /// Récupére le pointage des moyens entre 2 dates . Le pointage des moyen est reconnu  par la colonne AffectationMoyenId non nulle.
        /// </summary>
        /// <param name="startDate">Date de début de pointage limite</param>
        /// <param name="endDate">Date de fin de pointage limite</param>
        /// <returns>Liste des lignes des rapports</returns>
        public ExportTibcoRapportLigneModel[] GetPointageMoyenBetweenDates(DateTime startDate, DateTime endDate)
        {
            return Repository.GetPointageMoyenBetweenDates(startDate, endDate);
        }

        /// <summary>
        /// Get list des Majorations affected to list of personnel
        /// </summary>
        /// <param name="majorationPersonnelModel">Majoration Personnel get model</param>
        /// <returns>List des primes affected</returns>
        public List<MajorationPersonnelAffectationEnt> MajorationPersonnelAffected(MajorationPersonnelsGetEnt majorationPersonnelModel)
        {
            List<MajorationPersonnelAffectationEnt> majorationAffectationList = new List<MajorationPersonnelAffectationEnt>();

            if (majorationPersonnelModel != null && !majorationPersonnelModel.PersonnelIdList.IsNullOrEmpty())
            {
                foreach (int perseonnelId in majorationPersonnelModel.PersonnelIdList)
                {
                    MajorationPersonnelAffectationEnt majorationPersonnel = new MajorationPersonnelAffectationEnt();
                    majorationPersonnel.PersonnelId = perseonnelId;
                    majorationPersonnel.MajorationList = HandlePersonnelMajorationAffectation(perseonnelId, majorationPersonnelModel.DatePointage);
                    majorationAffectationList.Add(majorationPersonnel);
                }
            }

            return majorationAffectationList;
        }

        /// <summary>
        /// Retourne Vrai si au moins un pointage est présent pour l'édition
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Vrai si au moins un pointage est présent pour l'édition</returns>
        public bool IsExcelControlePointagesNotEmpty(EtatPaieExportModel etatPaieExportModel)
        {
            SearchRapportLigneEnt search = new SearchRapportLigneEnt();
            search.DatePointageMin = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1);
            search.DatePointageMax = search.DatePointageMin.AddMonths(1);
            search.PersonnelId = etatPaieExportModel.PersonnelId;
            if (etatPaieExportModel.PersonnelId == null)
            {
                switch (etatPaieExportModel.Filtre)
                {
                    case TypeFiltreEtatPaie.Population:
                        int userId = utilisateurManager.GetContextUtilisateur().UtilisateurId;
                        return Repository.IsPointagesVerrouillesByUserExists(userId, etatPaieExportModel.Annee, etatPaieExportModel.Mois);

                    case TypeFiltreEtatPaie.Perimetre:
                        userId = utilisateurManager.GetContextUtilisateur().UtilisateurId;
                        IEnumerable<int> allCisByUser = utilisateurManager.GetAllCIbyUser(userId, false).ToList();
                        return Repository.IsPointagesExists(search.GetExpressionWhere(), p => p.Personnel != null && allCisByUser.Contains(p.CiId));

                    case TypeFiltreEtatPaie.Autre:
                        IEnumerable<int> allCisByOrga = utilisateurManager.GetAllCIIdbyOrganisation(etatPaieExportModel.OrganisationId).ToList();
                        return Repository.IsPointagesExists(
                            search.GetExpressionWhere(),
                            p => p.Personnel != null && allCisByOrga.Contains(p.CiId)
                            && (!etatPaieExportModel.EtablissementPaieIdList.Any() || (p.Personnel != null && etatPaieExportModel.EtablissementPaieIdList.Contains(p.Personnel.EtablissementPaieId))));
                }
            }
            return Repository.IsPointagesExists(search.GetExpressionWhere());
        }

        /// <summary>
        /// Get Rapport ligne by rapport and personnel identifiers
        /// </summary>
        /// <param name="rapportId">Rapport identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport ligne</returns>
        public RapportLigneEnt GetRapportLigneByRapportIdAndPersonnelId(int rapportId, int personnelId)
        {
            return Repository.GetRapportLigneByRapportIdAndPersonnelId(rapportId, personnelId);
        }

        /// <summary>
        /// Handle prime affectation list 
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date pointage</param>
        /// <returns>Prime affectation List for personnel</returns>
        private List<MajorationAffectationEnt> HandlePersonnelMajorationAffectation(int personnelId, DateTime datePointage)
        {
            List<MajorationAffectationEnt> personnelMajorationAffectationList = new List<MajorationAffectationEnt>();
            List<RapportLigneEnt> rapportLignes = Repository.MajorationPersonnelAffected(personnelId, datePointage, datePointage.AddDays(6));
            if (!rapportLignes.IsNullOrEmpty())
            {
                foreach (RapportLigneEnt rapport in rapportLignes)
                {
                    if (!rapport.ListRapportLigneMajorations.IsNullOrEmpty())
                    {
                        foreach (RapportLigneMajorationEnt rapportMajoration in rapport.ListRapportLigneMajorations)
                        {
                            personnelMajorationAffectationList.Add(
                              new MajorationAffectationEnt
                              {
                                  CodeMajoration = rapportMajoration.CodeMajoration.Code,
                                  AffectationDay = (int)rapport.DatePointage.DayOfWeek,
                                  CiId = rapport.CiId
                              });
                        }
                    }
                }
            }

            return personnelMajorationAffectationList;
        }

        /// <summary>
        /// Get total des heures sup pour un CI
        /// </summary>
        /// <param name="rapportLignes">List des rapports lignes</param>
        /// <returns>List des ci heures supplementaire</returns>
        private List<CiHeuresSupSummaryEnt> GetTotalHeuresSupForCi(List<RapportLigneEnt> rapportLignes)
        {
            List<CiHeuresSupSummaryEnt> ciHeuresSupSummaryList = new List<CiHeuresSupSummaryEnt>();
            List<IGrouping<int, RapportLigneEnt>> pointageGroupByCi = rapportLignes.GroupBy(x => x.CiId).ToList();
            foreach (IGrouping<int, RapportLigneEnt> pointageCi in pointageGroupByCi)
            {
                double totalHeureSup = 0;
                List<IGrouping<int?, RapportLigneEnt>> pointageGroupByPersonnel = pointageCi.GroupBy(x => x.PersonnelId).ToList();
                foreach (IGrouping<int?, RapportLigneEnt> pointagePersonnel in pointageGroupByPersonnel)
                {
                    double? total = pointagePersonnel.Sum(s => s.ListRapportLigneTaches?.Sum(t => t.HeureTache) + s.HeureAbsence +
                                                        (s.ListRapportLigneMajorations.Any() ? s.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0))
                                                            - Constantes.MaxHeurePoinatageFES;
                    if (total.HasValue && total.Value > 0)
                    {
                        totalHeureSup += total.Value;
                    }
                }

                ciHeuresSupSummaryList.Add(new CiHeuresSupSummaryEnt
                {
                    CiId = pointageCi.FirstOrDefault().CiId,
                    HeuresSup = totalHeureSup
                });
            }

            return ciHeuresSupSummaryList;
        }

        /// <summary>
        /// Récuperer les rapports lignes par l'affectation moyen identifier
        /// </summary>
        /// <param name="affectationMoyenIdList">Affectation moyen list des identifiers</param>
        /// <returns>List des rapports lignes</returns>
        public IEnumerable<RapportLigneEnt> GetPointageByAffectaionMoyenIds(IEnumerable<int> affectationMoyenIdList)
        {
            return this.Repository.GetPointageByAffectaionMoyenIds(affectationMoyenIdList);
        }

        /// <summary>
        /// Permet d'inserer un nouveau rapport Ligne
        /// </summary>
        /// <param name="rapportLigne">rapport ligne</param>
        public void InsertRapportLigneFiggo(RapportLigneEnt rapportLigne)
        {
            Repository.Insert(rapportLigne);
            this.Save();
        }


        /// <summary>
        /// Get Logs des absence FIGGO importées
        /// </summary>
        /// <param name="dateDebut">Date de debut</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Logs des absences FIGGO importées</returns>
        public FiggoLogModel GetLogFiggo(DateTime dateDebut, DateTime dateFin)
        {
            var figgoRapportLignes = Repository.GetListPointagesFiggoByPeriode(dateDebut, dateFin);

            var tibcolist = new List<TibcoModel>();
            var personnels = personnelManager.GetPersonnelList().ToList();
            var societes = societeManager.GetSocieteList().ToList();
            var codeAbsences = codeAbsenceManager.GetCodeAbsList().ToList();
            var statutAbsences = statutAbsenceManager.GetStatutAbsList().ToList();

            foreach (var ligne in figgoRapportLignes)
            {
                tibcolist.AddRange(validator.CheckPointageFiggo(ligne, personnels, societes, codeAbsences, statutAbsences));
            }

            return new FiggoLogModel
            {
                NombreLigneRecu = figgoRapportLignes.Count(),
                NombreLigneErreur = tibcolist.Count,
                Tibco = tibcolist
            };
        }

        /// <summary>
        /// Retourne la liste des pointage pour le personnels et le Ci envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciId">id du Ci</param>
        /// <param name="personnelId">id du personnel</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        public IEnumerable<RapportLigneEnt> GetPointageByCiPersonnelAndDates(int ciId, int personnelId, DateTime startDate, DateTime endDate)
        {
            return Repository.GetPointageByCiPersonnelAndDates(ciId, personnelId, startDate, endDate);
        }
        /// <summary>
        /// Met a jour un rapport Ligne
        /// </summary>
        /// <param name="pointage">rapport ligne</param>
        public void UpdatePointageForFiggo(RapportLigneEnt pointage)
        {
            Repository.Update(pointage);
            this.Save();
        }
        /// <summary>
        ///   Créer une ligne de tache vide
        /// </summary>
        /// <param name="pointageReel">La ligne du rapport</param>
        /// <param name="tache">La tache</param>
        /// <param name="heure">heure tache</param>
        /// <returns>Une tache correspondant à une ligne de rapport</returns>
        public RapportLigneTacheEnt GetNewPointageTacheFiggo(RapportLigneEnt pointageReel, TacheEnt tache, double heure)
        {
            return new RapportLigneTacheEnt
            {
                TacheId = tache.TacheId,
                Tache = tache,
                RapportLigne = pointageReel,
                HeureTache = heure
            };
        }

        /// <summary>
        ///  Retourne les pointages interimaires qui n'ont pas encore été réceptionnés
        /// </summary>      
        /// <param name="interimaireId">interimaireId</param>
        /// <param name="dateDebut">dateDebut</param>
        /// <param name="dateFin">dateFin</param>
        /// <returns>Liste d'ids des reception non receptionnees</returns>
        public List<RapportLigneEnt> GetPointagesInterimaireNonReceptionnees(int interimaireId, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                return Repository.GetPointagesInterimaireNonReceptionnees(interimaireId, dateDebut, dateFin);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupére le pointage des personnels pour export vers tibco
        /// </summary>
        /// <param name="filter">option</param>
        /// <returns>model des lignes des rapports au format tibco</returns>
        public ExportPointagePersonnelTibcoModel GetPointagePersonnelForTibco(ExportPointagePersonnelFilterModel filter)
        {
            if (filter == null || !filter.EtablissementsComptablesCodes.Any())
            {
                throw new ArgumentNullException(FeatureRapport.ExportPointagePersonnel_Error_Input);
            }
            filter = GetDateControleSaisie(filter);
            var rapportLignes = pointageRepository.GetPointagePersonnelByFilter(filter);
            var TibcoModel = FormatToTibcoModel(filter.UserId, filter.Simulation, rapportLignes);

            return TibcoModel;
        }

        private ExportPointagePersonnelTibcoModel FormatToTibcoModel(int id, bool simulation, List<RapportLigneSelectModel> rapportLignes)
        {
            UtilisateurEnt user = utilisateurManager.GetUtilisateurById(id);
            return new ExportPointagePersonnelTibcoModel
            {
                SocieteUserCode = user?.Personnel.Societe.Code,
                MatriculeUser = user?.Personnel.Matricule,
                LoginFred = user?.Login,
                DateExport = DateTime.UtcNow,
                NombrePersonnels = rapportLignes.Select(r => r.Personnel).Distinct().Count(),
                NombreRapportLignes = rapportLignes.Count(),
                Simulation = simulation,
                RapportLignes = GetRapportLignesDetail(rapportLignes)
            };
        }

        private ExportPersonnelRapportLigneModel[] GetRapportLignesDetail(List<RapportLigneSelectModel> rapportLignes)
        {
            return rapportLignes.Select(l =>
                new ExportPersonnelRapportLigneModel
                {
                    numero = l.RapportLigneId,
                    IsStatutVerrouille = l.RapportLigneStatutId.HasValue && l.RapportLigneStatutId.Value.Equals(RapportStatutEnt.RapportStatutVerrouille.Key),
                    DatePointage = l.DatePointage,
                    SocieteCode = l.Personnel.SocieteCode,
                    EtablissementPaieCode = l.Personnel.EtablissementPaieCode,
                    EtablissementComptableCode = l.Personnel.EtablissementPaieEtablissementComptableCode,
                    PersonnelNom = l.Personnel.Nom,
                    PersonnelPrenom = l.Personnel.Prenom,
                    PersonnelMatricule = l.Personnel.Matricule,
                    SocieteCi = l.CiSocieteCode,
                    EtablissementComptableCi = l.CiEtablissementComptableCode,
                    CiCode = l.CiCode,
                    AbsenceCode = l.CodeAbsenceCode,
                    HeuresAbsences = l.HeureAbsence,
                    Commentaire = l.Commentaire,
                    TacheLignes = l.ListRapportLigneTaches.Select(t => new ExportPersonnelSousRapportLignesModel
                    {
                        Code = t.TacheCode,
                        Quantite = t.HeureTache
                    }).ToArray(),
                    MajorationLignes = l.ListRapportLigneMajorations.Select(t => new ExportPersonnelSousRapportLignesModel
                    {
                        Code = t.CodeMajorationCode,
                        Quantite = t.HeureMajoration
                    }).ToArray(),
                    PrimeLignes = l.ListRapportLignePrimes.Select(p => new ExportPersonnelSousRapportLignesModel
                    {
                        Code = p.PrimeCode,
                        Quantite = p.HeurePrime ?? 1
                    }).ToArray(),
                    AstreinteLignes = l.ListRapportLigneAstreintes.Select(a => new ExportPersonnelSousRapportLignesModel
                    {
                        Code = a.AstreinteCode,
                        Quantite = a.HeureAstreinte ?? 0
                    }).ToArray()
                }).ToArray();
        }

        /// <summary>
        /// Permet de controler les saisies pour Tibco
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns>liste de model des erreurs</returns>
        public IEnumerable<ControleSaisiesTibcoModel> ControleSaisiesForTibco(ExportPointagePersonnelFilterModel filter)
        {
            var controleSaisies = new List<ControleSaisiesTibcoModel>();
            if (filter == null || !filter.EtablissementsComptablesIds.Any())
            {
                throw new ArgumentNullException(FeatureRapport.ExportPointagePersonnel_Error_Input);
            }
            filter = GetDateControleSaisie(filter);

            var rapportLignes = pointageRepository.GetPointagePersonnelByFilter(filter);
            List<PersonnelEnt> personnels = getPersonnelBySocieteOrEtablissementComptables(filter);
            foreach (var personnel in personnels)
            {
                if (rapportLignes.Any(r => r.Personnel.PersonnelId == personnel.PersonnelId))
                {
                    controleSaisies.Add(new ControleSaisiesTibcoModel
                    {
                        EtablissementCode = rapportLignes.FirstOrDefault(x => x.Personnel.PersonnelId == personnel.PersonnelId).Personnel.SocieteCode,
                        PersonnelNom = rapportLignes.FirstOrDefault(x => x.Personnel.PersonnelId == personnel.PersonnelId).Personnel.Nom,
                        PersonnelPrenom = rapportLignes.FirstOrDefault(x => x.Personnel.PersonnelId == personnel.PersonnelId).Personnel.Prenom,
                        PersonnelMatricule = rapportLignes.FirstOrDefault(x => x.Personnel.PersonnelId == personnel.PersonnelId).Personnel.Matricule,
                        Erreurs = validator.CheckPointagesForTibco(rapportLignes
                        .Where(c => c.Personnel.PersonnelId.Equals(personnel.PersonnelId)).ToList(),
                        filter.DateDebut, filter.DateFin).OrderBy(c => c.DateRapport).ToList()
                    });
                }
                else
                {
                    controleSaisies.Add(new ControleSaisiesTibcoModel
                    {
                        EtablissementCode = personnel.Societe?.Code,
                        PersonnelNom = personnel.Nom,
                        PersonnelPrenom = personnel.Prenom,
                        PersonnelMatricule = personnel.Matricule,
                        Erreurs = new List<ControleSaisiesErreurTibcoModel> { new ControleSaisiesErreurTibcoModel { Message = FeatureRapport.MsgAucunPointages, DateRapport = null } }

                    });
                }
            }

            return controleSaisies.OrderBy(x => x.EtablissementCode).ThenBy(x => x.LibelleNoeud);
        }

        /// <summary>
        /// Modifie l'identifiant du contrat interimaire d'une liste de rappoer ligne
        /// </summary>
        /// <param name="rapportLignesIds">Liste des identifiants rapport ligne</param>
        /// <param name="contratInterimaireId">Identifiant contrat interimaire</param>
        public void UpdateContratId(List<int> rapportLignesIds, int contratInterimaireId)
        {
            Repository.UpdateContratId(rapportLignesIds, contratInterimaireId);
        }

        /// <summary>
        ///  check si l'export analytique a des erreurs
        /// </summary>
        /// <param name="filter">Object de recherche</param>
        /// <returns></returns>
        public string CheckExportAnalytiqueErrors(ExportPointagePersonnelFilterModel filter)
        {
            var controleSaisies = new List<ControleSaisiesTibcoModel>();
            if (filter == null || !filter.EtablissementsComptablesIds.Any())
            {
                throw new ArgumentNullException(FeatureRapport.ExportPointagePersonnel_Error_Input);
            }
            if (filter.Simulation)
            {
                return string.Empty;
            }

            // date de controle
            filter = GetDateControleSaisie(filter);
            var dateDebut = filter.DateDebut;
            var dateFin = filter.DateFin;
            List<PersonnelEnt> personnels = getPersonnelBySocieteOrEtablissementComptables(filter);

            //date de pointage du periode + (periode-1)
            filter = GetDateControleSaisie(filter, true);

            var rapportLignes = pointageRepository.GetPointagePersonnelByFilter(filter);
            var errors = new List<ControleSaisiesErreurTibcoModel>();
            foreach (var personnel in personnels)
            {
                if (rapportLignes.Any(r => r.Personnel.PersonnelId == personnel.PersonnelId))
                {
                    errors.AddRange(validator.CheckPointagesForTibco(rapportLignes
                       .Where(x =>
                       x.DatePointage >= dateDebut
                       && x.DatePointage <= dateFin
                       && x.Personnel.PersonnelId.Equals(personnel.PersonnelId))
                       .ToList(), dateDebut, dateFin).ToList());
                }
            }

            if (errors != null && errors.Any())
            {
                return FeatureRapport.RapportLigneValidator_ExportAnalytiqueErrors;
            }
            else
            {
                return string.Empty;
            }
        }

        private List<PersonnelEnt> getPersonnelBySocieteOrEtablissementComptables(ExportPointagePersonnelFilterModel filter)
        {
            List<PersonnelEnt> personnels = new List<PersonnelEnt>();
            if (filter.AllEtablissements)
            {
                var societe = societeManager.GetSocieteByCodeSocieteComptables(filter.SocieteCode);
                if (societe != null)
                {
                    return personnels = personnelRepository.GetPersonnelBySociete(societe.SocieteId, filter.DateDebut);
                }
                else
                {
                    throw new ArgumentNullException(FeatureRapport.ExportPointagePersonnel_Error_Input);
                }
            }
            else
            {
                return personnels = personnelRepository.GetPersonnelByEtablissementComptable(filter.EtablissementsComptablesIds, filter.DateDebut);
            }

        }

        private ExportPointagePersonnelFilterModel GetDateControleSaisie(ExportPointagePersonnelFilterModel filter, bool isExportAnalytique = false)
        {
            if (filter.Hebdo)
            {
                filter.DateDebut = filter.DateDebut.AddDays(-2);
                filter.DateFin = filter.DateDebut.AddDays(6);
            }
            else
            {
                filter.DateDebut = new DateTime(filter.DateDebut.Year, filter.DateDebut.Month, 1);
                if (!isExportAnalytique)
                {
                    filter.DateFin = filter.DateDebut.AddMonths(1).AddDays(-1);
                }
                else
                {
                    filter.DateDebut = filter.DateDebut.AddMonths(-1);
                    filter.DateFin = filter.DateDebut.AddMonths(2).AddDays(-1);
                }
            }
            return filter;
        }


        /// <summary>
        /// Récupére une liste de rapport ligne parune liste d'ID
        /// </summary>
        /// <param name="idsList">La liste des ids</param>
        /// <returns></returns>
        public IEnumerable<RapportLigneEnt> GetRapportLignesByIds(List<int> idsList)
        {
            return Repository.GetRapportLignesByIds(idsList);
        }

        public void UpdateRapportLigneAndRapportLigneTache(IEnumerable<RapportLigneEnt> rapportsLignes, IEnumerable<RapportLigneTacheEnt> rapportLignesTaches)
        {
            AddOrUpdateRapportLigneList(rapportsLignes);
            rapportLigneTacheRepo.UpdateRangeRapportLigneTache(rapportLignesTaches);
        }
    }
}
