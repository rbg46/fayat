using System.Collections.Generic;
using System.IO;
using Fred.Entities;

namespace Fred.Business.PieceJointe.Services
{
    /// <summary>
    /// Interface du service CommandePieceJointeService
    /// </summary>
    public interface IPieceJointeService : IService
    {

        /// <summary>
        /// Récupérer toutes les pièces jointes attachées à une entité
        /// </summary>
        /// <param name="typeEntite">Filtre sur le type de l'entité (Commande / Dépense / ...)</param>
        /// <param name="entiteId">Filtre sur l'id de l'entité</param>
        /// <returns>Liste des pièces jointes attachées à une entité</returns>
        List<PieceJointeEnt> GetPiecesJointes(PieceJointeTypeEntite typeEntite, int entiteId);

        /// <summary>
        /// Récupérer la pièce jointe correspondante à l'ID passé en paramètre
        /// </summary>
        /// <param name="commonFolder">Chemin commun de stockage</param>
        /// <param name="pieceJointeId">Identifiant de la pièce jointe</param>
        /// <returns>Retourne l'objet correspondant à la pièce jointe</returns>
        PieceJointeEnt GetPieceJointeWithFile(string commonFolder, int pieceJointeId);

        /// <summary>
        /// Ajouter une pièce jointe et l'attacher à une entité (Commande / Depense / ...)
        /// </summary>
        /// <param name="userId">L'id de l'utilisateur</param>
        /// <param name="typeEntite">Type de l'entité à laquelle sera attachée la pièce jointe</param>
        /// <param name="entiteId">L'Id de l'entité à laquelle sera attachée la pièce jointe</param>
        /// <param name="commonFolder">Répertoire commun de sauvegarde</param>
        /// <param name="specificPath">Répertoire spécifique de sauvegarde</param>
        /// <param name="fileName">Nom du fichier</param>
        /// <param name="localPath">Stockage temporaire</param>
        /// <returns>Pièce jointe ajoutée</returns>
        PieceJointeEnt Add(int userId, PieceJointeTypeEntite typeEntite, int entiteId, string commonFolder, string specificPath, string fileName, string localPath);

        /// <summary>
        /// Supprimer une pièce jointe et la détacher de l'entité
        /// </summary>
        /// <param name="commonFolder">Chemin commun de stockage</param>
        /// <param name="pieceJointeId">L'ID de la pièce jointe à supprimer</param>
        void Delete(string commonFolder, int pieceJointeId);
    }
}
