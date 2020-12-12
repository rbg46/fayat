using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les agences.
    /// </summary>
    public interface IAgenceRepository : IFredRepository<AgenceEnt>
    {
        /// <summary>
        /// Envoi la liste des agences par fournisseur
        /// </summary>
        /// <param name="fournisseurId">id fournisseur</param>
        /// <returns>liste des agences</returns>
        IEnumerable<AgenceEnt> GetAgencesByFournisseur(int fournisseurId);

        /// <summary>
        /// Envoi la liste des agences par fournisseur
        /// </summary>
        /// <param name="fournisseursIds">liste des ids fournisseur</param>
        /// <returns>liste des agences</returns>
        IEnumerable<AgenceEnt> GetAgencesByFournisseurIds(List<int> fournisseursIds);

        /// <summary>
        /// envoie une angence suivant id
        /// </summary>
        /// <param name="codeAgence">Code agence</param>
        /// <returns>retourne une agence</returns>
        AgenceEnt GetAgenceByCode(string codeAgence);

        /// <summary>
        /// Mise à jour d'une Agence
        /// </summary>
        /// <param name="agenceId">int  Agence</param>
        /// <param name="agence">Aagence à mettre  àjour</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void UpdateAgence(int agenceId, AgenceEnt agence, int userId);

        /// <summary>
        /// Ajouter une nouvelle Agence
        /// </summary>
        /// <param name="agence">Nouvelle Agence</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void AddAgence(AgenceEnt agence, int userId);

        /// <summary>
        /// Clôturer une liste d'agences
        /// </summary>
        /// <param name="agencesIds">Liste des identifiants à clôturer</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        void CloturerAgence(List<int> agencesIds, int userId);
    }
}
