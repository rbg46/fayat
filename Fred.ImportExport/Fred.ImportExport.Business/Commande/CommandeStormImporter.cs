using System;
using System.Collections.Generic;
using Fred.Business.Commande;
using Fred.Entities.Commande;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Sap;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Commande;

namespace Fred.ImportExport.Business.Commande
{
    /// <summary>
    /// Permet d'importer des commandes STORM.
    /// </summary>
    public class CommandeStormImporter : SapImporter<CommandeSapModel>, ICommandeStormImporter
    {
        private const string StormArticleSystemeImportCode = "STORM_ARTICLE";
        private const string StormCommandeSystemeExterneCode = "STORM_COMMANDE_RZB";
        private readonly ICommandeManager commandeManager;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly ICommandeTypeManager commandeTypeManager;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="commandeManager">Gestionnaire de commande</param>
        /// <param name="statutCommandeManager">Gestionnaire de status de commande</param>
        /// <param name="commandeTypeManager">Gestionnaire de type de commande</param>
        public CommandeStormImporter(
            ICommandeManager commandeManager,
            IStatutCommandeManager statutCommandeManager,
            ICommandeTypeManager commandeTypeManager)
          : base("COMMANDE", "ME23", "Import STORM")
        {
            this.commandeManager = commandeManager;
            this.statutCommandeManager = statutCommandeManager;
            this.commandeTypeManager = commandeTypeManager;
        }

        /// <inheritdoc/>
        protected override void ImportModel(CommandeSapModel model)
        {
            Logger.Info($"[{Contexte}][{Code}] Import des commandes STORM dans FRED");

            var utilisateur = GetFredIeUtilisateur();
            Logger.Info($"Reception commande STORM : {model.Numero} pour l'utilisateur {utilisateur.Login}");

            var commandeLignes = new List<CommandeLigneEnt>();

            // RG_4005_005
            var societe = GetSociete(model.SocieteComptableCode);
            var ci = GetCI(model.CiCode, model.SocieteComptableCode);
            var devise = GetDevise(model.DeviseIsoCode);

            // RG_4005_006
            CheckDeviseCI(ci, devise);

            var systemeImport = GetSystemImport(StormArticleSystemeImportCode);
            var systemExterne = GetSystemeExterne(StormCommandeSystemeExterneCode);
            var fournisseur = GetFournisseur(model.FournisseurCode, societe);

            var fournisseurPays = GetPays(model.FournisseurPaysCode);
            var facturationPays = GetPays(model.FacturationPaysCode);
            var livraisonPays = GetPays(model.LivraisonPaysCode);
            var commandeType = GetCommandeType(model.CommandeTypeId, commandeTypeManager);

            PaysEnt agencePays = null;
            int? agenceId = null;

            if (!string.IsNullOrWhiteSpace(model.AgenceCode))
            {
                agencePays = GetPays(model.AgencePaysCode);
                agenceId = GetAgence(model.AgenceCode, societe.GroupeId);
            }

            // La commande doit posséder au moins une ligne
            if (model.Lignes == null || model.Lignes.Count == 0)
            {
                throw new FredIeBusinessException($"La commande \"{model.Numero}\" ne contient pas de ligne");
            }

            int numero = 1;
            // Pour chaque ligne il faut trouver la bonne ressource
            foreach (var ligne in model.Lignes)
            {
                // On récupère le code de la ressource via la table de transcodage
                var transcoImport = GetTranscoImport(ligne.ArticleCode, societe, systemeImport);

                // On recherche ensuite la ressource dans le référentiel fixe
                var ressource = GetRessource(transcoImport, societe);

                // Récupère la nature associée à la ligne
                var nature = GetNature(ligne.NatureCode, societe);

                // Cette ressource doit être dans le référentiel étendu 
                GetReferentielEtendu(ressource, nature, societe);

                var unite = GetUnite(ligne.UniteCode);

                // Ajoute la ligne
                commandeLignes.Add(new CommandeLigneEnt()
                {
                    RessourceId = ressource.RessourceId,
                    Libelle = ligne.Libelle,
                    Quantite = ligne.Quantite,
                    PUHT = ligne.PUHT,
                    UniteId = unite.UniteId,
                    AuteurCreationId = utilisateur.UtilisateurId,
                    DateCreation = model.DateCreation,
                    NumeroLigne = numero,
                    NumeroCommandeLigneExterne = ligne.CommandeLigneSap
                });
                numero++;
            }

            // Crée la commande
            var commande = new CommandeEnt()
            {
                NumeroCommandeExterne = model.Numero,
                NumeroContratExterne = model.NumeroContratExterne,
                SystemeExterneId = systemExterne.SystemeExterneId,
                CiId = ci.CiId,
                TypeId = commandeType.CommandeTypeId,
                StatutCommandeId = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutVA).StatutCommandeId,
                Libelle = string.IsNullOrWhiteSpace(model.Libelle) ? $"{systemExterne.LibelleAffiche} {model.Numero}" : model.Libelle,
                Date = model.Date,
                DelaiLivraison = model.DelaiLivraison,
                DateMiseADispo = model.DateMiseADispo,
                MOConduite = model.MOConduite,
                EntretienMecanique = model.EntretienMecanique,
                EntretienJournalier = model.EntretienJournalier,
                Carburant = model.Carburant,
                Lubrifiant = model.Lubrifiant,
                FraisAmortissement = model.FraisAmortissement,
                FraisAssurance = model.FraisAssurance,
                ConditionSociete = model.ConditionSociete,
                ConditionPrestation = model.ConditionPrestation,
                ContactTel = model.ContactTel,
                LivraisonEntete = model.LivraisonEntete,
                LivraisonAdresse = model.LivraisonAdresse,
                LivraisonVille = model.LivraisonVille,
                LivraisonCPostale = model.LivraisonCPostale,
                LivraisonPaysId = livraisonPays.PaysId,
                FacturationAdresse = model.FacturationAdresse,
                FacturationVille = model.FacturationVille,
                FacturationCPostale = model.FacturationCPostale,
                FacturationPaysId = facturationPays.PaysId,
                Justificatif = model.Justificatif,
                CommentaireFournisseur = model.CommentaireFournisseur,
                CommentaireInterne = model.CommentaireInterne,
                AccordCadre = model.AccordCadre,
                FournisseurId = fournisseur.FournisseurId,
                FournisseurAdresse = agenceId.HasValue ? model.AgenceAdresse : model.FournisseurAdresse,
                FournisseurVille = agenceId.HasValue ? model.AgenceVille : model.FournisseurVille,
                FournisseurCPostal = agenceId.HasValue ? model.AgenceCPostal : model.FournisseurCPostal,
                FournisseurPaysId = agenceId.HasValue ? agencePays.PaysId : fournisseurPays.PaysId,
                AgenceId = agenceId,
                DeviseId = devise.DeviseId,
                DateValidation = model.DateValidation ?? DateTime.UtcNow,
                DateCloture = model.DateCloture,
                DateCreation = model.DateCreation ?? DateTime.UtcNow,
                AuteurCreationId = utilisateur.UtilisateurId,
                Lignes = commandeLignes,
            };

            // Enregistre
            commandeManager.AddCommandeExterne(commande);
        }

        private CommandeTypeEnt GetCommandeType(int commandeTypeId, ICommandeTypeManager commandeTypeManager)
        {
            var commandeType = commandeTypeManager.Get(commandeTypeId);
            if (commandeType == null)
            {
                // RG_4005_013
                throw new FredIeBusinessException($"'Commande Type Id' \"{commandeTypeId}\" non reconnu dans FRED");
            }
            return commandeType;
        }
    }
}
