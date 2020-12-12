using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Achat.Calculation
{
    public class MontantFactureRepository : IMontantFactureRepository
    {
        private FredDbContext context;
        private AchatCommonExpressionsFiltersHelper achatCommonExpressionsFiltersHelper;


        public MontantFactureRepository(FredDbContext fredDbContext)
        {
            context = fredDbContext;
            achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();
        }

        public List<ReceptionMontantFactureModel> GetMontantFactureForReceptions(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsFilter, DateTime? dateDebut, DateTime? dateFin)
        {
            var isReceptionAndNotDeletedFilter = this.achatCommonExpressionsFiltersHelper.GetIsReceptionAndNotDeletedFilter();

            var isFactureAvoirInItervalTimeFilter = this.achatCommonExpressionsFiltersHelper.GetIsFactureAvoirInItervalTimeFilter(dateDebut, dateFin);

            var result = (from reception in context.DepenseAchats.Where(selectionReceptionsFilter).Where(isReceptionAndNotDeletedFilter)
                          let factureAvoirs = context.DepenseAchats.Where(x => x.DepenseParentId == reception.DepenseId).Where(isFactureAvoirInItervalTimeFilter)
                          select new ReceptionMontantFactureModel
                          {
                              ReceptionId = reception.DepenseId,
                              MontantFacture = factureAvoirs.Sum(x => x.Quantite * x.PUHT)
                          })
                          .AsNoTracking()
                          .ToList();
            return result;
        }

    }
}
