using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente des Personnels sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSociete} Matricule = {Matricule} ")]
    public class RepriseExcelPersonnel
    {
        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// CodeSociete
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Matricule du personnel au sein de sa societé
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Type du personnel (Externe, Interne, Intérimaire)
        /// </summary>
        public string TypePersonnel { get; set; }

        /// <summary>
        /// Nom du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prénom du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Le type du pointage (H, J) = "CategoriePerso" en BDD
        /// </summary>
        public string TypePointage { get; set; }

        /// <summary>
        /// Date d'entrée du personnel
        /// </summary>
        public string DateEntree { get; set; }

        /// <summary>
        /// Date de sortie du personnel
        /// </summary>
        public string DateSortie { get; set; }

        /// <summary>
        /// Code Ressource
        /// </summary>
        public string CodeRessource { get; set; }

        /// <summary>
        /// Email du personnel
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Adresse 1
        /// </summary>
        public string Adresse1 { get; set; }

        /// <summary>
        /// Adresse 2
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        /// Adresse 3
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        /// CodePostal
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Ville
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Code du pays
        /// </summary>
        public string CodePays { get; set; }

        /// <summary>
        /// Tel1
        /// </summary>
        public string Tel1 { get; set; }

        /// <summary>
        /// Tel2
        /// </summary>
        public string Tel2 { get; set; }
    }
}
