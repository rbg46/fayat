using System;

namespace Fred.Web.Shared.Models.Journal
{
    public class JournalFamilleODModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du journal.
        /// </summary>
        public int JournalId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du journal.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libelle du journal.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithOrder { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithoutOrder { get; set; }

        /// <summary>
        /// Obtient ou définit si le journal est coché dans l'écran.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Obtient ou définit si le journal est sélectionné dans l'écran.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Obtient ou définit le type de journal.
        /// </summary>
        public string TypeJournal { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création.
        /// </summary>
        public DateTime DateCreation { get; set; }
    }
}
