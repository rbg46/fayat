using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Enum;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Common.Tests.Data.OperationDiverse.Builder
{
    /// <summary>
    /// Builder de <see cref="OperationDiverseAbonnementModel"/>
    /// </summary>
    public class OperationDiverseAbonnementModelBuilder : ModelDataTestBuilder<OperationDiverseAbonnementModel>
    {
        public override OperationDiverseAbonnementModel New()
        {
            base.New();
            Model.OperationDiverseId = 1;
            Model.EstUnAbonnement = false;
            Model.OperationDiverseMereIdAbonnement = null;
            return Model;
        }

        public OperationDiverseAbonnementModelBuilder OperationDiverseId(int operationDiverseId)
        {
            Model.OperationDiverseId = operationDiverseId;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder EstUnAbonnement(bool estUnAbonnement)
        {
            Model.EstUnAbonnement = estUnAbonnement;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder OperationDiverseMereIdAbonnement(int? operationDiverseMereIdAbonnement)
        {
            Model.OperationDiverseMereIdAbonnement = operationDiverseMereIdAbonnement;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder DureeAbonnement(int? dureeAbonnement)
        {
            Model.DureeAbonnement = dureeAbonnement;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder DateComptable(DateTime? dateComptable)
        {
            Model.DateComptable = dateComptable;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder DatePremiereODAbonnement(DateTime? datePremiereODAbonnement)
        {
            Model.DatePremiereODAbonnement = datePremiereODAbonnement;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder DateProchaineODAbonnement(DateTime? dateProchaineODAbonnement)
        {
            Model.DateProchaineODAbonnement = dateProchaineODAbonnement;
            return this;
        }

        public OperationDiverseAbonnementModelBuilder FrequenceAbonnementModel(EnumModel frequenceAbonnementModel)
        {
            Model.FrequenceAbonnementModel = frequenceAbonnementModel;
            return this;
        }

    }
}
