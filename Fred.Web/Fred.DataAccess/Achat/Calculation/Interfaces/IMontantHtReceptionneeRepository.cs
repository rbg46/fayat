using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Achat.Calculation.Commande;
using Fred.Entities.Commande;

namespace Fred.DataAccess.Achat.Calculation.Interfaces
{
    /// <summary>
    /// Repo qui calcule le montant HT receptionné
    /// </summary>
    public interface IMontantHtReceptionneeRepository : IMultipleRepository
    {
        /// <summary>
        /// Retourne le calcul des montants HT receptionnés d'une commande,a partir d'une expression sur les commandes
        /// </summary>
        /// <param name="selectionCommandeExpression"> expression sur les commandes</param>
        /// <returns>Liste de calculs de montant receptionnés</returns>
        List<CommandeMontantHTReceptionneModel> GetMontantHTReceptionne(Expression<Func<CommandeEnt, bool>> selectionCommandeExpression);
    }
}
