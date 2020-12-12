using System;

namespace Fred.ImportExport.Models.Ci
{
    /// <summary>
    /// Représente un model pour le CI.
    /// </summary>
    public class CiModel
    {
        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'ouverture.
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fermeture.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string LibelleLong { get; set; }

        /// <summary>
        /// Obtient ou définit si le CI est géré par FRED.
        /// </summary>
        public bool ChantierFRED { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une affaire est une SEP.
        /// </summary>
        public bool Sep { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'établissement comptable.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société de l'affaire
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit la ville.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire hors ANAEL (Hélios, etc...).
        /// </summary>
        public string CodeExterne { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une affaire ANAEL parente.
        /// </summary>
        public string CodeParent { get; set; }

        /// <summary>
        /// Obtient ou définit le type du chantier.
        /// </summary>
        public int? CITypeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifant du responsable affaire.
        /// </summary>
        public int? ResponsableAdministratifId { get; set; }

    }
}
