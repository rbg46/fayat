using System;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Framework.Extensions;

namespace Fred.Business.Commande
{
    /// <summary>
    ///   Commande helper
    /// </summary>
    public static class CommandeManagerHelper
    {
        /// <summary>
        ///   Récupération de la prochaine date de génération d'une réception en fonction de la fréquence de l'abonnement et de la dernière date de génération
        /// </summary>
        /// <param name="currentDate">Dernière date de génération réception auto</param>
        /// <param name="frequenceAbo">Fréquence abonnement</param>
        /// <returns>Nouvelle date de génération</returns>
        public static DateTime? GetDateProchaineReception(DateTime currentDate, int frequenceAbo)
        {
            if (frequenceAbo == FrequenceAbonnement.Jour.ToIntValue())
            {
                return currentDate.AddDays(1);
            }

            if (frequenceAbo == FrequenceAbonnement.Semaine.ToIntValue())
            {
                return currentDate.AddDays(7);
            }

            if (frequenceAbo == FrequenceAbonnement.Mois.ToIntValue())
            {
                return currentDate.AddMonths(1);
            }

            if (frequenceAbo == FrequenceAbonnement.Trimestre.ToIntValue())
            {
                return currentDate.AddMonths(3);
            }

            if (frequenceAbo == FrequenceAbonnement.Annee.ToIntValue())
            {
                return currentDate.AddYears(1);
            }

            return null;
        }

        /// <summary>
        ///   Clone la commande
        /// </summary>
        /// <param name="commandeEnt">commande à cloner.</param>
        /// <returns>La commande clonée, sinon nulle.</returns>
        public static CommandeEnt CloneCommande(CommandeEnt commandeEnt)
        {
            return new CommandeEnt
            {
                CommandeId = commandeEnt.CommandeId,
                TypeId = commandeEnt.TypeId,
                FournisseurId = commandeEnt.FournisseurId,
                CiId = commandeEnt.CiId,
                StatutCommandeId = commandeEnt.StatutCommandeId,
                ContactId = commandeEnt.ContactId,
                SuiviId = commandeEnt.SuiviId,
                Type = commandeEnt.Type,
                Fournisseur = commandeEnt.Fournisseur,
                CI = commandeEnt.CI,
                Contact = commandeEnt.Contact,
                Suivi = commandeEnt.Suivi,
                DeviseId = commandeEnt.DeviseId,
                Devise = commandeEnt.Devise,
                AuteurCreationId = commandeEnt.AuteurCreationId,
                ValideurId = commandeEnt.ValideurId,
                Numero = commandeEnt.Numero,
                Libelle = commandeEnt.Libelle,
                Date = commandeEnt.Date,
                DelaiLivraison = commandeEnt.DelaiLivraison,
                DateMiseADispo = commandeEnt.DateMiseADispo,
                MOConduite = commandeEnt.MOConduite,
                EntretienMecanique = commandeEnt.EntretienMecanique,
                EntretienJournalier = commandeEnt.EntretienJournalier,
                Carburant = commandeEnt.Carburant,
                Lubrifiant = commandeEnt.Lubrifiant,
                FraisAmortissement = commandeEnt.FraisAmortissement,
                FraisAssurance = commandeEnt.FraisAssurance,
                ConditionSociete = commandeEnt.ConditionSociete,
                ConditionPrestation = commandeEnt.ConditionPrestation,
                ContactTel = commandeEnt.ContactTel,
                DateCreation = commandeEnt.DateCreation,
                DateModification = commandeEnt.DateModification,
                AuteurModificationId = commandeEnt.AuteurModificationId,
                DateValidation = commandeEnt.DateValidation,
                LivraisonEntete = commandeEnt.LivraisonEntete,
                LivraisonAdresse = commandeEnt.LivraisonAdresse,
                LivraisonVille = commandeEnt.LivraisonVille,
                LivraisonCPostale = commandeEnt.LivraisonCPostale,
                LivraisonPaysId = commandeEnt.LivraisonPaysId,
                LivraisonPays = commandeEnt.LivraisonPays,
                FacturationAdresse = commandeEnt.FacturationAdresse,
                FacturationVille = commandeEnt.FacturationVille,
                FacturationCPostale = commandeEnt.FacturationCPostale,
                FacturationPaysId = commandeEnt.FacturationPaysId,
                FacturationPays = commandeEnt.FacturationPays,
                FournisseurAdresse = commandeEnt.FournisseurAdresse,
                FournisseurVille = commandeEnt.FournisseurVille,
                FournisseurCPostal = commandeEnt.FournisseurCPostal,
                FournisseurPaysId = commandeEnt.FournisseurPaysId,
                FournisseurPays = commandeEnt.FournisseurPays,
                DateSuppression = commandeEnt.DateSuppression,
                AuteurSuppressionId = commandeEnt.AuteurSuppressionId,
                DateCloture = commandeEnt.DateCloture,
                AccordCadre = commandeEnt.AccordCadre,
                Justificatif = commandeEnt.Justificatif,
                CommentaireFournisseur = commandeEnt.CommentaireFournisseur,
                CommentaireInterne = commandeEnt.CommentaireInterne,
                CommandeManuelle = commandeEnt.CommandeManuelle,
                IsAbonnement = commandeEnt.IsAbonnement,
                FrequenceAbonnement = commandeEnt.FrequenceAbonnement,
                DateProchaineReception = commandeEnt.DateProchaineReception,
                Lignes = commandeEnt.Lignes
            };
        }

        /// <summary>
        ///   Indique si la commande peur être validée
        /// </summary>
        /// <param name="commandeEnt">La commande à contrôler</param>
        /// <param name="currentUserId">Identifiant de l'utilisateur courant</param>
        /// <returns>
        ///   Vrai si la saisie de la commande est finalisée et que l'utilisateur à les droits de validation, Faux si
        ///   incomplète
        /// </returns>
        public static bool IsVisable(CommandeEnt commandeEnt, int currentUserId)
        {
            return commandeEnt?.IsCreated == false && !commandeEnt.IsStatutBrouillon && commandeEnt.AuteurCreationId != currentUserId;
        }
    }
}
