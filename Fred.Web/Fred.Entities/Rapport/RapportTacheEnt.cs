using Fred.Entities.Referential;

namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Entité RapportTacheEnt
    /// </summary>
    public class RapportTacheEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public int RapportTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportEnt Rapport { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit le commentaire d'une tache d'un rapport
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public bool IsCreated => RapportTacheId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
