using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Groupe;

namespace Fred.Business.Groupe
{
    /// <summary>
    ///   Gestionnaire des Groupes
    /// </summary>
    public interface IGroupeManager : IManager<GroupeEnt>
    {
        Task<int> GetGroupeIdByCodeAsync(string code);

        /// <summary>
        /// Get groupe entité by code
        /// </summary>
        /// <param name="code">Groupe code</param>
        /// <returns>Groupe entité</returns>
        GroupeEnt GetGroupeByCode(string code);

        /// <summary>
        /// Get groupe by code include societes
        /// </summary>
        /// <param name="code">groupe code</param>
        /// <returns>GroupeEnt</returns>
        GroupeEnt GetGroupeByCodeIncludeSocietes(string code);

        /// <summary>
        /// Get url tuto by groupe
        /// </summary>
        /// <returns>GroupeEnt</returns>
        string GetUrlTutoByGroupe();

        /// <summary>
        /// Get Tous les groupes
        /// </summary>
        /// <returns>Liste des groupes</returns>
        IEnumerable<GroupeEnt> GetAll();

        /// <summary>
        /// Recuperer Tous les groupes qui sont dans le perimetre de l'utilisateur connecte
        /// </summary>
        /// <returns>Liste des groupes</returns>
        List<GroupeEnt> GetAllGroupForUser();

        /// <summary>
        /// Recupre le groupe a partir d'un codeSocieteComptable d'une sosiete.
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable</param>
        /// <returns>Le groupe de la societe</returns>
        GroupeEnt GetGroupeByCodeSocieteComptableOfSociete(string codeSocieteComptable);
    }
}
