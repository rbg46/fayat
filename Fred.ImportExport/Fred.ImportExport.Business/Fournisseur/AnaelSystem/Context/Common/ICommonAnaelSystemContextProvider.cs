using System.Collections.Generic;
using Fred.Business;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common
{
    /// <summary>
    /// Provider de data commun a tous les type d'import ci (excel, par ciIds, par societe)
    /// </summary>
    public interface ICommonAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Recupere toutes les Pays (pays des ci) necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>Liste de pays</returns>
        List<PaysEnt> GetPaysOfFournisseurs(List<ImportFournisseurSocieteContext> societesContexts);

        /// <summary>
        ///  Recupere la liste des types de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        /// <returns>la liste des types de societe</returns>
        List<TypeSocieteEnt> GetTypeSocietes();
    }
}
