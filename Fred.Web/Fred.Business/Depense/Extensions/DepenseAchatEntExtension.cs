using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Facturation;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.Business.Depense
{
    /// <summary>
    /// Methodes d'extension pour DepenseAchatEntExtension
    /// </summary>
    public static class DepenseAchatEntExtension
    {
        /// <summary>
        /// Calcul du montant Hors Taxe (Montant Réceptionné)
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <returns>Dépense avec montant hors taxe</returns>
        public static DepenseAchatEnt ComputeMontantHT(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.MontantHT = depense.Quantite * depense.PUHT;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant des facturations de type Facture
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Montant facturation facture</returns>
        public static DepenseAchatEnt ComputeMontantFacturationFacture(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.MontantFacturationFacture = depense.FacturationsFacture != null ? depense.FacturationsFacture.Sum(x => x.MontantHT) : 0;
            }
            return depense;
        }

        /// <summary>
        /// Calcul de la quantité des facturations de type Facture
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Quantité facturation facture</returns>
        public static DepenseAchatEnt ComputeQuantiteFacturationFacture(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.QuantiteFacturationFacture = depense.FacturationsFacture != null ? depense.FacturationsFacture.Sum(x => x.Quantite) : 0;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant des facturations de type FactureEcart
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Montant facturation facture écart</returns>
        public static DepenseAchatEnt ComputeMontantFacturationFactureEcart(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.MontantFacturationFactureEcart = depense.FacturationsFactureEcart != null ? depense.FacturationsFactureEcart.Sum(x => x.MontantHT) : 0;
            }
            //// .Sum(x => x.MontantHT) : 0;
            return depense;
        }

        /// <summary>
        /// Calcul de la quantité des facturations de type FactureEcart
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Quantité facturation facture écart</returns>
        public static DepenseAchatEnt ComputeQuantiteFacturationFactureEcart(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.QuantiteFacturationFactureEcart = depense.FacturationsFactureEcart != null ? depense.FacturationsFactureEcart.Sum(x => x.Quantite) : 0;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant des facturations de type Reception
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Montant Réception</returns>
        public static DepenseAchatEnt ComputeMontantFacturationReception(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.MontantFacturationReception = depense.FacturationsReception != null ? depense.FacturationsReception.Sum(x => x.MontantHT) : 0;
            }
            //// .Sum(x => x.MontantHT) : 0;
            return depense;
        }

        /// <summary>
        /// Calcul de la quantité des facturations de type Réception
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Quantité facturation Réception</returns>
        public static DepenseAchatEnt ComputeQuantiteFacturationReception(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.QuantiteFacturationReception = depense.FacturationsReception != null ? depense.FacturationsReception.Sum(x => x.Quantite) : 0;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant des facturations de tous types
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Montant facturation</returns>
        public static DepenseAchatEnt ComputeMontantFacturation(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.ComputeMontantFacturationFacture();
                depense.ComputeMontantFacturationFactureEcart();
                depense.ComputeMontantFacturationReception();

                depense.MontantFacturation = depense.MontantFacturationFacture + depense.MontantFacturationFactureEcart + depense.MontantFacturationReception;
            }
            return depense;
        }

        /// <summary>
        /// Calcul de la quantité des facturations de tous types
        /// </summary>
        /// <param name="depense">Depense</param>
        /// <returns>Dépense avec Quantité facturation</returns>
        public static DepenseAchatEnt ComputeQuantiteFacturation(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.ComputeQuantiteFacturationFacture();
                depense.ComputeQuantiteFacturationFactureEcart();
                depense.ComputeQuantiteFacturationReception();

                depense.QuantiteFacturation = depense.QuantiteFacturationFacture + depense.QuantiteFacturationFactureEcart + depense.QuantiteFacturationReception;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du Solde Far
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Dépense avec Solde FAR</returns>
        public static DepenseAchatEnt ComputeSoldeFar(this DepenseAchatEnt depense, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            if (depense != null)
            {
                decimal ajustementFarSum = 0;
                if (depense.Depenses?.Count > 0)
                {
                    List<int> depenseTypeCodes = new List<int> { DepenseType.ExtourneFar.ToIntValue(), DepenseType.AjustementFar.ToIntValue() };
                    ajustementFarSum = depense.Depenses
                                              .Where(x => depenseTypeCodes.Contains(x.DepenseTypeId.Value)
                                                          && x.DateOperation.HasValue
                                                          && (!dateDebut.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) >= ((dateDebut.Value.Year * 100) + dateDebut.Value.Month))
                                                          && (!dateFin.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) <= ((dateFin.Value.Year * 100) + dateFin.Value.Month)))
                                              .Sum(x => x.Quantite * x.PUHT);
                }

                depense.SoldeFar = (depense.PUHT * depense.Quantite) + ajustementFarSum;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant facturé hors écarts
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <returns>Dépense avec Montant Facturé hors écart</returns>
        public static DepenseAchatEnt ComputeMontantFactureHorsEcart(this DepenseAchatEnt depense)
        {
            if (depense != null)
            {
                depense.MontantFactureHorsEcart = (depense.Quantite - depense.QuantiteDepense) * depense.PUHT;
            }
            return depense;
        }

        /// <summary>
        /// Calcul du montant facturé
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Dépense avec Montant Facturé</returns>
        public static DepenseAchatEnt ComputeMontantFacture(this DepenseAchatEnt depense, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            if (depense != null)
            {
                decimal allDepenseSum = 0;
                if (depense.Depenses?.Count > 0)
                {
                    List<int> depenseTypeCodes = new List<int>
                    {
                        DepenseType.Facture.ToIntValue(),
                        DepenseType.FactureEcart.ToIntValue(),
                        DepenseType.Avoir.ToIntValue(),
                        DepenseType.AvoirEcart.ToIntValue()
                    };

                    allDepenseSum = depense.Depenses.Where(x => depenseTypeCodes.Contains(x.DepenseTypeId.Value)
                                                          && x.DateOperation.HasValue
                                                          && (!dateDebut.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) >= ((dateDebut.Value.Year * 100) + dateDebut.Value.Month))
                                                          && (!dateFin.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) <= ((dateFin.Value.Year * 100) + dateFin.Value.Month)))
                                                .Sum(x => x.Quantite * x.PUHT);
                }

                depense.MontantFacture = allDepenseSum;
            }
            return depense;
        }

        /// <summary>
        /// Détermine s'il y a eu au moins une opération d'ajustement FAR (Annulation Far, Chargement, Déchargement)
        /// </summary>
        /// <param name="depense">Dépense achat</param>    
        /// <returns>DepenseAchatEnt avec valeur HasAjustementFar</returns>
        public static DepenseAchatEnt ComputeHasAjustementFar(this DepenseAchatEnt depense)
        {
            if (depense?.Depenses?.Count > 0)
            {
                bool hasAjustementFar = false;

                depense.Depenses.ForEach(x =>
                {
                    if (x.FacturationsFar?.Any(y => y.FacturationTypeId == 5 || y.FacturationTypeId == 6 || y.FacturationTypeId == 9) == true)
                    {
                        hasAjustementFar = true;
                    }
                });

                depense.HasAjustementFar = hasAjustementFar;
            }
            return depense;
        }

        /// <summary>
        /// Calcul de tous les champs calculés
        /// </summary>
        /// <param name="depense">Dépense</param>
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Dépense avec Solde FAR</returns>
        public static DepenseAchatEnt ComputeAll(this DepenseAchatEnt depense, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            if (depense != null)
            {
                depense.FacturationsFacture.ComputeAll();
                depense.FacturationsFactureEcart.ComputeAll();
                depense.FacturationsReception.ComputeAll();
                depense.ComputeMontantHT();
                depense.ComputeMontantFacturationFacture();
                depense.ComputeQuantiteFacturationFacture();
                depense.ComputeMontantFacturationFactureEcart();
                depense.ComputeQuantiteFacturationFactureEcart();
                depense.ComputeMontantFacturationReception();
                depense.ComputeQuantiteFacturationReception();
                depense.ComputeMontantFacturation();
                depense.ComputeMontantFacture(dateDebut, dateFin);
                depense.ComputeQuantiteFacturation();
                depense.ComputeSoldeFar(dateDebut, dateFin);
                depense.ComputeMontantFactureHorsEcart();
                depense.ComputeHasAjustementFar();
            }

            return depense;
        }

        /// <summary>
        /// Calcul du montant des facturations de type Reception
        /// </summary>
        /// <param name="depenses">Liste des DepenseEnt</param>
        /// <returns>Dépense avec Montant Réception</returns>
        public static IEnumerable<DepenseAchatEnt> ComputeMontantFacturationReception(this IEnumerable<DepenseAchatEnt> depenses)
        {
            depenses?.ForEach(depense => depense.ComputeMontantFacturationReception());
            return depenses;
        }

        /// <summary>
        /// Calcul du Solde Far
        /// </summary>
        /// <param name="depenses">Dépense</param>
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Dépense avec Solde FAR</returns>
        public static IEnumerable<DepenseAchatEnt> ComputeSoldeFar(this IEnumerable<DepenseAchatEnt> depenses, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            depenses?.ForEach(depense => depense.ComputeSoldeFar(dateDebut, dateFin));
            return depenses;
        }

        /// <summary>
        /// Calcul du montant facturé
        /// </summary>
        /// <param name="depenses">liste de Dépenses</param>
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste de Dépenses avec Montant Facturé</returns>
        public static IEnumerable<DepenseAchatEnt> ComputeMontantFacture(this IEnumerable<DepenseAchatEnt> depenses, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            depenses?.ForEach(depense => depense.ComputeMontantFacture(dateDebut, dateFin));
            return depenses;
        }

        /// <summary>
        /// Calcul de tous les champs calculés
        /// </summary>
        /// <param name="depenses">Liste des Dépenses</param>        
        /// <param name="dateDebut">Date début de comparaison pour calcul</param>
        /// <param name="dateFin">Date fin de comparaison pour calcul</param>
        /// <returns>Liste des Dépenses avec Solde FAR</returns>
        public static IEnumerable<DepenseAchatEnt> ComputeAll(this IEnumerable<DepenseAchatEnt> depenses, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            depenses?.ForEach(depense => depense.ComputeAll(dateDebut, dateFin));
            return depenses;
        }
    }
}
