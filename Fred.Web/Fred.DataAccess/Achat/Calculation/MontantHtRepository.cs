using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Achat.Calculation.Commande;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Achat.Calculation
{
    public class MontantHtRepository : IMontantHtRepository
    {
        private readonly FredDbContext context;
        private readonly AchatCommonExpressionsFiltersHelper achatCommonExpressionsFiltersHelper;


        public MontantHtRepository(FredDbContext fredDbContext)
        {
            context = fredDbContext;
            achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();
        }

        public List<CommandeMontantHtModel> GetMontantHT(Expression<Func<CommandeEnt, bool>> selectionCommandesFilter)
        {
            var isCommandeLigneDiminutionFilter = achatCommonExpressionsFiltersHelper.GetIsCommandeLigneDiminutionFilter();

            var isCommandeLigneAugmentationFilter = achatCommonExpressionsFiltersHelper.GetIsCommandeLigneAugmentationFilter();

            var commandeQuery = (from commande in context.Commandes.Where(selectionCommandesFilter)
                                 let commandeLignes = commande.Lignes.AsQueryable()
                                 let commandeLignesDiminutions = commandeLignes.Where(isCommandeLigneDiminutionFilter)
                                 let commandeLignesAugmentations = commandeLignes.Where(isCommandeLigneAugmentationFilter)
                                 let commandeLignesDiminutionsTotal = commandeLignesDiminutions.Count() > 0 ? commandeLignesDiminutions.Sum(x => x.Quantite * x.PUHT) : 0
                                 let commandeLignesAugmentationsTotal = commandeLignesAugmentations.Count() > 0 ? commandeLignesAugmentations.Sum(x => x.Quantite * x.PUHT) : 0
                                 select new CommandeMontantHtModel
                                 {
                                     CommandeId = commande.CommandeId,
                                     CommandLigneIds = commande.Lignes.Select(x => x.CommandeLigneId).ToList(),
                                     MontantHT = commandeLignesAugmentationsTotal - commandeLignesDiminutionsTotal
                                 });

            var commandeRequest = commandeQuery.ToList();

            return commandeRequest;
        }

        public List<DepenseAchatMontantHtModel> GetMontantHT(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsFilter)
        {
            var depenseAchatNotDeletedFilter = this.achatCommonExpressionsFiltersHelper.GetIsDepenseAchatNotDeletedFilter();

            var result = (from reception in context.DepenseAchats.Where(selectionReceptionsFilter).Where(depenseAchatNotDeletedFilter)
                          select new DepenseAchatMontantHtModel
                          {
                              DepenseId = reception.DepenseId,
                              MontantHt = reception.Quantite * reception.PUHT
                          })
                      .AsNoTracking()
                      .ToList();
            return result;
        }

    }
}
