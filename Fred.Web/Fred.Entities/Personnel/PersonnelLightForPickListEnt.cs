using System;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.Entities.Personnel
{
    /// <summary>
    /// Représente un membre du personnel
    /// </summary>
    [Serializable]
    public class PersonnelLightForPickListEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public SocieteLightEnt Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'id ressource du personnel.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet ressource d'une ligne de commande.
        /// </summary>
        public RessourceLightEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du membre du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient le libellé référentiel d'un personnel.
        /// </summary>
        public string LibelleRef { get; set; }

        /// <summary>
        /// Obtient le code référentiel d'un personnel.
        /// </summary>
        public string CodeRef { get; set; }

        /// <summary>
        /// CodeLibelleRef
        /// </summary>
        public string CodeLibelleRef { get; set; }
    }
}
