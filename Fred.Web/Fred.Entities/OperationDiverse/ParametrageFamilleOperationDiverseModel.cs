using Fred.Entities.Journal;
using Fred.Entities.Referential;
using System.Diagnostics;

namespace Fred.Entities.OperationDiverse
{
    /// <summary>
    /// Représente une famille d'OD
    /// </summary>
    [DebuggerDisplay("FamilleOperationDiverseId = {FamilleOperationDiverseId} Code = {Code} Libelle = {Libelle}")]
    public class ParametrageFamilleOperationDiverseModel
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
