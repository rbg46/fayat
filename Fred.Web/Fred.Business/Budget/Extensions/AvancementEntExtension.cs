using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Methodes d'extensions sur l'avancement
    /// </summary>
    public static class AvancementEntExtension
    {
        /// <summary>
        /// Retourne l'avancement en pourcentage. Si l'avancement saisie pour ce T4 est en quantité alors la fonction
        /// calcul le pourcentage en fonction de la quantité d'avancement et de la quantité budgété
        /// </summary>
        /// <param name="avancement">avancement saisie sur ce budget</param>
        /// <param name="t4">T4 parent du sous détail dont on veut connaitre l'avancement</param>
        /// <param name="quantiteSdBudgetee">Quantité budgété du sous détail</param>
        /// <returns>un decimal contenant la valeur, 0 si aucun avancement n'est saisi</returns>
        public static decimal GetPourcentageAvancementSousDetail(this AvancementEnt avancement, BudgetT4Ent t4, decimal quantiteSdBudgetee)
        {
            if( avancement == null)
            {
                return 0;
            }

            if (avancement.PourcentageSousDetailAvance.HasValue)
            {
                return avancement.PourcentageSousDetailAvance.Value;
            }

            if (avancement.QuantiteSousDetailAvancee.HasValue)
            {
                return (avancement.QuantiteSousDetailAvancee * 100 / quantiteSdBudgetee ?? 0);
            }

            return 0;
        }

    }
}
