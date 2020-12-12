using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Stair
{
    [Table("SPHINX_Reponse")]
    public class StairSphinxReponseEnt
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SPHINXReponseId { get; set; }

        [Column("LibelleReponse")]
        public string LibelleReponse { get; set; }

        [Column("NumeroQuestionnaire")]
        public int NumeroQuestionnaire { get; set; }

        [Column("SPHINXQuestionId")]
        public int SPHINXQuestionId { get; set; }

        [ForeignKey("SPHINXQuestionId")]
        public StairSphinxQuestionEnt SphinxQuestion { get; set; }
    }
}
