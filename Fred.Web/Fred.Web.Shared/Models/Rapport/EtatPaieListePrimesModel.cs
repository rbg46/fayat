using System.Collections.Generic;

namespace Fred.Web.Models.Rapport
{
    public class EtatPaieListePrimesModel
    {
        /// <summary>
        /// Obtient ou définit le Etablissement
        /// </summary>
        public string Etablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le Personnel
        /// </summary>
        public string Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit le Affaire
        /// </summary>
        public string Affaire { get; set; }

        /// <summary>
        /// Obtient ou définit le Code
        /// </summary>
        public string CodePrime { get; set; }

        /// <summary>
        /// Obtient ou définit le Quantite
        /// </summary>
        public double Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le Unite
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// Obtient ou définit le nom
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le montant
        /// </summary>
        public double Montant { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des jours concernés par les astreintes sous forme d'une liste de numéro de jours concaténés
        /// </summary>
        public List<int> ListJoursAstreintes { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des jours concernés par les astreintes sous forme d'une liste de numéro de jours concaténés
        /// </summary>
        public string JoursAstreintes { get; set; }
    }
}
