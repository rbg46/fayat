using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Achat.Calculation.Commande;
using Fred.Entities.Commande;
using Fred.EntityFramework;

namespace Fred.DataAccess.Achat.Calculation
{
    /// <summary>
    /// Repo qui calcule le montant HT receptionné
    /// </summary>
    public class MontantHtReceptionneeRepository : IMontantHtReceptionneeRepository
    {
        private readonly FredDbContext context;
        private readonly AchatCommonExpressionsFiltersHelper achatCommonExpressionsFiltersHelper;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="MontantHtReceptionneeRepository" />.
        /// </summary>       
        /// <param name="context">FredDbContext</param>
        public MontantHtReceptionneeRepository(FredDbContext context)
        {
            this.context = context;
            achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();
        }

        /// <summary>
        /// Retourne le calcul des montants HT receptionnés d'une commande,a partir d'une expression sur les commandes
        /// </summary>
        /// <param name="selectionCommandeFilter"> expression sur les commandes</param>
        /// <returns>Liste de calculs de montant receptionnés</returns>
        public List<CommandeMontantHTReceptionneModel> GetMontantHTReceptionne(Expression<Func<CommandeEnt, bool>> selectionCommandeFilter)
        {
            var isReceptionFilter = achatCommonExpressionsFiltersHelper.GetIsReceptionAndNotDeletedFilter();

            var commandeQuery = (from commande in context.Commandes.Where(selectionCommandeFilter)
                                 let depenses = commande.Lignes.SelectMany(x => x.AllDepenses).AsQueryable()
                                 let receptions = depenses.Where(isReceptionFilter)
                                 let montantHTReceptionne = receptions.Count() > 0 ? receptions.Sum(r => r.Quantite * r.PUHT) : 0
                                 select new CommandeMontantHTReceptionneModel
                                 {
                                     CommandeId = commande.CommandeId,
                                     MontantHTReceptionne = montantHTReceptionne
                                 });

            var commandeRequest = commandeQuery.ToList();

            return commandeRequest;
        }
    }
}
