using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Commande;
using Fred.Framework.Extensions;

namespace Fred.Business.Commande
{
    /// <summary>
    ///   Methodes d'extension pour CommandeEnt
    /// </summary>
    public static class CommandeEntExtension
    {
        #region CommandeEnt

        /// <summary>
        ///   Calcul du montant Hors taxe
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>CommandeEnt avec montant hors taxe</returns>
        public static CommandeEnt ComputeMontantHT(this CommandeEnt commande)
        {
            commande.MontantHT = 0;

            commande.Lignes.ComputeMontantHT();

            if (commande.Lignes != null)
            {
                foreach (var ligne in commande.Lignes)
                {
                    var ligneMontantHT = ligne.Quantite * ligne.PUHT;
                    if (ligne.AvenantLigne != null && ligne.AvenantLigne.IsDiminution)
                    {
                        commande.MontantHT -= ligneMontantHT;
                    }
                    else
                    {
                        commande.MontantHT += ligneMontantHT;
                    }
                }
            }
            return commande;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Réceptionné
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>Commande avec Montant HT réceptionné</returns>
        public static CommandeEnt ComputeMontantHTReceptionne(this CommandeEnt commande)
        {
            commande.MontantHTReceptionne = commande.Lignes != null ? commande.Lignes.Where(l => !l.IsDeleted).Sum(l => l.MontantHTReceptionne) : 0;

            return commande;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Soldé
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>Commande avec montant HT soldé</returns>
        public static CommandeEnt ComputeMontantHTSolde(this CommandeEnt commande)
        {
            commande.ComputeMontantHT();
            commande.ComputeMontantHTReceptionne();
            commande.MontantHTSolde = commande.MontantHT - commande.MontantHTReceptionne;

            return commande;
        }

        /// <summary>
        ///   Calcul du pourcentage réceptionné
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>Commande avec pourcentage réceptionné</returns>
        public static CommandeEnt ComputePourcentageReceptionne(this CommandeEnt commande)
        {
            commande.ComputeMontantHT();
            commande.ComputeMontantHTReceptionne();
            commande.PourcentageReceptionne = commande.MontantHT > 0 ? Math.Round(commande.MontantHTReceptionne / commande.MontantHT * 100, 2) : 100;

            return commande;
        }

        /// <summary>
        ///   Calcul du Solde FAR
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <param name="dateDebut">Date de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Commande avec Solde FAR</returns>
        public static CommandeEnt ComputeSoldeFar(this CommandeEnt commande, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commande.Lignes?.ComputeSoldeFar(dateDebut, dateFin);
            commande.SoldeFar = commande.Lignes != null ? commande.Lignes.Sum(l => l.SoldeFar) : 0;

            return commande;
        }

        /// <summary>
        ///   Calcul du montant facturé
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <returns>Commande avec pourcentage réceptionné</returns>
        public static CommandeEnt ComputeMontantHTFacture(this CommandeEnt commande)
        {
            commande.Lignes?.ComputeMontantHTFacture();
            commande.MontantHTFacture = commande.Lignes != null ? commande.Lignes.Sum(l => l.MontantHTFacture) : 0;
            return commande;
        }

        /// <summary>
        ///   Calcul du Montant Facturé de toutes les réceptions
        ///   RG_3656_064 : Fonction de calcul du Montant Facturé d’une réception à une date J
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J
        /// </summary>
        /// <param name="commande">Commande Ligne</param>
        /// <param name="dateDebut">Date de comparaison pour calcul du Montant Facturé</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>CommandeEnt avec Montant Facturé</returns>
        public static CommandeEnt ComputeMontantFacture(this CommandeEnt commande, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commande.Lignes?.ComputeMontantFacture(dateDebut, dateFin);
            commande.MontantFacture = commande.Lignes != null ? commande.Lignes.Sum(r => r.MontantFacture) : 0;
            return commande;
        }

        /// <summary>
        ///   Calcul de tous les champs calculés de l'entité CommandeEnt
        /// </summary>
        /// <param name="commande">Commande</param>        
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Commande avec tous ces champs calculés renseignés</returns>
        public static CommandeEnt ComputeAll(this CommandeEnt commande, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            if (commande != null)
            {
                commande.Lignes?.ComputeAll(dateDebut, dateFin);
                commande.ComputeMontantHT();
                commande.ComputeMontantHTReceptionne();
                commande.ComputeMontantHTSolde();
                commande.ComputePourcentageReceptionne();
                commande.ComputeSoldeFar(dateDebut, dateFin);
                commande.ComputeMontantHTFacture();
                commande.ComputeMontantFacture(dateDebut, dateFin);
            }
            return commande;
        }

        /// <summary>
        /// Indique si la commande est externe.
        /// </summary>
        /// <param name="commande">La commande concernée.</param>
        /// <returns>True si la commande est externe, sinon false.</returns>
        public static bool IsExterne(this CommandeEnt commande)
        {
            return commande.SystemeExterneId.HasValue && commande.SystemeExterneId.Value != 0;
        }

        #endregion

        #region Liste de CommandeEnt

        /// <summary>
        ///   Calcul du montant Hors taxe
        /// </summary>
        /// <param name="commandes">Liste des Commandes</param>
        /// <returns>CommandeEnt avec montant hors taxe</returns>
        public static IEnumerable<CommandeEnt> ComputeMontantHT(this IEnumerable<CommandeEnt> commandes)
        {
            commandes?.ForEach(commande => commande.ComputeMontantHT());
            return commandes;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Réceptionné
        /// </summary>
        /// <param name="commandes">Liste des Commandes</param>
        /// <returns>Liste des commandes avec Montant HT réceptionné</returns>
        public static IEnumerable<CommandeEnt> ComputeMontantHTReceptionne(this IEnumerable<CommandeEnt> commandes)
        {
            commandes?.ForEach(commande => commande.ComputeMontantHTReceptionne());
            return commandes;
        }

        /// <summary>
        ///   Calcul du Montant Hors Taxe Soldé
        /// </summary>
        /// <param name="commandes">Liste des Commandes</param>
        /// <returns>Liste des commandes avec montant HT soldé</returns>
        public static IEnumerable<CommandeEnt> ComputeMontantHTSolde(this IEnumerable<CommandeEnt> commandes)
        {
            commandes?.ForEach(commande => commande.ComputeMontantHTSolde());
            return commandes;
        }

        /// <summary>
        ///   Calcul du pourcentage réceptionné
        /// </summary>
        /// <param name="commandes">Liste des Commandes</param>
        /// <returns>Liste des commandes avec pourcentage réceptionné</returns>
        public static IEnumerable<CommandeEnt> ComputePourcentageReceptionne(this IEnumerable<CommandeEnt> commandes)
        {
            commandes?.ForEach(commande => commande.ComputePourcentageReceptionne());
            return commandes;
        }

        /// <summary>
        ///   Calcul du Solde FAR dans une liste de commande
        /// </summary>
        /// <param name="commandes">Liste de Commandes</param>
        /// <param name="dateDebut">Date de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste de Commandes avec Solde FAR</returns>
        public static IEnumerable<CommandeEnt> ComputeSoldeFar(this IEnumerable<CommandeEnt> commandes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandes?.ForEach(commande => commande.ComputeSoldeFar(dateDebut, dateFin));
            return commandes;
        }

        /// <summary>
        ///   Calcul du montant facturé
        /// </summary>
        /// <param name="commandes">Liste des Commandes</param>
        /// <returns>Liste des commandes avec MontantHTFacture</returns>
        public static IEnumerable<CommandeEnt> ComputeMontantHTFacture(this IEnumerable<CommandeEnt> commandes)
        {
            commandes?.ForEach(commande => commande.ComputeMontantHTFacture());
            return commandes;
        }

        /// <summary>
        ///   Calcul du Montant Facturé de toutes les réceptions
        ///   RG_3656_064 : Fonction de calcul du Montant Facturé d’une réception à une date J
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J
        /// </summary>
        /// <param name="commandes">Liste de Commandes</param>
        /// <param name="dateDebut">Date de comparaison pour calcul du Montant Facturé</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste de CommandeEnt avec Montant Facturé</returns>      
        public static IEnumerable<CommandeEnt> ComputeMontantFacture(this IEnumerable<CommandeEnt> commandes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandes?.ForEach(commande => commande.ComputeMontantFacture(dateDebut, dateFin));
            return commandes;
        }

        /// <summary>
        ///   Calcul tous les champs calculés de la commande pour une liste de commandes
        /// </summary>
        /// <param name="commandes">Liste de Commandes</param>        
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste de commandes avec tous ces champs calculés renseignés</returns>
        public static IEnumerable<CommandeEnt> ComputeAll(this IEnumerable<CommandeEnt> commandes, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            commandes?.ForEach(commande => commande.ComputeAll(dateDebut, dateFin));
            return commandes;
        }
        #endregion
    }
}
