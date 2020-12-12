using System;
using System.Collections.Generic;

namespace Fred.Entities.Bareme.Models
{
    public class ExportBaremeOrganisationExcelModel
    {
        public DateTime Period { get; set; }

        public int IdSelectedOrganisation { get; set; }

        public int LevelOfSelectedOrganisation { get; set; }

        public List<int> ListOfCheckedLevel { get; set; }

        public bool IsPdf { get; set; }
    }
}
