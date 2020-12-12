using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Facturation;

namespace Fred.Common.Tests.Data.Facturation.Builder
{
    public class FacturationBuilder : ModelDataTestBuilder<FacturationEnt>
    {
        public override FacturationEnt New()
        {
            return new FacturationEnt
            {
                CodeCi = ""
            };
        }

        public FacturationBuilder NumeroFactureSAP(string numeroSap)
        {
            Model.NumeroFactureSAP = numeroSap;
            return this;
        }

        public FacturationBuilder DepenseAchatReceptionId(int receptionId)
        {
            Model.DepenseAchatReceptionId = receptionId;
            return this;
        }

        public FacturationBuilder MontantHT(decimal montant)
        {
            Model.MontantHT = montant;
            return this;
        }

        public FacturationBuilder DateSaisie(DateTime dateSaisie)
        {
            Model.DateSaisie = dateSaisie;
            return this;
        }
    }
}
