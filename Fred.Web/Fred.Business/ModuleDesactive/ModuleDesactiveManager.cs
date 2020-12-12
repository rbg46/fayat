using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ModuleDesactive;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.ModuleDesactive
{
    /// <summary>
    ///   Gestionnaire des ModuleDesactive
    /// </summary>
    public class ModuleDesactiveManager : Manager<ModuleDesactiveEnt, IModuleDesactiveRepository>, IModuleDesactiveManager
    {
        private readonly ISocieteRepository societeRepo;

        public ModuleDesactiveManager(IUnitOfWork uow, IModuleDesactiveRepository moduleDesactiveRepository, ISocieteRepository societeRepo)
          : base(uow, moduleDesactiveRepository)
        {
            this.societeRepo = societeRepo;
        }

        /// <summary>
        /// Desactive un module pour une societe
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'id de l'element ModuleDesactiveEnt nouvellement créé.</returns>
        public int DisableModuleForSocieteId(int moduleId, int societeId)
        {
            var moduleDesactive = GetByModuleIdAndSocieteId(moduleId, societeId);
            if (moduleDesactive != null)
            {
                return moduleDesactive.ModuleDesactiveId;
            }
            var result = Repository.DisableModuleForSocieteId(moduleId, societeId);
            this.Save();

            return result;
        }

        /// <summary>
        /// Active un module pour une societeId et un moduleId
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="societeId">societeId</param>    
        public void EnableModuleForSocieteId(int moduleId, int societeId)
        {
            var moduleDesactive = GetByModuleIdAndSocieteId(moduleId, societeId);
            if (moduleDesactive != null)
            {
                this.Repository.DeleteById(moduleDesactive.ModuleDesactiveId);
                this.Save();
            }
        }


        private ModuleDesactiveEnt GetByModuleIdAndSocieteId(int moduleId, int societeId)
        {
            return this.Repository.Query().Filter(md => md.ModuleId == moduleId && md.SocieteId == societeId).Get().FirstOrDefault();
        }

        /// <summary>
        /// Retourne une liste de ModuleDesactiveEnt.
        /// Un module est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de ModuleDesactiveEnt</returns>
        public IEnumerable<ModuleDesactiveEnt> GetInactifModulesForSocieteId(int societeId)
        {
            return this.Repository.GetInactifModulesForSocieteId(societeId);
        }


        /// <summary>
        /// Retourne une liste d'id de module qui sont desactive sur au moins une societe
        /// </summary>
        /// <returns>liste d'id de module</returns>
        public IEnumerable<int> GetIdsOfModulesPartiallyDisabled()
        {
            return this.Repository.Query()
                                  .Get()
                                  .AsNoTracking()
                                  .GroupBy(md => md.ModuleId)
                                  .Select(g => g.Key)
                                  .ToList();
        }

        /// <summary>
        /// Retourne la listes des societes inactives pour un module donné.
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <returns>Liste d'organisationIDs des societes désactivées.</returns>
        public IEnumerable<int> GetInactivesSocietesForModuleId(int moduleId)
        {
            var fonctionnalitesDesactives = this.Repository.Query().Filter(fd => fd.ModuleId == moduleId).Get().AsNoTracking();
            var societesIds = fonctionnalitesDesactives.Select(fd => fd.SocieteId);
            var societes = societeRepo.Query().Include(s => s.Organisation).Filter(s => societesIds.Contains(s.SocieteId)).Get().AsNoTracking();
            var organisationIds = societes.Select(s => s.Organisation.OrganisationId);
            return organisationIds;
        }

        /// <summary>
        /// Desactive un module pour une liste de societes et un module donné.
        /// </summary>
        /// <param name="moduleId">Id du module </param>
        /// <param name="organisationIdsOfSocietesToDisable"> liste d'organisationId de societes a désactiver</param>
        /// <returns>Liste de societeId désactivé</returns> 
        public IEnumerable<int> DisableModuleByOrganisationIdsOfSocietesAndModuleId(int moduleId, List<int> organisationIdsOfSocietesToDisable)
        {

            var societesIdsAskedToDisable = societeRepo.Query()
                                                        .Include(s => s.Organisation)
                                                        .Filter(s => organisationIdsOfSocietesToDisable.Contains(s.Organisation.OrganisationId))
                                                        .Get()
                                                        .AsNoTracking()
                                                        .Select(s => s.SocieteId)
                                                        .ToList();

            var societesIdsAlreadyDisable = this.Repository.Query()
                                                      .Filter(md => md.ModuleId == moduleId && societesIdsAskedToDisable.Contains(md.SocieteId))
                                                      .Get()
                                                      .AsNoTracking()
                                                      .Select(s => s.SocieteId)
                                                      .ToList();

            var societeIdsReallyToDisable = societesIdsAskedToDisable.Except(societesIdsAlreadyDisable).ToList();

            foreach (var societeId in societeIdsReallyToDisable)
            {
                Repository.DisableModuleForSocieteId(moduleId, societeId);
            }
            this.Save();
            return societesIdsAskedToDisable;
        }


        /// <summary>
        /// Active un module pour une liste d' organisationId de societes et un module donné.
        /// </summary>
        /// <param name="moduleId">Id du module </param>
        /// <param name="organisationIdsOfSocietesToEnable"> liste d'organisationId de societes a sactiver</param>
        /// <returns>Liste de societeId activés</returns> 
        public IEnumerable<int> EnableModuleByOrganisationIdsOfSocietesAndModuleId(int moduleId, List<int> organisationIdsOfSocietesToEnable)
        {
            var societesAlreadyDisabled = this.Repository
                                              .Query()
                                              .Include(md => md.Societe)
                                              .Include(md => md.Societe.Organisation)
                                              .Filter(md => md.ModuleId == moduleId && organisationIdsOfSocietesToEnable.Contains(md.Societe.Organisation.OrganisationId))
                                              .Get()
                                              .AsNoTracking()
                                              .ToList();
            var moduleDesactiveToEnable = societesAlreadyDisabled.ToList();

            foreach (var moduleDesactive in moduleDesactiveToEnable)
            {
                this.Repository.DeleteById(moduleDesactive.ModuleDesactiveId);
            }
            this.Save();
            return moduleDesactiveToEnable.Select(md => md.SocieteId).ToList();
        }


    }
}
