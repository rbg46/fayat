using System;

namespace Fred.ImportExport.Models.Ci
{
    /// <summary>
    /// Représente un model pour le CI Fes.
    /// </summary>
    public class WebApiCiModel
    {
        /// <summary>
        /// Obtient ou définit la description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtient ou définit le code société.
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire.
        /// </summary>
        public string CodeEtablissementComptable { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

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
        /// Obtient ou définit la date d'ouverture.
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fermeture.
        /// </summary>
        public DateTime? DateFermeture { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire ANAEL.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire ANAEL du parent si sous CI.
        /// </summary>
        public string CodeParent { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire hors ANAEL (Hélios, etc...).
        /// </summary>
        public string CodeExterne { get; set; }

        /// <summary>
        /// Obtient ou définit le type du chantier.
        /// </summary>
        public string CiType { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du responsable affaire.
        /// </summary>
        public string MatriculeResponsableAffaire { get; set; }

        /// <summary>
        /// Obtient ou définit le code société. du responsable affaire.
        /// </summary>
        public string SocieteCodeResponsableAffaire { get; set; }

    }
}
