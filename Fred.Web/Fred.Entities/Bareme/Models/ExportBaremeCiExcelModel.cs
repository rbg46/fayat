using System;

namespace Fred.Entities.Bareme.Models
{
    public class ExportBaremeCiExcelModel
    {
        public DateTime Period { get; set; }

        public int SelectedCiId { get; set; }

        public int SelectedSocieteId { get; set; }

        public bool IsPdf  { get; set; }
    }
}
