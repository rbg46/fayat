using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// View Model Commande Enérgie
    /// </summary>
    public class CommandeEnergieExportModel
    {
        /// <summary>
        ///   Obtient ou définit le numéro d'une commande.
        /// </summary>        
        public string Numero { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du CI
        /// </summary>        
        public string CiCode { get; set; }

        /// <summary>
        ///   Obtient ou définit la periode de la commande.
        /// </summary>        
        public string Periode { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du fournisseur de la commande.
        /// </summary>        
        public string FournisseurCode { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes d'une commande
        /// </summary>
        public ICollection<PointageEtAjustementExportModel> PointagesEtAjustements { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes d'une commande
        /// </summary>
        public ICollection<LigneSupplementaireExportModel> LignesSupplementaires { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise de référence du CI
        /// </summary>        
        public string Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'énergie de la commande
        /// </summary>        
        public string TypeEnergie { get; set; }

        /// <summary>
        ///   Obtient ou définit le Motant Total de La commande d'energie
        /// </summary>    
        public decimal TotalMontantHT { get; set; }

    }
}
