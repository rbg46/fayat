using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Valorisation;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.CommandeEnergies
{
    public class CommandeEnergieService : ICommandeEnergieService
    {
        private readonly ITacheManager tacheManager;
        private readonly IUniteManager uniteManager;
        private readonly ICIManager ciManager;
        private readonly ICommandeTypeManager commandeTypeManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICommandeEnergieValidator commandeEnergieValidator;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly IDepenseTypeManager depenseTypeManager;
        private readonly ISepService sepService;
        private readonly ICommandeEnergieLigneService commandeEnergieLigneService;
        private readonly IMapper mapper;
        private readonly ICommandeEnergieRepository commandeEnergieRepository;
        private readonly ICommandeEnergieMapperService commandeEnergieMapperService;

        public CommandeEnergieService(
            ITacheManager tacheManager,
            IUniteManager uniteManager,
            ICIManager ciManager,
            ICommandeTypeManager commandeTypeManager,
            IUtilisateurManager utilisateurManager,
            ICommandeEnergieValidator commandeEnergieValidator,
            IStatutCommandeManager statutCommandeManager,
            IDepenseTypeManager depenseTypeManager,
            ISepService sepService,
            ICommandeEnergieLigneService commandeEnergieLigneService,
            IMapper mapper,
            ICommandeEnergieRepository commandeEnergieRepository,
            ICommandeEnergieMapperService commandeEnergieMapperService)
        {
            this.tacheManager = tacheManager;
            this.uniteManager = uniteManager;
            this.ciManager = ciManager;
            this.commandeTypeManager = commandeTypeManager;
            this.utilisateurManager = utilisateurManager;
            this.commandeEnergieValidator = commandeEnergieValidator;
            this.statutCommandeManager = statutCommandeManager;
            this.depenseTypeManager = depenseTypeManager;
            this.sepService = sepService;
            this.commandeEnergieLigneService = commandeEnergieLigneService;
            this.mapper = mapper;
            this.commandeEnergieRepository = commandeEnergieRepository;
            this.commandeEnergieMapperService = commandeEnergieMapperService;
        }

        #region public

        /// <summary>
        /// Pré-chargement d'une commande énergie en fonction d'une commande 
        /// </summary>        
        /// <param name="commande">Commande issue du front</param>
        /// <returns>Commande énergie pré-chargée</returns>
        public CommandeEnergie CommandeEnergiePreloading(CommandeEnergie commande)
        {
            SocieteEnt societe = ciManager.GetSocieteByCIId(commande.CiId);
            List<int> societeParticipanteIds = GetsocieteAssocieSep(societe.SocieteId, commande.FournisseurId);
            if (societeParticipanteIds.Count == 0)
            {
                throw new FredBusinessMessageResponseException(FeatureCommandeEnergie.Notification_Fournisseur_Eligible);
            }
            TacheEnt tache = tacheManager.GetTache(Constantes.TacheSysteme.CodeTacheEcartMaterielImmobilise, commande.CiId);
            UniteEnt unite = uniteManager.GetUnite(Constantes.CodeUnite.Heure);
            DeviseEnt deviseRef = ciManager.GetDeviseRef(commande.CiId);

            CommandeEnergie cmdEnergie = GenerateCommandeEnergie(commande, societe, deviseRef?.DeviseId);

            // Validation du model issue du front pour préchargement : Date, CI, Fournisseur, Type Energie obligatoire
            BusinessValidation(commande);

            if (cmdEnergie.TypeEnergie.Code != Constantes.TypeEnergie.Divers)
            {
                cmdEnergie.Lignes = commandeEnergieLigneService.GetGeneratedCommandeEnergieLignes(commande.TypeEnergie, commande.CiId, commande.Date, tache, unite, societeParticipanteIds);
            }
            return cmdEnergie;
        }

        /// <summary>
        /// Récupération d'une commande énergie avec tous les champs calculés
        /// Barème, Unité Bareme, Quantité pointée, Quantité convertie, Montant Valorisé, Ecart Pu, Ecart Quantité, Ecart Montant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>
        /// <returns>Commande énergie</returns>
        public CommandeEnergie GetCommandeEnergie(int commandeId)
        {
            CommandeEnt commandeEnt = commandeEnergieRepository.GetCommandeEnergie(commandeId);

            if (commandeEnt == null)
            {
                throw new FredBusinessNotFoundException(FeatureCommandeEnergie.Notification_Commande_Energie_Non_Trouvee);
            }

            CommandeEnergie commandeEnergie = mapper.Map<CommandeEnergie>(commandeEnt);

            SocieteEnt societe = ciManager.GetSocieteByCIId(commandeEnergie.CiId);
            List<int> societeParticipanteIds = GetsocieteAssocieSep(societe.SocieteId, commandeEnergie.FournisseurId);
            if (commandeEnergie.TypeEnergie.Code == Constantes.TypeEnergie.Personnel || commandeEnergie.TypeEnergie.Code == Constantes.TypeEnergie.Materiel)
            {
                List<CommandeEnergieLigne> generatedCommandeEnergieLignes = commandeEnergieLigneService.GetGeneratedCommandeEnergieLignes(commandeEnergie.TypeEnergie, commandeEnergie.CiId, commandeEnergie.Date, null, null, societeParticipanteIds);
                commandeEnergieLigneService.ComputeCalculatedFields(commandeEnergie.TypeEnergie, commandeEnergie.Lignes.ToList(), generatedCommandeEnergieLignes);
            }

            return commandeEnergie;
        }

        /// <summary>
        /// Réception d'une commande énergie 
        /// - Génération des DepenseAchatEnt à partir des CommandeLigneEnt de la commande énergie
        /// - Ajout en BD des DepenseAchatEnt
        /// - Annulation des valorisations
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>       
        public void ReceptionAuto(int commandeId)
        {
            try
            {
                List<DepenseAchatEnt> receptions = GenerateReceptions(commandeId);

                CancelValorisation(commandeId);

                if (receptions.Count > 0)
                {
                    commandeEnergieRepository.AddRangeReception(receptions);
                }
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Génération des réceptions à partir des lignes de commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>        
        /// <returns>Liste de réceptions</returns>
        private List<DepenseAchatEnt> GenerateReceptions(int commandeId)
        {
            try
            {
                int utilisateurId = utilisateurManager.GetContextUtilisateurId();
                DepenseTypeEnt depenseTypeReception = depenseTypeManager.GetByCode(DepenseType.Reception.ToIntValue());
                List<CommandeLigneEnt> lignes = commandeEnergieRepository.GetCommandeLignesForReception(commandeId);
                return commandeEnergieMapperService.CommandeLigneEntToDepenseAchatEnt(lignes, utilisateurId, depenseTypeReception.DepenseTypeId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupération des réceptions éligibles à l'envoie vers SAP d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Liste des Réceptions</returns>
        public List<DepenseAchatEnt> GetReceptionsForSap(int commandeId)
        {
            return commandeEnergieRepository.GetReceptionsForSap(commandeId);
        }

        #endregion

        #region private

        /// <summary>
        /// Annulation des valorisations du personnel ou matériel
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        private void CancelValorisation(int commandeId)
        {
            try
            {
                List<CommandeLigneEnt> lignes = commandeEnergieRepository.GetCommandeLignesForValorisation(commandeId);

                if (lignes.Count > 0)
                {
                    TacheEnt tache = tacheManager.GetTache(Constantes.TacheSysteme.CodeTacheEcartMaterielImmobilise, lignes[0].Commande.CiId.Value, asNoTracking: true);

                    List<ValorisationEnt> valorisations = commandeEnergieMapperService.CommandeLigneEntToValorisationEnt(lignes, tache);

                    if (valorisations.Count > 0)
                    {
                        commandeEnergieRepository.AddRangeValorisation(valorisations);
                    }
                }
            }
            catch (FredBusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private CommandeEnergie GenerateCommandeEnergie(CommandeEnergie commande, SocieteEnt societe, int? deviseRefId)
        {
            CommandeTypeEnt type = GetComandeTypeByTypeEnergie(commande.TypeEnergie.Code);
            StatutCommandeEnt statut = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutPREP);

            commande.Numero = "NumTemp";
            commande.DateCreation = DateTime.UtcNow;
            commande.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
            commande.Type = type;
            commande.TypeId = type.CommandeTypeId;
            commande.StatutCommande = statut;
            commande.StatutCommandeId = statut.StatutCommandeId;
            commande.Libelle = "Commande Energies";
            commande.Date = new DateTime(commande.Date.Year, commande.Date.Month, DateTime.DaysInMonth(commande.Date.Year, commande.Date.Month));
            commande.DeviseId = deviseRefId;
            commande.NumeroCommandeExterne = GenerateNumeroCommandeExterne(commande, societe);

            return commande;
        }

        /// <summary>
        /// RG_5438_022 : Bouton "Enregistrer" – Vérification de l’unicité du numéro externe de commande
        /// Au clic sur le bouton « Enregistrer », vérifier si le numéro généré automatiquement existe déjà en base (à vérifier dans les colonnes[Numero] et[NumeroCommandeExterne] de la table[FRED_COMMANDE]) : si c’est le cas ajouter un incrément « -1 », ou « -2 », … à la fin.
        /// Exemple : « 56123-435187-RAZE0001-201903-P » => « 56123-435187-RAZE0001-201903-P-1 »
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <param name="societe">Societe</param>
        /// <returns>Numero de commande externe</returns>
        private string GenerateNumeroCommandeExterne(CommandeEnergie commande, SocieteEnt societe)
        {
            string numCmdExterne = $"{societe.Code}-{commande.CI.Code}-{commande.Fournisseur.Code}-{commande.Date.ToString("yyyyMM")}-{commande.TypeEnergie.Code}";

            string existingNumeroExterne = commandeEnergieRepository.GetLastByNumeroCommandeExterne(numCmdExterne);

            if (!string.IsNullOrEmpty(existingNumeroExterne))
            {
                string[] splittedExistingNumeroExterne = existingNumeroExterne.Split('-');
                string counter = splittedExistingNumeroExterne[splittedExistingNumeroExterne.Length - 1]; // dernier élément splitté
                int parsedCounter;
                int.TryParse(counter, out parsedCounter);

                parsedCounter++;

                numCmdExterne += $"-{parsedCounter}";
            }

            return numCmdExterne;
        }

        /// <summary>
        /// Validation métier de la commande énergie par le validator CommandeEnergieValidator
        /// </summary>
        /// <param name="commande">Commande à valider</param>
        private void BusinessValidation(CommandeEnergie commande)
        {
            CommandeEnt commandeEnt = mapper.Map<CommandeEnt>(commande);

            ValidationResult result = commandeEnergieValidator.Validate(commandeEnt);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        /// <summary>
        /// Transposition Type énergie / Type de commande
        /// </summary>
        /// <param name="typeEnergieCode">Code du type énergie</param>
        /// <returns>Type de commande</returns>
        private CommandeTypeEnt GetComandeTypeByTypeEnergie(string typeEnergieCode)
        {
            switch (typeEnergieCode)
            {
                case Constantes.TypeEnergie.Personnel:
                case Constantes.TypeEnergie.Interimaire:
                    return commandeTypeManager.GetByCode(Constantes.CommandeType.Prestation);
                case Constantes.TypeEnergie.Materiel:
                    return commandeTypeManager.GetByCode(Constantes.CommandeType.Location);
                case Constantes.TypeEnergie.Divers:
                    return commandeTypeManager.GetByCode(Constantes.CommandeType.Fourniture);
                default:
                    return null;
            }
        }

        private List<int> GetsocieteAssocieSep(int societeId, int fournisseurid)
        {
            var filters = new List<Expression<Func<AssocieSepEnt, bool>>> { x => x.SocieteId == societeId && x.FournisseurId == fournisseurid };
            var includes = new List<Expression<Func<AssocieSepEnt, object>>> { { x => x.SocieteAssociee }, { x => x.TypeParticipationSep } };
            List<AssocieSepEnt> listassocietSEP = sepService.GetAssocieswithfilter(filters, null, includes).ToList();
            return listassocietSEP.ConvertAll(x => x.SocieteAssocieeId);
        }
        #endregion        
    }
}
