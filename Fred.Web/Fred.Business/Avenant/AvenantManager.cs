using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Commande.Validators;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.Avenant
{
    /// <summary>
    /// Manager des Avenants
    /// </summary>
    public class AvenantManager : Manager<CommandeAvenantEnt, ICommandeAvenantRepository>, IAvenantManager
    {
        private readonly IUtilisateurManager userManager;
        private UtilisateurEnt currentUser;
        private readonly ICommandeRepository commandeRepository;
        private readonly ICommandeLignesRepository commandeLignesRepository;
        private readonly ICommandeLigneAvenantRepository commandeLigneAvenantRepository;
        private readonly ICommandeAvenantSaveValidator commandeAvenantSaveValidator;

        public AvenantManager(
            IUnitOfWork uow,
            ICommandeAvenantRepository commandeAvenantRepository,
            IUtilisateurManager userManager,
            ICommandeRepository commandeRepository,
            ICommandeLignesRepository commandeLignesRepository,
            ICommandeLigneAvenantRepository commandeLigneAvenantRepository,
            ICommandeAvenantSaveValidator commandeAvenantSaveValidator)
              : base(uow, commandeAvenantRepository)
        {
            this.userManager = userManager;
            this.commandeRepository = commandeRepository;
            this.commandeLignesRepository = commandeLignesRepository;
            this.commandeLigneAvenantRepository = commandeLigneAvenantRepository;
            this.commandeAvenantSaveValidator = commandeAvenantSaveValidator;
        }

        /// <summary>
        /// Utilisateur Courant
        /// </summary>
        private UtilisateurEnt CurrentUser => currentUser ?? (currentUser = userManager.GetContextUtilisateur());

        /// <summary>
        /// Liste des avenants d'une commande
        /// </summary>
        /// <param name="commandeId">commande Id</param>
        /// <returns>Liste des avenants</returns>
        public IEnumerable<CommandeAvenantEnt> GetAvenantByCommandeId(int commandeId)
        {
            return Repository.GetAvenantByCommandeId(commandeId);
        }

        /// <summary>
        /// Retourne l'avenant courant d'une commande.
        /// </summary>
        /// <param name="commandeId">La commande Id</param>
        /// <returns>L'avenant courant de la commande.</returns>
        public CommandeAvenantEnt GetCurrentAvenant(int commandeId)
        {
            var avenant = Repository.GetAvenantByCommandeId(commandeId).OrderByDescending(a => a.NumeroAvenant).FirstOrDefault();

            var numeroAvenant = 1;
            if (avenant != null)
            {
                if (avenant.DateValidation == null)
                {
                    return avenant;
                }
                numeroAvenant = avenant.NumeroAvenant + 1;
            }

            avenant = new CommandeAvenantEnt()
            {
                CommandeId = commandeId,
                NumeroAvenant = numeroAvenant,
                DateCreation = DateTime.UtcNow,
                AuteurCreationId = CurrentUser.UtilisateurId
            };

            Repository.AddAvenant(avenant);
            Save();
            return avenant;
        }

        /// <summary>
        /// Met à jour l'idenfiant du job Hangfire pour un avenant de commande.
        /// </summary>
        /// <param name="commandeId">L'identifant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        public void UpdateAvenantHangfireJobId(int commandeId, int numeroAvenant, string hangfireJobId)
        {
            // ATTENTION : jamais appelé ! c'est le UpdateAvenantHangfireJobId du commandeManager qui est appelé par le commandeManagerExterne
            try
            {
                var avenant = Repository.GetAvenantByCommandeIdAndAvenantNumber(commandeId, numeroAvenant);
                avenant.HangfireJobId = hangfireJobId;
                Repository.UpdateAvenant(avenant);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Valide un
        /// </summary>
        /// <param name="commandeId">commande</param>
        /// <returns>un Model resultat</returns>
        public CommandeAvenantEnt ValideAvenant(int commandeId)
        {
            CommandeAvenantEnt avenant = Repository.GetAvenantByCommandeId(commandeId).FirstOrDefault(x => x.DateValidation == null);
            if (avenant == null)
            {
                ThrowBusinessValidationException("DateValidation", FeatureCommande.Commande_Detail_Notification_Erreur_Validation_Conccurente);
            }
            else
            {
                avenant.DateValidation = DateTime.UtcNow;
                avenant.AuteurValidationId = CurrentUser.UtilisateurId;
                Repository.UpdateAvenant(avenant);
                Save();
            }
            return avenant;
        }

        /// <summary>
        /// Enregistre l'avenant.
        /// </summary>
        /// <param name="model">Le model de l'avenant concerné.</param>
        /// <param name="date">La date à utiliser pour la création, la modification ou la validation.</param>
        /// <param name="commande">La commande concernée.</param>
        /// <param name="avenant">L'avenant concerné.</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        public CommandeAvenantSave.ResultModel SaveAvenant(CommandeAvenantSave.Model model, DateTime date, out CommandeEnt commande, out CommandeAvenantEnt avenant)
        {
            commande = commandeRepository.GetCommandeWithCommandeLignes(model.CommandeId);

            if (commande == null)
            {
                throw new ArgumentException(BusinessResources.AvenantManager_Model_Erreur_CommandeNull, nameof(commande));
            }

            // Modification sur la commande
            commande.CommentaireFournisseur = model.CommentaireFournisseur;
            commande.CommentaireInterne = model.CommentaireInterne;
            commande.IsAbonnement = model.Abonnement.IsAbonnement;
            commande.FrequenceAbonnement = model.Abonnement.Frequence;
            commande.DureeAbonnement = model.Abonnement.Duree;
            commande.DateProchaineReception = model.Abonnement.DateProchaineReception;
            commande.DatePremiereReception = model.Abonnement.DatePremiereReception;
            commande.DelaiLivraison = model.DelaiLivraison;
            commande.FournisseurAdresse = model.Fournisseur.Adresse;
            commande.FournisseurCPostal = model.Fournisseur.CodePostal;
            commande.FournisseurVille = model.Fournisseur.Ville;
            commande.FournisseurPaysId = model.Fournisseur.PaysId;

            var failures = new List<ValidationFailure>();
            var itemsCreated = new Dictionary<int, CommandeLigneEnt>();

            avenant = GetCurrentAvenant(commande.CommandeId);

            // Lignes d'avenant créées
            if (model.CreatedLignes.Count > 0)
            {
                foreach (var ligne in model.CreatedLignes)
                {
                    var ligneEnt = new CommandeLigneEnt()
                    {
                        CommandeId = model.CommandeId,
                        Libelle = ligne.Libelle,
                        TacheId = ligne.TacheId,
                        RessourceId = ligne.RessourceId,
                        Quantite = ligne.Quantite,
                        PUHT = ligne.PUHT,
                        UniteId = ligne.UniteId,
                        AuteurCreationId = CurrentUser.UtilisateurId,
                        DateCreation = date,
                        AvenantLigne = new CommandeLigneAvenantEnt()
                        {
                            AvenantId = avenant.CommandeAvenantId,
                            IsDiminution = ligne.IsDiminution
                        }
                    };
                    commandeLignesRepository.AddCommandeLigne(ligneEnt);
                    commande.Lignes.Add(ligneEnt);
                    itemsCreated.Add(ligne.ViewId, ligneEnt);
                }
            }

            // Lignes d'avenant modifiées
            foreach (var ligne in model.UpdatedLignes)
            {
                var ligneEnt = commande.Lignes.FirstOrDefault(a => a.CommandeLigneId == ligne.CommandeLigneId);
                if (ligneEnt == null)
                {
                    failures.Add(new ValidationFailure("ligne", string.Format(FeatureCommande.CmdManager_Avenant_LigneModifiee_Erreur_Inexistante, ligne.CommandeLigneId)));
                }
                else
                {
                    ligneEnt.NumeroLigne = ligne.NumeroLigne;
                    ligneEnt.Libelle = ligne.Libelle;
                    ligneEnt.TacheId = ligne.TacheId;
                    ligneEnt.RessourceId = ligne.RessourceId;
                    ligneEnt.UniteId = ligne.UniteId;
                    ligneEnt.Quantite = ligne.Quantite;
                    ligneEnt.PUHT = ligne.PUHT;
                    ligneEnt.AuteurModificationId = CurrentUser.UtilisateurId;
                    ligneEnt.DateModification = date;
                    ligneEnt.AvenantLigne.IsDiminution = ligne.IsDiminution;
                    commandeLignesRepository.UpdateCommandeLigne(ligneEnt);
                }
            }

            // Lignes d'avenant supprimée
            foreach (var ligneId in model.DeletedLigneIds)
            {
                var ligneEnt = commande.Lignes.FirstOrDefault(a => a.CommandeLigneId == ligneId);
                if (ligneEnt == null)
                {
                    failures.Add(new ValidationFailure("ligne", string.Format(FeatureCommande.CmdManager_Avenant_LigneSupprimee_Erreur_Inexistante, ligneId)));
                }
                else
                {
                    //Delete cascade 
                    commandeLigneAvenantRepository.DeleteCommandeLigneAvenant(ligneEnt.AvenantLigne);
                    commandeLignesRepository.DeleteCommandeLigne(ligneEnt);
                }
            }

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }

            BusinessValidation<CommandeEnt>(commande, commandeAvenantSaveValidator);
            Save();

            // Mets à jour les identifiants des éléments ajoutés pour la vue.
            var ret = new CommandeAvenantSave.ResultModel(avenant, commande.DateModification);
            foreach (var kvp in itemsCreated)
            {
                ret.ItemsCreated.Add(new CommandeAvenantSave.ItemCreatedModel(kvp.Key, kvp.Value.CommandeLigneId));
            }
            return ret;
        }
    }
}
