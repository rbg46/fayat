using System.Collections.Generic;
using Fred.Business;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Common
{
    /// <summary>
    /// Provider de data commun a tous les type d'import ci (excel, par ciIds, par societe)
    /// </summary>
    public interface ICommonAnaelSystemContextProvider : IService
    {
        /// <summary>
        /// Recupere tous les responsables necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>liste de responsables</returns>
        List<PersonnelEnt> GetResponsables(List<ImportCiSocieteContext> societesContexts);
        /// <summary>
        /// Recupere toutes les societs des responsables necessaire a l'import
        /// </summary>
        /// <param name="allResponsables">allResponsables</param>
        /// <returns>Liste de societes</returns>
        List<SocieteEnt> GetSocietesOfResponsables(List<PersonnelEnt> allResponsables);
        /// <summary>
        /// Recupere toutes les Pays (pays des ci) necessaire a l'import
        /// </summary>
        /// <param name="societesContexts">societesContexts</param>
        /// <returns>Liste de pays</returns>
        List<PaysEnt> GetPaysOfCis(List<ImportCiSocieteContext> societesContexts);

        /// <summary>
        ///  Recupere la liste des types de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        /// <returns>la liste des types de societe</returns>
        List<TypeSocieteEnt> GetTypeSocietes();
    }
}
