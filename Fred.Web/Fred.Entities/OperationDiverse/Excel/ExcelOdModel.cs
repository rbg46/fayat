using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.OperationDiverse.Excel
{
    /// <summary>
    /// Repersente une operation diverse sur un fichier excel d'import
    /// </summary>
    public class ExcelOdModel
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
        ///   Obtient ou définit la libelle.
        /// </summary>     
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité.
        /// </summary>     
        public string Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse2 de l'affaire.
        /// </summary>      
        public string PuHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>     
        public string CodeUnite { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de l'affaire.
        /// </summary>     
        public string CodeDevise { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de l'affaire.
        /// </summary>      
        public string DateComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du CI
        /// </summary>       
        public string CodeFamille { get; set; }

        /// <summary>
        ///   Obtient ou définit les heures Totales
        /// </summary>      
        public string CodeRessource { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de tache.
        /// </summary>       
        public string CodeTache { get; set; }
        /// <summary>
        ///   Obtient ou définit le code de tache.
        /// </summary>       
        public string Commentaire { get; set; }
    }
}
