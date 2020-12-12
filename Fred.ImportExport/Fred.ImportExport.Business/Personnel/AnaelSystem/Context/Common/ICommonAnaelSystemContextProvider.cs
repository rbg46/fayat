using System.Collections.Generic;
using Fred.Business;
using Fred.Entities;
using Fred.Entities.Referential;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common
{
    /// <summary>
    /// Provider de data commun a tous les type d'import personnel (excel, par personnelIds, par societe)
    /// </summary>
    public interface ICommonAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Recupere toutes les Pays (pays des personnels) necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>Liste de pays</returns>
        List<PaysEnt> GetPaysOfPersonnels(List<ImportPersonnelSocieteContext> societesContexts);

        /// <summary>
        ///  Recupere la liste des types de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        /// <returns>la liste des types de societe</returns>
        List<TypeSocieteEnt> GetTypeSocietes();
    }
}
