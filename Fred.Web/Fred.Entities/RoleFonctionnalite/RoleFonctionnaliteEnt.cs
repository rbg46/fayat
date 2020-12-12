using System.Diagnostics;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Role;

namespace Fred.Entities.RoleFonctionnalite
{
    /// <summary>
    ///   Représente un RoleFonctionnaliteEnt (association entre un rôle et une fonctionnalite)
    /// </summary>
    [DebuggerDisplay("RoleFonctionnaliteId = {RoleFonctionnaliteId} RoleId = {RoleId} FonctionnaliteId = {FonctionnaliteId} Mode = {Mode}")]
    public class RoleFonctionnaliteEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association
        /// </summary>
        public int RoleFonctionnaliteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit le rôle associé
        /// </summary>
        public virtual RoleEnt Role { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une Fonctionnalite.
        /// </summary>
        public int FonctionnaliteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le FonctionnaliteEnt associé
        /// </summary>
        public virtual FonctionnaliteEnt Fonctionnalite { get; set; }

        /// <summary>
        ///  Obtient ou définit le mode de la FonctionnaliteEnt
        /// </summary>
        public FonctionnaliteTypeMode Mode { get; set; }
    }
}