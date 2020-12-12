using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.DataAccess.Achat.Calculation.Common
{
    /// <summary>
    /// Helpers qui donne des expressions communes au requetes
    /// </summary>
    public class AchatCommonExpressionsFiltersHelper
    {
        private readonly int typeReception;
        private readonly List<int> ajustementFarsTypes;
        private readonly List<int> factureAvoirTypeCodes;

        /// <summary>
        /// ctor
        /// </summary>
        public AchatCommonExpressionsFiltersHelper()
        {
            typeReception = DepenseType.Reception.ToIntValue();
            ajustementFarsTypes = new List<int>
            {
                DepenseType.ExtourneFar.ToIntValue(),
                DepenseType.AjustementFar.ToIntValue()
            };
            factureAvoirTypeCodes = new List<int>
            {
                DepenseType.Facture.ToIntValue(),
                DepenseType.FactureEcart.ToIntValue(),
                DepenseType.Avoir.ToIntValue(),
                DepenseType.AvoirEcart.ToIntValue()
            };
        }

        public Expression<Func<DepenseAchatEnt, bool>> GetIsDepenseAchatNotDeletedFilter()
        {
            return x => !x.DateSuppression.HasValue;
        }

        /// <summary>
        /// Retourne une expression qui garde les depenses achats qui sont des receptions et qui ne sont pas supprimée logiquement
        /// </summary>
        /// <returns>Des depense achats de type reception</returns>
        public Expression<Func<DepenseAchatEnt, bool>> GetIsReceptionAndNotDeletedFilter()
        {
            return x => typeReception == x.DepenseTypeId.Value && !x.DateSuppression.HasValue;
        }
        public Expression<Func<DepenseAchatEnt, bool>> GetIsReceptionFilter()
        {
            return x => typeReception == x.DepenseTypeId.Value;
        }


        public Expression<Func<CommandeLigneEnt, bool>> GetIsCommandeLigneDiminutionFilter()
        {
            return x => x.AvenantLigneId != null && x.AvenantLigne.IsDiminution;
        }
        public Expression<Func<CommandeLigneEnt, bool>> GetIsCommandeLigneAugmentationFilter()
        {
            return x => !(x.AvenantLigneId != null && x.AvenantLigne.IsDiminution);
        }

        public Expression<Func<DepenseAchatEnt, bool>> GetIsAjustementFarsInItervalTimeExpression(DateTime? dateDebut, DateTime? dateFin)
        {
            return x => ajustementFarsTypes.Contains(x.DepenseTypeId.Value)
                                                   && x.DateOperation.HasValue
                                                   && (!dateDebut.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) >= ((dateDebut.Value.Year * 100) + dateDebut.Value.Month))
                                                   && (!dateFin.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) <= ((dateFin.Value.Year * 100) + dateFin.Value.Month));
        }

        public Expression<Func<DepenseAchatEnt, bool>> GetIsFactureAvoirInItervalTimeFilter(DateTime? dateDebut, DateTime? dateFin)
        {
            return x => factureAvoirTypeCodes.Contains(x.DepenseTypeId.Value)
                                                       && x.DateOperation.HasValue
                                                         && (!dateDebut.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) >= ((dateDebut.Value.Year * 100) + dateDebut.Value.Month))
                                                         && (!dateFin.HasValue || ((x.DateOperation.Value.Year * 100) + x.DateOperation.Value.Month) <= ((dateFin.Value.Year * 100) + dateFin.Value.Month));

        }

    }
}
