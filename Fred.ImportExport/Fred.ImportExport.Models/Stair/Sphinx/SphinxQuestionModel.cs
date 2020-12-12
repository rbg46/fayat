using System.Collections.Generic;

namespace Fred.ImportExport.Models.Stair.Sphinx
{
    public class SphinxQuestionModel
    {

        public int SPHINXQuestionId { get; set; }

        public string TitreQuestion { get; set; }

        public string LibelleQuestion { get; set; }

        public ICollection<SphinxReponseModel> Reponses { get; set; }
    }
}
