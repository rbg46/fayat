using Fred.Entities.Depense;
using System.Threading.Tasks;

namespace Fred.Business.Depense
{
    /// <summary>
    ///   Gestionnaire des dépenses.
    /// </summary>
    public interface IGroupeRemplacementTacheManager : IManager<GroupeRemplacementTacheEnt>
    {
        /// <summary>
        ///   Ajout une nouvelle tache de remplacement.
        /// </summary>
        /// <param name="groupe">Groupe à ajouter</param>
        /// <returns>Tache ajoutée</returns>
        GroupeRemplacementTacheEnt AddGroupeRemplacementTache(GroupeRemplacementTacheEnt groupe);

        /// <summary>
        ///   Supprime une dépense.
        /// </summary>
        /// <param name="groupeId">Identifiant unique du groupe à supprimer</param>
        Task DeleteGroupeRemplacementTacheByIdAsync(int groupeId);

        /// <summary>
        ///   Retourne la tache de remplacement l'identifiant unique indiqué.
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe à retrouver.</param>
        /// <returns>La tache retrouvée, sinon nulle.</returns>
        GroupeRemplacementTacheEnt GetGroupeRemplacementTacheById(int groupeId);

        /// <summary>
        /// Récupère la tâche associée à une OD ou Valo ou Dépense
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe à retrouver.</param>
        /// <returns>Liste d'entité associées au groupe ID</returns>
        Task<RemplacementTacheEnt> GetRemplacementTacheOrigineAsync(int groupeId);
    }
}
