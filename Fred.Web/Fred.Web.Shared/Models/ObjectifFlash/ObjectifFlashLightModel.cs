using System;

namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashLightModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Objectif flash.
        /// </summary>
        public int ObjectifFlashId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début du Objectif flash.
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin du Objectif flash.
        /// </summary>
        public DateTime DateFin { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire de la commande.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du centre d'imputation
        /// </summary>
        public string CiCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de clôture de la commande.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit la somme de l'objectif de l'Objectif Flash.
        /// </summary>
        public decimal TotalMontantObjectif { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du réalisé de l'Objectif Flash.
        /// </summary>
        public decimal? TotalMontantRealise { get; set; }

        /// <summary>
        /// Obtient ou définit l'ecart entre le montant objectif et le montant realisé de l'Objectif Flash.
        /// </summary>

        public decimal? EcartRealiseObjectif { get; set; }

        /// <summary>
        ///   Obtient ou définit le flag d'activation de l'objectif flash
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'objectif Flash est clôturé
        /// </summary>
        public bool IsClosed { get; set; }
    }
}
