using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fred.Entities;

namespace Fred.Web.Shared.Models.PieceJointe
{
    /// <summary>
    /// Représentation d'une pièce jointe envoyée / reçue
    /// </summary>
    public class PieceJointeModel
    {
        /// <summary>
        /// Obtient ou définti l'identifiant de la pièce jointe
        /// </summary>
        public int? PieceJointeId { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du fichier
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la taille du fichier en Ko
        /// </summary>
        public int SizeInKo { get; set; }

        /// <summary>
        /// Flag de suppression
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Flag d'ajout
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Date de création de la pièce jointe
        /// </summary>
        public DateTime DateCreation { get; set; }


    }
}
