using Fred.DataAccess.Interfaces;
using System.IO;

namespace Fred.DataAccess.PieceJointe
{
    /// <summary>
    /// Repository Piece Jointe
    /// </summary>
    public class PieceJointeStockagePhysiqueRepository : IPieceJointeStockagePhysiqueRepository
    {
        /// <summary>
        /// Supprimer un fichier
        /// </summary>
        /// <param name="fullPath">Chemin complet du fichier</param>
        public void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }

        /// <summary>
        /// Récupérer le fichier physique
        /// </summary>
        /// <param name="fullPath">Chemin complet du fichier</param>
        /// <returns>Fichier trouvé</returns>
        public byte[] GetFile(string fullPath)
        {
            return File.ReadAllBytes(fullPath);
        }

        /// <summary>
        /// Déplacer un fichier
        /// </summary>
        /// <param name="fromPath">Chemin source</param>
        /// <param name="toPath">Chemin destination</param>
        public void MoveFile(string fromPath, string toPath)
        {
            // If the entire path already exists, it will do nothing. (It won't throw an exception)
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(toPath));

            // Move file
            File.Move(fromPath, toPath);
        }
    }
}
