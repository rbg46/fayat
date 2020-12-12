using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;

namespace Fred.Entities.EcritureComptable
{
    public class EcritureComptableRetreiveResult
    {
        public string NumeroPiece { get; set; }

        public decimal Montant { get; set; }

        public int CiId { get; set; }

        public NatureEnt Nature { get; set; }

        public JournalEnt Journal { get; set; }

        public FamilleOperationDiverseEnt FamilleOperationDiverse { get; set; }
    }
}
