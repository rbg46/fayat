using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    public class RapportHebdoSubNode<T> : RapportHebdoNodeAbstract<T> where T : PointageCell
    {
        public int? RapportLigneId { get; set; }

        public List<RapportHebdoSubNode<T>> SubNodeList { get; set; }

        public int? SocieteId { get; set; }
    }
}
