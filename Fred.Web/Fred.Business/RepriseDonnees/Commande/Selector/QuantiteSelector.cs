using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Entities.RepriseDonnees.Excel;

namespace Fred.Business.RepriseDonnees.Commande.Selector
{
    /// <summary>
    /// Selection la quantite pour une commande ligne
    /// </summary>
    public class QuantiteSelector
    {

        /// <summary>
        /// Selection la quantite pour une commande ligne
        /// </summary>
        /// <param name="repriseExcelCommande">Contient les donnees necessaires au calcul de la quantité</param>
        /// <returns>La quantite de la commande ligne ou null si la quantite n'a pas pu etre calculée</returns>
        public decimal? GetQuantiteCommandeLigne(RepriseExcelCommande repriseExcelCommande)
        {
            var selector = new CommonFieldSelector();

            var quantiteCommandee = selector.GetDecimal(repriseExcelCommande.QuantiteCommandee);
            var quantiteFactureeRapprochee = selector.GetDecimal(repriseExcelCommande.QuantiteFactureeRapprochee);
            var quantiteReceptionnee = selector.GetDecimal(repriseExcelCommande.QuantiteReceptionnee);

            var result = quantiteCommandee - quantiteFactureeRapprochee;
            if (result > 0)
            {
                return result;
            }
            result = quantiteReceptionnee - quantiteFactureeRapprochee;
            if (result > 0)
            {
                return result;
            }
            // la quantite n'a pas pu etre calculée
            return null;
        }

        /// <summary>
        /// Permet de savoir si l'on peux créer une reception a partir d'une ligne excel
        /// </summary>
        /// <param name="repriseExcelCommande">repriseExcelCommande</param>
        /// <returns>vrai si l'on peux créer une reception a partir d'une ligne excel</returns>
        public bool CanCreateReception(RepriseExcelCommande repriseExcelCommande)
        {
            var quantiteSelector = new QuantiteSelector();
            var receptionQuantite = quantiteSelector.GetQuantiteReception(repriseExcelCommande);
            if (receptionQuantite > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Caclule la quantité de la reception
        /// </summary>
        /// <param name="repriseExcelCommande">repriseExcelCommande</param>
        /// <returns>la quantite de la reception</returns>
        public decimal GetQuantiteReception(RepriseExcelCommande repriseExcelCommande)
        {
            var selector = new CommonFieldSelector();
            var quantiteFactureeRapprochee = selector.GetDecimal(repriseExcelCommande.QuantiteFactureeRapprochee);
            var quantiteReceptionnee = selector.GetDecimal(repriseExcelCommande.QuantiteReceptionnee);
            return quantiteReceptionnee - quantiteFactureeRapprochee;
        }
    }
}
