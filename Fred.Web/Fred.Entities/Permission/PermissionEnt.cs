using System.Diagnostics;

namespace Fred.Entities.Permission
{
    /// <summary>
    /// Entité priviliege
    /// </summary>
    [DebuggerDisplay("ID = {PermissionId} PermissionKey = {PermissionKey} PermissionType = {PermissionType} Code = {Code} Libelle = {Libelle} PermissionContextuelle = {PermissionContextuelle} ")]
    public class PermissionEnt
    {
        /// <summary>
        /// clé permettant l'unicité d'un permission
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// clé permettant l'unicité d'un permission
        /// exemple : "menu.show.budget.index"
        /// </summary>   
        public string PermissionKey { get; set; }

        /// <summary>
        /// Type de permission par exemple affichage des menu
        /// </summary>
        public int PermissionType { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Decrit la permission
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Permet de savoir si la permission est dependante du contexte.
        /// Par exemple
        /// </summary>
        public bool PermissionContextuelle { get; set; }

        /// <summary>
        ///   Ce champ est calculé, il indique le mode de la fonctionnalité qui se trouve sur l'entité RoleFonctionnaliteEnt
        /// </summary>
        public FonctionnaliteTypeMode Mode { get; set; }
    }
}
