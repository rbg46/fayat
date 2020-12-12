using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données des astreintes d'une ligne de rapports de primes.
    /// </summary>
    public interface IRapportPrimeLigneAstreinteRepository : IFredRepository<RapportPrimeLigneAstreinteEnt>
    {
        /// <summary>
        /// Permet l'insertion d'une liste de ligne de prime d'astreinte
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        Task InsertRangeAsync(List<RapportPrimeLigneAstreinteEnt> list);

        /// <summary>
        /// Supprime une liste de ligne d'astreinte
        /// </summary>
        /// <param name="list">Liste à insérer</param>
        Task DeleteRangeAsync(List<RapportPrimeLigneAstreinteEnt> list);

        /// <summary>
        /// Retourne la liste des lignes d'astreinte pour une ligne de rapport de prime
        /// </summary>
        /// <param name="rapportPrimeLigne">Ligne de rapport de prime</param>
        /// <returns>List de RapportPrimeLigneAstreinteEnt </returns>
        Task<List<RapportPrimeLigneAstreinteEnt>> GetRapportPrimeLigneAstreintesAsync(RapportPrimeLigneEnt rapportPrimeLigne);

        /// <summary>
        /// Retourne le RapportPrimeLigneAstreinte pour l'Astreinte et le RapportPrimeLigne données en paramètre, null sinon
        /// </summary>
        /// <param name="astreinteId">Id de l'astreinte</param>
        /// <param name="rapportPrimeLigneId">Id de la ligne de rapport de prime</param>
        /// <returns>RapportPrimeLigneAstreinteEnt ou null</returns>
        Task<RapportPrimeLigneAstreinteEnt> GetRapportPrimeLigneAstreinteAsync(int astreinteId, int rapportPrimeLigneId);

        void RemoveRange(IEnumerable<RapportPrimeLigneAstreinteEnt> listAstreinteToDelete);
    }
}
