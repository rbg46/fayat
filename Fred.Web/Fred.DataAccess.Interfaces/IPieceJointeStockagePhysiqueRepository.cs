using System.IO;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Repository gérant le stockage phisique des fichiers
    /// </summary>
    public interface IPieceJointeStockagePhysiqueRepository  : IMultipleRepository
    {
        /// <summary>
        /// Déplacer un fichier
        /// </summary>
        /// <param name="fromPath">Chemin source</param>
        /// <param name="toPath">Chemin destination</param>
        void MoveFile(string fromPath, string toPath);

        /// <summary>
        /// Supprimer un fichier
        /// </summary>
        /// <param name="fullPath">Chemin complet du fichier</param>
        void DeleteFile(string fullPath);

        /// <summary>
        /// Récupérer le fichier physique
        /// </summary>
        /// <param name="fullPath">Chemin complet du fichier</param>
        /// <returns>Fichier trouvé</returns>
        byte[] GetFile(string fullPath);
    }
}
