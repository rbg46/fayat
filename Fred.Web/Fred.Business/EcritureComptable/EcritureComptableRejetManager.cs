using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.EcritureComptable.Validator;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.Framework.Models;
using Fred.Web.Shared.Models.EcritureComptable;

namespace Fred.Business.EcritureComptable
{
    public class EcritureComptableRejetManager : Manager<EcritureComptableRejetEnt, IEcritureComptableRejetRepository>, IEcritureComptableRejetManager
    {
        public EcritureComptableRejetManager(
            IUnitOfWork uow,
            IEcritureComptableRejetRepository ecritureComptableRejetRepository,
            IEcritureComptableRejetValidator ecritureComptableRejetValidator)
            : base(uow, ecritureComptableRejetRepository, ecritureComptableRejetValidator)
        { }

        public void AddRejet(List<EcritureComptableRejetModel> rejets)
        {
            List<EcritureComptableRejetEnt> rejetEnts = new List<EcritureComptableRejetEnt>();
            foreach (EcritureComptableRejetModel rejet in rejets)
            {
                rejetEnts.Add(new EcritureComptableRejetEnt { CiID = rejet.CiID, DateRejet = DateTime.UtcNow, NumeroPiece = rejet.NumeroPiece, RejetMessage = rejet.RejetMessage });
            }

            Repository.Insert(rejetEnts);
            Save();
        }

        public void AddRejets(List<EcritureComptableFayatTpRejetModel> rejets)
        {
            List<EcritureComptableRejetEnt> rejetEnts = new List<EcritureComptableRejetEnt>();

            foreach (EcritureComptableFayatTpRejetModel rejet in rejets)
            {
                rejetEnts.AddRange(rejet.RejetMessage.Select(error => new EcritureComptableRejetEnt { CiID = rejet.CiID, DateRejet = DateTime.UtcNow, NumeroPiece = rejet.NumeroPiece, RejetMessage = error }));
            }

            Repository.Insert(rejetEnts);
            Save();
        }

        public void TreatRejet(IEnumerable<Result<EcritureComptableRetreiveResult>> ecritureComptableDtos)
        {
            AddRejet(PopulateRejet(ecritureComptableDtos));
        }

        private List<EcritureComptableRejetModel> PopulateRejet(IEnumerable<Result<EcritureComptableRetreiveResult>> ecritureComptableDtos)
        {
            List<EcritureComptableRejetModel> results = new List<EcritureComptableRejetModel>();
            foreach (Result<EcritureComptableRetreiveResult> ecriture in ecritureComptableDtos)
            {
                results.Add(new EcritureComptableRejetModel
                {
                    CiID = ecriture.Value.CiId,
                    RejetMessage = ecriture.Error,
                    NumeroPiece = ecriture.Value.NumeroPiece,
                    CodeNature = ecriture.Value.Nature?.Code
                });
            }
            return results;
        }
    }
}
