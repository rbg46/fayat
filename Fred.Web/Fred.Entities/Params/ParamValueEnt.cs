using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Organisation;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Params
{
    /// <summary>
    ///  Représente un parametre/valeur
    /// </summary>
    public class ParamValueEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un param_valeur.
        /// </summary>
        public int ParamValueId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'organisation.
        /// </summary>
        public virtual OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la clé d'un parametre.
        /// </summary>
        public int ParamKeyId { get; set; }

        /// <summary>
        ///   Obtient ou définit le parametre.
        /// </summary>
        public virtual ParamKeyEnt ParamKey { get; set; }

        /// <summary>
        ///   Obtient ou définit la valeur d'un parametre.
        /// </summary>
        public string Valeur { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de la valeur du clé
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur qui a crée  l'enregistrement  
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur qui a crée  l'enregistrement  
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification de la valeur du clé
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur qui a modifié  l'enregistrement  
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur qui a modifié l'enregistrement  
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }
    }
}
