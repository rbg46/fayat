using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.DepenseGlobale;
using Fred.Business.Facturation;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Reception.Common;
using Fred.Business.Reception.Services;
using Fred.Business.Reception.Validators;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.Business.Reception
{
    /// <summary>
    ///   Manager pour l'ecran des Réceptions
    /// </summary>
    public class ReceptionManager : Manager<DepenseAchatEnt, IDepenseRepository>, IReceptionManager
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager ciManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly ICommandeManager commandeManager;
        private readonly IRapportManager rapportManager;
        private readonly IPointageManager pointageManager;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly ICommandeContratInterimaireManager commandeContratInterimaireManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDepenseTypeManager depenseTypeManager;
        private readonly IReceptionBlockedService receptionBlockedService;
        private readonly IVisableReceptionProviderService visableReceptionProviderService;
        private readonly IReceptionQuantityRulesValidator receptionQuantityRulesValidator;
        private readonly IDepenseRepository depenseRepository;
        private readonly IReceptionCreatorService receptionCreatorService;
        private readonly IUpdateReceptionListValidator updateReceptionListValidator;
        private readonly IReceptionViserRulesValidator receptionViserRulesValidator;
        private readonly ICommandeLigneLockingService commandeLigneLockingService;
        private readonly ITacheRepository tacheRepository;

        public ReceptionManager(
            IUnitOfWork uow,
            IDepenseRepository depenseRepository,
            IReceptionValidator validator,
            IUtilisateurManager utilisateurManager,
            ICIManager ciManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            ICommandeManager commandeManager,
            IRapportManager rapportManager,
            IPointageManager pointageManager,
            IContratInterimaireManager contratInterimaireManager,
            ICommandeContratInterimaireManager commandeContratInterimaireManager,
            IValorisationManager valorisationManager,
            IDepenseTypeManager depenseTypeManager,
            IReceptionBlockedService receptionBlockedService,
            IVisableReceptionProviderService visableReceptionProviderService,
            ICommandeLigneLockingService commandeLigneLockingService,
            ITacheRepository tacheRepository,
            IReceptionQuantityRulesValidator receptionQuantityRulesValidator,
            IReceptionCreatorService receptionCreatorService,
            IUpdateReceptionListValidator updateReceptionListValidator,
            IReceptionViserRulesValidator receptionViserRulesValidator)
            : base(uow, depenseRepository, validator)
        {
            this.utilisateurManager = utilisateurManager;
            this.ciManager = ciManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.commandeManager = commandeManager;
            this.rapportManager = rapportManager;
            this.pointageManager = pointageManager;
            this.contratInterimaireManager = contratInterimaireManager;
            this.commandeContratInterimaireManager = commandeContratInterimaireManager;
            this.valorisationManager = valorisationManager;
            this.depenseTypeManager = depenseTypeManager;
            this.receptionBlockedService = receptionBlockedService;
            this.visableReceptionProviderService = visableReceptionProviderService;
            this.depenseRepository = depenseRepository;
            this.commandeLigneLockingService = commandeLigneLockingService;
            this.tacheRepository = tacheRepository;
            this.receptionQuantityRulesValidator = receptionQuantityRulesValidator;
            this.receptionCreatorService = receptionCreatorService;
            this.updateReceptionListValidator = updateReceptionListValidator;
            this.receptionViserRulesValidator = receptionViserRulesValidator;
        }

        /// <summary>
        /// Récupération d'un filtre pour les réceptions
        /// </summary>
        /// <returns>Filtre</returns>
        public SearchDepenseEnt GetNewFilter()
        {
            return new SearchDepenseEnt
            {
                ValueText = string.Empty,
                AViser = true,
                Visees = false,
                Far = false,
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow,
                // Obligatoire
                DepenseTypeId = depenseTypeManager.Get(Entities.DepenseType.Reception.ToIntValue()).DepenseTypeId
            };
        }

        /// <summary>
        ///   Création d'une nouvelle réception en fonction d'une ligne de commande
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande</param>
        /// <returns>Réception créée</returns>
        public DepenseAchatEnt GetNew(int commandeLigneId)
        {
            return receptionCreatorService.Create(commandeLigneId);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////// CREATIONS /////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public DepenseAchatEnt AddAndValidate(DepenseAchatEnt reception)
        {
            ValidateOnAdd(reception);

            AddReception(reception, onAfertAdd: () => commandeLigneLockingService.AutomaticLockIfNeededOnAdd(reception));

            return this.depenseRepository.GetDepenseById(reception.DepenseId).ComputeAll();
        }

        public void ValidateOnAdd(DepenseAchatEnt reception)
        {
            BusinessValidation(new ReceptionListForValidate(new List<DepenseAchatEnt> { reception }), updateReceptionListValidator);

            // Surchage pour FTP
            BusinessValidation(ReceptionsValidationModel.CreateForAddOrUpdates(reception), receptionQuantityRulesValidator);

            BusinessValidation(reception);
        }

        public DepenseAchatEnt AddAndValidateForReceptionIterimaireAndMateriel(DepenseAchatEnt reception)
        {
            ValidateOnAddForReceptionIterimaireAndMateriel(reception);

            AddReception(reception, onAfertAdd: null);// on ne fait pas le verrouillage auto pour les receptions interimaires et materiels

            return this.depenseRepository.GetDepenseById(reception.DepenseId).ComputeAll();
        }

        private void ValidateOnAddForReceptionIterimaireAndMateriel(DepenseAchatEnt reception)
        {
            //ici pas de verification que la commande est verrouillée, pour l'instant seul RZB fait des receptions interim et materiel et que seul ftp verrouille les lignes

            // Surchage pour FTP
            BusinessValidation(ReceptionsValidationModel.CreateForAddOrUpdates(reception), receptionQuantityRulesValidator);

            BusinessValidation(reception);
        }


        public void AddReception(DepenseAchatEnt reception, System.Action onAfertAdd)
        {
            if (reception == null)
                throw new ArgumentNullException(nameof(reception));

            SetDateComptable(reception);
            SetDateAndAuteurCreation(reception);
            SetQuantiteAndPu(reception);

            var receptionSaved = this.depenseRepository.Add(reception);

            if (onAfertAdd != null)
            {
                onAfertAdd();
            }

            Save();
        }


        private void SetQuantiteAndPu(DepenseAchatEnt reception)
        {
            // RG_2853_020: Les réceptions doivent être gérées comme des Dépenses Achat de type « Réception» 
            reception.QuantiteDepense = reception.Quantite;
            reception.AfficherPuHt = true;
            reception.AfficherQuantite = true;
        }

        private void SetDateAndAuteurCreation(DepenseAchatEnt reception)
        {
            reception.DateCreation = DateTime.UtcNow;
            reception.AuteurCreationId = reception.AuteurCreationId ?? utilisateurManager.GetContextUtilisateur()?.UtilisateurId;
        }

        private void SetDateComptable(DepenseAchatEnt reception)
        {
            // RG_2863_110 : Vérifier si la période est "Bloquée en réception" pour le CI de la commande
            if (datesClotureComptableManager.IsBlockedInReception(reception.CiId.Value, reception.Date.Value.Year, reception.Date.Value.Month))
            {
                reception.DateComptable = datesClotureComptableManager.GetNextUnblockedInReceptionPeriod(reception.CiId.Value, reception.Date.Value);
            }
            else
            {
                reception.DateComptable = reception.Date;
            }
        }

        //////////////////////////////////////////////////////////////////// SUPPRESSION /////////////////////////////////////////////////////////

        public void Delete(int receptionId)
        {
            DepenseAchatEnt reception = depenseRepository.FindById(receptionId);
            BusinessValidation(ReceptionsValidationModel.CreateForDeletion(reception), receptionQuantityRulesValidator);
            BusinessValidation(new ReceptionListForValidate(ToReceptionsList(reception)), updateReceptionListValidator);
            BusinessValidation(new ReceptionListForValidate(ToReceptionsList(reception)), receptionViserRulesValidator);

            reception.DateSuppression = DateTime.UtcNow;

            reception.AuteurSuppressionId = utilisateurManager.GetContextUtilisateur()?.UtilisateurId;

            commandeLigneLockingService.AutomaticLockIfNeededOnDelete(reception);

            depenseRepository.UpdateDepenseWithoutSave(reception);

            this.Save();
        }

        /// <summary>
        /// Duplication d'une réception
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande courante</param>
        /// <param name="receptionId">Identifiant de la Réception courante</param>
        /// <returns>Réception dupliquée</returns>
        public DepenseAchatEnt Duplicate(int commandeLigneId, int receptionId)
        {
            DepenseAchatEnt duplicatedReception = GetNew(commandeLigneId);
            DepenseAchatEnt currentReception = this.depenseRepository.Query().Include(x => x.Ressource).Include(x => x.Tache).Get().FirstOrDefault(x => x.DepenseId == receptionId);
            duplicatedReception.RessourceId = currentReception?.RessourceId;
            duplicatedReception.Ressource = currentReception?.Ressource;
            duplicatedReception.TacheId = currentReception?.TacheId;
            duplicatedReception.Tache = currentReception?.Tache;
            duplicatedReception.Quantite = currentReception != null ? currentReception.Quantite : 0;
            return duplicatedReception;
        }

        /// <summary>
        /// Récupération d'une réception en fonction de son identifiant
        /// </summary>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>Réception trouvée</returns>
        public DepenseAchatEnt GetReception(int receptionId)
        {
            return depenseRepository.GetReception(receptionId).ComputeAll();
        }

        #region reception intérimaire
        /// <summary>
        ///   Récupération des lignes rapports  contenant des intérimaires non récéptionnées 
        /// </summary>
        /// <param name="listeCiId">liste d'identifiant unique de ci</param>
        /// /// <param name="utilisateurId">uilisateur qui a lancer le job</param>
        public void ReceptionInterimaire(List<int> listeCiId, int utilisateurId)
        {
            foreach (var ciId in listeCiId)
            {
                IEnumerable<RapportLigneEnt> rapportLigneEnts = rapportManager.GetRapportLigneVerrouillerByCiIdForReceptionInterimaire(ciId, RapportStatutEnt.RapportStatutVerrouille.Key);
                if (rapportLigneEnts.Any())
                {
                    TraitmentRapportLigneForReceptionInterimaire(rapportLigneEnts, utilisateurId);
                }
            }
        }

        private void TraitmentRapportLigneForReceptionInterimaire(IEnumerable<RapportLigneEnt> rapportLigneEnts, int utilisateurId)
        {
            IEnumerable<IGrouping<int?, RapportLigneEnt>> interimaires = rapportLigneEnts.GroupBy(rl => rl.PersonnelId);
            foreach (var interimaire in interimaires)
            {
                IEnumerable<IGrouping<int, RapportLigneEnt>> semaines = interimaire.GroupBy(i => CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(i.DatePointage, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));

                foreach (var semaine in semaines)
                {
                    IEnumerable<IGrouping<int, RapportLigneEnt>> mois = semaine.GroupBy(s => s.DatePointage.Month);

                    foreach (var m in mois)
                    {
                        var contratNotNull = new Dictionary<ContratInterimaireEnt, List<RapportLigneEnt>>(new ContratInterimaireComparer());

                        foreach (var rapportLigne in m)
                        {
                            ContratInterimaireEnt contratInterimaireActif = contratInterimaireManager.GetContratInterimaireByDatePointageForReceptionInterimaire(rapportLigne.PersonnelId, rapportLigne.DatePointage) ?? contratInterimaireManager.GetContratInterimaireByDatePointageAndSouplesseForReceptionInterimaire(rapportLigne.PersonnelId, rapportLigne.DatePointage);

                            if (contratInterimaireActif != null)
                            {
                                if (!contratNotNull.ContainsKey(contratInterimaireActif))
                                {
                                    contratNotNull.Add(contratInterimaireActif, new List<RapportLigneEnt>());
                                }

                                contratNotNull[contratInterimaireActif].Add(rapportLigne);
                            }
                        }

                        foreach (var contrat in contratNotNull)
                        {
                            GenerateReceptionInterimaire(contrat, semaine.Key, utilisateurId);
                        }
                    }
                }
            }
        }

        private void GenerateReceptionInterimaire(KeyValuePair<ContratInterimaireEnt, List<RapportLigneEnt>> rapportLigneByContrat, int numeroSemaine, int utilisateurId)
        {
            ContratInterimaireEnt contratInterimaireActif = rapportLigneByContrat.Key;

            CommandeContratInterimaireEnt commandeContratInterimaireEnt = commandeContratInterimaireManager.GetCommandeContratInterimaire(rapportLigneByContrat.Key.ContratInterimaireId, contratInterimaireActif.CiId.Value);
            decimal quantite = 0;

            if (commandeContratInterimaireEnt != null)
            {
                // RG_6472_012
                if (contratInterimaireActif.Ci.Societe.TypeSociete.Code == TypeSociete.Sep &&
                    contratInterimaireActif.Societe.TypeSociete.Code == TypeSociete.Partenaire &&
                    contratInterimaireActif.Energie)
                {
                    return;
                }

                int depenseTypeReceptionId = depenseTypeManager.Get(Entities.DepenseType.Reception.ToIntValue()).DepenseTypeId;

                foreach (RapportLigneEnt c in rapportLigneByContrat.Value)
                {
                    quantite += Convert.ToDecimal(c.HeureTotalTravail);
                }

                RapportLigneEnt rapportLigne = rapportLigneByContrat.Value.FirstOrDefault();
                int ciId = rapportLigne.CiId;
                bool generateValorisationNegativeForInterimaire = true;

                // RG_6472_011
                if (contratInterimaireActif.Ci.Societe.TypeSociete.Code == TypeSociete.Sep &&
                    contratInterimaireActif.Societe.TypeSociete.Code == TypeSociete.Interne &&
                    contratInterimaireActif.Energie)
                {
                    ciId = contratInterimaireActif.Ci.CompteInterneSepId.Value;
                    generateValorisationNegativeForInterimaire = false;
                }

                DepenseAchatEnt receptionInterimaire = CreateReceptionInterimaire(rapportLigne,
                                             commandeContratInterimaireEnt,
                                             contratInterimaireActif,
                                             depenseTypeReceptionId,
                                             numeroSemaine,
                                             utilisateurId,
                                             quantite,
                                             ciId);

                DepenseAchatEnt reception = AddAndValidateForReceptionIterimaireAndMateriel(receptionInterimaire);

                // RG_6472_011
                if (generateValorisationNegativeForInterimaire)
                {
                    valorisationManager.InsertValorisationNegativeForInterimaire(rapportLigne, reception);
                }

                UpdatePointageForReceptionInterimaire(reception, rapportLigneByContrat.Value, utilisateurId);
            }
        }

        private void UpdatePointageForReceptionInterimaire(DepenseAchatEnt reception, IEnumerable<RapportLigneEnt> rapportLigneByContrat, int utilisateurId)
        {
            if (reception != null)
            {
                foreach (RapportLigneEnt rapportLigne in rapportLigneByContrat)
                {
                    rapportLigne.Personnel = null;
                    rapportLigne.Ci = null;
                    pointageManager.UpdatePointageForReceptionInterimaire(rapportLigne, utilisateurId);
                }
                pointageManager.Save();
            }
        }

        private DepenseAchatEnt CreateReceptionInterimaire(RapportLigneEnt rapportLigne,
                                            CommandeContratInterimaireEnt commandeContratInterimaireEnt,
                                            ContratInterimaireEnt contratInterimaireActif,
                                            int depenseTypeReceptionId,
                                            int numeroSemaine,
                                            int utilisateurId,
                                            decimal quantite,
                                            int ciId)
        {
            var date = rapportLigne.DatePointage;

            return new DepenseAchatEnt()
            {
                NumeroBL = "S" + numeroSemaine + date.Year + "-" + rapportLigne.Personnel.Matricule.Replace("I_", string.Empty),
                DepenseTypeId = depenseTypeReceptionId,
                AuteurCreationId = utilisateurId,
                AuteurVisaReceptionId = utilisateurId,
                Date = date,
                DateVisaReception = DateTime.Now,
                DateComptable = datesClotureComptableManager.IsBlockedInReception(rapportLigne.CiId, date.Year, date.Month) ? datesClotureComptableManager.GetNextUnblockedInReceptionPeriod(rapportLigne.CiId, date) : date,
                DeviseId = commandeContratInterimaireEnt.Commande.DeviseId,
                FournisseurId = commandeContratInterimaireEnt.Commande.FournisseurId,
                Libelle = "S" + numeroSemaine + " - Intérim " + rapportLigne.Personnel.PrenomNom + " - " + rapportLigne.Personnel.Matricule.Replace("I_", string.Empty) + " - " + contratInterimaireActif.NumContrat,
                RessourceId = contratInterimaireActif.RessourceId,
                Commentaire = string.Empty,
                TacheId = tacheRepository.GetTacheIdInterimByCiId(ciId),
                UniteId = contratInterimaireActif.UniteId,
                Quantite = quantite,
                PUHT = contratInterimaireActif.Valorisation,
                CommandeLigneId = commandeManager.GetListCommandeLigneById(commandeContratInterimaireEnt.CommandeId).First().CommandeLigneId,
                CiId = ciId,
                IsReceptionInterimaire = true
            };
        }

        #endregion

        /// <summary>
        ///   Récupération des lignes rapports  contenant des matériels externe non récéptionnées 
        /// </summary>
        /// <param name="societeId">identifiant unique de la societe</param>
        public void ReceptionMaterielExterne(int societeId)
        {
            List<int> listeCiId = new List<int>();
            listeCiId.AddRange(ciManager.GetCiIdListBySocieteId(societeId));

            foreach (var ciId in listeCiId)
            {
                IEnumerable<RapportLigneEnt> rapportLigneEnts = rapportManager.GetRapportLigneVerrouillerByCiIdForReceptionMaterielExterne(ciId, RapportStatutEnt.RapportStatutVerrouille.Key);
                if (rapportLigneEnts.Any())
                {
                    TraitmentRapportLigneForMaterielExterne(rapportLigneEnts);
                }
            }
        }

        private void TraitmentRapportLigneForMaterielExterne(IEnumerable<RapportLigneEnt> rapportLigneEnts)
        {
            IEnumerable<IGrouping<int?, RapportLigneEnt>> materiels = rapportLigneEnts.GroupBy(rl => rl.MaterielId);
            foreach (var materiel in materiels)
            {
                IEnumerable<IGrouping<int, RapportLigneEnt>> semaines = materiel.GroupBy(i => CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(i.DatePointage, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));

                foreach (var semaine in semaines)
                {
                    IEnumerable<IGrouping<int, RapportLigneEnt>> mois = semaine.GroupBy(s => s.DatePointage.Month);

                    foreach (var m in mois)
                    {
                        GenerateReceptionMaterielExterne(m, semaine.Key);
                    }
                }
            }
        }

        private void GenerateReceptionMaterielExterne(IGrouping<int, RapportLigneEnt> rapportLigneByMateriel, int numeroSemaine)
        {
            RapportLigneEnt rapportLigne = rapportLigneByMateriel.FirstOrDefault();
            CommandeLigneEnt commandeLigne = commandeManager.GetCommandeLigneByMaterielId((int)rapportLigne.MaterielId);

            decimal quantite = 0;
            if (commandeLigne?.Commande != null)
            {
                int depenseTypeReceptionId = depenseTypeManager.Get(Entities.DepenseType.Reception.ToIntValue()).DepenseTypeId;

                rapportLigneByMateriel.ForEach(rl => quantite += Convert.ToDecimal(rl.MaterielMarche));

                if (commandeLigne.Unite.Code == CodeUnite.Jour)
                {
                    quantite /= 8;
                }
                else if (commandeLigne.Unite.Code == CodeUnite.Semaine)
                {
                    quantite /= 40;
                }

                quantite = decimal.Round(quantite, 2, MidpointRounding.AwayFromZero);

                int utilisateurIdFredIE = utilisateurManager.GetByLogin("fred_ie").UtilisateurId;
                string numeroCommande = string.IsNullOrEmpty(commandeLigne.Commande.NumeroCommandeExterne) ? commandeLigne.Commande.Numero : commandeLigne.Commande.NumeroCommandeExterne;

                DateTime date = rapportLigne.DatePointage;
                DepenseAchatEnt receptionMaterielExterne = new DepenseAchatEnt()
                {
                    NumeroBL = "S" + numeroSemaine + date.Year + "-" + numeroCommande + " - " + commandeLigne.NumeroLigne,
                    DepenseTypeId = depenseTypeReceptionId,
                    AuteurCreationId = utilisateurIdFredIE,
                    Date = date,
                    DateComptable = datesClotureComptableManager.IsBlockedInReception(rapportLigne.CiId, date.Year, date.Month) ? datesClotureComptableManager.GetNextUnblockedInReceptionPeriod(rapportLigne.CiId, date) : date,
                    DeviseId = commandeLigne.Commande.DeviseId,
                    FournisseurId = commandeLigne.Commande.FournisseurId,
                    Libelle = "S" + numeroSemaine + " - " + rapportLigne.Materiel.Code + " - Désignation " + rapportLigne.Materiel.Libelle,
                    RessourceId = commandeLigne.RessourceId,
                    Commentaire = string.Empty,
                    TacheId = rapportLigne.Ci.Taches.Where(t => t.TacheType == TacheType.EcartMaterielExterne.ToIntValue()).Select(t => t.TacheId).FirstOrDefault(),
                    UniteId = commandeLigne.UniteId,
                    Quantite = decimal.Round(quantite, 2, MidpointRounding.AwayFromZero),
                    PUHT = commandeLigne.PUHT,
                    CommandeLigneId = commandeLigne.CommandeLigneId,
                    CiId = commandeLigne.Commande.CiId,
                    DateVisaReception = DateTime.UtcNow,
                    AuteurVisaReceptionId = utilisateurIdFredIE,
                    IsReceptionMaterielExterne = true
                };

                DepenseAchatEnt reception = AddAndValidateForReceptionIterimaireAndMateriel(receptionMaterielExterne);
                valorisationManager.InsertValorisationNegativeForMaterielExterne(rapportLigne, reception);

                if (reception != null)
                {
                    foreach (var c in rapportLigneByMateriel)
                    {
                        c.Materiel = null;
                        c.Ci = null;
                        pointageManager.UpdatePointageForReceptionMaterielExterne(c);
                    }
                    pointageManager.Save();
                }
            }
        }

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        public IEnumerable<DepenseAchatEnt> GetReceptions(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            return depenseRepository.GetReceptions(ciId, ressourceId, periodeDebut, periodeFin, deviseId);
        }

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="filtres">Liste de <see cref="DepenseGlobaleFiltre" /></param>
        /// <returns>Liste de <see cref="DepenseAchatEnt" /></returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<DepenseGlobaleFiltre> filtres)
        {
            List<int> ciIds = filtres.Select(q => q.CiId).ToList();
            List<DateTime?> periodeDebut = filtres.Select(q => q.PeriodeDebut).ToList();
            List<DateTime?> periodeFin = filtres.Select(q => q.PeriodeFin).ToList();

            return await depenseRepository.GetReceptionsAsync(ciIds, periodeDebut, periodeFin).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">liste d'identifiants de CIs</param>
        /// <param name="tacheIdList">liste d'identifiants de taches</param>    
        /// <param name="dateDebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <param name="includeProperties">include des navigation properties</param>
        /// <returns>Liste de dépense de type réception</returns>
        public async Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut = null, DateTime? dateFin = null, int? deviseId = null, bool includeProperties = false)
        {
            return await depenseRepository.GetReceptionsAsync(ciIdList, tacheIdList, dateDebut, dateFin, deviseId, includeProperties).ConfigureAwait(false);
        }

        /// <summary>
        ///   Récupère les identifiant unique des réceptions intérimaire qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <param name="listeCiId">liste d'identifiant unique de ci</param>
        /// <returns>Réception</returns>
        public IEnumerable<int> GetReceptionInterimaireToSend(List<int> listeCiId)
        {
            return depenseRepository.GetReceptionInterimaireToSend(listeCiId);
        }

        /// <summary>
        ///   Récupère les identifiant unique des réceptions matériel externe qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <returns>Réception</returns>
        public IEnumerable<int> GetReceptionMaterielExterneToSend()
        {
            return depenseRepository.GetReceptionMaterielExterneToSend();
        }

        //////////////////////////////////////////////////////////////////// Viser un reception (envoie a sap) ///////////////////////////////////////////////////

        /// <summary>
        /// Permet l'idenfiant du job Hangfire pour une commande.
        /// </summary>
        /// <param name="receptionIds">La liste d'identifant des réceptions.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        public void UpdateHangfireJobId(IEnumerable<int> receptionIds, string hangfireJobId)
        {
            var receptions = new List<DepenseAchatEnt>();
            foreach (int receptionId in receptionIds)
            {
                var reception = this.depenseRepository.FindById(receptionId);
                reception.HangfireJobId = hangfireJobId;
                receptions.Add(reception);
            }
            this.depenseRepository.MarkFieldsAsUpdated(receptions, x => x.HangfireJobId);

            Save();
        }

        /// <summary>
        /// Permet de viser une liste de réceptions. Cad de signée la reception avec l'utilisateur courrant
        /// </summary>
        /// <param name="receptionIds">les ids des receptions a visées </param>
        /// <param name="callFredIeAndSetHangfireJobIdAction">callFredIeAndSetHangfireJobIdAction</param>
        public async Task ViserReceptionsAsync(List<int> receptionIds, Func<List<DepenseAchatEnt>, Task> callFredIeAndSetHangfireJobIdAction)
        {
            if (callFredIeAndSetHangfireJobIdAction == null)
            {
                throw new ArgumentNullException(nameof(callFredIeAndSetHangfireJobIdAction));
            }

            var visableReceptionsResponse = visableReceptionProviderService.GetReceptionsVisables(receptionIds);

            var visableReceptions = visableReceptionsResponse.ReceptionsVisables;

            if (!visableReceptions.Any())
            {
                return;
            }

            var savedReceptions = SaveForRollback(visableReceptions);

            var currentUserId = utilisateurManager.GetContextUtilisateur().UtilisateurId;

            receptionBlockedService.SetDateComtapleOfReceptions(visableReceptions);

            foreach (var reception in visableReceptions)
            {
                reception.AuteurModificationId = currentUserId;
                reception.DateModification = DateTime.UtcNow;
                reception.AuteurVisaReceptionId = currentUserId;
                reception.DateVisaReception = DateTime.UtcNow;
            }

            this.depenseRepository.MarkFieldsAsUpdatedAndSaveInOneTransaction(visableReceptions,
              x => x.DateComptable,// ici car j'ai mis a jour la date Comptble juste avant dans 'SetDateComtapleOfReceptions'
              x => x.AuteurVisaReceptionId,
              x => x.DateVisaReception,
              x => x.DateModification,
              x => x.AuteurModificationId);

            try
            {
                await callFredIeAndSetHangfireJobIdAction(visableReceptions).ConfigureAwait(false);
            }
            catch (Exception)
            {
                RollbackOnFail(savedReceptions, visableReceptions);
                this.depenseRepository.MarkFieldsAsUpdatedAndSaveInOneTransaction(visableReceptions,
                    x => x.DateComptable,
                    x => x.AuteurVisaReceptionId,
                    x => x.DateVisaReception,
                    x => x.DateModification,
                    x => x.AuteurModificationId);
                throw;
            }

            this.depenseRepository.MarkFieldsAsUpdatedAndSaveInOneTransaction(visableReceptions, x => x.HangfireJobId);
        }

        private List<DepenseAchatEnt> SaveForRollback(List<DepenseAchatEnt> oldReceptions)
        {
            var result = new List<DepenseAchatEnt>();

            foreach (var beforeChangeReception in oldReceptions)
            {
                var savedReception = new DepenseAchatEnt();
                savedReception.DepenseId = beforeChangeReception.DepenseId;
                savedReception.DateComptable = beforeChangeReception.DateComptable;
                savedReception.AuteurModificationId = beforeChangeReception.AuteurModificationId;
                savedReception.DateModification = beforeChangeReception.DateModification;
                savedReception.AuteurVisaReceptionId = beforeChangeReception.AuteurVisaReceptionId;
                savedReception.DateVisaReception = beforeChangeReception.DateVisaReception;
                result.Add(savedReception);
            }
            return result;
        }

        private void RollbackOnFail(List<DepenseAchatEnt> savedReceptions, List<DepenseAchatEnt> actualReceptions)
        {
            foreach (var savedReception in savedReceptions)
            {
                var actualReception = actualReceptions.Find(x => x.DepenseId == savedReception.DepenseId);
                actualReception.DateComptable = savedReception.DateComptable;
                actualReception.AuteurModificationId = savedReception.AuteurModificationId;
                actualReception.DateModification = savedReception.DateModification;
                actualReception.AuteurVisaReceptionId = savedReception.AuteurVisaReceptionId;
                actualReception.DateVisaReception = savedReception.DateVisaReception;
            }
        }
        //////////////////////////////////////////////////////////////////////// RECHERCHE  //////////////////////////////////////////////////////////////////////

        /// <summary>
        ///   Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        public List<DepenseAchatEnt> Get(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                                Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                                List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = true)
        {
            return this.depenseRepository.Get(filters, orderBy, includeProperties, page, pageSize, asNoTracking).ToList();
        }

        //////////////////////////////////////////////////////////////////// MISE A JOUR AVEC VALIDATIONS /////////////////////////////////////////////////////////

        /// <summary>
        ///  Mise à jour d'une liste de réceptions
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>       
        public void UpdateAndSaveWithValidation(List<DepenseAchatEnt> receptions)
        {
            if (receptions == null)
                throw new ArgumentNullException(nameof(receptions));

            this.ValidateOnUpdates(receptions);
            this.UpdateAndSaveReceptions(receptions);
            this.Save();
        }

        /// <summary>
        ///  Mise à jour d'une réception
        /// </summary>
        /// <param name="reception">Liste de réceptions à mettre à jour</param>  
        public void UpdateAndSaveWithValidation(DepenseAchatEnt reception)
        {
            var receptions = ToReceptionsList(reception);
            this.ValidateOnUpdates(receptions);
            this.UpdateAndSaveReceptions(receptions);
            this.Save();
        }

        public void ValidateOnUpdates(List<DepenseAchatEnt> receptions)
        {
            if (receptions == null)
                throw new ArgumentNullException(nameof(receptions));

            BusinessValidation(new ReceptionListForValidate(receptions), updateReceptionListValidator);
            BusinessValidation(new ReceptionListForValidate(receptions), receptionViserRulesValidator);
            foreach (var reception in receptions)
            {
                BusinessValidation(reception);
            }
            BusinessValidation(ReceptionsValidationModel.CreateForAddOrUpdates(receptions), receptionQuantityRulesValidator);
        }


        private void UpdateAndSaveReceptions(List<DepenseAchatEnt> receptions)
        {
            //je met a jour la date comptable de toutes les receptions 
            // => recherche de la prochaine date disponible si le ci de la reception est cloturer.
            receptionBlockedService.SetDateComtapleOfReceptions(receptions);

            var user = utilisateurManager.GetContextUtilisateur();

            foreach (var reception in receptions)
            {
                reception.DateModification = DateTime.UtcNow;
                reception.AuteurModificationId = user?.UtilisateurId;
                reception.AuteurModification = user;
                reception.QuantiteDepense = reception.Quantite;
                reception.AfficherPuHt = true;
                reception.AfficherQuantite = true;
                depenseRepository.UpdateDepenseWithoutSave(reception);
            }

            commandeLigneLockingService.AutomaticLockIfNeededOnUpdate(receptions);

        }

        //////////////////////////////////////////////////////////////////// MISE A JOUR SANS VALIDATION /////////////////////////////////////////////////////////
        ////////////////// (Necessaire pour mettre a jour certains champs qui ne necessite pas tous le process de validation ex retour migo ou pieceJointeId) ////       
        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        public void UpdateAndSaveWithoutValidation(IEnumerable<DepenseAchatEnt> receptions, int auteurModificationId)
        {
            try
            {
                foreach (var reception in receptions)
                {
                    InternalUpdateWithoutValidation(reception, auteurModificationId);
                }
                this.Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="reception">Réception à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        public void UpdateAndSaveWithoutValidation(DepenseAchatEnt reception, int auteurModificationId)
        {
            try
            {
                InternalUpdateWithoutValidation(reception, auteurModificationId);
                this.Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation (methode interne commune)
        /// </summary>
        /// <param name="reception">Réception à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        private void InternalUpdateWithoutValidation(DepenseAchatEnt reception, int auteurModificationId)
        {
            try
            {
                reception.DateModification = DateTime.UtcNow;
                reception.AuteurModificationId = auteurModificationId;
                depenseRepository.UpdateDepenseWithoutSave(reception);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
        /////////////////////////////////////////  VERFICATION SI UNE OU PLUSIEURS RECEPTIONS SONT 'BLOQUÉES EN RECEPTION' ///////////////////////////////////////     

        /// <summary>
        ///   Détermine s'il y a au moins une réception dont la date correspond est comprise dans une période bloquée en réception
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>
        public bool CheckAnyReceptionsIsBlocked(List<int> receptionIds)
        {
            var receptions = this.depenseRepository.Get().AsNoTracking().Where(x => receptionIds.Contains(x.DepenseId)).ToList();

            return receptionBlockedService.CheckAnyReceptionsIsBlocked(receptions);
        }


        /// <summary>
        /// Retourne le total du montant receptionné pour une liste de ligne de commandes
        /// </summary>
        /// <param name="commandeLigneIds">Identifiant des lignes de commande</param>
        /// <returns>PUHT * Qte des receptions des lignes de commande </returns>
        public decimal GetMontant(List<int> commandeLigneIds)
        {
            IReadOnlyList<DepenseAchatEnt> receptions = depenseRepository.GetReceptions(commandeLigneIds);
            return receptions.Sum(q => q.PUHT * q.Quantite);
        }

        public List<DepenseAchatEnt> ToReceptionsList(DepenseAchatEnt reception)
        {
            return new List<DepenseAchatEnt>()
            {
                reception
            };
        }

    }
}
