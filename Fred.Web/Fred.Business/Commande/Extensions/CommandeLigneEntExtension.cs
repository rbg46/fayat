using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Depense;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.Business.Commande
{
    /// <summary>
    ///   Methodes d'extension pour CommandeLigneEnt
    /// </summary>
    public static class CommandeLigneEntExtension
    {
        #region Construction de listes

        /// <summary>
        ///   Récupération de la liste des réceptions
        /// </summary>
        /// <param name="commandeLigne">CommandeLigne</param>
        /// <returns>CommandeLigne avec Liste des Réceptions</returns>
        public static CommandeLigneEnt GetDepensesReception(this CommandeLigneEnt commandeLigne)
        {
            if (commandeLigne.AllDepenses != null)
            {
                var receptions = commandeLigne.AllDepenses.Where(r => !r.DateSuppression.HasValue && r.DepenseType.Code == DepenseType.Reception.ToIntValue());
                var receptionsList = receptions.ToList();
                commandeLigne.DepensesReception = receptionsList;
            }
            return commandeLigne;
        }

        /// <summary>
        ///   Récupération de la liste des dépenses Facture
        /// </summary>
        /// <param name="commandeLigne">CommandeLigne</param>
        /// <returns>CommandeLigne avec Liste des Réceptions</returns>
        public static CommandeLigneEnt GetDepensesFacture(this CommandeLigneEnt commandeLigne)
        {
            if (commandeLigne.AllDepenses != null)
            {
                List<DepenseAchatEnt> depenses = new List<DepenseAchatEnt>();
                commandeLigne.AllDepenses.Where(r => !r.DateSuppression.HasValue).ForEach(x =>
                {
                    var list = x.Depenses?.Where(r => r.DepenseType.Code == DepenseType.Facture.ToIntValue());
                    if (list?.Any() == true)
                    {
                        depenses.AddRange(list.ToList());
                    }
                });
                commandeLigne.DepensesFar = depenses;
            }
            return commandeLigne;
        }

        /// <summary>
        ///   Récupération de la liste des dépenses Facture Ecart
        /// </summary>
        /// <param name="commandeLigne">CommandeLigne</param>
        /// <returns>CommandeLigne avec Liste des Réceptions</returns>
        public static CommandeLigneEnt GetDepensesFactureEcart(this CommandeLigneEnt commandeLigne)
        {
            if (commandeLigne.AllDepenses != null)
            {
                List<DepenseAchatEnt> depenses = new List<DepenseAchatEnt>();
                commandeLigne.AllDepenses.Where(r => !r.DateSuppression.HasValue).ForEach(x =>
                {
                    var list = x.Depenses?.Where(r => r.DepenseType.Code == DepenseType.FactureEcart.ToIntValue());
                    if (list?.Any() == true)
                    {
                        depenses.AddRange(list.ToList());
                    }
                });
                commandeLigne.DepensesFar = depenses;
            }
            return commandeLigne;
        }

        /// <summary>
        ///   Récupération de la liste des Dépenses FAR    
        /// </summary>
        /// <param name="commandeLigne">CommandeLigne</param>    
        /// <returns>CommandeLigne avec Liste des Réceptions</returns>
        public static CommandeLigneEnt GetDepensesFar(this CommandeLigneEnt commandeLigne)
        {
            if (commandeLigne.AllDepenses != null)
            {
                List<DepenseAchatEnt> depenses = new List<DepenseAchatEnt>();
                commandeLigne.AllDepenses.Where(r => !r.DateSuppression.HasValue).ForEach(x =>
                {
                    var list = x.Depenses?.Where(r => r.DepenseType.Code == DepenseType.AjustementFar.ToIntValue() || r.DepenseType.Code == DepenseType.ExtourneFar.ToIntValue());
                    if (list?.Any() == true)
                    {
                        depenses.AddRange(list.ToList());
                    }
                });
                commandeLigne.DepensesFar = depenses;
            }
            return commandeLigne;
        }

        #endregion

        #region CommandeLigneEnt

        /// <summary>
        ///   Calcul du montant Hors taxe
        /// </summary>
        /// <param name="commandeLigne">Commande</param>
        /// <returns>CommandeLigneEnt avec montant hors taxe</returns>
        public static CommandeLigneEnt ComputeMontantHT(this CommandeLigneEnt commandeLigne)
        {
            var montantHT = commandeLigne.Quantite * commandeLigne.PUHT;
            if (commandeLigne.AvenantLigne != null && commandeLigne.AvenantLigne.IsDiminution)
            {
                montantHT = -montantHT;
            }
            commandeLigne.MontantHT = montantHT;
            return commandeLigne;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Réceptionné
        /// </summary>
        /// <param name="commandeLigne">Commande</param>
        /// <returns>Commande avec Montant HT réceptionné</returns>
        public static CommandeLigneEnt ComputeMontantHTReceptionne(this CommandeLigneEnt commandeLigne)
        {
            commandeLigne.GetDepensesReception();
            commandeLigne.MontantHTReceptionne = commandeLigne.DepensesReception != null ? commandeLigne.DepensesReception.Sum(r => r.Quantite * r.PUHT) : 0;

            return commandeLigne;
        }

        /// <summary>
        ///   Calcul de la quantité réceptionnée
        /// </summary>
        /// <param name="commandeLigne">Ligne de commande</param>
        /// <returns>Ligne de Commande avec Quantité Réceptionnée</returns>
        public static CommandeLigneEnt ComputeQuantiteReceptionnee(this CommandeLigneEnt commandeLigne)
        {
            commandeLigne.GetDepensesReception();
            commandeLigne.QuantiteReceptionnee = commandeLigne.DepensesReception != null ? commandeLigne.DepensesReception.Sum(r => r.Quantite) : 0;

            return commandeLigne;
        }

        /// <summary>
        ///   Calcul du pourcentage réceptionné
        /// </summary>
        /// <param name="commandeLigne">Commande</param>
        /// <returns>Commande avec pourcentage réceptionné</returns>
        public static CommandeLigneEnt ComputeMontantHTSolde(this CommandeLigneEnt commandeLigne)
        {
            commandeLigne.ComputeMontantHT();
            commandeLigne.ComputeMontantHTReceptionne();
            commandeLigne.MontantHTSolde = commandeLigne.MontantHT - commandeLigne.MontantHTReceptionne;
            return commandeLigne;
        }

        /// <summary>
        ///   Calcul du montant facturé
        /// </summary>
        /// <param name="commandeLigne">Commande</param>
        /// <returns>Commande avec pourcentage réceptionné</returns>
        public static CommandeLigneEnt ComputeMontantHTFacture(this CommandeLigneEnt commandeLigne)
        {
            commandeLigne.GetDepensesReception();
            commandeLigne.DepensesReception.ComputeMontantFacturationReception();
            commandeLigne.MontantHTFacture = commandeLigne.DepensesReception != null ? commandeLigne.DepensesReception.Sum(reception => reception.MontantFacturationReception) : 0;
            return commandeLigne;
        }

        /// <summary>
        ///   Calcul du Solde Far de toutes les réceptions
        /// </summary>
        /// <param name="commandeLigne">Commande</param>
        /// <param name="dateDebut">Date de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>CommandeLigneEnt avec Solde FAR</returns>
        public static CommandeLigneEnt ComputeSoldeFar(this CommandeLigneEnt commandeLigne, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandeLigne.GetDepensesReception();
            commandeLigne.DepensesReception.ComputeSoldeFar(dateDebut, dateFin);
            commandeLigne.SoldeFar = commandeLigne.DepensesReception != null ? commandeLigne.DepensesReception.Sum(r => r.SoldeFar) : 0;
            return commandeLigne;
        }

        /// <summary>
        ///   Calcul du Montant Facturé de toutes les réceptions
        ///   RG_3656_064 : Fonction de calcul du Montant Facturé d’une réception à une date J
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J
        /// </summary>
        /// <param name="commandeLigne">Commande Ligne</param>
        /// <param name="dateDebut">Date de comparaison pour calcul du Montant Facturé</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>CommandeLigneEnt avec Montant Facturé</returns>
        public static CommandeLigneEnt ComputeMontantFacture(this CommandeLigneEnt commandeLigne, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandeLigne.GetDepensesReception();
            commandeLigne.DepensesReception.ComputeMontantFacture(dateDebut, dateFin);
            commandeLigne.MontantFacture = commandeLigne.DepensesReception != null ? commandeLigne.DepensesReception.Sum(r => r.MontantFacture) : 0;
            return commandeLigne;
        }

        /// <summary>
        ///   Calcul tous les champs calculé d'une CommandeLigneEnt
        /// </summary>
        /// <param name="commandeLigne">CommandeLigneEnt</param>        
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>CommandeLigneEnt avec tous ces champs calculé</returns>
        public static CommandeLigneEnt ComputeAll(this CommandeLigneEnt commandeLigne, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            if (commandeLigne != null)
            {
                commandeLigne.AllDepenses?.ComputeAll(dateDebut, dateFin);
                commandeLigne.GetDepensesFacture();
                commandeLigne.GetDepensesFactureEcart();
                commandeLigne.GetDepensesFar();
                commandeLigne.ComputeMontantHT();
                commandeLigne.ComputeMontantHTReceptionne();
                commandeLigne.ComputeQuantiteReceptionnee();
                commandeLigne.ComputeMontantHTSolde();
                commandeLigne.ComputeMontantHTFacture();
                commandeLigne.ComputeSoldeFar(dateDebut, dateFin);
                commandeLigne.ComputeMontantFacture(dateDebut, dateFin);
            }

            return commandeLigne;
        }

        #endregion

        #region Liste de CommandeLigneEnt

        /// <summary>
        ///   Calcul du montant Hors taxe
        /// </summary>
        /// <param name="commandeLignes">Liste des commandeLignes</param>
        /// <returns>CommandeLigneEnt avec montant hors taxe</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeMontantHT(this IEnumerable<CommandeLigneEnt> commandeLignes)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeMontantHT());
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Réceptionné
        /// </summary>
        /// <param name="commandeLignes">Liste des commandeLignes</param>
        /// <returns>Liste des commandeLignes avec Montant HT réceptionné</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeMontantHTReceptionne(this IEnumerable<CommandeLigneEnt> commandeLignes)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeMontantHTReceptionne());
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul de la quantité réceptionnée
        /// </summary>
        /// <param name="commandeLignes">Liste de Ligne de Commande</param>
        /// <returns>Liste de Ligne de Commandes avec Quantité Réceptionnée</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeQuantiteReceptionnee(this IEnumerable<CommandeLigneEnt> commandeLignes)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeQuantiteReceptionnee());
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Soldé
        /// </summary>
        /// <param name="commandeLignes">Liste des commandeLignes</param>
        /// <returns>Liste des commandeLignes avec montant HT soldé</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeMontantHTSolde(this IEnumerable<CommandeLigneEnt> commandeLignes)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeMontantHTSolde());
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul du montant facturé
        /// </summary>
        /// <param name="commandeLignes">Liste des commandeLignes</param>
        /// <returns>Liste des commandeLignes avec pourcentage réceptionné</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeMontantHTFacture(this IEnumerable<CommandeLigneEnt> commandeLignes)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeMontantHTFacture());
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul du Solde Far de toutes les réceptions
        /// </summary>
        /// <param name="commandeLignes">Liste de commandeLigne</param>
        /// <param name="dateDebut">Date de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>liste de CommandeLigneEnt avec Solde FAR</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeSoldeFar(this IEnumerable<CommandeLigneEnt> commandeLignes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeSoldeFar(dateDebut, dateFin));
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul du Montant Facturé de toutes les réceptions
        ///   RG_3656_064 : Fonction de calcul du Montant Facturé d’une réception à une date J
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J
        /// </summary>
        /// <param name="commandeLignes">Liste de Commande Ligne</param>
        /// <param name="dateDebut">Date de comparaison pour calcul du Montant Facturé</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>liste de CommandeLigneEnt avec Montant Facturé</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeMontantFacture(this IEnumerable<CommandeLigneEnt> commandeLignes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeMontantFacture(dateDebut, dateFin));
            return commandeLignes;
        }

        /// <summary>
        ///   Calcul tous les champs calculé d'une CommandeLigneEnt
        /// </summary>
        /// <param name="commandeLignes">CommandeLigneEnt</param>        
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste de CommandeLigneEnt avec tous ces champs calculé</returns>
        public static IEnumerable<CommandeLigneEnt> ComputeAll(this IEnumerable<CommandeLigneEnt> commandeLignes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandeLignes?.ForEach(commandeLigne => commandeLigne.ComputeAll(dateDebut, dateFin));
            return commandeLignes;
        }
        #endregion
    }
}
