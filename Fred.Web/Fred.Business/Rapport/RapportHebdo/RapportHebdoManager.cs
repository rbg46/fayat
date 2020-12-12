using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Affectation;
using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Datas;
using Fred.Business.Referential.Tache;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;
using MoreLinq;
using static Fred.Entities.Constantes;

namespace Fred.Business.Rapport.RapportHebdo
{
    public class RapportHebdoManager : Manager<RapportEnt, IRapportRepository>, IRapportHebdoManager
    {
        public readonly int StatutVerrouille = RapportStatutEnt.RapportStatutVerrouille.Key;
        public readonly int StatutValide = RapportStatutEnt.RapportStatutValide2.Key;

        private readonly IAffectationManager affectationManager;
        private readonly IUtilisateurManager userManager;
        private readonly IMapper mapper;
        private readonly IPointageRepository pointageRepository;
        private readonly ITacheManager tacheManager;
        private readonly ICIManager ciManager;
        private readonly IPersonnelManager personnelManager;
        private readonly RapportHebdoSaver rapportHebdoSaver;
        private readonly IRapportHebdoService rapportHebdoService;

        public RapportHebdoManager(
            IUnitOfWork uow,
            IRapportRepository rapportRepository,
            IAffectationManager affectationManager,
            IUtilisateurManager userManager,
            IMapper mapper,
            ITacheManager tacheManager,
            ICIManager ciManager,
            IPointageRepository pointageRepository,
            IPersonnelManager personnelManager,
            RapportHebdoSaver rapportHebdoSaver,
            IRapportHebdoService rapportHebdoService)
          : base(uow, rapportRepository)
        {
            this.tacheManager = tacheManager;
            this.affectationManager = affectationManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.pointageRepository = pointageRepository;
            this.personnelManager = personnelManager;
            this.rapportHebdoSaver = rapportHebdoSaver;
            this.rapportHebdoService = rapportHebdoService;
            this.ciManager = ciManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Récupérer le rapport hebdomadaire par CI
        /// </summary>
        /// <param name="ciPersonnelListPairs">Dictionnaire contenant les CI et la liste des personnels correspandates</param>
        /// <param name="mondayDate">Date de lundi de la semaine choisie</param>
        /// <returns>Le rapport hebdomadaire</returns>
        public List<RapportHebdoNode<PointageCell>> GetRapportHebdoByCi(Dictionary<int, List<int>> ciPersonnelListPairs, DateTime mondayDate)
        {
            var currentUser = userManager.GetContextUtilisateur();
            Dictionary<int, List<RapportEnt>> ciRapportsPairs = new Dictionary<int, List<RapportEnt>>();

            AjouterCisAbsenceGenerique(ciPersonnelListPairs, currentUser);

            foreach (int ciId in ciPersonnelListPairs.Keys)
            {
                List<RapportEnt> rapportList = rapportHebdoService.GetCiRapportsByWeek(ciId, mondayDate, mondayDate.AddDays(6), ciPersonnelListPairs[ciId]);

                foreach (int personnelId in ciPersonnelListPairs[ciId])
                {
                    rapportHebdoService.AddPersonnelPointageToAllRapports(rapportList, personnelId);
                }
                ciRapportsPairs.Add(ciId, rapportList);
            }

            return MapWeekRapportsToRapportHebdoViewModel(ciRapportsPairs, currentUser.UtilisateurId);
        }

        private void AjouterCisAbsenceGenerique(Dictionary<int, List<int>> ciPersonnelListPairs, UtilisateurEnt currentUser)
        {
            Dictionary<int, List<int>> etabPersonnelPairs = new Dictionary<int, List<int>>();

            if (Constantes.CodeGroupeFES.Equals(currentUser.Personnel.Societe.Groupe.Code))
            {
                ciPersonnelListPairs.ForEach(p =>
                {
                    List<int> personnelsIds = p.Value;

                    foreach (var personnelId in personnelsIds)
                    {
                        var dbPersonnel = personnelManager.GetPersonnelById(personnelId);
                        var etablissementId = dbPersonnel.EtablissementPaie.EtablissementComptableId.Value;

                        AlimenterDictionnaireEtabPersonnel(etabPersonnelPairs, personnelId, etablissementId);
                    }
                });

                foreach (var ci in ciManager.GetCisAbsenceGenerique())
                {
                    List<int> personnelEtab;
                    List<int> personnelCi;
                    var etabComptableIds = ci.Societe.EtablissementComptables.Select(x => x.EtablissementComptableId).Distinct().ToList();
                    foreach (var etabComptableId in etabComptableIds)
                    {
                        if (etabPersonnelPairs.ContainsKey(etabComptableId) && etabPersonnelPairs.Any()
                            && etabPersonnelPairs.TryGetValue(etabComptableId, out personnelEtab)
                            && !ciPersonnelListPairs.TryGetValue(ci.CiId, out personnelCi))
                        {
                            ciPersonnelListPairs.Add(ci.CiId, etabPersonnelPairs[etabComptableId]);
                        }
                    }
                }
            }
        }

        private void AlimenterDictionnaireEtabPersonnel(Dictionary<int, List<int>> etabPersonnelPairs, int personnelId, int etablissementId)
        {
            List<int> personnelEtab;
            if (!etabPersonnelPairs.TryGetValue(etablissementId, out personnelEtab))
            {
                etabPersonnelPairs.Add(etablissementId, new List<int> { personnelId });
            }
            else
            {
                etabPersonnelPairs[etablissementId].Add(personnelId);
            }
        }

        /// <summary>
        /// Récupérer le rapport hebdomadaire par employé
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="mondayDate">Date de lundi de la semaine choisie</param>
        /// <param name="allCi">Booléan indique s'il faut récupérer tous les CI pour le pointage</param>
        /// <returns>Le rapport hebdomadaire</returns>
        public RapportHebdoNode<PointageCell> GetRapportHebdoByEmployee(int personnelId, DateTime mondayDate, bool allCi = false)
        {
            var currentUser = userManager.GetContextUtilisateur();

            var personnel = personnelManager.GetPersonnel(personnelId, RapportHebdoByEmployee.Personnel.Selector);
            if (personnel == null)
            {
                throw new FredBusinessException(FeatureRapportHebdo.Erreur_PersonnelNonExistant);
            }

            List<RapportHebdoByEmployee.CI> ciList;
            if (allCi)
            {
                ciList = affectationManager.GetPersonnelAffectationCiList(personnelId, RapportHebdoByEmployee.CI.Selector).OrderBy(x => x.CiId).ToList();
            }
            else
            {
                // Code temporaire avant la modification de la fonction GetCiListOfResponsable
                ciList = new List<RapportHebdoByEmployee.CI>();
                foreach (var ci in GetCiListOfResponsable(personnelId))
                {
                    if (ci != null)
                    {
                        ciList.Add(RapportHebdoByEmployee.CI.From(ci));
                    }
                }
            }

            if (Constantes.CodeGroupeFES.Equals(currentUser.Personnel.Societe.Groupe.Code))
            {
                CIEnt ci = ciManager.GetCIByCodeAndSocieteId("ABSENCES", currentUser.Personnel.Societe.SocieteId);
                if (ci != null)
                {
                    ciList.Add(RapportHebdoByEmployee.CI.From(ci));
                }
            }

            RapportHebdoNode<PointageCell> personnelNode = new RapportHebdoNode<PointageCell>
            {
                NodeId = personnelId,
                NodeText = $"{personnel.SocieteCode} - {personnel.Matricule} - {personnel.Nom} {personnel.Prenom}",
                NodeType = NodeType.Personnel,
                Statut = this.GetPersonnelStatut(personnel?.Statut ?? string.Empty),
                SubNodeList = new List<RapportHebdoSubNode<PointageCell>>()
            };

            int? societeId = personnel?.SocieteId;
            List<RapportLigneEnt> allReportLines = new List<RapportLigneEnt>();
            var rapportByCis = GetCiRapportsByWeekForEmployee(ciList.Select(c => c.CiId), personnelId, mondayDate, personnelNode.Statut);

            foreach (var ci in ciList)
            {
                var rapportList = rapportByCis.First(x => x.Key == ci.CiId).Value;
                societeId = societeId ?? ci?.SocieteId;
                FulfillCiPointageForPersonnel(ci, personnelNode, rapportList, societeId, currentUser.UtilisateurId, ci.Organisation?.OrganisationId);

                IEnumerable<RapportLigneEnt> reportLines = rapportList.SelectMany(r => r.ListLignes).Where(rl => !rl.DateSuppression.HasValue);
                if (reportLines?.Any() == true)
                {
                    allReportLines.AddRange(reportLines);
                }
            }

            return personnelNode;
        }

        /// <summary>
        /// Indique si au moins une ligne de rapport de la liste a été validée par un supérieur de l'utilisateur :
        /// si l'utilisateur est délégué d'un CI, on regarde si le valideur est le responsable du CI.
        /// </summary>
        /// <param name="reportLines">Liste des lignes de rapport à vérifier</param>
        /// <param name="userId">Identifiant de l'utilisateur pour lequel vérifier les lignes de rapports</param>
        /// <returns>True si un supérieur de l'utilisateur a validé au moins une ligne de rapport de la liste</returns>
        private bool CheckWorkerReportValidationBySuperior(IEnumerable<RapportLigneEnt> reportLines, int userId)
        {
            IEnumerable<RapportLigneEnt> approvedReportLines = reportLines.Where(r => r.ValideurId.HasValue);
            if (approvedReportLines?.Any() == true)
            {
                IEnumerable<int> ciDelegueIdList = userManager.GetCiListForDelegue(userId)?.Select(c => c.CiId);
                if (ciDelegueIdList?.Any() == true)
                {
                    IEnumerable<RapportLigneEnt> approvedDelegatedReportLines = approvedReportLines
                                                        .Where(r => r.ValideurId.Value != userId && ciDelegueIdList.Contains(r.CiId));
                    IEnumerable<int> reportLinesCiDelegueIdList = approvedDelegatedReportLines.Select(r => r.CiId);
                    IEnumerable<int> approversId = approvedDelegatedReportLines.Select(r => r.ValideurId.Value).Distinct();
                    foreach (var approverId in approversId)
                    {
                        IEnumerable<int> ciManagedIdList = userManager.GetCiListOfResponsable(approverId).Select(c => c.CiId);
                        if (ciManagedIdList.Intersect(reportLinesCiDelegueIdList)?.Any() == true)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Enregistrer le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model pour enregistrer le rapport hebdomadaire</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public RapportHebdoSaveResultModel SaveRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel)
        {
            try
            {
                rapportHebdoSaver.Save(rapportHebdoSaveViewModel);
                return rapportHebdoSaver.Results;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Vérifier et valider le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model de Rapport hebdomadaire pour enregistrer</param>
        /// <param name="isEtamIac">Boolean indique si la validation concerne ETAM \ IAC ou Ouvrier</param>
        /// <returns>Résultat de validation d'un rapport hebdomadaire</returns>
        public RapportHebdoValidationReponseViewModel CheckAndValidateRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel, bool isEtamIac)
        {
            RapportHebdoValidationReponseViewModel ret;
            var saveResult = SaveRapportHebdo(rapportHebdoSaveViewModel);
            if (saveResult.Errors.Count == 0)
            {
                if (isEtamIac)
                {
                    ret = CheckAndValidateRapportHebdoForEtamIac(rapportHebdoSaveViewModel);
                }
                else
                {
                    ret = CheckAndValidateRapportHebdoForWorkers(rapportHebdoSaveViewModel);
                }
            }
            else
            {
                ret = new RapportHebdoValidationReponseViewModel();
                ret.Errors = saveResult.Errors;
            }

            ret.Warnings = saveResult.Warnings;
            return ret;
        }

        /// <summary>
        /// Get synthese mensuelle
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="dateMonth">Mois du pointage</param>
        /// <returns>Rapport hebdo synthese mensuelle model</returns>
        public IEnumerable<RapportHebdoSyntheseMensuelleEnt> GetSyntheseMensuelleRapportHebdo(int utilisateurId, DateTime dateMonth)
        {
            List<RapportHebdoSyntheseMensuelleEnt> rapportHebdoSyntheseMensuelleList = new List<RapportHebdoSyntheseMensuelleEnt>();
            IEnumerable<int> personnelIdList = personnelManager.GetManagedPersonnelIds(utilisateurId);
            DateTime lastDayInMonth = dateMonth.AddMonths(1).AddDays(-1);

            List<RapportStatutEnt> rapportStatutList = Repository.GetRapportStatutList(false).ToList();
            RapportStatutEnt rapportStatutEC = rapportStatutList.FirstOrDefault(s => s.Code == RapportStatutEnt.RapportStatutEnCours.Value);
            foreach (int personnelId in personnelIdList)
            {
                PersonnelEnt personnel = personnelManager.GetSimplePersonnel(personnelId);
                IEnumerable<RapportLigneEnt> rapportLignes = pointageRepository.GetEtamIacRapportsForValidation(personnelId, dateMonth, lastDayInMonth);
                RapportHebdoSyntheseMensuelleEnt rapportHebdoSyntheseMensuelleEnt = new RapportHebdoSyntheseMensuelleEnt();
                if (!rapportLignes.IsNullOrEmpty())
                {
                    rapportHebdoSyntheseMensuelleEnt.Personnel = personnel;
                    rapportHebdoSyntheseMensuelleEnt.NbreJoursPointes = GetNbreJoursPointeSyntheMensuelle(rapportLignes);
                    rapportHebdoSyntheseMensuelleEnt.NbreAbsences = GetNbreJoursAbsenceSyntheseMensuelle(rapportLignes);
                    rapportHebdoSyntheseMensuelleEnt.NbrePrimes = GetNbrePrimesSyntheseMensuelle(rapportLignes);
                    string statutCode = GetSyntheseMensuelleRapportStatut(rapportLignes, dateMonth);
                    RapportStatutEnt rapportStatut = rapportStatutList.FirstOrDefault(s => s.Code == statutCode);
                    rapportHebdoSyntheseMensuelleEnt.PointageStatut = rapportStatut.Libelle;
                    rapportHebdoSyntheseMensuelleEnt.PersonnelStatut = personnel.Statut;
                }
                else
                {
                    rapportHebdoSyntheseMensuelleEnt.Personnel = personnel;
                    rapportHebdoSyntheseMensuelleEnt.NbreJoursPointes = 0;
                    rapportHebdoSyntheseMensuelleEnt.NbreAbsences = 0;
                    rapportHebdoSyntheseMensuelleEnt.NbrePrimes = 0;
                    rapportHebdoSyntheseMensuelleEnt.PointageStatut = rapportStatutEC.Libelle;
                    rapportHebdoSyntheseMensuelleEnt.PersonnelStatut = personnel.Statut;
                }

                rapportHebdoSyntheseMensuelleList.Add(rapportHebdoSyntheseMensuelleEnt);
            }
            return rapportHebdoSyntheseMensuelleList.OrderBy(r => r.Personnel.Nom);
        }

        /// <summary>
        /// Synthese mensuelle validation Model pour les Etam et Iac
        /// </summary>
        /// <param name="validateSyntheseMensuelleModel">Validation synthese mensuelle model</param>
        public void ValiderSyntheseMensuelleEtamIac(ValidateSyntheseMensuelleModel validateSyntheseMensuelleModel)
        {
            if (validateSyntheseMensuelleModel != null && !validateSyntheseMensuelleModel.PersonnelIdList.IsNullOrEmpty())
            {
                DateTime lastDayInMonth = validateSyntheseMensuelleModel.FirstDayInMonth.AddMonths(1).AddDays(-1);
                ValidatePersonnelPointageBetweenTwoDates(
                    validateSyntheseMensuelleModel.FirstDayInMonth,
                    lastDayInMonth,
                    validateSyntheseMensuelleModel.PersonnelIdList,
                    RapportStatutEnt.RapportStatutValide2.Value);
            }
        }

        /// <summary>
        /// verifie si l'astreinte est prise entre un intervale de jour
        /// </summary>
        /// <param name="rapportLigne">Représente ou défini une sortie asteinte associées à une ligne de rapport</param>
        public void CheckinAstreinteIntervalDay(RapportLigneEnt rapportLigne)
        {
            if (rapportLigne?.ListRapportLigneAstreintes?.Any() == true)
            {
                rapportHebdoService.AddOrUpdateAstreintePrime(rapportLigne);
            }
        }

        /// <summary>
        /// Recupere l'id d'un statut de rapport verrouille
        /// </summary>
        /// <returns>Id du statut verrouille</returns>
        public int GetRapportStatutVerrouille()
        {
            return this.Repository.GetRapportStatutByCode(RapportStatutEnt.RapportStatutVerrouille.Value).RapportStatutId;
        }

        /// <summary>
        /// Get les statuts des rapport lignes pour vérification Rapport hebdo
        /// </summary>
        /// <param name="personnelId">Personnek identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>List des rapport hebdo new pointage statut</returns>
        public List<RapportHebdoNewPointageStatutEnt> GetRapportLigneStatutForNewPointage(int personnelId, int ciId, DateTime mondayDate)
        {
            List<RapportHebdoNewPointageStatutEnt> rapportLigneStatutForNewPointageList = new List<RapportHebdoNewPointageStatutEnt>();
            for (int i = 0; i < 7; i++)
            {
                DateTime datePointage = mondayDate.AddDays(i);
                string rapportStatutCode = pointageRepository.GetRapportLigneStatutCode(personnelId, ciId, datePointage);
                rapportLigneStatutForNewPointageList.Add(
                    new RapportHebdoNewPointageStatutEnt
                    {
                        DayOfWeekIndex = (int)datePointage.DayOfWeek,
                        DatePointage = datePointage,
                        IsRapportLigneValide2 = rapportStatutCode.IsNullOrEmpty() ? false : rapportStatutCode.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                        IsRapportLigneVerouiller = rapportStatutCode.IsNullOrEmpty() ? false : rapportStatutCode.Equals(RapportStatutEnt.RapportStatutVerrouille.Value)
                    });
            }

            return rapportLigneStatutForNewPointageList;
        }

        /// <summary>
        /// Récupère une Node de sortie d'astreintes
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>une Node de sortie d'astreintes</returns>
        public RapportHebdoSubNode<PointageCell> GetSortie(int personnelId, int ciId, DateTime mondayDate)
        {
            List<AstreintePointageHebdoCell> astreintePointageHebdoCellList = new List<AstreintePointageHebdoCell>();
            for (var i = 0; i < 7; i++)
            {
                var date = mondayDate.AddDays(i);
                var astreintePointageHebdoCell = new AstreintePointageHebdoCell();
                AstreinteEnt astreinte = affectationManager.GetAstreinte(ciId, personnelId, date);
                if (astreinte != null)
                {
                    astreintePointageHebdoCell.HasAstreinte = true;
                    astreintePointageHebdoCell.AstreinteId = astreinte.AstreintId;
                    astreintePointageHebdoCell.ListRapportLigneAstreintes = new List<RapportLigneAstreinteModel>();
                    astreintePointageHebdoCell.Date = date;
                }
                else
                {
                    astreintePointageHebdoCell.HasAstreinte = false;
                    astreintePointageHebdoCell.AstreinteId = 0;
                    astreintePointageHebdoCell.ListRapportLigneAstreintes = new List<RapportLigneAstreinteModel>();
                    astreintePointageHebdoCell.Date = date;
                }
                astreintePointageHebdoCellList.Add(astreintePointageHebdoCell);
            }

            return new RapportHebdoSubNode<PointageCell>
            {
                NodeText = FeatureRapportHebdo.RapportHebdo_Sortie_Astreinte,
                NodeType = NodeType.Astreinte,
                Items = astreintePointageHebdoCellList
            };
        }

        /// <summary>
        /// Get Validation Affaires 
        /// </summary>
        /// <param name="dateDebut">Date du lundi d'une semaine</param>
        /// <returns>List du synthese validation affaires model</returns>
        public async Task<IEnumerable<SyntheseValidationAffairesModel>> GetValidationAffairesByResponsableAsync(DateTime dateDebut)
        {
            DateTime dateFin = dateDebut.AddDays(6);
            IEnumerable<CIEnt> ciList = userManager.GetCiListForRole(RoleSpecification.ResponsableCI);
            List<int> ciListIds = ciList.Select(x => x.CiId).ToList();
            IEnumerable<AffectationEnt> ouvrierAffectationList = await affectationManager.GetOuvriersListIdsByCiListAsync(ciListIds).ConfigureAwait(false);
            List<int> ouvrierIdsList = ouvrierAffectationList.Select(x => x.PersonnelId).ToList();
            IEnumerable<RapportLigneEnt> rapportLigneList = await Repository.GetRapportsLignesValidationAffairesByResponsableAsync(ciListIds, ouvrierIdsList, dateDebut, dateFin)
                                                                            .ConfigureAwait(false);
            List<SyntheseValidationAffairesModel> syntheseValidationAffaireModelList = new List<SyntheseValidationAffairesModel>();
            List<RapportStatutEnt> rapportStatutList = Repository.GetRapportStatutList(false).ToList();
            foreach (int ciId in ciListIds)
            {
                CIEnt ci = ciList.FirstOrDefault(x => x.CiId == ciId);
                List<RapportLigneEnt> rapportsList = rapportLigneList.Where(x => x.CiId == ciId).ToList();
                SyntheseValidationAffairesModel syntheseValidationAffaireModel = HandleCreateSyntheseValidationAffairesModel(ci, rapportsList, ouvrierAffectationList, rapportStatutList);
                syntheseValidationAffaireModelList.Add(syntheseValidationAffaireModel);
            }

            return syntheseValidationAffaireModelList;
        }

        /// <summary>
        ///  Validation Pointage (Staut V2) par CiList and Affected ouvrier id List
        /// </summary>
        /// <param name="validationAffaireModel">Validation affaire mopdel</param>
        /// <returns>Tas</returns>
        public async Task ValidateAffairesByResponsableAsync(ValidationAffaireModel validationAffaireModel)
        {
            if (validationAffaireModel == null || !validationAffaireModel.CiIdsList.Any() || !validationAffaireModel.PersonnelIdsList.Any())
            {
                return;
            }

            DateTime dateFin = validationAffaireModel.DateDebut.AddDays(6);
            IEnumerable<int> personnelIdsList = validationAffaireModel.PersonnelIdsList.Distinct();
            IEnumerable<RapportLigneEnt> rapportLigneList = await Repository.GetRapportsLignesValidationAffairesByResponsableAsync(validationAffaireModel.CiIdsList, personnelIdsList, validationAffaireModel.DateDebut, dateFin)
                                                                            .ConfigureAwait(false);
            if (rapportLigneList.Any())
            {
                int valideurId = userManager.GetContextUtilisateurId();
                foreach (RapportLigneEnt rapportLigne in rapportLigneList)
                {
                    if (rapportLigne.RapportLigneStatutId != RapportStatutEnt.RapportStatutValide2.Key)
                    {
                        rapportLigne.RapportLigneStatutId = RapportStatutEnt.RapportStatutValide2.Key;
                        rapportLigne.ValideurId = valideurId;
                        rapportLigne.DateValidation = DateTime.Today;
                        rapportLigne.DateModification = DateTime.Today;
                    }
                }

                await pointageRepository.UpdateRangeRapportLigneAsync(rapportLigneList).ConfigureAwait(false);
                await SaveAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Créer un rapport hebdomadaire view model à partir des ci et les listes des rapports journaliers correspandants
        /// </summary>
        /// <param name="ciRapportsPairs">Dictionnaire des CI et ses rapports journaliers</param>
        /// <param name="currentUser">l'utilisateur courant</param>
        /// <returns>List des RapportHebdoNode</returns>
        private List<RapportHebdoNode<PointageCell>> MapWeekRapportsToRapportHebdoViewModel(Dictionary<int, List<RapportEnt>> ciRapportsPairs, int currentUser)
        {
            List<RapportHebdoNode<PointageCell>> rapportHebdoNodes = new List<RapportHebdoNode<PointageCell>>();
            foreach (int ciId in ciRapportsPairs.Keys)
            {
                CIEnt ci = ciManager.GetCI(ciId);
                RapportHebdoNode<PointageCell> rapportHebdoNode = new RapportHebdoNode<PointageCell>
                {
                    NodeId = ciId,
                    IsAbsence = ci.IsAbsence,
                    NodeText = $"{ci.Code} - {ci.Libelle}",
                    NodeType = GetCiNodeType(ci.CIType?.Designation),
                    Statut = GetRapportStatut(ciRapportsPairs[ciId].SelectMany(y => y.ListLignes))
                };
                FulfillPersonnelSubNodes(rapportHebdoNode, ciRapportsPairs[ciId], ci?.SocieteId, currentUser, ci.Organisation?.OrganisationId);

                IEnumerable<RapportLigneEnt> allReportLines = ciRapportsPairs[ciId].SelectMany(r => r.ListLignes);
                rapportHebdoNode.ApprovedBySuperior = CheckWorkerReportValidationBySuperior(allReportLines, userManager.GetContextUtilisateurId());

                rapportHebdoNodes.Add(rapportHebdoNode);
            }

            return rapportHebdoNodes;
        }

        /// <summary>
        /// Récupérer le type de noeud en fonction de type du CI
        /// </summary>
        /// <param name="ciTypeDesignation">La désignation du type de CI</param>
        /// <returns>Type de noeud</returns>
        private NodeType GetCiNodeType(string ciTypeDesignation)
        {
            if (ciTypeDesignation == null)
            {
                return NodeType.Affaire;
            }

            if (ciTypeDesignation == FeatureCI.CI_Search_CIType_Affaire)
            {
                return NodeType.Affaire;
            }
            else if (ciTypeDesignation == FeatureCI.CI_Search_CIType_Etude)
            {
                return NodeType.Etude;
            }
            else if (ciTypeDesignation == FeatureCI.CI_Search_CIType_Section)
            {
                return NodeType.Section;
            }
            else
            {
                return NodeType.Affaire;
            }
        }

        /// <summary>
        /// Checks if the node is a CI node
        /// </summary>
        /// <param name="nodeType">Node type</param>
        /// <returns>Booléan indicates if the node is a CI node</returns>
        private bool IsCiNode(NodeType nodeType)
        {
            return nodeType == NodeType.Affaire || nodeType == NodeType.Etude || nodeType == NodeType.Section;
        }

        /// <summary>
        /// Remplir les noeuds des personnels dans le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoNode">Le noeud du CI</param>
        /// <param name="rapportList">La liste des rapports</param>
        /// <param name="societeId">Societe Id</param>
        /// <param name="currentUser">l'utilisateur courant</param>
        /// <param name="organisationId">Organisation identifier</param>
        private void FulfillPersonnelSubNodes(RapportHebdoNode<PointageCell> rapportHebdoNode, List<RapportEnt> rapportList, int? societeId, int currentUser, int? organisationId)
        {
            List<RapportHebdoSubNode<PointageCell>> personnelSubNodes = new List<RapportHebdoSubNode<PointageCell>>();
            List<int> personnelIdList = rapportList.FirstOrDefault().ListLignes.Select(l => l.PersonnelId.Value).ToList();
            foreach (int personnelId in personnelIdList)
            {
                PersonnelEnt personnel = personnelManager.GetPersonnel(personnelId);
                bool isDesaffectedCiPersonnel = Managers.Pointage.GetpointageByPersonnelAndCiForReadOnly(rapportHebdoNode.NodeId, personnel.PersonnelId);
                RapportHebdoSubNode<PointageCell> personnelNode = new RapportHebdoSubNode<PointageCell>
                {
                    NodeId = personnel.PersonnelId,
                    NodeText = $"{personnel.Societe.Code} - {personnel.Matricule} - {personnel.Nom} {personnel.Prenom}",
                    NodeType = NodeType.Personnel,
                    IsAbsence = rapportHebdoNode.IsAbsence,
                    IsPersonnelCiDesaffected = isDesaffectedCiPersonnel && !rapportList.Any(x => x.ListLignes.Any(y => y.PersonnelId.Value == personnelId && y.RapportLigneId > 0)),
                    PersonnelToReadOnly = isDesaffectedCiPersonnel,
                    SubNodeList = new List<RapportHebdoSubNode<PointageCell>>(),
                    Statut = this.GetPersonnelStatut(personnel?.Statut ?? string.Empty),
                    IsUserGsp = Managers.Utilisateur.IsRoleGSPWithoutConsideringPaieLevel(currentUser, organisationId)
                };

                if (!personnelNode.IsPersonnelCiDesaffected)
                {
                    if (!personnelNode.IsAbsence)
                    {
                        FulfillAstreintesForPersonnelSubNodes(personnelNode, rapportList);
                        personnelNode.SubNodeList.Add(FulfillMajorationsSubNodesPersonnel(rapportList, personnel));
                        personnelNode.SubNodeList.Add(FulfillPrimesSubNodesPersonnel(rapportList, personnel));
                    }

                    FulfillTasksForSubNodes(personnelNode, rapportList, (personnel?.SocieteId ?? societeId), rapportHebdoNode);
                }

                personnelSubNodes.Add(personnelNode);
            }

            rapportHebdoNode.SubNodeList = personnelSubNodes;
        }

        /// <summary>
        /// Remplir les noeuds des astreintes
        /// </summary>
        /// <param name="personnelSubNode">Le noeud personnel à remplir</param>
        /// <param name="rapportList">La liste des rapports</param>
        private void FulfillAstreintesForPersonnelSubNodes(RapportHebdoSubNode<PointageCell> personnelSubNode, List<RapportEnt> rapportList)
        {
            if (personnelSubNode?.SubNodeList != null)
            {
                var listAstreintes = GetAstreintePointageHebdoCells(personnelSubNode.NodeId, rapportList);
                int nombreSorties = GetNombreSortieMaximum(listAstreintes);

                AffectAstreinteToSubNode(personnelSubNode, listAstreintes, nombreSorties);
            }
        }

        private int GetNombreSortieMaximum(List<AstreintePointageHebdoCell> listAstreintes)
        {
            int maxSorties = 0;
            listAstreintes.ForEach(x =>
            {
                if (maxSorties < x.ListRapportLigneAstreintes.Count)
                {
                    maxSorties = x.ListRapportLigneAstreintes.Count;
                }
            });
            return maxSorties;
        }

        private void AffectAstreinteToSubNode(RapportHebdoSubNode<PointageCell> subNode, List<AstreintePointageHebdoCell> listAstreintes, int nombreSorties)
        {
            if (nombreSorties > 0)
            {
                for (int i = 0; i < nombreSorties; i++)
                {
                    var sortie = new List<AstreintePointageHebdoCell>();
                    BuildListAstreinte(listAstreintes, sortie, i);

                    if (sortie.Count > 0)
                    {
                        subNode.SubNodeList.Add(new RapportHebdoSubNode<PointageCell>
                        {
                            NodeText = FeatureRapportHebdo.RapportHebdo_Sortie_Astreinte,
                            NodeType = NodeType.Astreinte,
                            Items = sortie
                        });
                    }
                }
            }
        }

        private void BuildListAstreinte(List<AstreintePointageHebdoCell> listAstreintes, List<AstreintePointageHebdoCell> sortie, int indexSortie)
        {
            for (var j = 0; j < listAstreintes.Count; j++)
            {
                if (listAstreintes[j].ListRapportLigneAstreintes.Count > indexSortie)
                {
                    var listRapportLigneAstreintes = new List<RapportLigneAstreinteModel>();
                    listRapportLigneAstreintes.Add(listAstreintes[j].ListRapportLigneAstreintes[indexSortie]);
                    sortie.Add(new AstreintePointageHebdoCell
                    {
                        AstreinteId = listAstreintes[j].AstreinteId,
                        HasAstreinte = listAstreintes[j].HasAstreinte,
                        Date = listAstreintes[j].Date,
                        PersonnelVerrouille = listAstreintes[j].PersonnelVerrouille,
                        RapportId = listAstreintes[j].RapportId,
                        RapportLigneId = listAstreintes[j].RapportLigneId,
                        RapportValide = listAstreintes[j].RapportValide,
                        ListRapportLigneAstreintes = listRapportLigneAstreintes
                    });
                }
                else
                {
                    if (listAstreintes[j].HasAstreinte)
                    {
                        sortie.Add(new AstreintePointageHebdoCell
                        {
                            HasAstreinte = true,
                            AstreinteId = listAstreintes[j].AstreinteId,
                            Date = listAstreintes[j].Date,
                            ListRapportLigneAstreintes = new List<RapportLigneAstreinteModel>()
                        });
                    }
                    else
                    {
                        sortie.Add(new AstreintePointageHebdoCell
                        {
                            HasAstreinte = false,
                            AstreinteId = 0,
                            ListRapportLigneAstreintes = new List<RapportLigneAstreinteModel>()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Remplir les noeuds des taches
        /// </summary>
        /// <param name="subNode">Le noeud à remplir</param>
        /// <param name="rapportList">La liste des rapports</param>
        /// <param name="societedId">Societe id</param>
        /// <param name="rapportHebdoNode">rapport Hebdo Node</param>
        /// <param name="isAffichageParOuvrier">Is affichage par ouvrier</param>
        /// <param name="personnelNode">Personnel node</param>
        private void FulfillTasksForSubNodes(
          RapportHebdoSubNode<PointageCell> subNode,
          List<RapportEnt> rapportList,
          int? societedId,
          RapportHebdoNode<PointageCell> rapportHebdoNode = null,
          bool isAffichageParOuvrier = false,
          RapportHebdoNode<PointageCell> personnelNode = null
          )
        {
            if (rapportList.IsNullOrEmpty() || subNode?.SubNodeList == null)
            {
                return;
            }

            // Différenciation des affichages
            Func<RapportLigneEnt, bool> queryRapportLigne = GetQueryRapportLigneForTaskSubNode(subNode, personnelNode, isAffichageParOuvrier);
            int ciId = isAffichageParOuvrier ? subNode.NodeId : rapportHebdoNode.NodeId;
            int personnelId = isAffichageParOuvrier ? personnelNode.NodeId : subNode.NodeId;

            if (subNode.IsAbsence && rapportList.Any(rl => rl.ListLignes.Count > 1))
            {
                FillMultiLignesAbsenceFiggoForSubNode(subNode, rapportList, societedId, queryRapportLigne, ciId, personnelId);
            }
            else
            {
                FillSingleAbsenceForSubNode(subNode, rapportList, societedId, queryRapportLigne, ciId, personnelId);
            }

            FillTachesForSubNode(subNode, rapportList, societedId, rapportHebdoNode, isAffichageParOuvrier, queryRapportLigne, personnelId);
        }

        private void FillTachesForSubNode(RapportHebdoSubNode<PointageCell> subNode, List<RapportEnt> rapportList, int? societedId, RapportHebdoNode<PointageCell> rapportHebdoNode, bool isAffichageParOuvrier, Func<RapportLigneEnt, bool> queryRapportLigne, int personnelId)
        {
            int ciId = isAffichageParOuvrier ? subNode.NodeId : rapportHebdoNode.NodeId;

            // Remplissage des tâches
            List<TacheEnt> tList = new List<TacheEnt>();
            rapportList.ForEach(rp =>
            {
                IEnumerable<TacheEnt> taches = rp?.ListLignes?.FirstOrDefault(queryRapportLigne)?.ListRapportLigneTaches?.Select(r => r.Tache);
                if (!taches.IsNullOrEmpty())
                {
                    tList.AddRange(taches);
                }
            });
            tList = tList.DistinctBy(d => d.TacheId).ToList();
            var listDates = rapportList.OrderBy(o => o.DateChantier).Select(x => x.DateChantier);

            if (tList == null || tList.Count == 0)
            {
                AddDefaultTaskInSubNodeList(subNode, societedId, listDates, rapportHebdoNode, isAffichageParOuvrier, ciId, personnelId);
                return;
            }

            foreach (var task in tList)
            {
                if (task == null)
                {
                    continue;
                }
                var items = new List<Tuple<DateTime, RapportLigneTacheEnt>>();
                foreach (var r in rapportList.OrderBy(o => o.DateChantier))
                {
                    var rapportLigne = r.ListLignes.FirstOrDefault(queryRapportLigne)?
                                                        .ListRapportLigneTaches.FirstOrDefault(o => o.TacheId == task?.TacheId)
                                                                    ?? new RapportLigneTacheEnt();
                    var item = Tuple.Create(r.DateChantier, rapportLigne);
                    items.Add(item);
                }
                subNode.SubNodeList.Add(new RapportHebdoSubNode<PointageCell>
                {
                    NodeId = task.TacheId,
                    NodeText = task.Libelle,
                    NodeCode = task.Code,
                    IsDefaultTask = task.TacheParDefaut,
                    NodeType = NodeType.Task,
                    SocieteId = societedId,
                    Items = GetPointageTaskHebdo(items, ciId, personnelId)
                });


            }
            AddDefaultTaskInSubNodeList(subNode, societedId, listDates, rapportHebdoNode, isAffichageParOuvrier, ciId, personnelId);
        }

        private void FillSingleAbsenceForSubNode(RapportHebdoSubNode<PointageCell> subNode, List<RapportEnt> rapportList, int? societedId, Func<RapportLigneEnt, bool> queryRapportLigne, int ciId, int personnelId)
        {
            // Remplissage des absences
            var taskAbsenceList = from r in rapportList.OrderBy(t => t.DateChantier)
                                  group r by r.DateChantier into g
                                  select new
                                  {
                                      DateChantier = g.Key,
                                      Absence = g.Select(o => o.ListLignes?.FirstOrDefault(queryRapportLigne))?.FirstOrDefault(z => z != null)
                                  };

            subNode.SubNodeList.Add(new RapportHebdoSubNode<PointageCell>
            {
                NodeText = FeatureRapportHebdo.RapportHebdo_Absence,
                NodeType = NodeType.Task,
                SocieteId = societedId,
                IsAbsence = subNode.IsAbsence,
                Items = GetPointageAbsenceHebdo(taskAbsenceList.ToDictionary(x => x.DateChantier, v => v.Absence), ciId, personnelId)
            });
        }

        private void FillMultiLignesAbsenceFiggoForSubNode(RapportHebdoSubNode<PointageCell> subNode, List<RapportEnt> rapportList, int? societedId, Func<RapportLigneEnt, bool> queryRapportLigne, int ciId, int personnelId)
        {
            List<int?> codeAbsence = rapportList.SelectMany(rl => rl.ListLignes.Where(queryRapportLigne))
             .DistinctBy(d => d.CodeAbsenceId).Select(s => s.CodeAbsenceId).ToList();

            // absence ci générique
            foreach (var code in codeAbsence.Where(c => c != null))
            {
                var items = new Dictionary<DateTime, RapportLigneEnt>();

                foreach (var r in rapportList.OrderBy(o => o.DateChantier))
                {
                    var rapportLigne = r.ListLignes.Where(l => l.CodeAbsenceId == code).FirstOrDefault(queryRapportLigne) ?? new RapportLigneEnt();
                    items.Add(r.DateChantier, rapportLigne);
                }

                subNode.SubNodeList.Add(new RapportHebdoSubNode<PointageCell>
                {
                    NodeText = FeatureRapportHebdo.RapportHebdo_Absence,
                    NodeType = NodeType.Task,
                    SocieteId = societedId,
                    IsAbsence = subNode.IsAbsence,
                    Items = GetPointageAbsenceHebdo(items, ciId, personnelId)
                });
            }

            //Concatener les commentaires
            if (subNode?.SubNodeList != null && subNode.SubNodeList.Any())
            {
                for (int i = 0; i < subNode.SubNodeList[0].Items.Count(); i++)
                {
                    var commentaires = new StringBuilder();
                    var delimiter = "";
                    for (int y = 0; y < subNode.SubNodeList.Count; y++)
                    {
                        var com = subNode.SubNodeList[y].Items.ToList()[i].Commentaire;
                        if (!string.IsNullOrEmpty(com))
                        {
                            commentaires.Append(delimiter);
                            commentaires.Append(com);
                            delimiter = " ";
                        }
                    }
                    subNode.SubNodeList[0].Items.ToList()[i].Commentaire = commentaires.ToString();
                }
            }
        }

        /// <summary>
        /// Get rapport ligne query for task subnode
        /// </summary>
        /// <param name="subNode">Rapport hebdo subnode pointage</param>
        /// <param name="personnelNode">Rapport hebdo node</param>
        /// <param name="isAffichageParOuvrier">is Affichage est par ouvrier</param>
        /// <returns>Query func</returns>
        private Func<RapportLigneEnt, bool> GetQueryRapportLigneForTaskSubNode(RapportHebdoSubNode<PointageCell> subNode, RapportHebdoNode<PointageCell> personnelNode, bool isAffichageParOuvrier)
        {
            Func<RapportLigneEnt, bool> queryRapportLigneCi = (r => r != null
                                                       && r.CiId == subNode.NodeId
                                                       && personnelNode != null
                                                       && r.PersonnelId == personnelNode.NodeId
                                                       && r.DateSuppression == null);
            Func<RapportLigneEnt, bool> queryRapportLigneOuvrier = (r => r != null && r.PersonnelId == subNode.NodeId && r.DateSuppression == null);
            Func<RapportLigneEnt, bool> queryRapportLigne = isAffichageParOuvrier ? queryRapportLigneCi : queryRapportLigneOuvrier;
            return queryRapportLigne;
        }

        /// <summary>
        /// ajouter la tache par defaut sur SubNodeList
        /// </summary>
        /// <param name="subNode">subNode</param>
        /// <param name="societedId">societed Id</param>
        /// <param name="listDates">list Dates</param>
        /// <param name="rapportHebdoNode">rapport Hebdo Node</param>
        /// <param name="isAffichageParOuvrier">isAffichageParOuvrier</param>
        /// <param name="ciId">Ci Idenrifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        private void AddDefaultTaskInSubNodeList(RapportHebdoSubNode<PointageCell> subNode, int? societedId, IEnumerable<DateTime> listDates, RapportHebdoNode<PointageCell> rapportHebdoNode, bool isAffichageParOuvrier, int ciId, int personnelId)
        {
            if ((isAffichageParOuvrier && subNode.NodeType == NodeType.Etude) || (!isAffichageParOuvrier && rapportHebdoNode.NodeType == NodeType.Etude))
            {
                return;
            }

            bool existDefaultTask = subNode.SubNodeList.Any(s => s.IsDefaultTask);
            if (!existDefaultTask)
            {
                var defaultTask = this.tacheManager.GetTacheParDefaut(ciId);
                if (defaultTask != null && !subNode.IsPersonnelCiDesaffected)
                {
                    var items = new List<Tuple<DateTime, RapportLigneTacheEnt>>();
                    listDates.ForEach(dateChantier =>
                    {
                        var item = Tuple.Create(dateChantier, new RapportLigneTacheEnt());
                        items.Add(item);
                    });

                    subNode.SubNodeList.Add(new RapportHebdoSubNode<PointageCell>
                    {
                        NodeId = defaultTask.TacheId,
                        NodeText = defaultTask.Libelle,
                        NodeCode = defaultTask.Code,
                        IsDefaultTask = defaultTask.TacheParDefaut,
                        NodeType = NodeType.Task,
                        SocieteId = societedId,
                        Items = GetPointageTaskHebdo(items, ciId, personnelId)
                    });
                }
            }
        }

        /// <summary>
        /// Fulfill Majorations sub nodes
        /// </summary>
        /// <param name="rapportList">Lists des rapports</param>
        /// <param name="personnel">Personnel</param>
        /// <returns>Rapport hebdo sub node</returns>
        private RapportHebdoSubNode<PointageCell> FulfillMajorationsSubNodesPersonnel(List<RapportEnt> rapportList, PersonnelEnt personnel)
        {
            RapportHebdoSubNode<PointageCell> majorationNode = new RapportHebdoSubNode<PointageCell>
            {
                NodeId = personnel.PersonnelId,
                NodeText = $"{personnel.Societe.Code} - {personnel.Matricule} - {personnel.Nom} {personnel.Prenom}",
                NodeType = NodeType.Majoration,
                Items = new List<MajorationPointageHebdoCell>()
            };
            var mondayDate = rapportList.FirstOrDefault().DateChantier;
            majorationNode.Items = HandleMajorationPersonnelNodeItems(rapportList, personnel.PersonnelId);
            FulfillMajorationsItems(majorationNode.Items.ToList(), mondayDate);
            return majorationNode;
        }

        /// <summary>
        /// Récupérer les colonnes des pointages pour les sorties astreintes
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="rapportList">La liste des rapports</param>
        /// <returns>La liste des colonnes des pointages pour les sorties astreintes</returns>
        private List<AstreintePointageHebdoCell> GetAstreintePointageHebdoCells(int personnelId, List<RapportEnt> rapportList)
        {
            List<AstreintePointageHebdoCell> astreintePointageHebdoCellList = new List<AstreintePointageHebdoCell>();

            foreach (RapportEnt rapport in rapportList.OrderBy(r => r.DateChantier))
            {
                List<RapportLigneEnt> pointagesToTreat = rapport.ListLignes?.Where(l => !l.DateSuppression.HasValue).ToList();
                string rapportLigneStatut = string.Empty;
                if (rapport.ListLignes.Any())
                {
                    rapportLigneStatut = GetRapportLigneNodeStatut(rapport.CiId, personnelId, rapport.DateChantier, pointagesToTreat?.FirstOrDefault(l => l.PersonnelId == personnelId)?.RapportLigneStatutId);
                }
                AstreintePointageHebdoCell astreintePointageHebdoCell = new AstreintePointageHebdoCell
                {
                    Date = rapport.DateChantier,
                    RapportId = rapport.RapportId,
                    RapportLigneId = pointagesToTreat?.FirstOrDefault(l => l.PersonnelId == personnelId)?.RapportLigneId,
                    PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                    RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value)
                };

                AstreinteEnt astreinte = affectationManager.GetAstreinte(rapport.CiId, personnelId, rapport.DateChantier);
                if (astreinte != null)
                {
                    astreintePointageHebdoCell.HasAstreinte = true;
                    astreintePointageHebdoCell.AstreinteId = astreinte.AstreintId;
                    astreintePointageHebdoCell.ListRapportLigneAstreintes = mapper.Map<List<RapportLigneAstreinteModel>>(Repository.GetRapportLigneAstreintes(rapport.RapportId, personnelId, rapport.CiId, rapport.DateChantier));
                }
                else
                {
                    astreintePointageHebdoCell.HasAstreinte = false;
                    astreintePointageHebdoCell.AstreinteId = 0;
                    astreintePointageHebdoCell.ListRapportLigneAstreintes = new List<RapportLigneAstreinteModel>();
                }
                astreintePointageHebdoCellList.Add(astreintePointageHebdoCell);

            }
            return astreintePointageHebdoCellList;
        }

        /// <summary>
        /// Fulfill Majoration for Ci SubNodes
        /// </summary>
        /// <param name="ci">Centre d'imputation</param>
        /// <param name="personnelNode">Personnel node</param>
        /// <param name="rapportList">List des rapports</param>
        /// <param name="societeId">Societe id pour l'utilisation des lookups</param>
        /// <param name="currentUser">l'utilisateur courant</param>
        /// <param name="organisationId">Organisation identifier</param>
        private void FulfillCiPointageForPersonnel(RapportHebdoByEmployee.CI ci, RapportHebdoNode<PointageCell> personnelNode, List<RapportEnt> rapportList, int? societeId, int currentUser, int? organisationId)
        {
            if (personnelNode != null)
            {
                bool isDesaffectedCiPersonnel = Managers.Pointage.GetpointageByPersonnelAndCiForReadOnly(ci.CiId, personnelNode.NodeId);
                RapportHebdoSubNode<PointageCell> ciNode = new RapportHebdoSubNode<PointageCell>
                {
                    NodeId = ci.CiId,
                    NodeType = GetCiNodeType(ci.TypeDesignation),
                    NodeText = $"{ci.Code} - {ci.Libelle}",
                    SubNodeList = new List<RapportHebdoSubNode<PointageCell>>(),
                    IsPersonnelCiDesaffected = isDesaffectedCiPersonnel && !rapportList.Any(x => x.ListLignes.Any(y => y.PersonnelId.Value == personnelNode.NodeId && y.RapportLigneId > 0)),
                    PersonnelToReadOnly = isDesaffectedCiPersonnel,
                    IsAbsence = ci.IsAbsence,
                    IsUserGsp = Managers.Utilisateur.IsRoleGSPWithoutConsideringPaieLevel(currentUser, organisationId)
                };

                if (!ciNode.IsPersonnelCiDesaffected)
                {
                    if (!ciNode.IsAbsence)
                    {
                        FulfillAstreintesForCiSubNodes(personnelNode.NodeId, ciNode, rapportList);
                        ciNode.SubNodeList.Add(FulfillMajorationsSubNodesCi(rapportList, ci, personnelNode.NodeId));
                        ciNode.SubNodeList.Add(FulfillPrimesSubNodesCi(rapportList, ci, personnelNode.NodeId));
                    }

                    FulfillTasksForSubNodes(ciNode, rapportList, societeId, null, true, personnelNode);
                }

                ciNode.SubNodeList = ciNode.SubNodeList.OrderBy(x => x.NodeType).ToList();
                personnelNode.SubNodeList.Add(ciNode);
            }
        }

        /// <summary>
        /// Remplir les noeuds des astreintes
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="ciSubNode">Le noeud CI à remplir</param>
        /// <param name="rapportList">La liste des rapports</param>
        private void FulfillAstreintesForCiSubNodes(int personnelId, RapportHebdoSubNode<PointageCell> ciSubNode, List<RapportEnt> rapportList)
        {
            if (ciSubNode?.SubNodeList != null)
            {
                var listAstreintes = GetAstreintePointageHebdoCells(personnelId, rapportList);
                int nombreSorties = GetNombreSortieMaximum(listAstreintes);

                AffectAstreinteToSubNode(ciSubNode, listAstreintes, nombreSorties);
            }
        }

        /// <summary>
        /// Fulfill majorations subNodes Ci
        /// </summary>
        /// <param name="rapportList">Lists des rapports</param>
        /// <param name="ci">CI</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport hebdo sub node</returns>
        private RapportHebdoSubNode<PointageCell> FulfillMajorationsSubNodesCi(List<RapportEnt> rapportList, RapportHebdoByEmployee.CI ci, int personnelId)
        {
            RapportHebdoSubNode<PointageCell> majorationNode = new RapportHebdoSubNode<PointageCell>
            {
                NodeId = ci.CiId,
                NodeText = $"{ci.Code} - {ci.Libelle}",
                IsAbsence = ci.IsAbsence,
                NodeType = NodeType.Majoration,
                Items = new List<MajorationPointageHebdoCell>()
            };
            var mondayDate = rapportList.FirstOrDefault().DateChantier;
            majorationNode.Items = HandleMajorationPersonnelNodeItems(rapportList, personnelId);
            FulfillMajorationsItems(majorationNode.Items.ToList(), mondayDate);
            return majorationNode;
        }

        /// <summary>
        /// Remplir les jours du semaine par les majorations
        /// </summary>
        /// <param name="majorationLigne">Ligne de majoration</param>
        /// <param name="mondayDate">date de la premiere journee de la semaine</param>
        private void MajorationFullWeek(MajorationPointageHebdoCell majorationLigne, DateTime mondayDate)
        {
            if (majorationLigne.MajorationHeurePerDayList.Count < 7)
            {
                var nextday = mondayDate;
                for (int i = 0; i < 7; i++)
                {
                    MajorationHeurePerDay majorationHeurePerDay = new MajorationHeurePerDay();
                    MajorationHeurePerDay majorationCell = majorationLigne.MajorationHeurePerDayList.FirstOrDefault(x => x.DayOfWeek == i);
                    if (majorationCell == null)
                    {
                        majorationHeurePerDay.DayOfWeek = i;
                        majorationHeurePerDay.datePointage = nextday;
                        majorationLigne.MajorationHeurePerDayList.Add(majorationHeurePerDay);
                    }
                    nextday = nextday.AddDays(1);
                }

            }

            majorationLigne.MajorationHeurePerDayList = majorationLigne.MajorationHeurePerDayList.OrderBy(x => x.datePointage).ToList();
        }

        /// <summary>
        /// Fulfill majoration list items
        /// </summary>
        /// <param name="majorationPointageHebdoCells">List des majorations pointage cell</param>
        /// <param name="mondayDate">date de la premiere journee de la semaine</param>
        private void FulfillMajorationsItems(List<PointageCell> majorationPointageHebdoCells, DateTime mondayDate)
        {
            if (majorationPointageHebdoCells != null && majorationPointageHebdoCells.Any())
            {
                foreach (MajorationPointageHebdoCell majorationLigne in majorationPointageHebdoCells.OfType<MajorationPointageHebdoCell>())
                {
                    if (majorationLigne.MajorationHeurePerDayList != null && majorationLigne.MajorationHeurePerDayList.Any())
                    {
                        MajorationFullWeek(majorationLigne, mondayDate);
                    }
                }
            }
        }

        /// <summary>
        /// Handle majoration node items
        /// </summary>
        /// <param name="rapportList">List des rapport journaliers</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>List des majorations pointage hebdo cell</returns>
        private List<MajorationPointageHebdoCell> HandleMajorationPersonnelNodeItems(List<RapportEnt> rapportList, int personnelId)
        {
            List<MajorationPointageHebdoCell> majorationSubNodes = new List<MajorationPointageHebdoCell>();
            foreach (RapportEnt rapport in rapportList.OrderBy(x => x.DateChantier))
            {
                if (rapport.ListLignes?.Any() == true)
                {
                    RapportLigneEnt personnelLigne = rapport.ListLignes.FirstOrDefault(x => !x.DateSuppression.HasValue && x.PersonnelId == personnelId);
                    if (personnelLigne?.ListRapportLigneMajorations?.Any() == true)
                    {
                        majorationSubNodes = HandleMajorationLigneNodeItems(rapport, personnelLigne, majorationSubNodes);
                    }
                }
            }

            return majorationSubNodes;
        }

        /// <summary>
        /// Handle majoration ligne node 
        /// </summary>
        /// <param name="rapport">Rapport journalier</param>
        /// <param name="personnelLigne">ligne rapport journalier</param>
        /// <param name="majorationSubNodes">Majoration sub nodes</param>
        /// <returns>List des majorations pointage hebdo cell</returns>
        private List<MajorationPointageHebdoCell> HandleMajorationLigneNodeItems(RapportEnt rapport, RapportLigneEnt personnelLigne, List<MajorationPointageHebdoCell> majorationSubNodes)
        {
            string rapportLigneStatut = GetRapportLigneNodeStatut(personnelLigne.CiId, personnelLigne.PersonnelId.Value, personnelLigne.DatePointage, personnelLigne.RapportLigneStatutId);
            foreach (RapportLigneMajorationEnt majoration in personnelLigne.ListRapportLigneMajorations)
            {
                if (!majorationSubNodes.Any(x => x.CodeMajorationId == majoration.CodeMajorationId))
                {
                    majorationSubNodes.Add(new MajorationPointageHebdoCell
                    {
                        CodeMajoration = majoration.CodeMajoration?.Code,
                        CodeMajorationId = majoration.CodeMajorationId,
                        IsHeureNuit = majoration.CodeMajoration != null ? majoration.CodeMajoration.IsHeureNuit : false,
                        MajorationHeurePerDayList = new List<MajorationHeurePerDay>
                  {
                  new MajorationHeurePerDay
                  {
                    RapportId = rapport.RapportId,
                    DayOfWeek = GetDayOfWeek(rapport.DateChantier),
                    HeureMajoration = majoration.HeureMajoration,
                    RapportLigneId = personnelLigne.RapportLigneId,
                    HeureMojorationOldValue = majoration.HeureMajoration,
                    PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                    RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                    datePointage = rapport.DateChantier
                  }
                }
                    });
                }
                else
                {
                    MajorationPointageHebdoCell majorationPointage = majorationSubNodes.FirstOrDefault(x => x.CodeMajorationId == majoration.CodeMajorationId);
                    majorationPointage.MajorationHeurePerDayList.Add(
                      new MajorationHeurePerDay
                      {
                          RapportId = rapport.RapportId,
                          DayOfWeek = GetDayOfWeek(rapport.DateChantier),
                          HeureMajoration = majoration.HeureMajoration,
                          RapportLigneId = personnelLigne.RapportLigneId,
                          HeureMojorationOldValue = majoration.HeureMajoration,
                          PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                          datePointage = rapport.DateChantier,
                          RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value)
                      }
                      );
                }
            }
            return majorationSubNodes;
        }


        /// <summary>
        /// Get pointage task hebdo
        /// </summary>
        /// <param name="taskList">Task list</param>
        /// <param name="ciId">Ci Identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>List of PointageCell</returns>
        private List<PointageCell> GetPointageTaskHebdo(List<Tuple<DateTime, RapportLigneTacheEnt>> taskList, int ciId, int personnelId)
        {
            if (taskList.IsNullOrEmpty())
            {
                return new List<PointageCell>();
            }
            List<PointageCell> result = new List<PointageCell>();

            foreach (var ligneTache in taskList)
            {
                DateTime date = ligneTache.Item1;
                RapportLigneTacheEnt ligne = ligneTache.Item2;
                string rapportLigneStatut = GetRapportLigneNodeStatut(ciId, personnelId, date, ligne?.RapportLigne?.RapportLigneStatutId);
                PointageCell pointageCell = new PointageCell
                {
                    Date = date,
                    RapportId = ligne?.RapportLigne?.Rapport?.RapportId,
                    RapportLigneId = ligne?.RapportLigne?.RapportLigneId,
                    Id = ligne?.RapportLigneTacheId,
                    TotalHours = ligne?.HeureTache ?? 0,
                    PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                    RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                };

                result.Add(pointageCell);
            }

            return result;
        }

        /// <summary>
        /// Get pointage absence hebdo
        /// </summary>
        /// <param name="absenceList">Absence list</param>
        /// <param name="ciId">Ci Identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <returns>List of PointageCell</returns>
        private IEnumerable<PointageCell> GetPointageAbsenceHebdo(Dictionary<DateTime, RapportLigneEnt> absenceList, int ciId, int personnelId)
        {

            if (absenceList.IsNullOrEmpty())
            {
                return new List<PointageCell>();
            }

            List<PointageCell> result = new List<PointageCell>();

            foreach (var absence in absenceList)
            {
                string rapportLigneStatut = GetRapportLigneNodeStatut(ciId, personnelId, absence.Key, absence.Value?.RapportLigneStatutId);
                PointageCell pointageCell = new PointageCell
                {
                    Date = absence.Key,
                    RapportId = absence.Value?.RapportId,
                    RapportLigneId = absence.Value?.RapportLigneId,
                    Id = absence.Value?.RapportLigneId,
                    TotalHours = absence.Value?.HeureAbsence ?? 0,
                    CodeAbsence = absence.Value?.CodeAbsence?.Code,
                    CodeAbsenceId = absence.Value?.CodeAbsenceId,
                    isAbsenceCell = true,
                    PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                    RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                    Commentaire = absence.Value?.Commentaire,
                    ValueInPanel = new ValueInPanelModel()
                };

                result.Add(pointageCell);
            }

            return result;
        }

        /// <summary>
        /// Fulfill Primes sub nodes
        /// </summary>
        /// <param name="rapportList">Lists des rapports</param>
        /// <param name="personnel">Personnel</param>
        /// <returns>Rapport hebdo sub node</returns>
        private RapportHebdoSubNode<PointageCell> FulfillPrimesSubNodesPersonnel(List<RapportEnt> rapportList, PersonnelEnt personnel)
        {
            RapportHebdoSubNode<PointageCell> primeNode = new RapportHebdoSubNode<PointageCell>
            {
                NodeId = personnel.PersonnelId,
                NodeText = $"{personnel.Societe.Code} - {personnel.Matricule} - {personnel.Nom} {personnel.Prenom}",
                NodeType = NodeType.Prime,
                Items = new List<PrimePointageHebdoCell>()
            };
            var mondayDate = rapportList.FirstOrDefault().DateChantier;
            primeNode.Items = HandlePrimePersonnelNodeItems(rapportList, personnel.PersonnelId);
            FulfillPrimeItems(primeNode.Items.ToList(), mondayDate);
            return primeNode;
        }

        /// <summary>
        /// Fulfill primes subNodes Ci
        /// </summary>
        /// <param name="rapportList">Lists des rapports</param>
        /// <param name="ci">CI</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>Rapport hebdo sub node</returns>
        private RapportHebdoSubNode<PointageCell> FulfillPrimesSubNodesCi(List<RapportEnt> rapportList, RapportHebdoByEmployee.CI ci, int personnelId)
        {
            RapportHebdoSubNode<PointageCell> primeNode = new RapportHebdoSubNode<PointageCell>
            {
                NodeId = ci.CiId,
                NodeText = $"{ci.Code} - {ci.Libelle}",
                IsAbsence = ci.IsAbsence,
                NodeType = NodeType.Prime,
                Items = new List<PrimePointageHebdoCell>()
            };
            var mondayDate = rapportList.FirstOrDefault().DateChantier;
            primeNode.Items = HandlePrimePersonnelNodeItems(rapportList, personnelId);
            FulfillPrimeItems(primeNode.Items.ToList(), mondayDate);
            return primeNode;
        }

        /// <summary>
        /// Handle prime node items
        /// </summary>
        /// <param name="rapportList">List des rapport journaliers</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <returns>List des primes pointage hebdo cell</returns>
        private List<PrimePointageHebdoCell> HandlePrimePersonnelNodeItems(List<RapportEnt> rapportList, int personnelId)
        {
            List<PrimePointageHebdoCell> primeSubNodes = new List<PrimePointageHebdoCell>();
            foreach (RapportEnt rapport in rapportList.OrderBy(x => x.DateChantier))
            {
                if (rapport.ListLignes != null && rapport.ListLignes.Any())
                {
                    RapportLigneEnt personnelLigne = rapport.ListLignes.FirstOrDefault(x => x.PersonnelId == personnelId && x.DateSuppression == null);
                    if (personnelLigne?.ListRapportLignePrimes?.Any() == true)
                    {
                        primeSubNodes = HandlePrimeLigneNodeItems(rapport, personnelLigne, primeSubNodes);
                    }
                }
            }

            return primeSubNodes;
        }

        /// <summary>
        /// Handle majoration ligne node 
        /// </summary>
        /// <param name="rapport">Rapport journalier</param>
        /// <param name="personnelLigne">ligne rapport journalier</param>
        /// <param name="primeSubNodes">Prime sub nodes</param>
        /// <returns>List des primes pointage hebdo cell</returns>
        private List<PrimePointageHebdoCell> HandlePrimeLigneNodeItems(RapportEnt rapport, RapportLigneEnt personnelLigne, List<PrimePointageHebdoCell> primeSubNodes)
        {
            string rapportLigneStatut = GetRapportLigneNodeStatut(personnelLigne.CiId, personnelLigne.PersonnelId.Value, personnelLigne.DatePointage, personnelLigne.RapportLigneStatutId);
            foreach (RapportLignePrimeEnt prime in personnelLigne.ListRapportLignePrimes)
            {
                if (!primeSubNodes.Any(x => x.PrimeId == prime.PrimeId))
                {
                    primeSubNodes.Add(new PrimePointageHebdoCell
                    {
                        PrimeId = prime.PrimeId,
                        PrimeLibelle = prime?.Prime?.Libelle,
                        PrimeCode = prime?.Prime?.Code,
                        IsPrimeJournaliere = prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere ? true : false,
                        IsPrimeAstreinte = prime.Prime?.IsPrimeAstreinte == null ? false : prime.Prime?.IsPrimeAstreinte,
                        RapportHebdoPrimePerDayList = new List<RapportHebdoPrimePerDay>
                    {
                        new RapportHebdoPrimePerDay
                          {
                            RapportId = rapport.RapportId,
                            DayOfWeek = GetDayOfWeek(rapport.DateChantier),
                            HeurePrime = prime.HeurePrime,
                            IsChecked = prime.IsChecked,
                            RapportLigneId = personnelLigne.RapportLigneId,
                            HeurePrimeOldValue = prime.HeurePrime,
                            PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                            RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                            datePointage = personnelLigne.DatePointage.Date
                          }
                    }
                    });
                }
                else
                {
                    PrimePointageHebdoCell primePointage = primeSubNodes.FirstOrDefault(x => x.PrimeId == prime.PrimeId);
                    if (!primePointage.RapportHebdoPrimePerDayList.Any(x => x.DayOfWeek == GetDayOfWeek(rapport.DateChantier)))
                    {
                        primePointage.RapportHebdoPrimePerDayList.Add(new RapportHebdoPrimePerDay
                        {
                            RapportId = rapport.RapportId,
                            DayOfWeek = GetDayOfWeek(rapport.DateChantier),
                            HeurePrime = prime.HeurePrime,
                            IsChecked = prime.IsChecked,
                            RapportLigneId = personnelLigne.RapportLigneId,
                            HeurePrimeOldValue = prime.HeurePrime,
                            PersonnelVerrouille = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutVerrouille.Value),
                            RapportValide = rapportLigneStatut.Equals(RapportStatutEnt.RapportStatutValide2.Value),
                            datePointage = personnelLigne.DatePointage.Date
                        });
                    }
                }
            }

            return primeSubNodes;
        }

        /// <summary>
        /// Fulfill primes list items
        /// </summary>
        /// <param name="primePointageHebdoCells">List des primes pointage cell</param>
        /// <param name="mondayDate">date de la premiere journee de la semaine</param>
        private void FulfillPrimeItems(List<PointageCell> primePointageHebdoCells, DateTime mondayDate)
        {
            if (primePointageHebdoCells != null && primePointageHebdoCells.Any())
            {
                foreach (PrimePointageHebdoCell primeLigne in primePointageHebdoCells.OfType<PrimePointageHebdoCell>())
                {
                    if (primeLigne.RapportHebdoPrimePerDayList != null && primeLigne.RapportHebdoPrimePerDayList.Any())
                    {
                        PrimeFullWeek(primeLigne, mondayDate);
                    }
                }
            }
        }

        /// <summary>
        /// Remplir les jours du semaine par les primes
        /// </summary>
        /// <param name="primeLigne">Ligne de prime</param>
        /// <param name="mondayDate">date de la premiere journee de la semaine</param>
        private void PrimeFullWeek(PrimePointageHebdoCell primeLigne, DateTime mondayDate)
        {
            var nextday = mondayDate;
            if (primeLigne.RapportHebdoPrimePerDayList.Count < 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    RapportHebdoPrimePerDay primeHeurePerDay = new RapportHebdoPrimePerDay();
                    RapportHebdoPrimePerDay primeCell = primeLigne.RapportHebdoPrimePerDayList.FirstOrDefault(x => x.DayOfWeek == i);
                    if (primeCell == null)
                    {
                        primeHeurePerDay.DayOfWeek = i;
                        primeHeurePerDay.datePointage = nextday;
                        primeLigne.RapportHebdoPrimePerDayList.Add(primeHeurePerDay);
                    }

                    nextday = nextday.AddDays(1);
                }
            }

            primeLigne.RapportHebdoPrimePerDayList = primeLigne.RapportHebdoPrimePerDayList.OrderBy(x => x.datePointage).ToList();
        }

        /// <summary>
        /// Récupérer la liste des identifiants des personnels
        /// </summary>
        /// <param name="pointagePanelViewModel">Le model de rapport hebdomadaire</param>
        /// <returns>Une liste des identifiants des personnels</returns>
        private List<int> GetPersonnelsIdFromRapportHebdoNodes(IEnumerable<RapportHebdoNode<PointageCell>> pointagePanelViewModel)
        {
            List<int> personnelIdList = new List<int>();

            if (IsCiNode(pointagePanelViewModel.FirstOrDefault().NodeType))
            {
                foreach (RapportHebdoNode<PointageCell> ciNode in pointagePanelViewModel)
                {
                    foreach (RapportHebdoSubNode<PointageCell> personnelNode in ciNode.SubNodeList)
                    {
                        personnelIdList.Add(personnelNode.NodeId);
                    }
                }
            }
            else
            {
                foreach (RapportHebdoNode<PointageCell> personnelNode in pointagePanelViewModel)
                {
                    personnelIdList.Add(personnelNode.NodeId);
                }
            }
            return personnelIdList.Distinct().ToList();
        }

        /// <summary>
        /// Remplir la liste des erreurs et des avertissements sur les pointages des ouvriers
        /// </summary>
        /// <param name="validationReponseViewModel">Résultat de validation d'un rapport hebdomadaire</param>
        /// <param name="personnel">Le personnel</param>
        /// <param name="datePointage">La date de pointage</param>
        private void FulfillPersonnelPointageErrorsAndWarnings(RapportHebdoValidationReponseViewModel validationReponseViewModel, PersonnelEnt personnel, DateTime datePointage)
        {
            double totalHoursWithoutMajorations = Repository.GetTotalHoursWithoutMajorations(personnel.PersonnelId, datePointage);
            double allTotalHours = Repository.GetTotalHoursWorkAndAbsence(personnel.PersonnelId, datePointage);

            if (allTotalHours < 7 && datePointage.DayOfWeek != DayOfWeek.Saturday && datePointage.DayOfWeek != DayOfWeek.Sunday)
            {
                if (!validationReponseViewModel.PersonnelWarningList.ContainsKey(personnel.PersonnelId))
                {
                    validationReponseViewModel.PersonnelWarningList.Add(personnel.PersonnelId, new List<string> { });
                }

                string warnningMsg = string.Format(FeatureRapportHebdo.Rapport_Hebdo_LessThanSevenHoursPerDay_WarningMessage, datePointage.ToShortDateString(), personnel.Nom, personnel.Prenom);
                validationReponseViewModel.PersonnelWarningList[personnel.PersonnelId].Add(warnningMsg);
            }
            else if (allTotalHours > 7 && allTotalHours <= 10)
            {
                if (!validationReponseViewModel.PersonnelWarningList.ContainsKey(personnel.PersonnelId))
                {
                    validationReponseViewModel.PersonnelWarningList.Add(personnel.PersonnelId, new List<string> { });
                }

                string warnningMsg = string.Format(FeatureRapportHebdo.Rapport_Hebdo_MoreThanSevenHoursPerDay_WarningMessage, personnel.Nom, personnel.Prenom, datePointage.ToShortDateString());
                validationReponseViewModel.PersonnelWarningList[personnel.PersonnelId].Add(warnningMsg);
            }
            else if (totalHoursWithoutMajorations > 10)
            {
                if (!validationReponseViewModel.PersonnelErrorList.ContainsKey(personnel.PersonnelId))
                {
                    validationReponseViewModel.PersonnelErrorList.Add(personnel.PersonnelId, new List<string> { });
                }
                string errorMsg = string.Format(FeatureRapportHebdo.Rapport_Hebdo_MoreThanTenHoursPerDay_ErrorMessage, personnel.Nom, personnel.Prenom, datePointage.ToShortDateString());
                validationReponseViewModel.PersonnelErrorList[personnel.PersonnelId].Add(errorMsg);
            }
        }

        /// <summary>
        /// Récupérer le statut d'un personnel
        /// </summary>
        /// <param name="statutId">L'identifiant du statut</param>
        /// <returns>Le statut</returns>
        private string GetPersonnelStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return PersonnelStatutValue.Ouvrier;
                case "2":
                    return PersonnelStatutValue.ETAM;
                case "3":
                    return PersonnelStatutValue.Cadre;
                case "4":
                    return PersonnelStatutValue.ETAM;
                case "5":
                    return PersonnelStatutValue.ETAM;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get nombre jours pointées
        /// </summary>
        /// <param name="rapportLignes">List des rapports lignes</param>
        /// <returns>nombre des jours pointées</returns>
        private double GetNbreJoursPointeSyntheMensuelle(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            double somme = 0;
            var rapportLignesByDatePointage = rapportLignes.GroupBy(x => x.DatePointage);
            foreach (var groupRapportLigne in rapportLignesByDatePointage)
            {
                bool haveMajoration = groupRapportLigne.Any(x => x.ListRapportLigneMajorations.Any(y => y.CodeMajoration.IsHeureNuit && y.HeureMajoration > 0));
                if (groupRapportLigne.Any(x => x.HeureNormale > 0) || haveMajoration)
                {
                    somme++;
                }
            }

            return somme;
        }

        /// <summary>
        /// Get nombre des jours d'absence
        /// </summary>
        /// <param name="rapportLignes">List des rapports lignes</param>
        /// <returns>nombre des jours d'absence</returns>
        private double GetNbreJoursAbsenceSyntheseMensuelle(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            if (!rapportLignes.IsNullOrEmpty())
            {
                double somme = 0;
                var rapportLignesByDatePointage = rapportLignes.GroupBy(x => x.DatePointage);
                foreach (var groupRapportLigne in rapportLignesByDatePointage)
                {
                    if (groupRapportLigne.Any(x => x.HeureAbsence > 0))
                    {
                        somme++;
                    }
                }

                return somme;
            }

            return 0;
        }

        /// <summary>
        /// Get nombre des jours d'absence
        /// </summary>
        /// <param name="rapportLignes">List des rapports lignes</param>
        /// <returns>nombre des jours d'absence</returns>
        private int GetNbrePrimesSyntheseMensuelle(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            int somme = 0;
            foreach (RapportLigneEnt rapportLigne in rapportLignes)
            {
                if (!rapportLigne.ListRapportLignePrimes.IsNullOrEmpty() && rapportLigne.DateSuppression == null)
                {
                    foreach (RapportLignePrimeEnt lignePrime in rapportLigne.ListRapportLignePrimes)
                    {
                        if (lignePrime.IsChecked)
                        {
                            somme++;
                        }
                    }
                }
            }

            return somme;
        }

        /// <summary>
        /// Get pointage statut for synthese mensuelle
        /// </summary>
        /// <param name="rapportLignes">List des rapports ligne</param>
        /// <returns>Pointage statut</returns>
        private string GetRapportStatut(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            rapportLignes = rapportLignes.Where(r => r.RapportLigneId > 0);
            if (rapportLignes.Any())
            {
                bool isStatutVerrou = rapportLignes.All(x => x.RapportLigneStatutId.HasValue && x.DateSuppression == null && x.RapportLigneStatutId == RapportStatutEnt.RapportStatutVerrouille.Key);
                if (isStatutVerrou)
                {
                    return RapportStatutEnt.RapportStatutVerrouille.Value;
                }

                bool isStatutV2 = rapportLignes.All(x => x.RapportLigneStatutId.HasValue && x.DateSuppression == null && x.RapportLigneStatutId == RapportStatutEnt.RapportStatutValide2.Key);
                if (isStatutV2)
                {
                    return RapportStatutEnt.RapportStatutValide2.Value;
                }
            }

            return RapportStatutEnt.RapportStatutEnCours.Value;
        }

        /// <summary>
        /// Update Rapport statut validation en se basant sur les lignes de rapport
        /// </summary>
        /// <param name="rapportList">Liste des rapports</param>
        /// /// <param name="isStatutVerrouille">indique si le rapport est verrouillé déja </param>
        private void UpdateRapportsStatutDependingOnRapportLignes(IEnumerable<RapportEnt> rapportList, bool isStatutVerrouille)
        {
            if (rapportList?.Any() == true && !isStatutVerrouille)
            {
                RapportStatutEnt statut;
                foreach (var rapport in rapportList)
                {
                    string rapportStatut = GetRapportStatut(rapport.ListLignes);
                    statut = Repository.GetRapportStatutByCode(rapportStatut);
                    rapport.RapportStatutId = statut.RapportStatutId;
                    rapport.AuteurModificationId = userManager.GetContextUtilisateurId();
                    rapport.DateModification = DateTime.UtcNow;

                    Repository.Update(rapport);
                }
                Save();
            }
        }

        /// <summary>
        /// Get ci list of the current user
        /// </summary>
        /// <param name="personnelId">Personnel id</param>
        /// <returns>List des Ci dont l'utilisateur actuel est responsable</returns>
        private List<CIEnt> GetCiListOfResponsable(int personnelId)
        {
            IEnumerable<int> ciListOfResponsable = userManager.GetCiListOfResponsable()?.Select(c => c.CiId);
            if (ciListOfResponsable.IsNullOrEmpty())
            {
                return new List<CIEnt>();
            }

            List<CIEnt> actifAffectationList = affectationManager.GetPersonnelActifAffectationCiList(personnelId).ToList();
            return actifAffectationList?.FindAll(a => ciListOfResponsable.Contains(a.CiId)) ?? new List<CIEnt>();
        }

        /// <summary>
        /// Vérifier et valider le rapport hebdomadaire pour les ouvriers
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model de Rapport hebdomadaire pour enregistrer</param>
        /// <returns>Résultat de validation d'un rapport hebdomadaire</returns>
        private RapportHebdoValidationReponseViewModel CheckAndValidateRapportHebdoForWorkers(RapportHebdoSaveViewModel rapportHebdoSaveViewModel)
        {
            RapportHebdoValidationReponseViewModel validationReponseViewModel = new RapportHebdoValidationReponseViewModel
            {
                PersonnelErrorList = new Dictionary<int, List<string>>(),
                PersonnelWarningList = new Dictionary<int, List<string>>(),
                DailyRapportErrorList = new Dictionary<int, List<string>>()
            };
            List<int> personnelIdList = GetPersonnelsIdFromRapportHebdoNodes(rapportHebdoSaveViewModel.PointagePanelViewModel);

            foreach (int personnelId in personnelIdList)
            {
                PersonnelEnt personnel = personnelManager.GetPersonnel(personnelId);
                for (int i = 0; i < 7; i++)
                {
                    DateTime datePointage = rapportHebdoSaveViewModel.MondayDate.AddDays(i);
                    FulfillPersonnelPointageErrorsAndWarnings(validationReponseViewModel, personnel, datePointage);
                }
            }

            if (!validationReponseViewModel.PersonnelErrorList.Any())
            {
                ValidatePersonnelPointageBetweenTwoDates(
                    rapportHebdoSaveViewModel.MondayDate,
                    rapportHebdoSaveViewModel.MondayDate.AddDays(6),
                    personnelIdList,
                    RapportStatutEnt.RapportStatutValide2.Value);
                validationReponseViewModel.IsValidated = true;
            }
            return validationReponseViewModel;
        }

        /// <summary>
        /// Vérifier et valider le rapport hebdomadaire pour les ETAM \ IAC
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model de Rapport hebdomadaire pour enregistrer</param>
        /// <returns>Résultat de validation d'un rapport hebdomadaire</returns>
        private RapportHebdoValidationReponseViewModel CheckAndValidateRapportHebdoForEtamIac(RapportHebdoSaveViewModel rapportHebdoSaveViewModel)
        {
            RapportHebdoValidationReponseViewModel validationReponseViewModel = new RapportHebdoValidationReponseViewModel
            {
                PersonnelErrorList = new Dictionary<int, List<string>>(),
                PersonnelWarningList = new Dictionary<int, List<string>>(),
                DailyRapportErrorList = new Dictionary<int, List<string>>()
            };
            List<int> personnelIdList = GetPersonnelsIdFromRapportHebdoNodes(rapportHebdoSaveViewModel.PointagePanelViewModel);

            foreach (int personnelId in personnelIdList)
            {
                PersonnelEnt personnel = personnelManager.GetPersonnel(personnelId);
                for (int i = 0; i < 7; i++)
                {
                    DateTime datePointage = rapportHebdoSaveViewModel.MondayDate.AddDays(i);
                    FulfillEtamIacPointageErrorsAndWarnings(validationReponseViewModel, personnel, datePointage);
                }
            }

            if (!validationReponseViewModel.PersonnelErrorList.Any())
            {
                ValidatePersonnelPointageBetweenTwoDates(
                    rapportHebdoSaveViewModel.MondayDate,
                    rapportHebdoSaveViewModel.MondayDate.AddDays(6),
                    personnelIdList,
                    RapportStatutEnt.RapportStatutValide2.Value);
                validationReponseViewModel.IsValidated = true;
            }

            return validationReponseViewModel;
        }

        /// <summary>
        /// Remplir la liste des erreurs et des avertissements sur les pointages des ETAM \ IAC
        /// </summary>
        /// <param name="validationReponseViewModel">Résultat de validation d'un rapport hebdomadaire</param>
        /// <param name="personnel">Le personnel</param>
        /// <param name="datePointage">La date de pointage</param>
        private void FulfillEtamIacPointageErrorsAndWarnings(RapportHebdoValidationReponseViewModel validationReponseViewModel, PersonnelEnt personnel, DateTime datePointage)
        {
            double allTotalHours = Repository.GetTotalHoursWorkAndAbsence(personnel.PersonnelId, datePointage);

            if (allTotalHours < 7 && datePointage.DayOfWeek != DayOfWeek.Saturday && datePointage.DayOfWeek != DayOfWeek.Sunday)
            {
                if (!validationReponseViewModel.PersonnelWarningList.ContainsKey(personnel.PersonnelId))
                {
                    validationReponseViewModel.PersonnelWarningList.Add(personnel.PersonnelId, new List<string> { });
                }

                string warnningMsg = string.Format(FeatureRapportHebdo.Rapport_Hebdo_LessThanSevenHoursPerDay_WarningMessage, datePointage.ToShortDateString(), personnel.Nom, personnel.Prenom);
                validationReponseViewModel.PersonnelWarningList[personnel.PersonnelId].Add(warnningMsg);
            }
        }

        /// <summary>
        /// Valider les pointages entre deux dates données
        /// </summary>
        /// <param name="dateDebut">Date début de la période</param>
        /// <param name="dateFin">Date fin de la période</param>
        /// <param name="personnelIdList">La liste des personnels</param>
        /// <param name="statutCode">Le code de statut de validation</param>
        private void ValidatePersonnelPointageBetweenTwoDates(DateTime dateDebut, DateTime dateFin, IEnumerable<int> personnelIdList, string statutCode)
        {
            if (personnelIdList != null)
            {
                foreach (int personnelId in personnelIdList)
                {
                    IEnumerable<RapportLigneEnt> rapportLigneList = pointageRepository.GetEtamIacRapportsForValidation(personnelId, dateDebut, dateFin);
                    RapportStatutEnt rapportLigneStatut = Repository.GetRapportStatutByCode(statutCode);
                    if (rapportLigneList.IsNullOrEmpty())
                    {
                        continue;
                    }
                    ValidatePersonnelPointage(rapportLigneList, rapportLigneStatut, statutCode);
                }
            }
        }

        /// <summary>
        /// Valider les pointages
        /// </summary>
        /// <param name="rapportLigneList">La liste des personnels</param>
        /// <param name="rapportLigneStatut">statut du rapport ligne</param>
        /// <param name="statutCode">le code du statut</param>
        private void ValidatePersonnelPointage(IEnumerable<RapportLigneEnt> rapportLigneList, RapportStatutEnt rapportLigneStatut, string statutCode)
        {
            bool isStatutVerrouille = false;
            foreach (RapportLigneEnt rapportLigne in rapportLigneList)
            {
                if (rapportLigne.RapportLigneStatutId != RapportStatutEnt.RapportStatutVerrouille.Key)
                {
                    rapportLigne.RapportLigneStatutId = rapportLigneStatut.RapportStatutId;
                    if (statutCode != RapportStatutEnt.RapportStatutEnCours.Value && statutCode != RapportStatutEnt.RapportStatutVerrouille.Value)
                    {
                        rapportLigne.ValideurId = rapportLigne.AuteurModificationId;
                        rapportLigne.DateValidation = rapportLigne.DateModification;
                        if (rapportLigne.Rapport.RapportStatutId != RapportStatutEnt.RapportStatutVerrouille.Key
                            && rapportLigne.Rapport.RapportStatutId != rapportLigneStatut.RapportStatutId
                            && rapportLigne.Rapport.ListLignes.All(x => x.RapportLigneStatutId == rapportLigneStatut.RapportStatutId))
                        {
                            rapportLigne.Rapport.RapportStatutId = rapportLigneStatut.RapportStatutId;
                            rapportLigne.Rapport.DateValidationCDT = DateTime.UtcNow;
                            rapportLigne.Rapport.ValideurCDTId = rapportLigne.AuteurModificationId;
                        }
                    }

                    pointageRepository.Update(rapportLigne);
                }
                else
                {
                    isStatutVerrouille = true;
                }
            }

            Save();
            IEnumerable<RapportEnt> rapportList = rapportLigneList.Select(x => x.Rapport).Distinct().ToList();
            UpdateRapportsStatutDependingOnRapportLignes(rapportList, isStatutVerrouille);
        }

        /// <summary>
        /// Get rapport statut pour la synthése mensuelle
        /// </summary>
        /// <param name="rapportLignes">Rapport lignes</param>
        /// <param name="pointageMonth">Le mois du pointage</param>
        /// <returns>Pointage statut</returns>
        private string GetSyntheseMensuelleRapportStatut(IEnumerable<RapportLigneEnt> rapportLignes, DateTime pointageMonth)
        {
            int openDays = CalculateOpenDaysInMonth(pointageMonth);
            var rapportLignesByDatePointage = rapportLignes.GroupBy(x => x.DatePointage);
            if (openDays > rapportLignesByDatePointage.Count() || rapportLignes.Any(x => x.RapportLigneStatutId.HasValue && x.RapportLigneStatutId == RapportStatutEnt.RapportStatutEnCours.Key))
            {
                return RapportStatutEnt.RapportStatutEnCours.Value;
            }

            bool isStatutVr = rapportLignes.All(x => x.RapportLigneStatutId.HasValue && x.RapportLigneStatutId == RapportStatutEnt.RapportStatutVerrouille.Key);
            if (isStatutVr)
            {
                return RapportStatutEnt.RapportStatutVerrouille.Value;
            }

            bool isStatutValide2 = rapportLignes.All(x => x.RapportLigneStatutId.HasValue && (x.RapportLigneStatutId == RapportStatutEnt.RapportStatutValide2.Key || x.RapportLigneStatutId == RapportStatutEnt.RapportStatutVerrouille.Key));
            if (isStatutValide2)
            {
                return RapportStatutEnt.RapportStatutValide2.Value;
            }

            return RapportStatutEnt.RapportStatutEnCours.Value;
        }

        /// <summary>
        /// Calculer le nombre des jours ouverts d'un mois(sans weekends)
        /// </summary>
        /// <param name="date">Mois</param>
        /// <returns>Nombre des jours</returns>
        private int CalculateOpenDaysInMonth(DateTime date)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
                      .Select(day => new DateTime(date.Year, date.Month, day))
                      .Count(d => d.DayOfWeek != DayOfWeek.Saturday &&
                                  d.DayOfWeek != DayOfWeek.Sunday);
        }

        /// <summary>
        /// Recuperer statut du rapport ligne 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="datePointage">Date Pointage</param>
        /// <param name="rapportligneStatutId">Rapport ligne statut identifier</param>
        /// <returns>Rapport ligne statut code</returns>
        public string GetRapportLigneNodeStatut(int ciId, int personnelId, DateTime datePointage, int? rapportligneStatutId)
        {
            if (rapportligneStatutId.HasValue && rapportligneStatutId.Value > 0)
            {
                if (rapportligneStatutId.Value == StatutVerrouille)
                {
                    return RapportStatutEnt.RapportStatutVerrouille.Value;
                }
                if (rapportligneStatutId.Value == StatutValide)
                {
                    return RapportStatutEnt.RapportStatutValide2.Value;
                }

                return string.Empty;
            }

            return pointageRepository.GetRapportLigneStatutCode(personnelId, ciId, datePointage);
        }

        /// <summary>
        /// Gets Rapport for rapport hebdo Employee
        /// </summary>
        /// <param name="ciIds">Ci Identifier</param>
        /// <param name="personnelId">Personnel Identifier</param>
        /// <param name="dateDebut">Date debut</param>
        /// <returns>Dictionnaire</returns>
        private Dictionary<int, List<RapportEnt>> GetCiRapportsByWeekForEmployee(IEnumerable<int> ciIds, int personnelId, DateTime dateDebut, string statut)
        {
            var dateFin = dateDebut.AddDays(6);
            var groupes = Repository.GetCiRapportHebdomadaireEmployee(ciIds, personnelId, dateDebut, dateFin);
            return rapportHebdoService.HandleHebdoRapportsForEmployee(groupes, ciIds, dateDebut, dateFin, statut);
        }

        private int GetDayOfWeek(DateTime date)
        {
            return (int)date.DayOfWeek > 0 ? (int)date.DayOfWeek - 1 : 6;
        }

        private SyntheseValidationAffairesModel HandleCreateSyntheseValidationAffairesModel(CIEnt ci, IEnumerable<RapportLigneEnt> rapportLigneList, IEnumerable<AffectationEnt> ouvrierAffectationList, List<RapportStatutEnt> rapportStatutList)
        {
            return new SyntheseValidationAffairesModel
            {
                CiId = ci.CiId,
                AffectedPersonnelsIds = ouvrierAffectationList.Where(x => x.CiId == ci.CiId).Select(x => x.PersonnelId).ToList(),
                CodeSociete = ci?.Societe?.Code,
                CodeCi_Libelle = ci.Code + " - " + ci.Libelle,
                CodeEtabComptable = ci?.EtablissementComptable?.Code,
                TotalHeure = GetNbreHeurePointeSyntheseValidation(rapportLigneList),
                TotalHeureAbsence = rapportLigneList.Any() ? rapportLigneList.Sum(x => x.HeureAbsence) : 0.0,
                TotalHeureAstreinte = GetNbreHeureAstreinteSyntheseValidation(rapportLigneList),
                TotalHeureSup = GetNbreHeuresSupSyntheseValidation(rapportLigneList),
                TotalNbrePrime = GetNbrePrimesSyntheseMensuelle(rapportLigneList),
                Statut = rapportStatutList.FirstOrDefault(s => s.Code == GetRapportStatut(rapportLigneList))?.Libelle
            };
        }

        private double GetNbreHeurePointeSyntheseValidation(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            double somme = 0.0;
            if (rapportLignes != null && rapportLignes.Any())
            {
                bool haveMajoration = rapportLignes.Any(x => x.ListRapportLigneMajorations.Any(y => y.CodeMajoration.IsHeureNuit && y.HeureMajoration > 0));
                if (rapportLignes.Any(x => x.HeureNormale > 0) || haveMajoration)
                {
                    somme = rapportLignes.Sum(x => x.ListRapportLigneTaches.Sum(y => y.HeureTache)) + rapportLignes.Sum(x => x.ListRapportLigneMajorations.Where(y => y.CodeMajoration.IsHeureNuit).Sum(h => h.HeureMajoration));
                }
            }

            return somme;
        }

        private double GetNbreHeuresSupSyntheseValidation(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            double somme = 0.0;
            if (rapportLignes != null && rapportLignes.Any())
            {
                List<IGrouping<int?, RapportLigneEnt>> pointageGroupByPersonnel = rapportLignes.GroupBy(x => x.PersonnelId).ToList();
                foreach (IGrouping<int?, RapportLigneEnt> pointagePersonnel in pointageGroupByPersonnel)
                {
                    double? total = pointagePersonnel.Sum(s => s.ListRapportLigneTaches?.Sum(t => t.HeureTache) + s.HeureAbsence +
                                                        (s.ListRapportLigneMajorations.Any() ? s.ListRapportLigneMajorations.Where(m => m.CodeMajoration.IsHeureNuit).Sum(m => m.HeureMajoration) : 0))
                                                            - Constantes.MaxHeurePoinatageFES;
                    if (total.HasValue && total.Value > 0)
                    {
                        somme += total.Value;
                    }
                }
            }

            return somme > 0 ? somme : 0.0;
        }

        private double GetNbreHeureAstreinteSyntheseValidation(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            double somme = 0.0;
            if (rapportLignes != null && rapportLignes.Any())
            {
                foreach (RapportLigneEnt rapportLigne in rapportLignes)
                {
                    foreach (RapportLigneAstreinteEnt rapporLigneAstreinte in rapportLigne.ListRapportLigneAstreintes)
                    {
                        somme += (rapporLigneAstreinte.DateFinAstreinte - rapporLigneAstreinte.DateDebutAstreinte).TotalHours;
                    }
                }
            }

            return somme;
        }
    }
}
