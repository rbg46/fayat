using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Moyen;

namespace Fred.Common.Tests.Data.Moyen.Builder
{
    /// <summary>
    /// builder de l'entity <see cref="MaterielLocationEnt"/>
    /// </summary>
    public class MaterielLocationBuilder : ModelDataTestBuilder<MaterielLocationEnt>
    {
        public override MaterielLocationEnt New()
        {
            Model = new MaterielLocationEnt
            {
                MaterielLocationId = 1,
                MaterielId = 1,
                Immatriculation = "MAt-000-",
                Libelle = "MaterielLocation1",
                DateCreation  = new DateTime(2019,1,1),
                AuteurCreationId = 1
            };
            return Model;
        }
          
        public MaterielLocationBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public MaterielLocationBuilder MaterielLocationId(int id)
        {
            Model.MaterielLocationId = id;
            return this;
        }
    }
}
