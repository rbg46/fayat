using System.Collections.Generic;

using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des ci pour la Reprises des données
    /// </summary>
    public interface IRepriseCiRepository : IMultipleRepository
    {
        /// <summary>
        /// retourne les pays en fonctions des codes passés en parametres
        /// </summary>
        /// <param name="allCodesPays">les codes pays</param>
        /// <returns>liste de pays </returns>
        List<PaysEnt> GetPaysByCodes(List<string> allCodesPays);

        /// <summary>
        /// Mise a jour des cis, seules quelque proprietes sont mise à jours
        /// </summary>
        /// <param name="cis">cis a mettre a jours</param>
        void UpdateCis(List<CIEnt> cis);

        /// <summary>
        ///  Recupere les personnels dont le matricule est contenu dans la liste matricules et pour plusieurs societes
        ///  ATTENTION !!!! : il se peut que 2 personnels aient le meme matricule pour 2 societes differentes.
        /// </summary>
        /// <param name="societeIds">liste des societes dans lequel les personnels existent</param>
        /// <param name="matricules">liste des matricules recherchés</param>
        /// <returns>les personnels pour plusieurs societes</returns>
        List<PersonnelEnt> GetPersonnelListBySocieteIdsAndMatricules(List<int> societeIds, List<string> matricules);
    }
}
