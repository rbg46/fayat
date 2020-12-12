using System;
using System.Collections.Generic;

namespace Fred.Web.Models.RepartitionEcart
{
    /// <summary>
    /// Represente l'aglomerat de plusieur RepartitionEcart
    /// </summary>
    public class RepartitionEcartModel
    {

        /// <summary>
        /// Represente l'index de la ligne lors de l'affichage de la synthese.
        /// </summary>   
        public int RowIndex { get; set; }

        /// <summary>
        /// Obtient ou définit l'affaire .
        /// </summary>   
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le mois de cloture
        /// </summary>    
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id dU Chapitre d'une Repartition d'écart.
        /// </summary>   
        public List<string> ChapitreCodes { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la Valorisation initiale.
        /// </summary>  
        public decimal ValorisationInitiale { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la Valorisation Rectifiee.
        /// </summary>  
        public decimal ValorisationRectifiee { get; set; }

        /// <summary>
        /// Obtient ou définit le montant Capitalise.
        /// </summary>   
        public decimal MontantCapitalise { get; set; }

        /// <summary>
        /// Obtient ou définit le montant de la Valorisation initiale.
        /// </summary>   
        public decimal Ecart { get; set; }
    }
}
