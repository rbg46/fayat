using System;

namespace Fred.Web.Shared.Models.Rapport
{
    public class PointageDuplicateModel
    {
        public int RapportLigneId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CiId { get; set; }

    }
}
