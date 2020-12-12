using Fred.Entities.OperationDiverse;
using Fred.Web.Models;
using Fred.Web.Shared.Models.OperationDiverse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Interface des managers des familles d'OD
    /// </summary>
    public interface IFamilleOperationDiverseManager : IManager<FamilleOperationDiverseEnt>
    {
        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        IEnumerable<FamilleOperationDiverseEnt> GetFamiliesBySociety(int societeId);

        IEnumerable<FamilleOperationDiverseEnt> GetFamiliesBySociety(List<int> societeIds);

        IEnumerable<FamilleOperationDiverseModel> GetFamiliesOdOrdered(int societeId);

        /// <summary>
        /// Récupère la liste des familles d'OD pour la société d'un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI dont la société est rattachée aux familles d'OD</param>
        /// <returns>Liste des familles d'OD de la société du CI</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        IEnumerable<FamilleOperationDiverseEnt> GetFamiliesByCI(int ciId);

        /// <summary>
        /// Lance le contrôle paramétrage pour les journaux
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Un modèle contenant le résultat du contrôle paramétrage</returns>
        IReadOnlyList<ControleParametrageFamilleOperationDiverseModel> LaunchControleParametrageForJournal(int societeId);

        /// <summary>
        /// Lance le contrôle paramétrage pour les natures
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Un modèle contenant le résultat du contrôle paramétrage</returns>
        IReadOnlyList<ControleParametrageFamilleOperationDiverseModel> LaunchControleParametrageForNature(int societeId);

        /// <summary>
        /// Sauvegarde les modifications d'une famille OD
        /// </summary>
        /// <param name="familleOperationDiverseModel">Famille OD à modifier</param>
        Task UpdateFamilleOperationDiverseAsync(FamilleOperationDiverseModel familleOperationDiverseModel);

        /// <summary>
        /// Récupére un famille d'opération diverse pour un id
        /// </summary>
        /// <param name="familleOperationDiverseId">Identifiant Famille OD</param>
        /// <returns><see cref="FamilleOperationDiverseModel"/></returns>
        FamilleOperationDiverseModel GetFamille(int familleOperationDiverseId);

        /// <summary>
        /// Retourne la liste des familles d'opérations diverse pour une liste de code de famille
        /// </summary>
        /// <param name="familleIds">Identifiants des mfamilles</param>
        /// <returns>Liste de <see cref="FamilleOperationDiverseModel"/></returns>
        IReadOnlyList<FamilleOperationDiverseModel> GetFamilles(List<int> familleIds);

        /// <summary>
        /// Permet de mettre à jour la famille associée aux natures et des journaux (RZB et Moulin)
        /// </summary>
        /// <param name="journaux"></param>
        /// <param name="natures"></param>
        Task<IReadOnlyList<FamilleOperationDiverseNatureJournalModel>> SetParametrageNaturesJournaux(FamilleOperationDiverseModel fod);

        /// <summary>
        /// Retourne la liste des parametrages famille OD / Natures / Journaux
        /// </summary>
        /// <param name="societeId"></param>
        /// <returns>liste des parametrages famille OD / Natures / Journaux</returns>
        IReadOnlyList<FamilleOperationDiverseNatureJournalModel> GetAllParametrageFamilleOperationDiverseNaturesJournaux(int societeId);

        /// <summary>
        /// Récupère les paramétrages (doublons) pour une famille
        /// </summary>
        /// <param name="fod">famille operation diverse</param>
        /// <returns>Une liste de doublons si existe</returns>
        IReadOnlyList<FamilleOperationDiverseNatureJournalModel> GetDuplicateParametrageFamilleOperationDiverse(FamilleOperationDiverseModel fod);

        IEnumerable<TypeOdFilterExplorateurDepense> GetTypeOdFilter(int societeId);

        int GetFamilyTaskId(int familleOperationDiverseId);
    }
}
