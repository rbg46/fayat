using System.Collections.Generic;

namespace Fred.ImportExport.Models.Stair.Sphinx
{
    public class SphinxFormulaireModel
    {
        public int SPHINXFormulaireId { get; set; }

        public string TitreFormulaire { get; set; }

        public int NombreEnregistrement { get; set; }

        public string DateCreationFormulaire { get; set; }

        public string DateDerniereReponse { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<SphinxQuestionModel> Questions { get; set; }
    }
}
