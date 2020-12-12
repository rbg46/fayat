using System;

namespace Fred.ImportExport.Models.Ci
{
    /// <summary>
    /// Représente un model pour le CI Anael
    /// </summary>
    public class CiAnaelModel
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
        public DateTime? DateFermeture { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire.
        /// </summary>
        public string CodeAffaire { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé long.
        /// </summary>
        public string LibelleLong { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le chantier FRED.
        /// </summary>
        public string ChantierFRED { get; set; }

        public override string ToString()
        {
            return $"CodeSociete (= {CodeSociete}) - " +
                   $"CodeAffaire (= {CodeAffaire}) - " +
                   $"DateOuverture (= {DateOuverture}) - " +
                   $"DateFermeture (= {DateFermeture}) - " +
                   $"CodeEtablissement (= {CodeEtablissement}) - " +
                   $"LibelleLong (= {LibelleLong}) - " +
                   $"Libelle (= {Libelle}) - " +
                   $"ChantierFRED (= {ChantierFRED}) - ";
        }
    }
}
