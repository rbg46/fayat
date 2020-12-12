using Fred.Entities.Referential;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini les tâches associées à une ligne de rapport
    /// </summary>
    public class RapportLigneTacheEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne prime du rapport
        /// </summary>
        public int RapportLigneTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportLigneEnt RapportLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la tâche
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'heure de la tâche
        /// </summary>
        public double HeureTache { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public bool IsCreated => RapportLigneTacheId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit le commentaire de la tache
        /// </summary>
        [NotMapped]
        public string Commentaire { get; set; } = string.Empty;

        /// <summary>
        ///   Désalloue la propriété Tache
        /// </summary>
        public void CleanLinkedProperties()
        {
            RapportLigne = null;
            Tache = null;
        }

        /// <summary>
        ///   Duplication de l'entité (sans l'identifiant de l'entité)
        /// </summary>
        /// <returns>Entité dupliquée</returns>
        public RapportLigneTacheEnt Duplicate()
        {
            return new RapportLigneTacheEnt
            {
                RapportLigneTacheId = 0,
                RapportLigneId = 0,
                RapportLigne = null,
                TacheId = TacheId,
                Tache = Tache,
                HeureTache = HeureTache,
                IsDeleted = IsDeleted
            };
        }
    }
}