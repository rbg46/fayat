using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Fred.ImportExport.Web.Models
{
    /// <summary>
    /// Model RepriseDonneesModel
    /// </summary>
    public class RepriseDonneesModel
    {
        public RepriseDonneesModel()
        {
            Files = new List<HttpPostedFileBase>();
        }

        /// <summary>
        /// Fichiers importer depuis le client web
        /// </summary>
        public List<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// liste des groupes
        /// </summary>
        public List<SelectListItem> Groupes { get; set; }

        /// <summary>
        /// le groupe selectionner
        /// </summary>
        public string SelectedGroupe { get; set; }

        /// <summary>
        /// La liste de erreurs
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

    }
}
