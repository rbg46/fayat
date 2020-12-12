using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    public class FamilleOperationDiverseNatureJournalModel
    {
        /// <summary>
        /// Associated Famille Operation Diverse
        /// </summary>
        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }

        /// <summary>
        /// Associated Journal
        /// </summary>
        public JournalEnt Journal { get; set; }

        /// <summary>
        /// Associated Nature
        /// </summary>
        public NatureEnt Nature { get; set; }
    }
}
