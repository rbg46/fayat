using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Role
{
    public class RoleRepository : FredRepository<RoleEnt>, IRoleRepository
    {
        private readonly IOrganisationTreeRepository organisationTreeRepository;

        public RoleRepository(FredDbContext context, IOrganisationTreeRepository organisationTreeRepository)
          : base(context)
        {
            this.organisationTreeRepository = organisationTreeRepository;
        }

        protected bool CheckRoleDependencies(int roleId)
        {
            bool isDeletable = false;
            SqlParameter[] parameters =
            {
                new SqlParameter("origTableName", "FRED_ROLE"),
                new SqlParameter("exclusion", string.Empty),
                new SqlParameter("dependance", string.Empty),
                new SqlParameter("origineId", roleId),
                new SqlParameter("delimiter", string.Empty),
                new SqlParameter("resu", SqlDbType.Int) { Direction = ParameterDirection.Output }
              };

            // ReSharper disable once CoVariantArrayConversion
            Context.Database.ExecuteSqlCommand("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUTPUT", parameters);

            // Vérifie s'il y a aucune dépendances (paramètre "resu")
            if (Convert.ToInt32(parameters.First(x => x.ParameterName == "resu").Value) == 0)
            {
                isDeletable = true;
            }

            return isDeletable;
        }

        public RoleEnt GetRoleByCode(string codeRole)
        {
            return Context.Roles.AsNoTracking().FirstOrDefault(o => o.Code == codeRole);
        }

        public bool IsExistingRoleByCodeNomFamilierAndSocieteId(string codeNomFamilier, int societeId)
        {
            return Context.Roles.AsNoTracking().FirstOrDefault(o =>
                codeNomFamilier != null && o.CodeNomFamilier != null &&
                                            o.CodeNomFamilier.ToUpper() == codeNomFamilier.ToUpper() &&
                                             o.SocieteId == societeId) != null;
        }

        public RoleEnt GetRoleBySpecification(int specification)
        {
            return Context.Roles.AsNoTracking().FirstOrDefault(o => o.RoleSpecification.HasValue && (int)o.RoleSpecification.Value == specification);
        }

        public IEnumerable<RoleEnt> GetRoles(string code)
        {
            return Context.Roles
              .Where(r => r.Code == code)

              .AsNoTracking()
              .ToList();
        }

        public IEnumerable<RoleEnt> GetRoleListBySocieteId(int societeId)
        {
            return Context.Roles
              .Include(m => m.SeuilsValidation)
              .Where(r => r.SocieteId == societeId)

              .AsNoTracking()
              .ToList();
        }

        public IEnumerable<RoleEnt> GetRoleByIds(List<int> roleIds)
        {
            return Context.Roles
              .Where(r => roleIds.Contains(r.RoleId))

              .AsNoTracking()
              .ToList();
        }

        public bool IsDeletable(int roleId)
        {
            return CheckRoleDependencies(roleId);
        }

        #region Gestion des seuils
        public IEnumerable<SeuilValidationEnt> GetSeuilValidationListByRoleId(int roleId)
        {
            return Context.SeuilValidations.Include(s => s.Devise).Where(sv => sv.RoleId == roleId).AsNoTracking();
        }

        public SeuilValidationEnt GetSeuilValidationById(int seuilId)
        {
            return Context.SeuilValidations
                       .Include(d => d.Devise)
                       .AsNoTracking()
                       .SingleOrDefault(s => s.SeuilValidationId == seuilId);
        }

        public IEnumerable<SeuilValidationEnt> GetSeuilValidationListByDeviseId(int deviseId)
        {
            return Context.SeuilValidations.Include(d => d.Devise).AsNoTracking();
        }

        public SeuilValidationEnt AddSeuilValidation(SeuilValidationEnt seuilValidation)
        {
            Context.SeuilValidations.Add(seuilValidation);

            return seuilValidation;
        }

        public SeuilValidationEnt UpdateSeuilValidation(SeuilValidationEnt seuilValidationEnt)
        {
            Context.SeuilValidations.Update(seuilValidationEnt);

            return GetSeuilValidationById(seuilValidationEnt.SeuilValidationId);
        }

        public void DeleteSeuilValidationById(int seuilId)
        {
            var seuil = new SeuilValidationEnt { SeuilValidationId = seuilId };
            Context.SeuilValidations.Remove(seuil);
        }

        public IEnumerable<AffectationSeuilUtilisateurEnt> GetUtilisateurRoleList(int utilisateurId, int roleId)
        {
            return Context.UtilOrgaRoleDevises.Where(a => a.UtilisateurId == utilisateurId && a.RoleId == roleId);
        }

        public RoleEnt GetDelegueRoleByOrganisationId(int organisationId)
        {
            var organisationTree = organisationTreeRepository.GetOrganisationTree();

            var societeId = organisationTree.GetSocieteParent(organisationId)?.Id;

            if (!societeId.HasValue)
            {
                return null;
            }

            RoleEnt delegueCiRole = GetRoleListBySocieteId(societeId.Value)
                                        ?.FirstOrDefault(r => r.RoleSpecification.HasValue
                                        && r.RoleSpecification.Value == RoleSpecification.Delegue);

            return delegueCiRole;
        }

        public bool IsRoleDelegue(int roleId)
        {
            return Context.Roles.Any(x => x.RoleId == roleId && x.RoleSpecification == RoleSpecification.Delegue);
        }

        #endregion Gestion des seuils
    }
}
