using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Achat.Calculation.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Achat.Calculation.Reception;
using Fred.Entities.Depense;
using Fred.EntityFramework;

namespace Fred.DataAccess.Achat.Calculation
{
    public class SoldeFarRepository : ISoldeFarRepository
    {

        private readonly FredDbContext context;
        private readonly AchatCommonExpressionsFiltersHelper achatCommonExpressionsFiltersHelper;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="MontantHtReceptionneeRepository" />.
        /// </summary>       
        /// <param name="context">FredDbContext</param>
        public SoldeFarRepository(FredDbContext context)
        {
            this.context = context;
            achatCommonExpressionsFiltersHelper = new AchatCommonExpressionsFiltersHelper();
        }

        public List<ReceptionSoldeFarModel> GetSoldeFar(Expression<Func<DepenseAchatEnt, bool>> selectionReceptionsFilter, DateTime? dateDebut, DateTime? dateFin)
        {
            var isReceptionAndNotDeletedFilter = this.achatCommonExpressionsFiltersHelper.GetIsReceptionAndNotDeletedFilter();

            var isAjustementFarsInItervalTimeFilter = this.achatCommonExpressionsFiltersHelper.GetIsAjustementFarsInItervalTimeExpression(dateDebut, dateFin);


            var soldeFarsQuery = (from reception in context.DepenseAchats.Where(selectionReceptionsFilter).Where(isReceptionAndNotDeletedFilter)
                                  let ajustementFars = context.DepenseAchats.Where(x => x.DepenseParentId == reception.DepenseId).Where(isAjustementFarsInItervalTimeFilter)
                                  select new ReceptionSoldeFarModel
                                  {
                                      ReceptionId = reception.DepenseId,
                                      SoldeFar = ajustementFars.Count() > 0 ? (reception.PUHT * reception.Quantite) + ajustementFars.Sum(x => x.Quantite * x.PUHT) : (reception.PUHT * reception.Quantite)
                                  })
                                 .ToList();

            return soldeFarsQuery;

        }
    }
}
