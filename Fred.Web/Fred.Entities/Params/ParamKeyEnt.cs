using System;
using System.Collections.Generic;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Params
{
    /// <summary>
    ///   Représente un clé de parametre
    /// </summary>
    public class ParamKeyEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une clé d'un  parametre.
        /// </summary>
        public int ParamKeyId { get; set; }

        /// <summary>
        /// Obtient ou définit libellé d'une clé d'un parametre.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la description d'une clé d'un parametre.
        /// </summary>
        public string Description { get; set; }

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
        public DateTime DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur qui a modifié  l'enregistrement  
        /// </summary>
        public int AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur qui a modifié l'enregistrement  
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des valeurs associées au parametre
        /// </summary>
        public ICollection<ParamValueEnt> ParamValues { get; set; }
    }
}
