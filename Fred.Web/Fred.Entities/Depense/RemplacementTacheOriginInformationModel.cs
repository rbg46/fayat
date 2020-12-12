using System;
using Fred.Entities.Referential;

namespace Fred.Entities.Depense
{
    public class RemplacementTacheOriginInformationModel
    {
        public int CiId { get; set; }
        public TacheEnt Tache { get; set; }
        public DateTime? DateComptableRemplacement { get; set; }
        public int RequestForGroupeRemplacementTacheId { get; set; }

        public RemplacementTacheEnt ToRemplacementTacheEnt()
        {
            return new RemplacementTacheEnt()
            {
                CiId = this.CiId,
                Tache = this.Tache,
                DateComptableRemplacement = this.DateComptableRemplacement
            };
        }
    }
}
