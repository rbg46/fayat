using System.Collections.Generic;
using Fred.Entities;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Interface Manager Type Société
    /// </summary>
    public interface ITypeSocieteManager : IManager<TypeSocieteEnt>
    {
        /// <summary>
        ///     Récupération de tous les types de sociétés
        /// </summary>
        /// <returns>Liste de tous les types de sociétés</returns>
        List<TypeSocieteEnt> GetAll();

        /// <summary>
        /// Récupération d'un type société en fonction de son code
        /// </summary>
        /// <param name="code">Code type société</param>
        /// <returns>TypeSocieteEnt</returns>
        TypeSocieteEnt GetByCode(string code);
    }
}
