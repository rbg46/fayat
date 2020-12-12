using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Tache.Builder
{
    /// <summary>
    /// Builder de l'entité <see cref="TacheEnt"/>
    /// </summary>
    public class TacheBuilder : ModelDataTestBuilder<TacheEnt>
    {
        public TacheEnt Niveau1()
        {
            New();
            Model.CiId = 1;
            Model.Code = "00";
            Model.Libelle = "Niveau 1";
            Model.Niveau = 1;
            Model.Active = true;
            Model.TacheType = (int)TacheType.EcartNiveau1;
            return Model;
        }

        public TacheEnt Niveau2()
        {
            New();
            Model.CiId = 1;
            Model.Code = "0000";
            Model.Libelle = "Niveau 2";
            Model.Niveau = 2;
            Model.Active = true;
            Model.TacheType = (int)TacheType.EcartNiveau2;
            return Model;
        }

        public TacheEnt Niveau3()
        {
            New();
            Model.CiId = 1;
            Model.Code = "000000";
            Model.Libelle = "Niveau 3";
            Model.Niveau = 3;
            Model.Active = true;
            return Model;
        }

        public override TacheEnt New()
        {
            base.New();
            Model.TacheId = 1;
            Model.Code = "00";
            Model.Libelle = "TACHE PAR DEFAUT";

            return Model;
        }

        public TacheBuilder TacheId(int tacheid)
        {
            Model.TacheId = tacheid;
            return this;
        }

        public TacheBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }
        public TacheBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }
        public TacheBuilder CiId(int ciId)
        {
            Model.CiId = ciId;
            return this;
        }

        public TacheBuilder Niveau(int niveau)
        {
            Model.Niveau = niveau;
            return this;
        }
        public TacheBuilder Active(bool active)
        {
            Model.Active = active;
            return this;
        }
    }
}
