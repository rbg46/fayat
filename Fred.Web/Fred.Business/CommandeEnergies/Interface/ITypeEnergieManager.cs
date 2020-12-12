using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface Gestionnaire des Types Energie
    /// </summary>
    public interface ITypeEnergieManager : IManager<TypeEnergieEnt>
    {
        /// <summary>
        /// Récupération de toute la liste des Types d'énergies
        /// </summary>
        /// <returns>Liste des types d'énergies</returns>
        List<TypeEnergieEnt> GetAll();

        /// <summary>
        /// Récupération du type énergie par son identifiant
        /// </summary>
        /// <param name="typeEnergieId">Identifiant du type énergie</param>
        /// <returns>Entité Type Energie</returns>
        TypeEnergieEnt Get(int typeEnergieId);

        /// <summary>
        /// Récupération du type énergie par son code
        /// </summary>
        /// <param name="code">Code du type énergie</param>
        /// <returns>Entité Type Energie</returns>
        TypeEnergieEnt GetByCode(string code);
    }
}
