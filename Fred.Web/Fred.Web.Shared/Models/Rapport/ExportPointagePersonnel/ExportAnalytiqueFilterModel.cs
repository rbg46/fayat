using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class ExportAnalytiqueFilterModel
    {
        public int UserId { get; set; }
        public DateTime DateDebut { get; set; }
        public bool Hebdo { get; set; }
        public bool Simulation { get; set; }
        public string SocieteCode { get; set; }
        public List<int> EtablissementsComptablesIds { get; set; }
        public List<string> EtablissementsComptablesCodes { get; set; }
        public bool AllEtablissements { get; set; }
    }
}
