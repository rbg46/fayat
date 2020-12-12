using System.Collections.Generic;
using Fred.ImportExport.Models.Societe;

namespace Fred.ImportExport.Models.Groupe
{
    public class GroupeInterimaireModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du groupe de la société
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle du groupe
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de société non intérimaire
        /// </summary>
        public List<SocieteNotInterimaireModel> SocieteNotInterimaires { get; set; }
    }
}
