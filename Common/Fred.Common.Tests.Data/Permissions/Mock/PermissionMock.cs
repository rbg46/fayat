using Fred.Entities.Permission;
using Fred.Framework.Extensions;

namespace Fred.Common.Tests.Data.Permissions.Mock
{
    public static class PermissionMock
    {
        public static PermissionEnt GetPermissionToShowCiList()
        {
            return new PermissionEnt
            {
                PermissionId = 2,
                PermissionKey = "menu.show.ci.index",
                PermissionType = PermissionTypes.AffichageMenu.ToIntValue(),
                Code = "0002",
                Libelle = "Affichage du menu / Accès à la page 'Gérer les centres d'imputation'",
                PermissionContextuelle = false,

            };
        }
    }
}
