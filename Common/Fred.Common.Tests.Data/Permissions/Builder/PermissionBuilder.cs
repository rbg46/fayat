using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Permission;

namespace Fred.Common.Tests.Data.Permissions.Builder
{
    /// <summary>
    /// Classe Builder de l'entite PermissionEnt
    /// </summary>
    public class PermissionBuilder : ModelDataTestBuilder<PermissionEnt>
    {
        public PermissionBuilder PermissionId(int id)
        {
            Model.PermissionId = id;
            return this;
        }

        public PermissionBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public PermissionBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public PermissionBuilder PermissionKey(string permissionKey)
        {
            Model.PermissionKey = permissionKey;
            return this;
        }
    }
}
