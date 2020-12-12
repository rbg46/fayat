using System;
using System.Collections.Generic;

namespace Fred.Business.Commande.Models
{
    /// <summary>
    /// Model Ligne de commande Excel
    /// </summary>
    public class ExcelLigneCommandeModel
    {
        /// <summary>
        /// Numéro du ligne
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// Date Commande
        /// </summary>
        public string NumeroComande { get; set; }

        /// <summary>
        /// Désignation ligne Commande
        /// </summary>
        public string DesignationLigneCommande { get; set; }

        /// <summary>
        /// Code Ressource
        /// </summary>
        public string CodeRessource { get; set; }

        /// <summary>
        /// Code Tache
        /// </summary>
        public string CodeTache { get; set; }

        /// <summary>
        /// Prix Unitaire
        /// </summary>
        public string PuHt { get; set; }

        /// <summary>
        /// Unité
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// Quantité
        /// </summary>
        public string QuantiteCommande { get; set; }

        /// <summary>
        /// Diminution
        /// </summary>
        public string IsDiminution { get; set; }

        /// <summary>
        /// Listes des erreurs
        /// </summary>
        public List<string> Erreurs { get; set; } = new List<string>();
    }
}
