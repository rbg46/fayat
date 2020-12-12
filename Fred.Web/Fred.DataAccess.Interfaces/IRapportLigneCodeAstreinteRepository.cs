
using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Rapport Ligne Code Astreinte Repository  Interface
    /// </summary>
    public interface IRapportLigneCodeAstreinteRepository : IRepository<RapportLigneCodeAstreinteEnt>
    {
        /// <summary>
        /// Ajouter rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneId">rapportLigneId</param>
        /// <param name="codeAstreinteId">codeAstreinteId</param>
        void AddRapportLigneAstreintes(int rapportLigneId, int codeAstreinteId);

        /// <summary>
        /// Ajouter rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneId">rapportLigneId</param>
        /// <param name="codeAstreinteId">codeAstreinteId</param>
        /// <param name="rapportLigneAstreinteId">Rapport ligne astreinte id</param>
        /// <param name="isPrimeNuit">True if is prime pour heures nuit</param>
        /// <returns>Rapport ligne code astreinre</returns>
        RapportLigneCodeAstreinteEnt AddRapportLigneAstreintes(int rapportLigneId, int codeAstreinteId, int rapportLigneAstreinteId, bool isPrimeNuit);

        /// <summary>
        /// Delete prime astreinte by astreinte id
        /// </summary>
        /// <param name="astreinteId">Astreinte identifier</param>
        void DeletePrimesAstreinteByAstreinteId(int astreinteId);

        /// <summary>
        /// Delete prime astreinte by rapport Ligne Astreinte Id
        /// </summary>
        /// <param name="rapportLigneAstreinteId">Astreinte identifier</param>
        void DeletePrimesAstreinteByLigneAstreinteId(int rapportLigneAstreinteId);

        /// <summary>
        /// Get rapport ligne code astreinte
        /// </summary>
        /// <param name="rapportLigneAstreinteId">Rapport ligne astreinte id</param>
        /// <param name="isPrimeNuit">si l'astreinte est  nuit</param>
        /// <returns>Rapport ligne code astreinre</returns>
        RapportLigneCodeAstreinteEnt GetRapportLigneCodeAstreinteEnt(int rapportLigneAstreinteId, bool isPrimeNuit);

        /// <summary>
        /// Delete prime astreinte by rapport Ligne Astreinte Id liste
        /// </summary>
        /// <param name="rapportLigneAstreinteIdList">rapport Ligne Astreinte Id liste</param>
        void DeletePrimesAstreinteByLigneAstreinteList(List<int> rapportLigneAstreinteIdList);
    }
}
