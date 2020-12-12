using Fred.Entities.Role;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un module.
    /// </summary>
    [DebuggerDisplay("SeuilValidationId = {SeuilValidationId} DeviseId = {DeviseId} RoleId = {RoleId}  Montant = {Montant}")]
    public class SeuilValidationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un seuil de validation
        /// </summary>
        public int SeuilValidationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la devise associée
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise associée au seuil de validation
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant du seuil
        /// </summary>
        public int Montant { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du rôle associé
        /// </summary>
        public int? RoleId { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        /////////////////////////////////////////////////////////////////////////// 
        /// <summary>
        /// Parent Role pointed by [FRED_ROLE_DEVISE].([RoleId]) (FK_ROLE_DEVISE_ROLE)
        /// </summary>
        public virtual RoleEnt Role { get; set; } // FK_ROLE_DEVISE_ROLE
    }
}