using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Tache
{
    public class TacheBuilder : EntityTestBuilder<TacheEnt>
    {
        public override TacheEnt New()
        {
            return new TacheEnt
            {
                TacheId = 1,
                Code = "00",
                Libelle = "TACHE PAR DEFAUT"
            };
        }

        public TacheBuilder TacheId(int tacheid)
        {
            Entite.TacheId = tacheid;
            return this;
        }

        public TacheBuilder Code(string code)
        {
            Entite.Code = code;
            return this;
        }
        public TacheBuilder Libelle(string libelle)
        {
            Entite.Libelle = libelle;
            return this;
        }

    }
}
