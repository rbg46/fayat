using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    /// <summary>
    /// Represente les informations necessaire pour une societe
    /// </summary>
    public class ImportPersonnelsSocieteData
    {
        public List<EtablissementPaieEnt> EtablissementPaiesForSociete { get; set; }
        public SocieteEnt Societe { get; internal set; }
        public List<PersonnelEnt> FredPersonnelsForSociete { get; internal set; }
        public List<CIEnt> Cis { get; internal set; }
        public List<EtablissementComptableEnt> EtablissementComptablesForSociete { get; set; }
    }
}
