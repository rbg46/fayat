using System;
using System.Collections.Generic;

namespace Fred.ImportExport.Entities.ImportExport
{
    public class FluxEnt
    {
        public int FluxId { get; set; }

        public string Code { get; set; }

        public string Libelle { get; set; }

        public string Titre { get; set; }

        public string Description { get; set; }

        public bool IsActif { get; set; }

        public string SocieteCode { get; set; }

        public string SocieteModeleCode { get; set; }

        public string GroupCode { get; set; }

        public DateTime? DateDerniereExecution { get; set; }

        public ICollection<JobEnt> Jobs { get; set; }
    }
}
