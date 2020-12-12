using System;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input
{

    public class ImportPersonnelsBySocieteInput
    {
        public SocieteEnt Societe { get; internal set; }
        public bool IsFullImport { get; internal set; }
        public DateTime? DateDerniereExecution { get; internal set; }
    }
}
