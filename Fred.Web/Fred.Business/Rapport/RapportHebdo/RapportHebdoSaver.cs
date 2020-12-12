using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.DatesClotureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;
using MoreLinq;

namespace Fred.Business.Rapport.RapportHebdo
{
    public class RapportHebdoSaver : ManagersAccess
    {
        private readonly IMapper mapper;
        private readonly PersonnelsRapports personnelsRapports;
        private readonly IRapportRepository rapportRepository;
        private readonly IRapportLigneTacheRepository rapportLigneTacheRepository;
        private readonly IRapportLigneAstreinteRepository rapportLigneAstreinteRepository;
        private readonly IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository;
        private readonly IPointageRepository pointageRepository;
        private readonly IRapportLigneMajorationRepository rapportLigneMajorationRepository;
        private readonly IRapportLignePrimeRepository rapportLignePrimeRepository;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly IPrimeManager primeManager;
        private readonly ICodeDeplacementManager codeDeplacementManager;
        private readonly IRapportHebdoService rapportHebdoService;
        private readonly IUnitOfWork unitOfWork;

        private readonly Lazy<int> rapportStatutECId;
        private List<ClotureComptable> clotureComptables;
        private Dictionary<int, List<RapportEnt>> lazyCiRapportsByWeeks;
        private RapportHebdoSaveViewModel datas;

        public RapportHebdoSaver(
            IMapper mapper,
            IRapportRepository rapportRepository,
            IRapportLigneTacheRepository rapportLigneTacheRepository,
            IRapportLigneAstreinteRepository rapportLigneAstreinteRepository,
            IRapportLigneCodeAstreinteRepository rapportLigneCodeAstreinteRepository,
            IPointageRepository pointageRepository,
            IRapportLigneMajorationRepository rapportLigneMajorationRepository,
            IRapportLignePrimeRepository rapportLignePrimeRepository,
            IFeatureFlippingManager featureFlippingManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            ICodeMajorationManager codeMajorationManager,
            ICodeZoneDeplacementManager codeZoneDeplacementManager,
            IPrimeManager primeManager,
            ICodeDeplacementManager codeDeplacementManager,
            IRapportHebdoService rapportHebdoService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.rapportRepository = rapportRepository;
            this.rapportLigneTacheRepository = rapportLigneTacheRepository;
            this.rapportLigneAstreinteRepository = rapportLigneAstreinteRepository;
            this.rapportLigneCodeAstreinteRepository = rapportLigneCodeAstreinteRepository;
            this.pointageRepository = pointageRepository;
            this.rapportLigneMajorationRepository = rapportLigneMajorationRepository;
            this.rapportLignePrimeRepository = rapportLignePrimeRepository;
            this.featureFlippingManager = featureFlippingManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.codeMajorationManager = codeMajorationManager;
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.primeManager = primeManager;
            this.codeDeplacementManager = codeDeplacementManager;
            this.rapportHebdoService = rapportHebdoService;
            this.unitOfWork = unitOfWork;

            lazyCiRapportsByWeeks = new Dictionary<int, List<RapportEnt>>();
            personnelsRapports = new PersonnelsRapports();
            Results = new RapportHebdoSaveResultModel();
            rapportStatutECId = new Lazy<int>(() =>
            {
                return rapportRepository.GetRapportStatutByCode(RapportStatutEnt.RapportStatutEnCours.Value).RapportStatutId;
            });
        }

        #region Propriétés

        /// <summary>
        /// Le résultat de l'enregistrement.
        /// </summary>
        public RapportHebdoSaveResultModel Results { get; private set; }

        #endregion
        #region Fonctions publiques

        /// <summary>
        /// Enregistre.
        /// </summary>
        /// <remarks>En cas d'erreur, les données ne seront pas enregistrées.</remarks>
        public void Save(RapportHebdoSaveViewModel datas)
        {
            this.datas = datas;

            Results = new RapportHebdoSaveResultModel();
            if (datas != null)
            {
                ProcessUsedRapports();
                ProcessAstreintes();
                ProcessPointages();
                ProcessMajorations();
                ProcessPrimes();

                var validator = new RapportHebdoValidator(featureFlippingManager, codeMajorationManager, primeManager);
                if (validator.Validate(personnelsRapports))
                {
                    var deplacement = new RapportHebdoDeplacement(codeMajorationManager, codeZoneDeplacementManager, primeManager, codeDeplacementManager);
                    deplacement.UpdateCodeEtZone(personnelsRapports, false);
                    Results.Warnings = deplacement.Warnings;

                    var utilisateurId = Managers.Utilisateur.GetContextUtilisateurId();
                    SaveChanges(utilisateurId);
                }
                else
                {
                    foreach (var errorMessage in validator.GetErrorMessages())
                    {
                        Results.Errors.Add(errorMessage);
                    }
                }
            }
        }

        #endregion
        #region Astreintes

        /// <summary>
        /// Enregistrer la panel des astreintes
        /// </summary>
        private void ProcessAstreintes()
        {
            var astreintes = datas.AstreintePanelViewModel;
            if (!astreintes.IsNullOrEmpty())
            {
                if (IsCiNode(astreintes.FirstOrDefault().NodeType))
                {
                    foreach (var ciNode in astreintes)
                    {
                        ProcessCiNodeAstreinte(ciNode);
                    }
                }
                else
                {
                    foreach (var personnelNode in astreintes)
                    {
                        ProcessPersonnelNodeAstreinte(personnelNode);
                    }
                }
            }
        }

        /// <summary>
        /// Enregister un noeud CI
        /// </summary>
        /// <param name="ciNode">Le noeud CI</param>
        private void ProcessCiNodeAstreinte(RapportHebdoNode<AstreintePointageHebdoCell> ciNode)
        {
            var rapports = GetCiRapportsByWeek(ciNode.NodeId);
            foreach (var personnelNode in ciNode.SubNodeList)
            {
                personnelsRapports.Add(personnelNode.NodeId, rapports);
                rapportHebdoService.AddPersonnelPointageToAllRapports(rapports, personnelNode.NodeId);
                AddOrUpdateSortiesAstreinteInRapportLignes(rapports, personnelNode.NodeId, personnelNode.SubNodeList);
            }
        }

        /// <summary>
        /// Enregister un noeud personnel
        /// </summary>
        /// <param name="personnelNode">Le noeud personnel</param>
        private void ProcessPersonnelNodeAstreinte(RapportHebdoNode<AstreintePointageHebdoCell> personnelNode)
        {
            foreach (var ciNode in personnelNode.SubNodeList)
            {
                var rapports = GetCiRapportsByWeek(ciNode.NodeId);
                personnelsRapports.Add(personnelNode.NodeId, rapports);
                rapportHebdoService.AddPersonnelPointageToAllRapports(rapports, personnelNode.NodeId);
                AddOrUpdateSortiesAstreinteInRapportLignes(rapports, personnelNode.NodeId, ciNode.SubNodeList);
            }
        }

        /// <summary>
        /// Ajouter ou méttre à jours des sorties astreintes
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreintesLignes">La liste des noeuds qui contient les sorties astreintes</param>
        private void AddOrUpdateSortiesAstreinteInRapportLignes(List<RapportEnt> rapports, int personnelId, List<RapportHebdoSubNode<AstreintePointageHebdoCell>> astreintesLignes)
        {
            foreach (var astreintesLigne in astreintesLignes)
            {
                foreach (var astreinteCell in astreintesLigne.Items)
                {
                    if (astreinteCell.HasAstreinte)
                    {
                        AddOrUpdateSortiesAstreinte(rapports, personnelId, astreinteCell);
                    }
                }
            }
            DeleteSortiesAstreintes(rapports, personnelId, astreintesLignes);
        }

        /// <summary>
        /// Ajouter ou méttre à jours des sorties astreintes
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreinteCell">La cellule qui contient une sortie astreintes</param>
        private void AddOrUpdateSortiesAstreinte(List<RapportEnt> rapports, int personnelId, AstreintePointageHebdoCell astreinteCell)
        {
            var rapport = rapports.FirstOrDefault(r => r.DateChantier == astreinteCell.Date);
            var rapportLigne = rapport?.ListLignes?.FirstOrDefault(l => l.PersonnelId == personnelId && l.DateSuppression == null);

            foreach (var astreinteModel in astreinteCell.ListRapportLigneAstreintes)
            {
                var astreinteDb = rapportLigne?.ListRapportLigneAstreintes?.FirstOrDefault(a => a.RapportLigneAstreinteId == astreinteModel.RapportLigneAstreinteId && astreinteModel.RapportLigneAstreinteId != 0);
                if (astreinteDb != null)
                {
                    astreinteDb.DateDebutAstreinte = astreinteModel.DateDebutAstreinte;
                    astreinteDb.DateFinAstreinte = astreinteModel.DateFinAstreinte;
                }
                else
                {
                    rapportLigne?.ListRapportLigneAstreintes.Add(new RapportLigneAstreinteEnt
                    {
                        AstreinteId = astreinteModel.AstreinteId,
                        DateDebutAstreinte = astreinteModel.DateDebutAstreinte,
                        DateFinAstreinte = astreinteModel.DateFinAstreinte,
                        RapportLigneId = rapportLigne.RapportLigneId
                    });
                }
            }
        }

        /// <summary>
        /// Supprimer des sorties astreintes
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreintesLignes">La liste des noeuds qui contient les sorties astreintes</param>
        private void DeleteSortiesAstreintes(List<RapportEnt> rapports, int personnelId, List<RapportHebdoSubNode<AstreintePointageHebdoCell>> astreintesLignes)
        {
            if (astreintesLignes.Count == 0)
            {
                DeleteAllAstreinteForWeek(rapports, personnelId);
            }
            else
            {
                for (var j = 0; j < 7; j++)
                {
                    if (astreintesLignes[0].Items.ToList()[j].HasAstreinte)
                    {
                        DeleteAstreinte(rapports, personnelId, astreintesLignes, j);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer des sorties astreintes pour une journée
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="astreintesLignes">La liste des noeuds qui contient les sorties astreintes</param>
        /// <param name="indexJour">Index de la journée</param>
        private void DeleteAstreinte(List<RapportEnt> rapports, int personnelId, List<RapportHebdoSubNode<AstreintePointageHebdoCell>> astreintesLignes, int indexJour)
        {
            var listAstreinteByDay = new List<int>();
            for (var i = 0; i < astreintesLignes.Count; i++)
            {
                if (astreintesLignes[i].Items.ToList()[indexJour].ListRapportLigneAstreintes.Count > 0 && astreintesLignes[i].Items.ToList()[indexJour].ListRapportLigneAstreintes[0].RapportLigneAstreinteId != 0)
                {
                    listAstreinteByDay.Add(astreintesLignes[i].Items.ToList()[indexJour].ListRapportLigneAstreintes[0].RapportLigneAstreinteId);
                }
            }
            var rapport = rapports.FirstOrDefault(r => r.DateChantier == astreintesLignes[0].Items.ToList()[indexJour].Date);
            var rapportLigne = rapport?.ListLignes?.FirstOrDefault(l => l.PersonnelId == personnelId && l.DateSuppression == null);
            if (rapportLigne != null)
            {
                foreach (var astreinteDb in rapportLigne.ListRapportLigneAstreintes.Where(a => a.RapportLigneAstreinteId != 0).ToList())
                {
                    if (!listAstreinteByDay.Contains(astreinteDb.RapportLigneAstreinteId))
                    {
                        rapportLigneCodeAstreinteRepository.DeletePrimesAstreinteByLigneAstreinteId(astreinteDb.RapportLigneAstreinteId);
                        rapportLigne.ListRapportLigneAstreintes.Remove(astreinteDb);
                        rapportLigneAstreinteRepository.DeleteById(astreinteDb.RapportLigneAstreinteId);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer des sorties astreintes pour une semaine
        /// </summary>
        /// <param name="rapports">La liste des rapports</param>
        /// <param name="personnelId">L'identifiant du personnel</param>
        private void DeleteAllAstreinteForWeek(List<RapportEnt> rapports, int personnelId)
        {
            for (var i = 0; i < 7; i++)
            {
                var date = datas.MondayDate.AddDays(i);
                var rapport = rapports.FirstOrDefault(r => r.DateChantier == date);
                var rapportLigne = rapport?.ListLignes?.FirstOrDefault(l => l.PersonnelId == personnelId && l.DateSuppression == null);
                if (rapportLigne != null)
                {
                    foreach (var astreinteDb in rapportLigne.ListRapportLigneAstreintes.Where(a => a.RapportLigneAstreinteId != 0).ToList())
                    {
                        rapportLigneCodeAstreinteRepository.DeletePrimesAstreinteByLigneAstreinteId(astreinteDb.RapportLigneAstreinteId);
                        rapportLigne.ListRapportLigneAstreintes.Remove(astreinteDb);
                        rapportLigneAstreinteRepository.DeleteById(astreinteDb.RapportLigneAstreinteId);
                    }
                }
            }
        }

        #endregion
        #region Pointages

        /// <summary>
        /// Save rapport hebdo pointage panel
        /// </summary>
        private void ProcessPointages()
        {
            var pointages = datas.PointagePanelViewModel;
            if (!pointages.IsNullOrEmpty())
            {
                foreach (var pointage in pointages)
                {
                    if (IsCiNode(pointage.NodeType))
                    {
                        ProcessCiNodePointage(pointage);
                    }
                    else
                    {
                        ProcessPersonnelNodePointage(pointage);
                    }
                }
            }
        }

        /// <summary>
        /// Enregister un noeud CI
        /// </summary>
        /// <param name="ciNode">Le noeud CI</param>
        private void ProcessCiNodePointage(RapportHebdoNode<PointageCell> ciNode)
        {
            var rapports = GetCiRapportsByWeek(ciNode.NodeId);
            foreach (var personnelNode in ciNode.SubNodeList)
            {
                personnelsRapports.Add(personnelNode.NodeId, rapports);
                rapportHebdoService.AddPersonnelPointageToAllRapports(rapports, personnelNode.NodeId);
                AddOrUpdateTacheAbsenceRapportLignes(rapports, personnelNode.NodeId, personnelNode.SubNodeList);
            }
        }

        /// <summary>
        /// Enregister un noeud personnel
        /// </summary>
        /// <param name="personnelNode">Le noeud personnel</param>
        private void ProcessPersonnelNodePointage(RapportHebdoNode<PointageCell> personnelNode)
        {
            if (personnelNode.SubNodeList.Any())
            {
                foreach (var ciNode in personnelNode.SubNodeList)
                {
                    var rapports = GetCiRapportsByWeek(ciNode.NodeId);
                    personnelsRapports.Add(personnelNode.NodeId, rapports);
                    rapportHebdoService.AddPersonnelPointageToAllRapports(rapports, personnelNode.NodeId);
                    AddOrUpdateTacheAbsenceRapportLignes(rapports, personnelNode.NodeId, ciNode.SubNodeList);
                }
            }
        }

        /// <summary>
        /// Add or update tache absence rapport ligne
        /// </summary>
        /// <param name="rapports">Rapport list</param>
        /// <param name="personnelId">Personnel id</param>
        /// <param name="pointageLignes">Pointage lignes</param>
        private void AddOrUpdateTacheAbsenceRapportLignes(List<RapportEnt> rapports, int personnelId, List<RapportHebdoSubNode<PointageCell>> pointageLignes)
        {
            if (rapports.IsNullOrEmpty() || pointageLignes.IsNullOrEmpty())
            {
                return;
            }

            foreach (var node in pointageLignes)
            {
                foreach (var pointageCell in node.Items)
                {
                    var rapport = rapports.FirstOrDefault(r => r.DateChantier == pointageCell.Date);
                    if (rapport != null)
                    {
                        var rapportLigne = rapport.ListLignes.FirstOrDefault(l => l.PersonnelId == personnelId && l.DateSuppression == null);
                        if (pointageCell.isAbsenceCell)
                        {
                            rapportLigne.Commentaire = pointageCell.Commentaire;
                        }

                        AddUpdatePointageTask(rapportLigne, pointageCell, node);
                    }
                }
            }
        }

        /// <summary>
        /// Add or update pointage task
        /// </summary>
        /// <param name="rapportLigne">Rapport ligne</param>
        /// <param name="pointageCell">Pointage cell</param>
        /// <param name="node">Rapport hebdo subnode</param>
        private void AddUpdatePointageTask(RapportLigneEnt rapportLigne, PointageCell pointageCell, RapportHebdoSubNode<PointageCell> node)
        {
            if (rapportLigne != null)
            {
                if (pointageCell.isAbsenceCell)
                {
                    if (!rapportLigne.IsAllReadyAddedInRapportHebdo)
                    {
                        rapportLigne.CodeAbsenceId = pointageCell.CodeAbsenceId;
                        rapportLigne.HeureAbsence = pointageCell.TotalHours.GetValueOrDefault();
                        if (rapportLigne.CodeAbsenceId.HasValue && rapportLigne.HeureAbsence != 0)
                        {
                            rapportLigne.IsAllReadyAddedInRapportHebdo = true;
                        }
                    }
                }
                else
                {
                    var rapportLigneTache = rapportLigne.ListRapportLigneTaches?.FirstOrDefault(n => n.TacheId == node.NodeId);
                    if (rapportLigneTache != null)
                    {
                        rapportLigneTache.HeureTache = pointageCell.TotalHours.GetValueOrDefault();
                    }
                    else
                    {
                        rapportLigne.ListRapportLigneTaches.Add(new RapportLigneTacheEnt
                        {
                            TacheId = node.NodeId,
                            HeureTache = pointageCell.TotalHours.GetValueOrDefault(),
                            IsDeleted = false,
                            RapportLigneId = rapportLigne.RapportLigneId
                        });
                    }
                }
            }
        }

        #endregion
        #region Majorations

        /// <summary>
        /// Save Majoration rapport hebdo
        /// </summary>
        private void ProcessMajorations()
        {
            if (!datas.MajorationPanelViewModel.IsNullOrEmpty())
            {
                var majorationsModel = mapper.Map<List<MajorationPersonnelCiEnt>>(datas.MajorationPanelViewModel);
                foreach (var majorationModel in majorationsModel)
                {
                    var dateChantier = GetDatePointage(majorationModel.DayOfWeek);
                    var rapport = GetCiRapportsByWeek(majorationModel.CiId).FirstOrDefault(r => r.DateChantier == dateChantier);
                    if (rapport != null)
                    {
                        personnelsRapports.Add(majorationModel.PersonnelId, rapport);
                        rapportHebdoService.AddPersonnelPointageToAllRapports(new List<RapportEnt>() { rapport }, majorationModel.PersonnelId);
                        var rapportLigne = rapport.ListLignes.FirstOrDefault(l => l.PersonnelId == majorationModel.PersonnelId && l.DateSuppression == null);

                        var majorationEnt = rapportLigne.ListRapportLigneMajorations.FirstOrDefault(m => m.CodeMajorationId == majorationModel.MajorationCodeId);
                        if (majorationEnt == null)
                        {
                            rapportLigne.ListRapportLigneMajorations.Add(new RapportLigneMajorationEnt
                            {
                                CodeMajorationId = majorationModel.MajorationCodeId,
                                HeureMajoration = majorationModel.HeureMajoration
                            });
                        }
                        else
                        {
                            majorationEnt.HeureMajoration = majorationModel.HeureMajoration;
                        }
                    }
                }
            }
        }

        #endregion
        #region Primes

        /// <summary>
        /// Save prime rapport hebdo
        /// </summary>
        private void ProcessPrimes()
        {
            if (!datas.PrimePanelViewModel.IsNullOrEmpty())
            {
                var primesModel = mapper.Map<List<PrimeRapportHebdoEnt>>(datas.PrimePanelViewModel);
                foreach (var primeModel in primesModel)
                {
                    var dateChantier = GetDatePointage(primeModel.DayOfWeek);
                    var rapport = GetCiRapportsByWeek(primeModel.CiId).FirstOrDefault(r => r.DateChantier == dateChantier);
                    if (rapport != null)
                    {
                        personnelsRapports.Add(primeModel.PersonnelId, rapport);
                        rapportHebdoService.AddPersonnelPointageToAllRapports(new List<RapportEnt>() { rapport }, primeModel.PersonnelId);
                        var rapportLigne = rapport.ListLignes.FirstOrDefault(l => l.PersonnelId == primeModel.PersonnelId && l.DateSuppression == null);

                        var primeEnt = rapportLigne.ListRapportLignePrimes.FirstOrDefault(p => p.PrimeId == primeModel.PrimeId);
                        if (primeEnt == null)
                        {
                            rapportLigne.ListRapportLignePrimes.Add(new RapportLignePrimeEnt
                            {
                                PrimeId = primeModel.PrimeId,
                                IsChecked = primeModel.IsChecked,
                                HeurePrime = primeModel.HeurePrime
                            });
                        }
                        else
                        {
                            primeEnt.IsChecked = primeModel.IsChecked;
                            primeEnt.HeurePrime = primeModel.HeurePrime;
                        }
                    }
                }
            }
        }

        #endregion
        #region Autre

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
        /// Enregistre les modifications.
        /// </summary>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur connecté.</param>
        private void SaveChanges(int utilisateurId)
        {
            var now = DateTime.UtcNow;
            clotureComptables = new List<ClotureComptable>();
            var usedRapports = personnelsRapports.GetUsedRapports().ToList();
            foreach (var rapport in usedRapports)
            {
                CheckPointageForSave(rapport);
                if (rapport.RapportId != 0)
                {
                    UpdateRapport(rapport, utilisateurId, now);
                }
                else if (rapport.RapportId == 0 && rapport.ListLignes.Any())
                {
                    AddRapport(rapport, utilisateurId, now);
                }

                if (rapport.RapportId != 0)
                {
                    DeleteInvalidePointage(rapport);
                }

            }

            unitOfWork.Save();

            foreach (var rapport in usedRapports)
            {
                rapportHebdoService.CreateOrUpdatePrimeAstreinte(rapport);
            }

            unitOfWork.Save();
        }

        private void CheckPointageForSave(RapportEnt rapport)
        {
            var newLignes = CheckPointageForSave(rapport.ListLignes.ToList());
            if (newLignes.Count != rapport.ListLignes.Count)
            {
                var toDeletes = new List<RapportLigneEnt>();
                foreach (var currentLigne in rapport.ListLignes)
                {
                    if (!newLignes.Contains(currentLigne))
                    {
                        toDeletes.Add(currentLigne);
                    }
                }
                foreach (var toDelete in toDeletes)
                {
                    rapport.ListLignes.Remove(toDelete);
                }
            }
        }

        /// <summary>
        /// Verifier si le rapport comporte des pointages
        /// </summary>
        /// <param name="listRapportLigne">listes des rapports ligne</param>
        /// <returns>le rapport a enregistré</returns>
        public List<RapportLigneEnt> CheckPointageForSave(IEnumerable<RapportLigneEnt> listRapportLigne)
        {
            List<RapportLigneEnt> result = new List<RapportLigneEnt>();

            foreach (RapportLigneEnt rapportLigne in listRapportLigne)
            {
                if (CheckRapportLigneBeforeSave(rapportLigne))
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
        /// supprimer les pointage du zero 
        /// </summary>
        /// <param name="rapport">rapport</param>
        private void DeleteInvalidePointage(RapportEnt rapport)
        {
            rapport.ListLignes.ToList().ForEach(ligne =>
            {
                ligne.ListRapportLigneTaches.ToList().ForEach(ligneTache =>
                {
                    if (ligneTache.IsDeleted)
                    {
                        rapportLigneTacheRepository.Delete(ligneTache, true);
                    }
                });
                ligne.ListRapportLigneMajorations.ToList().ForEach(ligneMajorations =>
                {
                    if (ligneMajorations.IsDeleted)
                    {
                        rapportLigneMajorationRepository.Delete(ligneMajorations, true);
                    }
                });
                ligne.ListRapportLignePrimes.ToList().ForEach(lignePrimes =>
                {
                    if (lignePrimes.IsDeleted)
                    {
                        rapportLignePrimeRepository.Delete(lignePrimes, true);
                    }
                });
                if (ligne.IsDeleted)
                {
                    pointageRepository.Delete(ligne, true);
                }
            });
            if (rapport.ListLignes.Count == 0 || rapport.ListLignes.All(x => x.IsDeleted))
            {
                rapportRepository.Delete(rapport, true);
            }
        }

        /// <summary>
        /// Méttre à jour un rapport journalier
        /// </summary>
        /// <param name="rapport">Le rapport journalier</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur connecté.</param>
        /// <param name="date">La date de modification a utiliser.</param>
        private void UpdateRapport(RapportEnt rapport, int utilisateurId, DateTime date)
        {
            //changement du dernier modificateur
            rapport.AuteurModificationId = utilisateurId;
            rapport.DateModification = date;

            //On traite les données de chaque pointage
            foreach (var ligne in rapport.ListLignes)
            {
                if (ligne.RapportLigneId > 0)
                {
                    ligne.AuteurModificationId = utilisateurId;
                    ligne.DateModification = date;
                }
                else
                {
                    ligne.AuteurCreationId = utilisateurId;
                    ligne.DateCreation = date;
                }
                double heureNormale = (ligne.ListRapportLigneTaches?.Sum(s => s.HeureTache) ?? 0);
                ligne.RapportId = rapport.RapportId;
                ligne.CiId = rapport.CiId;
                ligne.DatePointage = rapport.DateChantier;
                ligne.HeureNormale = heureNormale;
            }
        }

        /// <summary>
        /// Ajouter un nouveau rapport journalier
        /// </summary>
        /// <param name="rapport">Le rapport journalier</param>
        /// <param name="utilisateurId">L'identifiant de l'utilisateur connecté.</param>
        /// <param name="date">La date de modification a utiliser.</param>
        private void AddRapport(RapportEnt rapport, int utilisateurId, DateTime date)
        {
            rapport.RapportStatutId = rapportStatutECId.Value;
            rapport.AuteurCreationId = utilisateurId;
            rapport.DateCreation = date;

            var clotureComptable = clotureComptables.FirstOrDefault(cc => cc.IsSameAs(rapport));
            if (clotureComptable != null)
            {
                rapport.Cloture = clotureComptable.Cloture;
            }
            else
            {
                rapport.Cloture = datesClotureComptableManager.IsPeriodClosed(rapport.CiId, rapport.DateChantier.Year, rapport.DateChantier.Month);
                clotureComptables.Add(new ClotureComptable(rapport));
            }

            rapport.ListLignes.ForEach(l =>
            {
                l.Cloture = rapport.Cloture;
                l.DatePointage = rapport.DateChantier;
                l.DateCreation = date;
                l.AuteurCreationId = utilisateurId;
                l.CiId = rapport.CiId;
                l.RapportLigneStatutId = rapport.RapportStatutId;
                l.HeureNormale = l.ListRapportLigneTaches?.Sum(s => s.HeureTache) ?? 0;
            });

            rapportRepository.Insert(rapport);
        }

        /// <summary>
        /// Get date pointage
        /// </summary>
        /// <param name="dayOfWeek">Jour de la semaine</param>
        /// <returns>Date</returns>
        private DateTime GetDatePointage(int dayOfWeek)
        {
            return datas.MondayDate.AddDays(dayOfWeek);
        }

        /// <summary>
        /// Récupérer la liste des rapports d'un CI sur la semaine
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>List des rapports</returns>
        private List<RapportEnt> GetCiRapportsByWeek(int ciId)
        {
            var rapports = lazyCiRapportsByWeeks.FirstOrDefault(kvp => kvp.Key == ciId).Value;
            if (rapports == null)
            {
                rapports = rapportHebdoService.GetCiRapportsByWeek(ciId, datas.MondayDate, datas.MondayDate.AddDays(6));
                lazyCiRapportsByWeeks.Add(ciId, rapports);
            }
            return rapports;
        }

        private void ProcessUsedRapports()
        {
            var ciIds = new List<int>();
            var pointages = datas.PointagePanelViewModel;
            if (!pointages.IsNullOrEmpty())
            {
                if (IsCiNode(pointages.FirstOrDefault().NodeType))
                {
                    pointages.ForEach(p =>
                    {
                        if (!ciIds.Any(id => id == p.NodeId))
                        {
                            ciIds.Add(p.NodeId);
                        }
                    });
                }
                else
                {
                    ciIds = pointages.SelectMany(p => p.SubNodeList.Select(s => s.NodeId)).Distinct().ToList();
                }

                string statut = rapportHebdoService.GetStatutPersonnelRapportHebdo(pointages);
                lazyCiRapportsByWeeks = rapportHebdoService.GetCiRapportsByWeek(ciIds, datas.MondayDate, statut);
            }
        }

        #endregion

        #region Classes

        private class ClotureComptable
        {
            public ClotureComptable(RapportEnt rapport)
            {
                CiId = rapport.CiId;
                Year = rapport.DateChantier.Year;
                Month = rapport.DateChantier.Month;
                Cloture = rapport.Cloture;
            }

            public int CiId { get; private set; }
            public int Year { get; private set; }
            public int Month { get; private set; }
            public bool Cloture { get; private set; }

            public bool IsSameAs(RapportEnt rapport)
            {
                return rapport.CiId == CiId
                    && rapport.DateChantier.Year == Year
                    && rapport.DateChantier.Month == Month;
            }
        }

        #endregion
    }
}
