using System.Collections.Generic;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.Valorisation;

namespace Fred.Business.ExplorateurDepense.Models
{
    public class DataOfCiForComputeResult
    {
        public List<DepenseAchatEnt> Depenses { get; set; }
        public List<OperationDiverseEnt> OperationDiverses { get; set; }
        public List<ValorisationEnt> Valorisations { get; set; }
        public List<int?> DepenseAchatWithFactureIds { get; set; }
        public UniteEnt UniteHeure { get; set; }
    }
}