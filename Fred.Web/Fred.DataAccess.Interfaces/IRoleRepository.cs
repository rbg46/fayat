
using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;

namespace Fred.DataAccess.Interfaces

{
    public interface IRoleRepository : IRepository<RoleEnt>
    {
        RoleEnt GetRoleByCode(string codeRole);

        bool IsExistingRoleByCodeNomFamilierAndSocieteId(string codeNomFamilier, int societeId);

        RoleEnt GetRoleBySpecification(int specification);

        IEnumerable<RoleEnt> GetRoleListBySocieteId(int societeId);

        bool IsDeletable(int roleId);

        #region Gestion des seuils

        SeuilValidationEnt GetSeuilValidationById(int seuilId);

        IEnumerable<SeuilValidationEnt> GetSeuilValidationListByRoleId(int roleId);

        IEnumerable<SeuilValidationEnt> GetSeuilValidationListByDeviseId(int deviseId);

        SeuilValidationEnt AddSeuilValidation(SeuilValidationEnt seuilValidation);

        SeuilValidationEnt UpdateSeuilValidation(SeuilValidationEnt seuilValidationEnt);

        void DeleteSeuilValidationById(int seuilId);

        IEnumerable<RoleEnt> GetRoles(string code);

        #endregion Gestion des seuils

        IEnumerable<AffectationSeuilUtilisateurEnt> GetUtilisateurRoleList(int utilisateurId, int roleId);

        RoleEnt GetDelegueRoleByOrganisationId(int organisationId);

        bool IsRoleDelegue(int roleId);

        IEnumerable<RoleEnt> GetRoleByIds(List<int> roleIds);
    }
}
