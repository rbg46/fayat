using System;
using System.Collections.Generic;
using System.Text;
using Fred.Entities.Personnel.Interimaire;

namespace Fred.Business.Reception.Common
{
    public class ContratInterimaireComparer : IEqualityComparer<ContratInterimaireEnt>
    {
        public bool Equals(ContratInterimaireEnt x, ContratInterimaireEnt y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return x != null && y != null
                && x.ContratInterimaireId == y.ContratInterimaireId;
        }
        public int GetHashCode(ContratInterimaireEnt obj)
        {
            if (obj == null) return 0;

            return obj.ContratInterimaireId.GetHashCode();
        }
    }
}
