using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente un ci sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} NumeroCommandeExterne = {NumeroCommandeExterne} CodeSociete = {CodeSociete} CodeCi = {CodeCi} ")]
    public class RepriseExcelCommande
    {

        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Le code du ci
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        /// NumeroCommandeExterne
        /// </summary>
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        /// CodeFournisseur
        /// </summary>
        public string CodeFournisseur { get; set; }

        /// <summary>
        /// TypeCommande
        /// </summary>
        public string TypeCommande { get; set; }

        /// <summary>
        /// LibelleEnteteCommande
        /// </summary>
        public string LibelleEnteteCommande { get; set; }

        /// <summary>
        /// CodeDevise
        /// </summary>
        public string CodeDevise { get; set; }

        /// <summary>
        /// DateCommande
        /// </summary>
        public string DateCommande { get; set; }

        /// <summary>
        /// DesignationLigneCommande
        /// </summary>
        public string DesignationLigneCommande { get; set; }

        /// <summary>
        /// CodeRessource
        /// </summary>
        public string CodeRessource { get; set; }

        /// <summary>
        /// CodeTache
        /// </summary>
        public string CodeTache { get; set; }

        /// <summary>
        /// Unite
        /// </summary>
        public string Unite { get; set; }

        /// <summary>
        /// PuHt
        /// </summary>
        public string PuHt { get; set; }

        /// <summary>
        /// QuantiteCommandee
        /// </summary>
        public string QuantiteCommandee { get; set; }

        /// <summary>
        /// QuantiteReceptionnee
        /// </summary>
        public string QuantiteReceptionnee { get; set; }

        /// <summary>
        /// QuantiteFactureeRapprochee
        /// </summary>
        public string QuantiteFactureeRapprochee { get; set; }

        /// <summary>
        /// DateReception
        /// </summary>
        public string DateReception { get; set; }

        /// <summary>
        /// Far
        /// </summary>
        public string Far { get; set; }
    }
}
