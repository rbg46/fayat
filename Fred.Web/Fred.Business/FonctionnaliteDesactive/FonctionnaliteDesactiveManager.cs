using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.FonctionnaliteDesactive;
using Microsoft.EntityFrameworkCore;

namespace Fred.Business.FonctionnaliteDesactive
{
    /// <summary>
    ///   Gestionnaire des FonctionnaliteDesactive
    /// </summary>
    public class FonctionnaliteDesactiveManager : Manager<FonctionnaliteDesactiveEnt, IFonctionnaliteDesactiveRepository>, IFonctionnaliteDesactiveManager
    {
        private readonly ISocieteRepository societeRepo;

        public FonctionnaliteDesactiveManager(IUnitOfWork uow, IFonctionnaliteDesactiveRepository fonctionnaliteDesactiveRepository, ISocieteRepository societeRepo)
          : base(uow, fonctionnaliteDesactiveRepository)
        {
            this.societeRepo = societeRepo;
        }

        /// <summary>
        /// Retourne une liste de FonctionnaliteDesactiveEnt.
        /// Un Fonctionnalite est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de FonctionnaliteDesactiveEnt</returns>
        public IEnumerable<FonctionnaliteDesactiveEnt> GetInactifFonctionnalitesForSocieteId(int societeId)
        {
            return this.Repository.GetInactifFonctionnalitesForSocieteId(societeId);
        }


        /// <summary>
        /// Retourne une liste d'id de module qui sont desactive sur au moins une societe
        /// </summary>
        /// <returns>liste d'id de module</returns>
        public IEnumerable<int> GetIdsOfFonctionnalitesPartiallyDisabled()
        {
            return this.Repository.Query()
                                  .Get()
                                  .AsNoTracking()
                                  .GroupBy(md => md.FonctionnaliteId)
                                  .Select(g => g.Key)
                                  .ToList();
        }

        /// <summary>
        /// Retourne la listes des societes inactives pour un fonctionnalite donnée.
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <returns>Liste d'organisationIDs des societes désactivées.</returns>
        public IEnumerable<int> GetInactivesSocietesForFonctionnaliteId(int fonctionnaliteId)
        {
            var fonctionnalitesDesactives = this.Repository.Query().Filter(fd => fd.FonctionnaliteId == fonctionnaliteId).Get();
            var societesIds = fonctionnalitesDesactives.Select(fd => fd.SocieteId);
            var societes = societeRepo.Query().Include(s => s.Organisation).Filter(s => societesIds.Contains(s.SocieteId)).Get();
            var organisationIds = societes.Select(s => s.Organisation.OrganisationId);
            return organisationIds;
        }


        /// <summary>
        /// Desactive une fonctionnalité pour une liste de societes et un fonctionnalité donnée.
        /// </summary>
        /// <param name="fonctionnaliteId">Id de la fonctionnalité </param>
        /// <param name="organisationIdsOfSocietesToDisable"> liste d'organisationId de societes a désactiver</param>
        /// <returns>Liste de societeId désactivé</returns> 
        public IEnumerable<int> DisableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(int fonctionnaliteId, List<int> organisationIdsOfSocietesToDisable)
        {
            var societesIdsAskedToDisable = societeRepo.Query()
                                                        .Include(s => s.Organisation)
                                                        .Filter(s => organisationIdsOfSocietesToDisable.Contains(s.Organisation.OrganisationId))
                                                        .Get()
                                                        .AsNoTracking()
                                                        .Select(s => s.SocieteId)
                                                        .ToList();

            var societesIdsAlreadyDisable = this.Repository.Query()
                                                      .Filter(md => md.FonctionnaliteId == fonctionnaliteId && societesIdsAskedToDisable.Contains(md.SocieteId))
                                                      .Get()
                                                      .AsNoTracking()
                                                      .Select(s => s.SocieteId)
                                                      .ToList();

            var societeIdsReallyToDisable = societesIdsAskedToDisable.Except(societesIdsAlreadyDisable).ToList();

            foreach (var societeId in societeIdsReallyToDisable)
            {
                Repository.DisableFonctionnaliteForSocieteId(fonctionnaliteId, societeId);
            }
            this.Save();
            return societesIdsAskedToDisable;
        }


        /// <summary>
        /// Active une fonctionnalité pour une liste d' organisationId de societes et une fonctionnalité donnée.
        /// </summary>
        /// <param name="fonctionnaliteId">Id de la fonctionnalité </param>
        /// <param name="organisationIdsOfSocietesToEnable"> liste d'organisationId de societes a activer</param>
        /// <returns>Liste de societeId activés</returns> 
        public IEnumerable<int> EnableFonctionnaliteByOrganisationIdsOfSocietesAndFonctionnaliteId(int fonctionnaliteId, List<int> organisationIdsOfSocietesToEnable)
        {
            var societesAlreadyDisabled = this.Repository
                                              .Query()
                                              .Include(md => md.Societe)
                                              .Include(md => md.Societe.Organisation)
                                              .Filter(md => md.FonctionnaliteId == fonctionnaliteId && organisationIdsOfSocietesToEnable.Contains(md.Societe.Organisation.OrganisationId))
                                              .Get()
                                              .AsNoTracking()
                                              .ToList();
            var fonctionnalitesDesactiveToEnable = societesAlreadyDisabled.ToList();

            foreach (var fonctionnaliteDesactive in fonctionnalitesDesactiveToEnable)
            {
                this.Repository.DeleteById(fonctionnaliteDesactive.FonctionnaliteDesactiveId);
            }
            this.Save();
            return fonctionnalitesDesactiveToEnable.Select(md => md.SocieteId).ToList();
        }

        /// <summary>
        /// Permet de verifer si une fonctionnalite est desactive pour une societe.
        /// </summary>
        /// <param name="fonctionnaliteId">L'id de la fonctionnalite à verifer</param>
        /// <param name="societeId">L'id de la societe</param>
        /// <returns>Boolean indique si la fonctionnalite est desactivee ou non</returns>
        public bool IsFonctionnaliteDesactiveForSociete(int fonctionnaliteId, int societeId)
        {
            return this.Repository.Any(x => x.FonctionnaliteId == fonctionnaliteId && x.SocieteId == societeId);
        }

        /// <summary>
        /// Permet de verifer si une fonctionnalite est desactive pour une societe.
        /// </summary>
        /// <param name="fonctionnaliteCode">Le code de la fonctionnalite à verifer</param>
        /// <param name="societeId">L'id de la societe</param>
        /// <returns>Boolean indique si la fonctionnalite est desactivee ou non</returns>
        public bool IsFonctionnaliteDesactiveForSociete(string fonctionnaliteCode, int societeId)
        {
            return this.Repository.Any(x => x.Fonctionnalite.Code == fonctionnaliteCode && x.SocieteId == societeId);
        }
    }
}
