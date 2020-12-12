using System.Collections.Generic;
using Fred.Web.Models.Commande;

namespace Fred.Business.Commande.Models
{
    /// <summary>
    /// Resultat d'un import Rapport
    /// </summary>
    public class ImportResultImportLignesCommande
    {
        /// <summary>
        /// Permet de savoir si l'import est valide
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Permet de savoir si l'import est valide
        /// </summary>
        public List<LigneCommandeImportModel> CommandeLignes { get; set; } = new List<LigneCommandeImportModel>();

        /// <summary>
        /// Liste des messages d'erreurs
        /// </summary>
        public List<List<string>> ErrorMessages { get; set; } = new List<List<string>>();
    }
}
