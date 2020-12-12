using Fred.Entities.EntityBase;

namespace Fred.Entities
{
    /// <summary>
    /// Entité représentant une pièce jointe
    /// </summary>
    public class PieceJointeEnt : AuditableEntity
    {
        /// <summary>
        /// Obtient ou définti l'identifiant de la pièce jointe
        /// </summary>
        public int PieceJointeId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la pièce jointe
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le chemin de stockage de la pièce jointe
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Obtient ou définit la taille du fichier en Ko
        /// </summary>
        public int SizeInKo { get; set; }

        /// <summary>
        /// Obtient ou définit le tableau d'octect de la pièce jointe
        /// </summary>
        public byte[] PieceJointeArray { get; set; }
    }
}
