using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Depense;

namespace Fred.Common.Tests.Data.Depense.Builder
{
    /// <summary>
    /// Classe Builder de l'Entité <see cref="DepenseAchatEnt"/>
    /// </summary>
    public class DepenseAchatBuilder : ModelDataTestBuilder<DepenseAchatEnt>
    {
        public DepenseAchatBuilder Default()
        {
            base.New();
            Model.DepenseId = 1;
            Model.CiId = 1;
            Model.Quantite = 0;
            Model.CommandeLigneId = null;
            return this;
        }

        public DepenseAchatBuilder HangfireJobId(string code)
        {
            Model.HangfireJobId = code;
            return this;
        }

        public DepenseAchatBuilder IsReceptionInterimaire()
        {
            Model.IsReceptionInterimaire = true;
            return this;
        }

        public DepenseAchatBuilder Quantite(decimal quantite)
        {
            Model.Quantite = quantite;
            return this;
        }

        public DepenseAchatBuilder DepenseId(int depenseId)
        {
            Model.DepenseId = depenseId;
            return this;
        }

        public DepenseAchatBuilder CommandeLigneId(int? commandeLigneId)
        {
            Model.CommandeLigneId = commandeLigneId;
            return this;
        }

        public DepenseAchatBuilder CiId(int id)
        {
            Model.CiId = id;
            return this;
        }
    }
}
