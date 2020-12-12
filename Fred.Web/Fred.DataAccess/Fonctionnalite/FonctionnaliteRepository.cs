using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Module;
using Fred.Entities.RoleFonctionnalite;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Fonctionnalite
{
    /// <summary>
    /// repo de Fonctionnalite
    /// </summary>
    public class FonctionnaliteRepository : FredRepository<FonctionnaliteEnt>, IFonctionnaliteRepository
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logManager">logManager</param>
        /// <param name="uow">uow</param>
        public FonctionnaliteRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        ///   Retourne la liste des fonctionnalités.
        /// </summary>
        /// <returns>Liste des fonctionnalités.</returns>
        public IEnumerable<FonctionnaliteEnt> GetFeatureList()
        {
            return this.Query().Filter(f => f.DateSuppression == null).Get().AsNoTracking();
        }

        /// <summary>
        ///   Retourne la liste de toutes les fonctionnalités.
        /// </summary>
        /// <returns>Liste des fonctionnalités.</returns>
        public IEnumerable<FonctionnaliteEnt> GetAllFeatureList()
        {
            return this.Query().Get().AsNoTracking();
        }

        /// <summary>
        ///   Retourne l'Fonctionnalite portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="featureId">Identifiant de l'Fonctionnalite à retrouver.</param>
        /// <returns>L'Fonctionnalite retrouvée, sinon nulle.</returns>
        public FonctionnaliteEnt GetFeatureById(int featureId)
        {
            return GetFeatureList().SingleOrDefault(f => f.FonctionnaliteId == featureId);
        }

        /// <summary>
        ///   Retourne le Fonctionnalite par le code indiqué.
        /// </summary>
        /// <param name="code">Code de la Fonctionnalite à retrouver.</param>
        /// <returns>Le Fonctionnalite retrouvé, sinon null.</returns>
        public FonctionnaliteEnt GetFeatureByCode(string code)
        {
            return GetFeatureList().SingleOrDefault(f => f.Code == code);
        }

        /// <summary>
        ///   Retourne une liste de fonctionnalités via l'identifiant du module lié.
        /// </summary>
        /// <param name="moduleId">Identifiant du module lié aux fonctionnalités à retrouver.</param>
        /// <returns>Une liste de fonctionnalités.</returns>
        public IEnumerable<FonctionnaliteEnt> GetFeatureListByModuleId(int moduleId)
        {
            return GetFeatureList().Where(m => m.ModuleId == moduleId);
        }

        /// <summary>
        ///   Ajoute une nouvelle Fonctionnalité
        /// </summary>
        /// <param name="feature">Fonctionnalité à ajouter</param>
        /// <returns>La fonctionnalité ajoutée</returns>
        public FonctionnaliteEnt AddFeature(FonctionnaliteEnt feature)
        {
            this.Insert(feature);

            return feature;
        }

        /// <summary>
        ///   Suppression logique d'une Fonctionnalite en fonction de son identifiant
        /// </summary>
        /// <param name="id">Identifiant de la Fonctionnalité</param>
        public void DeleteFeatureById(int id)
        {
            FonctionnaliteEnt feature = this.FindById(id);
            feature.DateSuppression = DateTime.Now;
            this.Update(feature);
        }

        /// <summary>
        ///   Suppression logique de toutes les fonctionnaltiés liées à un module
        /// </summary>
        /// <param name="moduleId">Identifiant du module</param>
        public void DeleteFeatureListByModuleId(int moduleId)
        {
            var moduleList = GetFeatureListByModuleId(moduleId);

            foreach (FonctionnaliteEnt f in moduleList.ToList())
            {
                DeleteFeatureById(f.FonctionnaliteId);
            }
        }

        /// <summary>
        ///   Met à jour une fonctionnalité.
        /// </summary>
        /// <param name="feature">L'identifiant de la fonctionnalité à mettre à jour</param>
        /// <returns>Fonctionnalité mise à jour</returns>
        public FonctionnaliteEnt UpdateFeature(FonctionnaliteEnt feature)
        {
            this.Update(feature);

            return feature;
        }

        /// <summary>
        /// Recupere les fonctionnalites pour un role donné.
        /// le role doit etre actif sinon rien n'est retourné.
        /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE
        /// sinon aucune fonctionnalite du module n'est retournée.
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>Liste de fonctionnalités 'active'</returns>
        public IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForRoleId(int roleId)
        {
            //recupere la liste des fonctionnalite desactive par le biais des ModuleDesactives
            // Donc, d'abord recuperation des roles inactifs
            var moduleDesactives = (from r in this.Context.Roles
                                    where r.RoleId == roleId
                                    from md in this.Context.ModuleDesactives
                                    where md.SocieteId == r.SocieteId
                                    from m in this.Context.Modules
                                    where m.DateSuppression == null && m.ModuleId == md.ModuleId
                                    select m)
                                    .AsNoTracking()
                                    .Distinct();

            var fonctionnalitesDesactivesDeModuleDesactives = GetFonctionnalitesDesactivesBecauseModuleIsDesactive(moduleDesactives);

            List<FonctionnaliteEnt> fonctionnalitesDesactives = (from r in Context.Roles
                                                                 where r.RoleId == roleId
                                                                 from fd in this.Context.FonctionnaliteDesactives
                                                                 where fd.SocieteId == r.SocieteId
                                                                 from f in this.Context.Fonctionnalites
                                                                 where f.DateSuppression == null && f.FonctionnaliteId == fd.FonctionnaliteId
                                                                 select f)
                                  .AsNoTracking()
                                  .Distinct()
                                  .ToList();

            // recuperation des fonctionnalites autre que non affecté des roles actifs
            List<RoleFonctionnaliteEnt> rolefonctionnalitesDuRoleActif = (from rf in Context.RoleFonctionnalites
                                                                          where rf.RoleId == roleId && rf.Role.Actif /* && rf.Mode != FonctionnaliteTypeMode.UnAffected */
                                                                          select rf)
                                                  .Include(rf => rf.Fonctionnalite)
                                                  .AsNoTracking()
                                                  .Distinct()
                                                  .ToList();

            var allFonctionnalitesDesactives = new List<FonctionnaliteEnt>();
            allFonctionnalitesDesactives.AddRange(fonctionnalitesDesactivesDeModuleDesactives);
            allFonctionnalitesDesactives.AddRange(fonctionnalitesDesactives);
            var fonctionnalitesRestantes = rolefonctionnalitesDuRoleActif.Select(rf => rf.Fonctionnalite).Except(allFonctionnalitesDesactives.ToList(), new FonctionnaliteComparer()).ToList();

            MapPropertyFonctionnaliteMode(rolefonctionnalitesDuRoleActif, fonctionnalitesRestantes);

            return fonctionnalitesRestantes;
        }

        /// <summary>
        /// Recupere les fonctionnalites pour un utilisateur donné.
        /// le role doit etre actif sinon rien n'est retourné.
        /// Le module lié a la fonctionnalite ne doit pas etre desactive voir table FRED_MODULE_DESACTIVE 
        /// sinon aucune fonctionnalite du module n'est retournée.
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>Liste de fonctionnalités 'active'</returns>
        public IEnumerable<FonctionnaliteEnt> GetFonctionnalitesForUtilisateur(int utilisateurId)
        {
            IQueryable<ModuleEnt> moduleDesactives = (from asu in Context.UtilOrgaRoleDevises //AffectationSeuilUtilisateurEnt
                                                      where asu.UtilisateurId == utilisateurId
                                                      from r in Context.Roles
                                                      where r.RoleId == asu.RoleId
                                                      from md in this.Context.ModuleDesactives
                                                      where md.SocieteId == r.SocieteId
                                                      from m in this.Context.Modules
                                                      where m.DateSuppression == null && m.ModuleId == md.ModuleId
                                                      select m)
                                    .AsNoTracking()
                                    .Distinct();

            var fonctionnalitesDesactivesDeModuleDesactives = GetFonctionnalitesDesactivesBecauseModuleIsDesactive(moduleDesactives);

            List<FonctionnaliteEnt> fonctionnalitesDesactives = (from asu in Context.UtilOrgaRoleDevises //AffectationSeuilUtilisateurEnt
                                                                 where asu.UtilisateurId == utilisateurId
                                                                 from r in Context.Roles
                                                                 where r.RoleId == asu.RoleId
                                                                 from fd in this.Context.FonctionnaliteDesactives
                                                                 where fd.SocieteId == r.SocieteId
                                                                 from f in this.Context.Fonctionnalites
                                                                 where f.DateSuppression == null && f.FonctionnaliteId == fd.FonctionnaliteId
                                                                 select f)
                                    .AsNoTracking()
                                    .Distinct()
                                    .ToList();


            // recuperation des fonctionnalites des roles actifs
            var rolefonctionnalitesDesRolesActifs = (from asu in Context.UtilOrgaRoleDevises //AffectationSeuilUtilisateurEnt
                                                     where asu.UtilisateurId == utilisateurId && asu.Role.Actif
                                                     from rf in Context.RoleFonctionnalites
                                                     where rf.RoleId == asu.RoleId && rf.Role.Actif /* && rf.Mode != FonctionnaliteTypeMode.UnAffected */
                                                     select rf)
                                                  .Include(rf => rf.Fonctionnalite)
                                                  .AsNoTracking()
                                                  .Distinct()
                                                  .ToList();

            var allFonctionnalitesDesactives = new List<FonctionnaliteEnt>();
            allFonctionnalitesDesactives.AddRange(fonctionnalitesDesactivesDeModuleDesactives);
            allFonctionnalitesDesactives.AddRange(fonctionnalitesDesactives);
            var fonctionnalitesRestantes = rolefonctionnalitesDesRolesActifs.Select(rf => rf.Fonctionnalite).Except(allFonctionnalitesDesactives.ToList(), new FonctionnaliteComparer()).ToList();

            MapPropertyFonctionnaliteMode(rolefonctionnalitesDesRolesActifs, fonctionnalitesRestantes);

            return fonctionnalitesRestantes;
        }


        private IQueryable<FonctionnaliteEnt> GetFonctionnalitesDesactivesBecauseModuleIsDesactive(IQueryable<ModuleEnt> moduleDesactives)
        {
            return (from md in moduleDesactives
                    from f in Context.Fonctionnalites
                    where f.DateSuppression == null && f.ModuleId == md.ModuleId
                    select f)
                    .AsNoTracking()
                    .Distinct();
        }


        private void MapPropertyFonctionnaliteMode(IEnumerable<RoleFonctionnaliteEnt> rolefonctionnalitesDuRoleActif, List<FonctionnaliteEnt> fonctionnalitesRestantes)
        {
            fonctionnalitesRestantes.ForEach(f =>
            {
                var roleFonctionnalite = rolefonctionnalitesDuRoleActif.FirstOrDefault(rf => rf.FonctionnaliteId == f.FonctionnaliteId);
                f.Mode = roleFonctionnalite?.Mode ?? FonctionnaliteTypeMode.UnAffected;
            });
        }



        /// <summary>
        /// Retourne les fonctionnalitees inactives pour une liste de societes
        /// </summary>
        /// <param name="societeIds"> societe Ids</param>
        /// <returns>Liste de FonctionnaliteInactiveResponse</returns>
        public List<FonctionnaliteInactiveResponse> GetFonctionnalitesInactives(List<int> societeIds)
        {
            var context = this.Context;

            var fonctionnalitesIdsDesactivesOfModuleDesactives = (from md in context.ModuleDesactives
                                                                  where societeIds.Contains(md.SocieteId)
                                                                  from m in context.Modules
                                                                  where m.DateSuppression == null && m.ModuleId == md.ModuleId
                                                                  from f in context.Fonctionnalites
                                                                  where f.DateSuppression == null && f.ModuleId == m.ModuleId
                                                                  select new FonctionnaliteInactiveResponse()
                                                                  {
                                                                      SocieteId = md.SocieteId,
                                                                      FonctionnaliteId = f.FonctionnaliteId
                                                                  })
                                                                  .ToList();

            var fonctionnalitesIdsDesactives = (from fd in context.FonctionnaliteDesactives
                                                where societeIds.Contains(fd.SocieteId)
                                                from f in context.Fonctionnalites
                                                where f.DateSuppression == null && f.FonctionnaliteId == fd.FonctionnaliteId
                                                select new FonctionnaliteInactiveResponse()
                                                {
                                                    SocieteId = fd.SocieteId,
                                                    FonctionnaliteId = f.FonctionnaliteId
                                                })
                                                .ToList();

            var mergedFonctionnaliteInactives = fonctionnalitesIdsDesactivesOfModuleDesactives
                                                            .Concat(fonctionnalitesIdsDesactives)
                                                            .ToList();
            return mergedFonctionnaliteInactives;
        }



        /// <summary>
        /// Retourne les fonctionnalitees pour une permission, une liste de societes et une liste de modes autorisés
        /// </summary>
        /// <param name="permissionId">permissionId</param>
        /// <param name="societeIds">societeIds</param>
        /// <param name="modesAutorized">modesAutorized</param>
        /// <returns>Liste de FonctionnaliteForPermissionResponse</returns>
        public List<FonctionnaliteForPermissionResponse> GetFonctionnalitesForPermission(int permissionId, List<int> societeIds, List<FonctionnaliteTypeMode> modesAutorized)
        {
            var context = this.Context;

            var rolefonctionnalitesDesRolesActifsOfSocietes = (from pf in context.PermissionFonctionnalites
                                                               where pf.PermissionId == permissionId
                                                               from rf in context.RoleFonctionnalites
                                                               where rf.FonctionnaliteId == pf.FonctionnaliteId
                                                               && rf.Role.Actif
                                                               && modesAutorized.Contains(rf.Mode)
                                                               && societeIds.Contains(rf.Role.SocieteId)
                                                               select new FonctionnaliteForPermissionResponse
                                                               {
                                                                   SocieteId = rf.Role.SocieteId,
                                                                   FonctionnaliteId = rf.FonctionnaliteId,
                                                                   RoleId = rf.RoleId
                                                               })
                          .ToList();

            return rolefonctionnalitesDesRolesActifsOfSocietes;
        }
    }
}
