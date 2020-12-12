using System.Collections.Generic;
using Fred.Entities.Organisation.Tree;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;

namespace Fred.ImportExport.Business.CI.WebApi.Fred
{
    /// <summary>
    /// Service qui effectue l'import par web api
    /// </summary>
    public interface IFredCiImportForWebApiService : IFredIEService
    {
        /// <summary>
        /// Execute l'import par web api par societe
        /// </summary>
        /// <param name="societesContexts">le context par societe</param>
        /// <param name="organisationTree">l'arbre des orgas</param>
        void ManageImportedCIsFromApi(List<ImportCiByWebApiSocieteContext> societesContexts, OrganisationTree organisationTree);
    }
}
