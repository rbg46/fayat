using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Stair
{
    [Table("SPHINX_Formulaire")]
    public class StairSphinxFormulaireEnt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SPHINXFormulaireId { get; set; }

        [Column("TitreFormulaire")]
        public string TitreFormulaire { get; set; }

        [Column("NombreEnregistrement")]
        public int NombreEnregistrement { get; set; }

        [Column("DateCreationFormulaire")]
        public string DateCreationFormulaire { get; set; }

        [Column("DateDerniereReponse")]
        public string DateDerniereReponse { get; set; }

        [Column("IsOpen")]
        public bool IsOpen { get; set; }

        public ICollection<StairSphinxQuestionEnt> Questions { get; set; }
    }
}
