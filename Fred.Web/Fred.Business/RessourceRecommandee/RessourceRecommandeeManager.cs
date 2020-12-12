using System.Collections.Generic;
using AutoMapper;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RessourcesRecommandees;
using Fred.Web.Shared.Models.RessourceRecommandee;

namespace Fred.Business.RessourceRecommandee
{
    /// <summary>
    /// RessourceRecommandeeManager Class
    /// </summary>
    public class RessourceRecommandeeManager : Manager<RessourceRecommandeeEnt, IRessourceRecommandeeRepository>, IRessourceRecommandeeManager
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        public RessourceRecommandeeManager(IUnitOfWork uow, IRessourceRecommandeeRepository ressourceRecommandeeRepository, IMapper mapper)
            : base(uow, ressourceRecommandeeRepository)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Adds the or update ressources recommandee list.
        /// </summary>
        /// <param name="ressourcesRecommandeesList">The ressources recommandees list.</param>
        /// <returns>List of RessourceRecommandeeModel</returns>
        public List<RessourceRecommandeeModel> AddOrUpdateRessourcesRecommandeeList(List<RessourceRecommandeeModel> ressourcesRecommandeesList)
        {
            var listEntitiesMapped = this.mapper.Map<List<RessourceRecommandeeEnt>>(ressourcesRecommandeesList);
            var result = this.Repository.AddOrUpdate(listEntitiesMapped);
            var listToReturnMapped = this.mapper.Map<List<RessourceRecommandeeModel>>(result);
            return listToReturnMapped;

        }

        /// <summary>
        /// Deletes the ressources recommandees list.
        /// </summary>
        /// <param name="listToDelete">The list to delete.</param>
        /// <returns>List of RessourceRecommandeeModel</returns>
        public List<RessourceRecommandeeModel> DeleteRessourcesRecommandeesList(List<RessourceRecommandeeModel> listToDelete)
        {
            var entitiesList = this.mapper.Map<List<RessourceRecommandeeEnt>>(listToDelete);
            var result = this.Repository.DeleteAll(entitiesList);
            return this.mapper.Map<List<RessourceRecommandeeModel>>(result);
        }

        /// <summary>
        /// Adds the ressources recommandees list.
        /// </summary>
        /// <param name="listToAdd">The list to add.</param>
        /// <returns>List of RessourceRecommandeeModel</returns>
        public List<RessourceRecommandeeModel> AddRessourcesRecommandeesList(List<RessourceRecommandeeModel> listToAdd)
        {
            var entitiesList = this.mapper.Map<List<RessourceRecommandeeEnt>>(listToAdd);
            var result = this.Repository.AddAll(entitiesList);
            return this.mapper.Map<List<RessourceRecommandeeModel>>(result);
        }
    }
}
