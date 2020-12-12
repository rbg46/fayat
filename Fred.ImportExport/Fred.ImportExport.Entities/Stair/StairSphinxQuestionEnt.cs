using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Stair
{
    [Table("SPHINX_Question")]
    public class StairSphinxQuestionEnt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SPHINXQuestionId { get; set; }

        [Column("TitreQuestion")]
        public string TitreQuestion { get; set; }

        [Column("LibelleQuestion")]
        public string LibelleQuestion { get; set; }

        [Column("SPHINXFormulaireId")]
        public int SPHINXFormulaireId { get; set; }

        [ForeignKey("SPHINXFormulaireId")]
        public StairSphinxFormulaireEnt SphinxFormulaire { get; set; }

        public ICollection<StairSphinxReponseEnt> Reponses { get; set; }
    }
}
