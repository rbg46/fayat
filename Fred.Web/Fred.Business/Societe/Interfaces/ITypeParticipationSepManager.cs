using System.Collections.Generic;
using Fred.Entities;

namespace Fred.Business.Societe
{
    /// <summary>
    /// Interface Type Participation Sep Manager
    /// </summary>
    public interface ITypeParticipationSepManager : IManager<TypeParticipationSepEnt>
    {
        /// <summary>
        ///     Récupération de tous les types de participation sep
        /// </summary>
        /// <returns>Liste de tous les types de participation sep</returns>
        List<TypeParticipationSepEnt> GetAll();

        /// <summary>
        /// Récupération d'un type participation sep selon le code
        /// </summary>
        /// <param name="code">Code type participation sep</param>
        /// <returns>TypeParticipationSepEnt</returns>
        TypeParticipationSepEnt Get(string code);
    }
}
