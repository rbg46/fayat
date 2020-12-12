using System.Collections.Generic;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.RessourcesSpecifiquesCI
{
    /// <summary>
    /// Gestionnaire des ressources spécifiques CI
    /// </summary>
    public interface IRessourcesSpecifiquesCiManager : IManager<RessourceEnt>
    {

        /// <summary>
        ///   Retourne la liste des referentielEtendus pour une ci spécifique.
        /// </summary>
        /// <param name="ciId">Identifiant de la CI</param>
        /// <returns>Liste des referentielEtendus.</returns>
        IEnumerable<ChapitreEnt> GetAllReferentielEtenduAsChapitreList(int ciId);

        /// <summary>
        ///   Ajoute une ressource specifique ci
        /// </summary>
        /// <param name="ressource">ressource créée</param>
        /// <returns>Ressource ajoutée</returns>
        RessourceEnt Add(RessourceEnt ressource);

        /// <summary>
        /// Permet de supprimer une ressource à partir de son identifiant
        /// </summary>
        /// <param name="ressourceId">Identifiant de la ressource</param>
        /// <returns>Ressource incative avec date de suppression</returns>
        RessourceEnt DeleteById(int ressourceId);

        /// <summary>
        /// Permet d'obtenir un code incrementé pour la nouvelle ressource créée
        /// </summary>
        /// <param name="ressourceRattachementId"> Identifiant de la ressource rattaché</param>
        /// <returns>le code incrémenté</returns>
        string GetNextRessourceCode(int ressourceRattachementId);
    }
}
