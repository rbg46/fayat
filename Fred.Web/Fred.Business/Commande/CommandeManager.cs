using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Avenant;
using Fred.Business.CI;
using Fred.Business.Commande.Reporting;
using Fred.Business.Commande.Validators;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.PieceJointe.Services;
using Fred.Business.Referential.Materiel;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Template.Models.CommandeValidation;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Import;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Export.Order.Models;
using Fred.Framework.Extensions;
using Fred.Framework.Services;
using Fred.Framework.Templating;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Commande;
using static Fred.Entities.Constantes;

namespace Fred.Business.Commande
{
    public class CommandeManager : Manager<CommandeEnt, ICommandeRepository>, ICommandeManager
    {
        private const string FREDCOMMANDPREFIX = "F";

        private readonly string commonFolder = ConfigurationManager.AppSettings["attachment:folder"];
        private readonly List<string> listCodeUniteAccepte = new List<string>() { CodeUnite.Heure, CodeUnite.Jour, CodeUnite.Semaine };

        private readonly IUnitOfWork uow;
        private readonly ICommandeExportExcelService commandeExportExcelService;
        private readonly IDateTimeExtendManager datetimeExtendMgr;
        private readonly ICommandeLignesRepository commandeLigneRepository;
        private readonly IAvenantManager avenantManager;
        private readonly IRepository<CommandeAvenantEnt> commandeAvenantRepo;
        private readonly ISystemeExterneRepository systemeExterneRepository;
        private readonly ICommandeHeaderValidator commandeHeaderValidator;
        private readonly ICommandeDeleteValidator commandeDeleteValidator;
        private readonly ICommandeExportService commandeExportService;
        private readonly ICIManager ciManager;
        private readonly IUniteManager uniteManager;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IUtilisateurManager userManager;
        private readonly ITacheManager tacheManager;
        private readonly IPersonnelImageManager personnelImageManager;
        private readonly IMaterielExterneManager materielExterneManager;
        private readonly IImageManager imageManager;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly ICommandeTypeManager commandeTypeManager;
        private readonly IPieceJointeService pieceJointeService;
        private readonly ISepService sepService;

        private UtilisateurEnt currentUser;
        private UtilisateurEnt CurrentUser => currentUser ?? (currentUser = userManager.GetContextUtilisateur());

        public CommandeManager(
            IUnitOfWork uow,
            ICommandeRepository commandeRepository,
            ICommandeValidator validator,
            ICommandeExportExcelService commandeExportExcelService,
            IDateTimeExtendManager datetimeExtendMgr,
            IAvenantManager avenantManager,
            ICommandeExportService commandeExportService,
            ICIManager ciManager,
            IUniteManager uniteManager,
            IReferentielFixeManager referentielFixeManager,
            IUtilisateurManager userManager,
            ITacheManager tacheManager,
            IPersonnelImageManager personnelImageManager,
            IMaterielExterneManager materielExterneManager,
            IImageManager imageManager,
            IStatutCommandeManager statutCommandeManager,
            ICommandeTypeManager commandeTypeManager,
            IPieceJointeService pieceJointeService,
            ISepService sepService,
            ICommandeLignesRepository commandeLigneRepository,
            ICommandeAvenantRepository commandeAvenantRepo,
            ISystemeExterneRepository systemeExterneRepository,
            ICommandeHeaderValidator commandeHeaderValidator,
            ICommandeDeleteValidator commandeDeleteValidator)
            : base(uow, commandeRepository, validator)
        {
            this.uow = uow;
            this.commandeExportExcelService = commandeExportExcelService;
            this.datetimeExtendMgr = datetimeExtendMgr;
            this.commandeExportService = commandeExportService;
            this.ciManager = ciManager;
            this.uniteManager = uniteManager;
            this.referentielFixeManager = referentielFixeManager;
            this.userManager = userManager;
            this.tacheManager = tacheManager;
            this.personnelImageManager = personnelImageManager;
            this.materielExterneManager = materielExterneManager;
            this.imageManager = imageManager;
            this.statutCommandeManager = statutCommandeManager;
            this.commandeTypeManager = commandeTypeManager;
            this.pieceJointeService = pieceJointeService;
            this.sepService = sepService;
            this.avenantManager = avenantManager;
            this.commandeLigneRepository = commandeLigneRepository;
            this.commandeAvenantRepo = commandeAvenantRepo;
            this.systemeExterneRepository = systemeExterneRepository;
            this.commandeHeaderValidator = commandeHeaderValidator;
            this.commandeDeleteValidator = commandeDeleteValidator;
        }

        #region Commandes
        /// <inheritdoc />
        public void ValidateCommande(int commandeId, StatutCommandeEnt statut)
        {
            CommandeEnt commande = Repository.GetCommandeWithLignes(commandeId).ComputeAll();
            if (statut.Code == StatutCommandeEnt.CommandeStatutVA
                && commande.Type.Code == CommandeType.Location)
            {
                AddMaterielExterne(commande);
            }
            ChangeStatutCommande(commande, statut);
        }

        private void ChangeStatutCommande(CommandeEnt commande, StatutCommandeEnt statut)
        {
            if (statut.Code == StatutCommandeEnt.CommandeStatutVA && commande.StatutCommandeId == statut.StatutCommandeId)
            {
                ThrowBusinessValidationException("DateValidation", FeatureCommande.Commande_Detail_Notification_Erreur_Validation_Conccurente);
            }

            commande.StatutCommandeId = statut.StatutCommandeId;
            if (statut.Code == StatutCommandeEnt.CommandeStatutVA)
            {
                commande.ValideurId = CurrentUser.UtilisateurId;
                commande.DateValidation = DateTime.UtcNow;
            }
            BusinessValidation(commande);
            Repository.Update(commande);
            //savegarde tout à la fin
            Save();
        }

        /// <inheritdoc />
        public IEnumerable<CommandeEnt> GetCommandeList()
        {
            return GetDefaultQuery().Filter(c => !c.DateSuppression.HasValue).Get().ComputeAll();
        }

        /// <inheritdoc />
        public IQueryable<CommandeEnt> GetCommandesMobile(DateTime? sinceDate = null, int? userId = null)
        {
            return Repository
               .Query()
               .Include(r => r.StatutCommande)
               .Filter(r => (sinceDate == null || r.DateCreation >= sinceDate.Value || r.DateModification >= sinceDate.Value || r.DateSuppression >= sinceDate.Value)
                            && (userId == null || r.AuteurCreationId == null || r.AuteurCreationId == userId)
                            && r.StatutCommande.Code == StatutCommandeEnt.CommandeStatutVA)
               .Get()
               .Take(6)
               .ComputeAll()
               .AsQueryable();
        }


        public CommandeEnt GetCommandeById(int commandeId)
        {
            CommandeEnt commande = Repository.GetFull(commandeId).ComputeAll();

            ThrowExceptionIfDeletedCommande(commande);
            MapCalculatedFieldCommande(commande);
            SetContactTel(commande);

            return commande;
        }

        private void ThrowExceptionIfDeletedCommande(CommandeEnt commande)
        {
            if (commande != null && commande.AuteurSuppressionId.HasValue && commande.DateSuppression.HasValue)
            {
                // On n'affiche pas une commande supprimée
                var user = userManager.GetUtilisateurById(commande.AuteurSuppressionId.Value);
                string messageError = string.Format(FeatureCommande.Commande_Detail_Notification_Erreur_Affichage_Commande_Supprimee_With_Detail, commande.Numero, user.PrenomNom, user.Personnel?.Matricule);
                ThrowBusinessValidationException("DateSuppression", messageError);
            }
        }

        private void MapCalculatedFieldCommande(CommandeEnt commande)
        {
            if (commande != null)
            {
                commande.IsVisable = CommandeManagerHelper.IsVisable(commande, CurrentUser.UtilisateurId);
                commande.CommandeManuelleAllowed = CurrentUser?.CommandeManuelleAllowed == true;
                commande.CI.IsCiHaveManyDevise = ciManager.IsCiHaveManyDevises(commande.CI.CiId);
            }
        }

        private void SetContactTel(CommandeEnt commande)
        {
            if (commande != null && commande.StatutCommande.Code == StatutCommandeEnt.CommandeStatutBR || commande.StatutCommande.Code == StatutCommandeEnt.CommandeStatutAV)
            {
                //si le contact tel est null en base, on lui affecte le tel renseigné sur la fiche personnel s'il en existe un.
                commande.ContactTel = commande.ContactTel == null ? commande.Contact?.Telephone1 : commande.ContactTel;
            }
        }

        /// <inheritdoc />
        public CommandeEnt GetNewCommande()
        {
            CommandeEnt commande;
            CIEnt lastCI = GetLastCICommande(CurrentUser.UtilisateurId);

            commande = new CommandeEnt
            {
                Date = DateTime.UtcNow,
                AuteurCreationId = CurrentUser.UtilisateurId,
                SuiviId = CurrentUser.UtilisateurId,
                Suivi = CurrentUser.Personnel,
                Lignes = new List<CommandeLigneEnt>() { new CommandeLigneEnt() }
            };

            commande.ContactId = CurrentUser.UtilisateurId;
            commande.Contact = CurrentUser.Personnel;
            commande.ContactTel = CurrentUser.Personnel.Telephone1;

            commande.CommandeManuelleAllowed = CurrentUser.CommandeManuelleAllowed;

            if (lastCI != null)
            {
                DeviseEnt lastCIdeviseRef = ciManager.GetDeviseRef(lastCI.CiId);
                commande.CI = lastCI;
                commande.CiId = lastCI.CiId;
                commande.CI.IsCiHaveManyDevise = ciManager.IsCiHaveManyDevises(lastCI.CiId);
                commande.LivraisonEntete = lastCI.EnteteLivraison;
                commande.LivraisonAdresse = lastCI.AdresseLivraison;
                commande.LivraisonVille = lastCI.VilleLivraison;
                commande.LivraisonCPostale = lastCI.CodePostalLivraison;
                commande.LivraisonPaysId = lastCI.PaysLivraisonId;
                commande.LivraisonPays = lastCI.PaysLivraison;

                if (lastCIdeviseRef != null)
                {
                    commande.Devise = lastCIdeviseRef;
                    commande.DeviseId = lastCIdeviseRef.DeviseId;
                }

                // [TSA] : Dans tous les cas, on prend l'adresse de facturation définie dans la fiche CI. La modifcation se fait donc dans la fiche CI
                commande.FacturationAdresse = lastCI.AdresseFacturation;
                commande.FacturationCPostale = lastCI.CodePostalFacturation;
                commande.FacturationVille = lastCI.VilleFacturation;
                commande.FacturationPaysId = lastCI.PaysFacturationId;
                commande.FacturationPays = lastCI.PaysFacturation;
            }

            return commande;
        }

        /// <inheritdoc />
        public SearchCommandeEnt GetNewFilter()
        {


            return new SearchCommandeEnt
            {
                ValueText = string.Empty,
                Libelle = true,
                Numero = true,
                NumeroCommandeExterne = true,
                CICodeLibelle = true,
                FournisseurCodeLibelle = true,
                NumeroAsc = true,
                CICodeLibelleAsc = true,
                DateAsc = false,
                AuteurCreationId = CurrentUser.UtilisateurId,
                DateFrom = DateTime.UtcNow.Date.AddYears(-1),
                DateTo = DateTime.UtcNow.Date.AddYears(1),
                Types = commandeTypeManager.GetAll(),
                // TSA: filtré pour fred V1: sera peut être réactivé plus tard...
                Statuts = statutCommandeManager.GetAll().Where(x => x.Code != StatutCommandeEnt.CommandeStatutMVA),
                MesCommandes = false,
                SystemeExternes = GetCommandeSystemeExterneListPourUtilisateurCourant()
            };
        }

        /// <inheritdoc />
        public int AddCommande(CommandeEnt commande, bool setTacheParDefaut = true)
        {
            DateTime now = DateTime.UtcNow;

            if (commande == null)
            {
                throw new ArgumentNullException(nameof(commande));
            }
            if (commande.CiId != null && setTacheParDefaut)
            {
                AffecterTacheParDefautCommande(commande);
            }
            // Obligatoire pour le 1er enregistrement
            commande.Numero = "NumeroTemp";

            if (commande.Lignes != null)
            {
                foreach (CommandeLigneEnt ligne in commande.Lignes)
                {
                    ligne.DateCreation = now;
                    ligne.AuteurCreationId = CurrentUser.UtilisateurId;
                }
            }
            commande.DateCreation = now;
            commande.AuteurCreationId = CurrentUser.UtilisateurId;

            commande.ComputeAll();
            commande.CleanProperties();

            BusinessValidation(commande);
            Repository.Insert(commande);
            Save();

            commande.Numero = FREDCOMMANDPREFIX + commande.CommandeId.ToString().PadLeft(9, '0');
            uow.Save();

            return commande.CommandeId;
        }

        /// <inheritdoc />
        public CommandeEnt AddCommandeExterne(CommandeEnt commande)
        {
            try
            {
                commande.Numero = "NumeroTemp";
                commande.ComputeAll();
                BusinessValidation(commande);
                commande.CleanProperties();
                this.Repository.Insert(commande);
                Save();
                commande.Numero = FREDCOMMANDPREFIX + commande.CommandeId.ToString().PadLeft(9, '0');
                Save();
                return commande;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public void UpdateCommande(CommandeEnt commande)
        {
            if (commande == null)
            {
                throw new ArgumentNullException(nameof(commande));
            }

            AffecterTacheParDefautCommande(commande);
            CheckUpdateCommandeStatutBrouillon(commande);
            CheckUpdateConcurrente(commande);

            commande.DateModification = DateTime.UtcNow;
            commande.AuteurModificationId = CurrentUser.UtilisateurId;
            commande.ComputeAll();
            commande.CleanProperties();
            commande = UpdateCommandeLigne(commande);

            BusinessValidation(commande);
            materielExterneManager.Save();
            Repository.Update(commande);
            Save();
        }

        private void CheckUpdateConcurrente(CommandeEnt commande)
        {
            //Recupere le statut de la commande depuis le back
            var statutCommandeFromBack = Repository.GetStatutCommandeByCommandeId(commande.CommandeId);
            // Si validation ou sauvegarde d'une commande déja valider
            if (statutCommandeFromBack != null && statutCommandeFromBack.Code == StatutCommandeEnt.CommandeStatutVA
                   && commande.StatutCommande.Code != StatutCommandeEnt.CommandeStatutVA)
            {
                ThrowBusinessValidationException("DateValidation", FeatureCommande.Commande_Detail_Notification_Erreur_Validation_Conccurente);
            }
        }

        private void CheckUpdateCommandeStatutBrouillon(CommandeEnt commande)
        {
            if (commande.AuteurCreationId != CurrentUser.UtilisateurId && commande.StatutCommande.Code == StatutCommandeEnt.CommandeStatutBR)
            {
                ThrowBusinessValidationException("AuteurModification", FeatureCommande.Commande_Notification_Update_Error);
            }
        }

        /// <summary>
        /// Mise à jour rapide d'un commande champ à champ
        /// /!\ Aucune RG appliquée, à utiliser avec précaution... Merci!
        /// </summary>
        /// <param name="commande">Commande à mettre à jour</param>
        /// <param name="userId">Identifiant de l'utilisateur ayant effectué la modification</param>
        /// <param name="fieldsToUpdate">Champs à mettre à jour</param>
        /// <returns>Commande mise à jour</returns>
        public CommandeEnt QuickUpdate(CommandeEnt commande, int userId, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate)
        {
            if (fieldsToUpdate == null)
            {
                throw new ArgumentNullException("fieldsToUpdate");
            }

            commande.AuteurModificationId = userId;
            commande.DateModification = DateTime.UtcNow;

            fieldsToUpdate.Add(x => x.AuteurModificationId);
            fieldsToUpdate.Add(x => x.DateModification);

            Repository.Update(commande, fieldsToUpdate);
            Save();

            return commande;
        }

        /// <summary>
        /// Permet de validé l'entête d'une commande
        /// </summary>
        /// <param name="commande">Commande a valider</param>
        /// <returns>Commande</returns>
        public void ValidateHeaderCommande(CommandeEnt commande)
        {
            if (commande == null)
            {
                throw new ArgumentNullException(nameof(commande));
            }

            BusinessHeaderValidation(commande);

        }

        /// <summary>
        /// Valide la commande.
        /// </summary>
        /// <param name="commande">La commande concernée.</param>
        private void BusinessHeaderValidation(CommandeEnt commande)
        {
            BusinessValidation(commande, commandeHeaderValidator);
        }


        /// <summary>
        ///   Permet de définir le statut de la commande
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <param name="statut">Statut de la commande</param>
        private void SetStatutCommande(CommandeEnt commande, string statut)
        {
            StatutCommandeEnt sc = statutCommandeManager.GetByCode(statut);
            commande.StatutCommandeId = sc.StatutCommandeId;
            commande.StatutCommande = sc;
        }

        /// <inheritdoc />
        public void DeleteCommandeById(int id)
        {
            CommandeEnt commande = GetCommandeById(id);
            int auteurCourantId = CurrentUser.UtilisateurId;

            if (commande == null)
            {
                throw new ArgumentException("commande is null");
            }

            if (commande.CommandeManuelle && commande.AuteurCreationId != auteurCourantId)
            {
                throw new ArgumentException("AuteurCreation must be different from a");
            }

            commande.DateSuppression = DateTime.UtcNow;
            commande.AuteurSuppressionId = auteurCourantId;

            // Validation si suppression
            BusinessValidation(commande, commandeDeleteValidator);

            this.Repository.Update(commande);
            this.Save();
        }

        public CommandeEnt DuplicateCommande(int id)
        {
            CommandeEnt commandeSource = Repository.GetFullWithoutDepenses(id);
            MapCalculatedFieldCommande(commandeSource);
            CommandeEnt commandeDupliquee = CommandeManagerHelper.CloneCommande(commandeSource);
            commandeDupliquee.CommandeId = 0;
            commandeDupliquee.Numero = string.Empty;
            commandeDupliquee.NumeroCommandeExterne = null;
            commandeDupliquee.DateSuppression = null;
            commandeDupliquee.DateModification = null;
            commandeDupliquee.DateCloture = null;
            commandeDupliquee.AuteurSuppressionId = null;
            commandeDupliquee.AuteurModificationId = null;
            commandeDupliquee.ValideurId = null;
            commandeDupliquee.DateValidation = null;
            commandeDupliquee.DateCreation = DateTime.UtcNow;
            commandeDupliquee.Date = DateTime.Today.ToUniversalTime();
            commandeDupliquee.AuteurCreationId = CurrentUser.UtilisateurId;
            commandeDupliquee.IsVisable = CommandeManagerHelper.IsVisable(commandeDupliquee, CurrentUser.UtilisateurId);
            commandeDupliquee.CommandeManuelle = false;
            commandeDupliquee.DeviseId = commandeSource.DeviseId;
            commandeDupliquee.Devise = commandeSource.Devise;
            commandeDupliquee.CommandeManuelleAllowed = commandeSource.CommandeManuelleAllowed;
            commandeDupliquee.IsMaterielAPointer = commandeSource.IsMaterielAPointer;
            commandeDupliquee.IsEnergie = commandeSource.IsEnergie;

            if (commandeSource.IsAbonnement)
            {
                commandeDupliquee.IsAbonnement = commandeSource.IsAbonnement;
                commandeDupliquee.FrequenceAbonnement = commandeSource.FrequenceAbonnement;
                commandeDupliquee.DateProchaineReception = commandeSource.DateProchaineReception;
            }

            // Supprime les lignes d'avenant
            commandeDupliquee.Lignes = commandeDupliquee.Lignes.Where(l => l.AvenantLigne == null).ToList();

            foreach (CommandeLigneEnt ligne in commandeDupliquee.Lignes)
            {
                ligne.CommandeId = 0;
                ligne.CommandeLigneId = 0;
                ligne.MaterielId = null;
                ligne.Commande = null;
            }

            commandeDupliquee.ComputeAll();
            return commandeDupliquee;
        }

        /// <inheritdoc />
        public int GetNombreCommandesBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            return this.Repository.GetNombreCommandesBuyer(codeEtab, dateDebut, dateFin);
        }

        /// <inheritdoc />
        public void ImporterCmdsBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin)
        {
            this.Repository.ImporterCmdsBuyer(codeEtab, dateDebut, dateFin);
        }

        /// <inheritdoc />
        public void SendByMail(CommandeEnt commande, Stream attachement)
        {
            //// Si passage par Id alors utiliser : CommandeEnt commande = this.Repository.Query().Include(s => s.StatutCommande).Get().First(c => c.CommandeId == id);

            if (commande == null)
            {
                throw new FredBusinessException(FeatureCommande.CmdManager_CommandeInconnu);
            }

            if (attachement == null)
            {
                ThrowBusinessValidationException("Email", FeatureCommande.CmdManager_EmailErrorPdfIndispo);
                return;
            }

            // RG_62_001: L'envoi par mail n'est accessible que pour les commandes validées.
            if (!commande.IsStatutValidee)
            {
                ThrowBusinessValidationException("IsStatutValidee", FeatureCommande.CmdManager_EmailErrorStatutCommande);
            }

            // RG_62_002 : L'envoi de mail doit envoyer la commande en tant que PJ dans un mail à la personne qui FAIT la commande      
            UtilisateurEnt user = CurrentUser;
            if (user == null)
            {
                ThrowBusinessValidationException("Email", FeatureCommande.CmdManager_EmailErrorAdresseMail);
                return;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                ThrowBusinessValidationException("Email", FeatureCommande.CmdManager_EmailErrorAdresseMail);
                return;
            }

            // Récupérer la liste des pièces jointes
            IEnumerable<PieceJointeEnt> pieceJointes = pieceJointeService.GetPiecesJointes(PieceJointeTypeEntite.Commande, commande.CommandeId);

            // Générer le html correspondant au model
            CommandeValidationTemplateModel model = new CommandeValidationTemplateModel() { Numero = commande.Numero };
            TemplatingService tempplatingService = new TemplatingService();
            var htmlContent = tempplatingService.GenererHtmlFromTemplate(TemplateNames.EmailCommandeImpression, model);

            try
            {
                using (SendMail sender = new SendMail())
                {
                    string subject = string.Format(CommandeResources.MailSubject, commande.Numero);
                    sender.AddAttachement(attachement, string.Format(CommandeResources.MailAttachment, commande.Numero));
                    long sizelength = attachement.Length;
                    foreach (var pieceJointe in pieceJointes)
                    {
                        string pieceJointeFilePath = Path.Combine(commonFolder, pieceJointe.RelativePath);
                        FileInfo fi = new FileInfo(pieceJointeFilePath);
                        sizelength += fi.Length;
                        if ((sizelength / 1048576) > 10)
                        {
                            ThrowBusinessValidationException("Email", "Dépassement de la limite");
                            return;
                        }
                        sender.AddAttachement(pieceJointeFilePath, pieceJointe.Libelle);
                    }
                    sender.SendFormattedEmail(user.Email, user.Email, subject, htmlContent, true);
                }
            }
            catch (FredTechnicalException e)
            {
                ThrowBusinessValidationException("Email", e.Message);
            }
        }

        /// <summary>
        ///   Customize le fichier excel contenant la liste des commandes
        /// </summary>
        /// <typeparam name="T">Commande</typeparam>  
        /// <param name="modelList">Liste de commandes</param>
        /// <returns>action de customisation d'un workbook</returns>
        public string CustomizeExcelFileForExport<T>(IEnumerable<T> modelList, string path)
        {
            var format = new Framework.Reporting.ExcelFormat();
            var action = commandeExportExcelService.CustomizeExcelFileForExport(modelList);

            return format.GenerateExcelAndSaveOnServer(path, modelList, action);
        }

        /// <summary>
        /// Retourne l'id de l'organisation associé au ci de la commande.
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>l'id de l'organisation associé au ci de la commande</returns>
        public int? GetOrganisationIdByCommandeId(int commandeId)
        {
            return this.Repository.GetOrganisationIdByCommandeId(commandeId);
        }

        /// <inheritdoc />
        public CommandeEnt GetCommande(string numero)
        {
            try
            {
                return Repository.Query()
                  .Include(c => c.CI)
                  .Get().FirstOrDefault(x => x.Numero == numero).ComputeAll();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public CommandeEnt GetCommandeByNumberOrExternalNumber(string numero)
        {
            try
            {
                return Repository.GetCommandeByNumberOrExternalNumber(numero).ComputeAll();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne la liste des commandes pour une liste de numéros de commandes
        /// </summary>
        /// <param name="numerosCommande">Liste de numéro de commande</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns><see cref="CommandeEcritureComptableOdModel"/></returns>
        public IReadOnlyList<CommandeEcritureComptableOdModel> GetCommandeEcritureComptableOdModelByNumeros(List<string> numerosCommande, int ciId)
        {
            List<CommandeEcritureComptableOdModel> commandeEcritureCompatbleOdModels = new List<CommandeEcritureComptableOdModel>();

            Repository.GetCommandes(numerosCommande, ciId).ComputeMontantHT().ForEach(commande => commandeEcritureCompatbleOdModels.Add(new CommandeEcritureComptableOdModel
            {
                CommandeId = commande.CommandeId,
                Montant = commande.MontantHT,
                NumeroCommande = commande.Numero,
                CommandeLigneId = commande.Lignes.Select(ligne => ligne.CommandeLigneId).ToList(),
                DateComptable = commande.Date
            }));

            return commandeEcritureCompatbleOdModels;
        }

        /// <inheritdoc />
        public CommandeEnt CloturerCommande(int commandeId)
        {
            CommandeEnt commande = GetCommandeById(commandeId);

            if (commande.IsStatutValidee)
            {
                DateTime now = DateTime.UtcNow;
                commande.DateCloture = now;
                commande.DateModification = now;
                commande.AuteurModificationId = CurrentUser.UtilisateurId;
                SetStatutCommande(commande, StatutCommandeEnt.CommandeStatutCL);
                Repository.Update(commande);
                Save();
                return commande;
            }

            return null;
        }

        /// <inheritdoc />
        public CommandeEnt DecloturerCommande(int commandeId)
        {
            CommandeEnt commande = GetCommandeById(commandeId);

            if (commande.IsStatutCloturee)
            {
                DateTime now = DateTime.UtcNow;
                commande.DateCloture = null;
                commande.DateModification = now;
                commande.AuteurModificationId = CurrentUser.UtilisateurId;
                SetStatutCommande(commande, StatutCommandeEnt.CommandeStatutVA);
                Repository.Update(commande);
                Save();
                return commande;
            }

            return null;
        }

        /// <summary>
        ///   Génération des réceptions pour les commandes abonnement
        /// </summary>
        /// <returns>Liste des commandes abonnements éligibles à la génération auto des réceptions</returns>
        public IEnumerable<CommandeEnt> GetCommandeAbonnementList()
        {
            return Repository.Query()
                             .Include(x => x.Lignes)
                             .Filter(x => x.IsAbonnement
                                            && x.StatutCommande.Code == StatutCommandeEnt.CommandeStatutVA
                                            && x.DureeAbonnement > 0
                                            && x.DateProchaineReception.Value.Date <= DateTime.UtcNow.Date)
                             .Get()
                             .ComputeAll();
        }

        /// <summary>
        ///   Récupération de la dernière date (dernière échéance) de génération des réceptions automatiques des commandes abonnements
        /// </summary>
        /// <param name="dateProchaineReception">Date prochaine génération</param>
        /// <param name="frequenceAbo">Fréquence de l'abonnement</param>
        /// <param name="dureeAbo">Nombre de récetpion restant à générer</param>
        /// <returns>Date</returns>
        public DateTime GetLastDateOfReceptionGeneration(DateTime dateProchaineReception, int frequenceAbo, int dureeAbo)
        {
            DateTime lastDate = dateProchaineReception;
            dureeAbo--;

            if (frequenceAbo == FrequenceAbonnement.Jour.ToIntValue())
            {
                while (dureeAbo >= 0)
                {
                    var isBusinessDay = datetimeExtendMgr.IsBusinessDay(lastDate);
                    if (isBusinessDay)
                    {
                        dureeAbo--;
                    }
                    if (dureeAbo >= 0)
                    {
                        lastDate = lastDate.AddDays(1);
                    }
                }
            }
            else if (frequenceAbo == FrequenceAbonnement.Semaine.ToIntValue())
            {
                lastDate = lastDate.AddDays(7 * dureeAbo);
            }
            else if (frequenceAbo == FrequenceAbonnement.Mois.ToIntValue())
            {
                lastDate = lastDate.AddMonths(1 * dureeAbo);
            }
            else if (frequenceAbo == FrequenceAbonnement.Trimestre.ToIntValue())
            {
                lastDate = lastDate.AddMonths(3 * dureeAbo);
            }
            else if (frequenceAbo == FrequenceAbonnement.Annee.ToIntValue())
            {
                lastDate = lastDate.AddYears(1 * dureeAbo);
            }
            return lastDate;
        }

        /// <inheritdoc />
        public void GetItemsToReturnToSap(int commandeId, out bool returnCommande, out IEnumerable<int> returnNumeroAvenants)
        {
            var commande = this.Repository.Query()
              .Include(s => s.StatutCommande)
              .Include(c => c.Lignes.Select(l => l.AvenantLigne.Avenant))
              .Filter(c => c.CommandeId == commandeId)
              .Get()
              .First();

            if (!commande.IsStatutValidee && !commande.IsStatutManuelleValidee)
            {
                returnCommande = false;
                returnNumeroAvenants = new List<int>();
            }
            else
            {
                returnCommande = commande.HangfireJobId == null;
                returnNumeroAvenants = commande.Lignes
                  .Where(l => l.AvenantLigne?.Avenant != null)
                  .Select(l => l.AvenantLigne.Avenant)
                  .Distinct()
                  .Where(a => a.DateValidation != null && a.HangfireJobId == null)
                  .Select(a => a.NumeroAvenant);
            }
        }

        #endregion

        #region Lignes Commande

        /// <summary>
        ///   Retourne la ligne de commande portant l'identifiant unique du materiel externe.
        /// </summary>
        /// <param name="materielId">Identifiant du materiel externe.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        public CommandeLigneEnt GetCommandeLigneByMaterielId(int materielId)
        {
            return commandeLigneRepository.Query()
                        .Include(c => c.Unite)
                        .Include(c => c.Commande)
                        .Filter(c => c.MaterielId == materielId)
                        .Get()
                        .FirstOrDefault();
        }

        /// <inheritdoc />
        public CommandeLigneEnt GetCommandeLigneById(int commandeLigneId)
        {
            return commandeLigneRepository.Query()
                                    .Include(c => c.Commande)
                                    .Include(c => c.Commande.Devise)
                                    .Include(c => c.Ressource)
                                    .Include(c => c.Tache)
                                    .Include(c => c.Unite)
                                    .Include(c => c.AllDepenses.Select(x => x.DepenseType))
                                    .Filter(c => c.CommandeLigneId.Equals(commandeLigneId))
                                    .Get()
                                    .FirstOrDefault()
                                    .ComputeAll();
        }

        /// <inheritdoc />
        public IEnumerable<CommandeLigneEnt> GetListCommandeLigneById(int commandeId)
        {
            return commandeLigneRepository.Query()
                                    .Include(c => c.Commande)
                                    .Include(c => c.Commande.Devise)
                                    .Include(c => c.Ressource)
                                    .Include(c => c.Tache)
                                    .Include(c => c.Unite)
                                    .Include(c => c.AllDepenses.Select(x => x.DepenseType))
                                    .Filter(c => c.CommandeId.Equals(commandeId))
                                    .Get()
                                    .ToList();
        }

        #endregion

        /// <inheritdoc />
        public void UpdateHangfireJobId(int commandeId, string hangfireJobId)
        {
            try
            {
                var commande = this.Repository.FindById(commandeId);
                commande.HangfireJobId = hangfireJobId;
                this.Repository.Update(commande);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        #region Export

        /// <inheritdoc/>
        public byte[] ExportBrouillonPdf(CommandeEnt commande)
        {
            CommandeExportModel model = CreateCommandeExportModel(commande);
            CommandeExportOptions exportOptions = new CommandeExportOptions
            {
                WithCGV = false,
                WaterMark = FeatureCommande.Commande_Export_Brouillon,
                WaterMarkFontSize = 140f
            };
            return commandeExportService.ToPdf(model, exportOptions, imageManager, userManager);
        }

        /// <inheritdoc/>
        public byte[] ExportPdf(CommandeEnt commande)
        {
            CommandeExportModel model = CreateCommandeExportModel(commande);
            CommandeExportOptions exportOptions = new CommandeExportOptions
            {
                WithCGV = true,
            };
            return commandeExportService.ToPdf(model, exportOptions, imageManager, userManager);
        }

        /// <inheritdoc/>
        public byte[] ExportPdfCommandeBrouillon(CommandeEnt commande)
        {
            CommandeExportModel model = CreateCommandeExportModel(commande);
            CommandeExportOptions exportOptions = new CommandeExportOptions
            {
                WithCGV = true,
                WaterMark = $"{FeatureCommande.Commande_Export_Provisoire}{Environment.NewLine}{FeatureCommande.Commande_Export_WaitForValidation}"
            };

            SetPremiereImpressionBrouillonDateToNow(commande.CommandeId);
            return commandeExportService.ToPdf(model, exportOptions, imageManager, userManager);
        }

        private CommandeExportModel CreateCommandeExportModel(CommandeEnt commande)
        {
            if (commande.ValideurId.HasValue && commande.Valideur?.Personnel == null)
            {
                // Je charge l'utilisateur (Valideur)
                this.Repository.PerformEagerLoading(commande, x => x.Valideur);
                // Je charge le Personnel associé à l'utilisateur (Valideur)
                this.Managers.Utilisateur.PerformEagerLoading(commande.Valideur, x => x.Personnel);
            }
            SocieteEnt societeGerante = sepService.GetSocieteGeranteForSep(ciManager.GetSocieteByCIId(commande.CiId.Value));
            CommandeExportModel model = commandeExportService.Convert(commande, societeGerante, personnelImageManager, imageManager, userManager);
            return model;
        }

        #endregion

        #region Avenants

        /// <summary>
        /// Enregistre l'avenant.
        /// </summary>
        /// <param name="model">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public CommandeAvenantSave.ResultModel SaveAvenant(CommandeAvenantSave.Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            CommandeEnt commande;
            CommandeAvenantEnt avenant;
            return avenantManager.SaveAvenant(model, DateTime.UtcNow, out commande, out avenant);
        }

        /// <summary>
        /// Enregistre l'avenant et le valide.
        /// </summary>
        /// <param name="model">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public CommandeAvenantSave.ResultModel ValideAvenant(CommandeAvenantSave.Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var avenant = avenantManager.ValideAvenant(model.CommandeId);
            return new CommandeAvenantSave.ResultModel(avenant, model.DateModification);
        }

        /// <summary>
        /// Met à jour l'idenfiant du job Hangfire pour un avenant de commande.
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        public void UpdateAvenantHangfireJobId(int commandeId, int numeroAvenant, string hangfireJobId)
        {
            try
            {
                var avenant = commandeAvenantRepo.Query().Filter(a => a.CommandeId == commandeId && a.NumeroAvenant == numeroAvenant).Get().First();
                avenant.HangfireJobId = hangfireJobId;
                commandeAvenantRepo.Update(avenant);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        #endregion

        #region Commandes externes

        /// <inheritdoc />
        public IEnumerable<SystemeExterneEnt> GetCommandeSystemeExterneListPourUtilisateurCourant()
        {
            var societeIds = currentUser.Personnel.Societe.Groupe.Societes.Select(s => s.SocieteId);
            return systemeExterneRepository.Query()
              .Filter(se => se.SystemeExterneTypeId == (int)SystemeExterneType.Commandes)
              .Filter(se => societeIds.Contains(se.SocieteId))
              .Get();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        ///   Requête de filtrage des commandes par défaut
        /// </summary>
        /// <returns>Retourne le prédicat de filtrage des commandes par défaut</returns>
        private IRepositoryQuery<CommandeEnt> GetDefaultQuery()
        {
            return this.Repository.Query()
                       .Include(a => a.CI)
                       .Include(a => a.Contact)
                       .Include(a => a.Suivi)
                       .Include(f => f.Fournisseur)
                       .Include(lp => lp.LivraisonPays)
                       .Include(fp => fp.FacturationPays)
                       .Include(fp => fp.FournisseurPays)
                       .Include(s => s.AuteurCreation.Personnel)
                       .Include(s => s.AuteurModification.Personnel)
                       .Include(v => v.Valideur.Personnel)
                       .Include(s => s.StatutCommande)
                       .Include(t => t.Type)
                       .Include(t => t.Devise)
                       .Include(c => c.Lignes.Select(l => l.AllDepenses.Select(r => r.FacturationsReception.Select(x => x.FacturationType))))
                       .Include(c => c.Lignes.Select(l => l.AllDepenses.Select(r => r.DepenseType)))
                       .Include(c => c.Lignes.Select(l => l.AllDepenses.Select(r => r.Depenses.Select(x => x.DepenseType))))
                       .Include(c => c.Lignes.Select(l => l.AvenantLigne.Avenant));
        }

        /// <summary>
        ///   Permet la récupération de l'ci de la dernière commande de l'utilisateur passé en paramètre.
        /// </summary>
        /// <param name="idSaisisseur">Identifiant unique de l'utilisateur des commandes</param>
        /// <returns>Retourne l'ci de la dernière commande de l'utilisateur</returns>
        private CIEnt GetLastCICommande(int idSaisisseur)
        {
            CIEnt lastCi = null;

            try
            {
                CommandeEnt lastCommande = this.Repository.Query()
                                               .Include(c => c.CI.Societe)
                                               .Include(c => c.CI.Societe.TypeSociete)
                                               .Include(c => c.CI.EtablissementComptable.Societe)
                                               .Filter(c => c.AuteurCreationId == idSaisisseur)
                                               .OrderBy(o => o.OrderByDescending(c => c.CommandeId))
                                               .Get()
                                               .FirstOrDefault();

                if (lastCommande?.CI != null)
                {
                    ciManager.CheckAccessToEntity(lastCommande?.CI);
                }

                lastCi = lastCommande?.CI;
            }
            catch (UnauthorizedAccessException)
            {
                NLog.LogManager.GetCurrentClassLogger().Error("Vous n'avez plus les droits d'accès au CI que vous avez utilisé dans votre dernière commande.");
            }

            return lastCi;
        }

        /// <summary>
        /// Définie la date de première impression à la date actuelle. Si une date est déjà renseignée, on ne met pas a jour.
        /// </summary>
        /// <param name="commandeId">Id de la commande concernée</param>
        private void SetPremiereImpressionBrouillonDateToNow(int commandeId)
        {
            CommandeEnt commande = Repository.GetById(commandeId);
            if (commande.DatePremiereImpressionBrouillon == null)
            {
                commande.DatePremiereImpressionBrouillon = DateTime.UtcNow;
                commande.AuteurPremiereImpressionBrouillonId = CurrentUser.UtilisateurId;
            }
            Repository.Update(commande);
            Save();
        }

        /// <summary>
        ///   Permet d'affecter un code tâche par défaut aux lignes d'une commande n'en possédant pas.
        /// </summary>
        /// <param name="commande">Commande dont les codes tâches des lignes sont à remplir.</param>
        private void AffecterTacheParDefautCommande(CommandeEnt commande)
        {
            TacheEnt tacheParDefaut = tacheManager.GetTacheParDefaut(commande.CiId.Value);

            foreach (CommandeLigneEnt ligne in commande.Lignes)
            {
                if (!ligne.TacheId.HasValue)
                {
                    ligne.TacheId = tacheParDefaut.TacheId;
                }
            }
        }

        private CommandeEnt UpdateCommandeLigne(CommandeEnt commande)
        {
            var listDeleted = new List<CommandeLigneEnt>();

            foreach (CommandeLigneEnt ligne in commande.Lignes.ToList())
            {
                if (ligne.IsDeleted)
                {
                    ligne.CommandeId = 0;
                    commandeLigneRepository.Delete(ligne);
                    listDeleted.Add(ligne);
                    ligne.IsDeleted = false;
                }
                else if (ligne.IsCreated)
                {
                    ligne.DateCreation = DateTime.UtcNow;
                    ligne.AuteurCreationId = CurrentUser.UtilisateurId;
                    ligne.CommandeId = commande.CommandeId;

                    // Dans le cadre d'une duplication de ligne, celle-ci est à la fois IsCreated et IsUpdated
                    if (ligne.IsUpdated)
                    {
                        ligne.DateModification = DateTime.UtcNow;
                        ligne.AuteurModificationId = CurrentUser.UtilisateurId;
                        ligne.IsUpdated = false;
                    }
                    commandeLigneRepository.Insert(ligne);
                }
                else if (ligne.IsUpdated)
                {
                    ligne.DateModification = DateTime.UtcNow;
                    ligne.AuteurModificationId = CurrentUser.UtilisateurId;
                    commandeLigneRepository.Update(ligne);
                    ligne.IsUpdated = false;
                }
            }

            foreach (CommandeLigneEnt ligneEnt in listDeleted)
            {
                commande.Lignes.Remove(ligneEnt);
            }

            return commande;
        }

        private void AddMaterielExterne(CommandeEnt commande)
        {
            int materielTypeRessourceId = referentielFixeManager.GetTypeRessourceByCode(TypeRessource.CodeTypeMateriel).TypeRessourceId;
            List<int> eligibleUniteIds = uniteManager.GetUniteIdsByListCode(listCodeUniteAccepte);

            foreach (var ligne in commande.Lignes.Where(l => l.Ressource != null))
            {
                if (ligne.MaterielId.HasValue)
                {
                    materielExterneManager.ChangeStatutMaterielExterne(ligne.MaterielId.Value, commande.IsMaterielAPointer, commande.AuteurModificationId);
                }

                if (commande.IsMaterielAPointer
                    && !ligne.MaterielId.HasValue
                    && ligne?.Ressource?.TypeRessourceId == materielTypeRessourceId
                    && (!ligne.UniteId.HasValue || eligibleUniteIds.Contains(ligne.UniteId.Value)))
                {
                    ligne.Materiel = materielExterneManager.AddMaterielExterne(ligne, commande, ligne.AuteurModificationId ?? CurrentUser.UtilisateurId);
                    commandeLigneRepository.Update(ligne);
                }
            }
        }
        #endregion
    }
}
