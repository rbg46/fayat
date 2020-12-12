using System;
using System.Linq.Expressions;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;

namespace Fred.DataAccess.Referential.Common
{
    public class TacheSearchHelper : ITacheSearchHelper
    {

        public int[] GetTechnicalTasks()
        {
            return new int[]
            {
                 //Penser à ajouter les nouveaux types de tâches d'écart dans les 2 méthodes.
                TacheType.EcartAchat.ToIntValue(),
                TacheType.EcartNiveau1.ToIntValue(),
                TacheType.EcartNiveau2.ToIntValue(),
                TacheType.EcartMOEncadrement.ToIntValue(),
                TacheType.EcartMOProduction.ToIntValue(),
                TacheType.EcartMaterielImmobilise.ToIntValue(),
                TacheType.EcartMateriel.ToIntValue(),
                TacheType.EcartAutreFrais.ToIntValue(),
                TacheType.EcartInterim.ToIntValue(),
                TacheType.EcartMaterielExterne.ToIntValue(),
                TacheType.EcartRecette.ToIntValue(),
                TacheType.EcartFraisGeneraux.ToIntValue(),
                TacheType.EcartAutresDepensesHorsDebours.ToIntValue(),
                TacheType.Litige.ToIntValue(),
            };
        }

        public bool IsTacheEcart(TacheEnt tache)
        {
            if (tache == null)
            {
                throw new ArgumentNullException(nameof(tache));
            }

            return IsTacheEcart(tache.TacheType);
        }

        /// <summary>
        /// Indique si une tâche est une tâche d'écart.
        /// </summary>
        /// <param name="tacheType">Le type de la tâche concernée.</param>
        /// <returns>True si la tâche est une tâche d'écart, sinon false.</returns>
        public bool IsTacheEcart(int tacheType)
        {
            switch ((TacheType)tacheType)
            {
                case TacheType.EcartNiveau1:
                case TacheType.EcartNiveau2:
                case TacheType.EcartMOEncadrement:
                case TacheType.EcartMOProduction:
                case TacheType.EcartMateriel:
                case TacheType.EcartAchat:
                case TacheType.EcartAutreFrais:
                case TacheType.EcartInterim:
                case TacheType.EcartMaterielImmobilise:
                case TacheType.EcartMaterielExterne:
                case TacheType.EcartRecette:
                case TacheType.EcartFraisGeneraux:
                case TacheType.EcartAutresDepensesHorsDebours:
                case TacheType.Litige:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Retourne une expression qui indique si une tâche est active et non supprimée.
        /// </summary>
        /// <returns>L'expression.</returns>
        public Expression<Func<TacheEnt, bool>> IsTacheActiveAndNotDeletedExpression
        {
            get
            {
                return tacheEnt => tacheEnt.Active && tacheEnt.DateSuppression == null;
            }
        }

        /// <summary>
        /// Retourne une expression qui indique si une tâche n'est pas une tâche d'écart.
        /// </summary>
        /// <returns>L'expression.</returns>
        public Expression<Func<TacheEnt, bool>> IsNotTacheEcartExpression
        {
            get
            {
                return tacheEnt => !(tacheEnt.TacheType == (int)TacheType.EcartNiveau1
                               || tacheEnt.TacheType == (int)TacheType.EcartNiveau2
                               || tacheEnt.TacheType == (int)TacheType.EcartMOEncadrement
                               || tacheEnt.TacheType == (int)TacheType.EcartMOProduction
                               || tacheEnt.TacheType == (int)TacheType.EcartMateriel
                               || tacheEnt.TacheType == (int)TacheType.EcartAchat
                               || tacheEnt.TacheType == (int)TacheType.EcartAutreFrais
                               || tacheEnt.TacheType == (int)TacheType.EcartInterim
                               || tacheEnt.TacheType == (int)TacheType.EcartMaterielImmobilise
                               || tacheEnt.TacheType == (int)TacheType.EcartMaterielExterne
                               || tacheEnt.TacheType == (int)TacheType.EcartRecette
                               || tacheEnt.TacheType == (int)TacheType.EcartFraisGeneraux
                               || tacheEnt.TacheType == (int)TacheType.EcartAutresDepensesHorsDebours
                               || tacheEnt.TacheType == (int)TacheType.Litige);
            }
        }
    }
}
