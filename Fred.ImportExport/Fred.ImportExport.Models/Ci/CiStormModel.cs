using System;

namespace Fred.ImportExport.Models.Ci
{
    /// <summary>
    /// Représente un model pour le CI Anael
    /// </summary>
    public class CiStormModel
    {
        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire.
        /// </summary>
        public string CodeAffaire { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'ouverture.
        /// </summary>
        public DateTime? DateOuverture { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fermeture.
        /// </summary>
        public DateTime? DateFermeture { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé long.
        /// </summary>
        public string LibelleLong { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// [CodeSocieteComptable] de la société de rattachement du Personnel défini 
        /// comme « Responsable Chantier » du CI (attention ce ne sera pas forcément la même société que celle du CI).
        /// </summary>
        public string CodeSocieteRespChantier { get; set; }

        /// <summary>
        /// [Matricule] du Personnel défini comme « Responsable Chantier » du CI.
        /// </summary>
        public string RespChantier { get; set; }

        /// <summary>
        /// [CodeSocieteComptable] de la société de rattachement du Personnel défini 
        /// comme « Responsable Administratif » du CI (attention ce ne sera pas forcément la même société que celle du CI).
        /// </summary>
        public string CodeSocieteRespAdmin { get; set; }

        /// <summary>
        /// [Matricule] du Personnel défini comme « Responsable Administratif » du CI.
        /// </summary>
        public string RespAdmin { get; set; }

        /// <summary>
        /// Concaténation des champs [Adresse] [Adresse2] [Adresse3] du CI (séparés par un espace)
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// [Ville] du CI
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// [CodePostal] du CI
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// [Code] associé au [PaysId] du CI
        /// </summary>
        public string CodePays { get; set; }

        public override string ToString()
        {
            return $" CodeSociete (= {CodeSociete}) - " +
                   $" CodeAffaire (= {CodeAffaire}) - " +
                   $" DateOuverture (= {DateOuverture}) - " +
                   $" DateFermeture (= {DateFermeture}) - " +
                   $" LibelleLong( = {LibelleLong}) - " +
                   $" Libelle (= {Libelle}) - " +
                   $" CodeEtablissement (= {CodeEtablissement}) - " +
                   $" CodeSocieteResponsableChantier (= {CodeSocieteRespChantier}) - " +
                   $" MatriculeResponsableChantier (= {RespChantier}) - " +
                   $" CodeSocieteResponsableAdministratif (= {CodeSocieteRespAdmin}) - " +
                   $" MatriculeResponsableAdministratif (= {RespAdmin}) - " +
                   $" Adresse (= {Adresse}) - " +
                   $" Ville (= {Ville}) - " +
                   $" CodePostal (= {CodePostal}) - " +
                   $" CodePays (= {CodePays}) - ";
        }

    }
}
