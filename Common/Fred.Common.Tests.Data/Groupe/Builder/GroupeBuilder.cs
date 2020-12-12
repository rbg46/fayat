using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Groupe;

namespace Fred.Common.Tests.Data.Groupe.Builder
{
    /// <summary>
    /// Classe Builder d'un Groupe
    /// </summary>
    public class GroupeBuilder : ModelDataTestBuilder<GroupeEnt>
    {

        /// <summary>
        /// Obtient un Groupe par défaut
        /// </summary>
        /// <returns>un groupe</returns>
        public GroupeEnt Prototype()
        {
            Model = New();
            Model.GroupeId = 1;
            Model.Code = "GRZB";
            Model.Libelle = "GROUPE RAZEL - BEC";
            return Model;
        }

        public GroupeBuilder RazelBec()
        {
            Model.GroupeId = 1;
            Model.Code = "GRZB";
            Model.Libelle = "GROUPE RAZEL - BEC";
            return this;
        }

        public GroupeBuilder Fondations()
        {
            Model.GroupeId = 4;
            Model.Code = "GFON";
            Model.Libelle = "FAYAT FONDATIONS";
            return this;
        }

        public GroupeBuilder FayatTP()
        {
            Model.GroupeId = 8;
            Model.Code = "GFTP";
            Model.Libelle = "GFAYAT TP";
            return this;
        }

        public GroupeBuilder FayatFes()
        {
            Model.GroupeId = 9;
            Model.Code = "GFES";
            Model.Libelle = "Groupe FES";
            return this;
        }
    }
}
