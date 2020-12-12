using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;


namespace Fred.ImportExport.Business.CI.WebApi.Fred
{
    /// <summary>
    /// Service qui s'occupe des organisations lors de l'import par api
    /// </summary>
    public interface IOrganisationManagerOnImportCiService : IFredIEService
    {
        /// <summary>
        /// Creation de l'organisation par web api
        /// </summary>
        /// <param name="typeOrganisations">liste des type d'organisation</param>
        /// <param name="organisationTree">l'arbre des organisations</param>
        /// <param name="newCi">le nouveau ci</param>
        /// <param name="existingCIs">la liste des ci existant</param>
        /// <param name="cisToAddOrUpdate">la liste des ci a creer ou mettre a jour dans l'imports</param>
        /// <returns>l'orgnisation a créer</returns>
        OrganisationEnt CreateOrganisationOnNewCi(List<TypeOrganisationEnt> typeOrganisations, OrganisationTree organisationTree, CIEnt newCi, List<CIEnt> existingCIs, List<CIEnt> cisToAddOrUpdate);
    }
}
