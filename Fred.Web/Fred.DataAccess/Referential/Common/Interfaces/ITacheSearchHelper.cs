using Fred.Entities.Referential;
using System;
using System.Linq.Expressions;

namespace Fred.DataAccess.Referential.Common
{
    public interface ITacheSearchHelper
    {
        int[] GetTechnicalTasks();

        bool IsTacheEcart(TacheEnt tache);

        /// <summary>
        /// Indique si une tâche est une tâche d'écart.
        /// </summary>
        /// <param name="tacheType">Le type de la tâche concernée.</param>
        /// <returns>True si la tâche est une tâche d'écart, sinon false.</returns>
        bool IsTacheEcart(int tacheType);

        /// <summary>
        /// Retourne une expression qui indique si une tâche est active et non supprimée.
        /// </summary>
        /// <returns>L'expression.</returns>
        Expression<Func<TacheEnt, bool>> IsTacheActiveAndNotDeletedExpression
        {
            get;
        }

        /// <summary>
        /// Retourne une expression qui indique si une tâche n'est pas une tâche d'écart.
        /// </summary>
        /// <returns>L'expression.</returns>
        Expression<Func<TacheEnt, bool>> IsNotTacheEcartExpression
        {
            get;
        }
    }
}
