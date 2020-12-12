using Fred.Common.Tests.EntityFramework;
using Fred.Entities.PermissionFonctionnalite;

namespace Fred.Common.Tests.Data.PermissionFonctionnalite.Builder
{
    /// <summary>
    /// Classe Builder de l'entite PermissionFonctionnaliteEnt
    /// </summary>
    public class PermissionFonctionnaliteBuilder : ModelDataTestBuilder<PermissionFonctionnaliteEnt>
    {
        public PermissionFonctionnaliteBuilder PermissionFonctionnaliteId(int id)
        {
            Model.PermissionFonctionnaliteId = id;
            return this;
        }

        public PermissionFonctionnaliteBuilder FonctionnaliteId(int id)
        {
            Model.FonctionnaliteId = id;
            return this;
        }

        public PermissionFonctionnaliteBuilder PermissionId(int id)
        {
            Model.PermissionId = id;
            return this;
        }
    }
}
