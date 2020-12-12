using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Facturation;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Facture;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.Facturation
{
    public class FacturationManager : Manager<FacturationEnt, IFacturationRepository>, IFacturationManager
    {
        public FacturationManager(IUnitOfWork uow, IFacturationRepository facturationRepository)
            : base(uow, facturationRepository)
        { }

        public void BulkInsert(IEnumerable<FacturationEnt> facturations)
        {
            if (facturations == null)
            {
                throw new ArgumentNullException(nameof(facturations));
            }

            try
            {
                Repository.InsertInMass(facturations);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public IEnumerable<FacturationEnt> GetList(string numFactureSap)
        {
            return Repository.Query().Filter(x => x.NumeroFactureSAP.Trim() == numFactureSap.Trim()).Get().AsNoTracking();
        }

        public IReadOnlyList<FactureEcritureComptableModel> GetExistingNumeroFactureSap(List<string> numFacturesSap)
        {
            return Repository.GetExistingNumeroFactureSap(numFacturesSap)
                .Select(f => new FactureEcritureComptableModel
                {
                    DepenseAchatFactureEcartId = f.DepenseAchatFactureEcartId,
                    DepenseAchatFactureId = f.DepenseAchatFactureId,
                    DepenseAchatFarId = f.DepenseAchatFarId,
                    DepenseAchatReceptionId = f.DepenseAchatReceptionId,
                    NumeroFactureSAP = f.NumeroFactureSAP
                }).ToList();
        }

        public IEnumerable<FacturationEnt> GetListByReceptionID(int receptionId, decimal montantHt, DateTime dateSaisie)
        {
            return Repository.GetExistingNumeroFactureSap(receptionId, montantHt, dateSaisie);
        }

        public List<int?> GetIdDepenseAchatWithFacture()
        {
            return Repository.GetIdDepenseAchatWithFacture();
        }
    }
}
