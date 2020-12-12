namespace Fred.ImportExport.Models.Societe
{
    public class SocieteNotInterimaireModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la société 
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit si il a été checké ou non 
        /// </summary>
        public bool Checked { get; set; }
    }
}
