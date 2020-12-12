using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fred.Business.Commande;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;

namespace Fred.Business.Reception.Services
{
    /// <summary>
    /// Genere automatiquement les reception pour les commandes abonnement
    /// </summary>
    public class ReceptionsAutomaticGenerator : IReceptionsAutomaticGenerator
    {


        private readonly IDepenseRepository depenseRepository;

        private readonly IUnitOfWork uow;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IDepenseTypeManager depenseTypeManager;
        private readonly ICommandeManager commandeManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IReceptionValidator receptionValidator;
        private readonly ICommandeRepository commandeRepository;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uow">uow</param>
        public ReceptionsAutomaticGenerator(IUtilisateurManager utilisateurManager,
                IDepenseTypeManager depenseTypeManager,
                ICommandeManager commandeManager,
                IDatesClotureComptableManager datesClotureComptableManager,
                IReceptionValidator receptionValidator,
                IDepenseRepository depenseRepository,
                ICommandeRepository commandeRepository,
                IUnitOfWork uow)
        {
            this.utilisateurManager = utilisateurManager;
            this.depenseTypeManager = depenseTypeManager;
            this.commandeManager = commandeManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.receptionValidator = receptionValidator;
            this.depenseRepository = depenseRepository;
            this.commandeRepository = commandeRepository;
            this.uow = uow;
        }

        /// <summary>
        /// Génération automatique des réceptions des commandes abonnement
        /// </summary>
        public void GenerateReceptions()
        {

            try
            {
                UtilisateurEnt fredIe = utilisateurManager.GetByLogin("fred_ie");
                DepenseTypeEnt typeReception = depenseTypeManager.Get(Entities.DepenseType.Reception.ToIntValue());

                var commandeAbos = commandeManager.GetCommandeAbonnementList().ToList();
                var receptions = depenseRepository.GetLastReceptionByCommandeLigneId(commandeAbos.SelectMany(x => x.Lignes).Select(f => f.CommandeLigneId).ToList());

                foreach (var cmd in commandeAbos)
                {
                    cmd.DatePremiereReception = cmd.DatePremiereReception ?? cmd.DateProchaineReception;
                    var dureeAbo = 0;
                    var dateProchaineReception = default(DateTime?);

                    foreach (var l in cmd.Lignes)
                    {
                        dureeAbo = cmd.DureeAbonnement.Value;
                        dateProchaineReception = cmd.DateProchaineReception;

                        var lastReception = receptions[l.CommandeLigneId];

                        while (dateProchaineReception.HasValue && dateProchaineReception.Value.Date <= DateTime.UtcNow.Date && dureeAbo > 0)
                        {
                            var reception = CreateReceptionAbonnement(cmd, l, lastReception, fredIe.UtilisateurId, dureeAbo, dateProchaineReception, typeReception);

                            // Ajout dans le contexte EF sans faire de Save
                            QuickAddReception(reception);

                            // Incrémentation de la date en fonction de la fréquence abonnement
                            dateProchaineReception = CommandeManagerHelper.GetDateProchaineReception(dateProchaineReception.Value, cmd.FrequenceAbonnement.Value);

                            // Dernière réception devient la réception courante car elle n'est pas encore stockée en BD
                            lastReception = reception;

                            // Décrémentation du nombre de réceptions à générer (donc de la durée de l'abonnement)
                            dureeAbo--;
                        }
                    }
                    cmd.DateProchaineReception = dateProchaineReception;
                    cmd.DureeAbonnement = dureeAbo;
                    commandeRepository.Update(cmd);
                }
                // Afin de simuler une transaction, on enregistre en BD qu'à la fin
                this.uow.Save();
            }
            catch (ValidationException ve)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(ve, "FRED: Erreur lors de la génération automatique des réceptions des commandes abonnement.");
                throw;
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e, "FRED: Erreur lors de la génération automatique des réceptions des commandes abonnement.");
                throw new FredBusinessException("Erreur lors de la génération des réceptions des commandes abonnement : " + e.Message, e);
            }
        }



        /// <summary>
        ///   Validation + Ajout d'une réception sans faire de Save
        /// </summary>
        /// <param name="reception">Réception</param>
        private void QuickAddReception(DepenseAchatEnt reception)
        {

            var result = receptionValidator.Validate(reception);

            if (!result.IsValid)
            {
                throw new ValidationException("Erreur de validation");
            }

            // RG_2863_110 : Vérifier si la période est "Bloquée en réception" pour le CI de la commande
            if (datesClotureComptableManager.IsBlockedInReception(reception.CiId.Value, reception.Date.Value.Year, reception.Date.Value.Month))
            {
                reception.DateComptable = datesClotureComptableManager.GetNextUnblockedInReceptionPeriod(reception.CiId.Value, reception.Date.Value);
            }
            else
            {
                reception.DateComptable = reception.Date;
            }
            depenseRepository.Insert(reception);
        }

        /// <summary>
        ///   Création de la réception auto générée
        /// </summary>
        /// <param name="cmd">Commande</param>
        /// <param name="l">Ligne de commande</param>
        /// <param name="lastReception">Dernière réception</param>
        /// <param name="fredIeUserId">Utilisateur Fred</param>
        /// <param name="dureeAbo">Nombre de réceptions</param>
        /// <param name="dateProchaineReception">Date prochaine récpetion</param>
        /// <param name="typeReception">Depense Type reception</param>
        /// <returns>Réception</returns>
        private DepenseAchatEnt CreateReceptionAbonnement(CommandeEnt cmd, CommandeLigneEnt l, DepenseAchatEnt lastReception, int fredIeUserId, int dureeAbo, DateTime? dateProchaineReception, DepenseTypeEnt typeReception)
        {
            var reception = new DepenseAchatEnt
            {
                DateCreation = DateTime.UtcNow,
                AuteurCreationId = fredIeUserId,
                Date = dateProchaineReception,
                DateComptable = dateProchaineReception,
                DepenseTypeId = typeReception.DepenseTypeId,
                CommandeLigneId = l.CommandeLigneId,
                CiId = cmd.CiId,
                FournisseurId = cmd.FournisseurId,
                DeviseId = cmd.DeviseId,
                Libelle = lastReception != null ? lastReception.Libelle : l.Libelle,
                TacheId = lastReception != null ? lastReception.TacheId : l.TacheId,
                RessourceId = lastReception != null ? lastReception.RessourceId : l.RessourceId,
                Quantite = lastReception != null ? lastReception.Quantite : (l.Quantite / dureeAbo),
                UniteId = l.UniteId,
                PUHT = l.PUHT,
                Commentaire = "Réception d'abonnement générée automatiquement",
                NumeroBL = "N/A",
                AfficherPuHt = true,
                AfficherQuantite = true
            };

            reception.QuantiteDepense = reception.Quantite;

            return reception;
        }


    }
}
