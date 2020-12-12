using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Stair
{
    [Table("STAIR_Indicateurs")]
    public class StairSafeEnt
    {

        public StairSafeEnt(string[] attributs)
        {

            CodeForm = attributs[0];
            IDHistory = int.Parse(attributs[1]);
            UserGeneric = attributs[2];
            Datetime = attributs[3];
            ProjectPath = attributs[4];
            CategoryPath = attributs[5];
            IDQuestion = int.Parse(attributs[6]);
            LabelQuestion = attributs[7];
            IDResponse = int.Parse(attributs[8]);
            TypeResponse = attributs[9];
            LabelResponse = attributs[10];
            AnswerData = attributs[11];
            ContactRecipients = attributs[12];
            UserRecipients = attributs[13];
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STAIRIndicateurId { get; set; }

        [Column("CodeForm")]
        public string CodeForm { get; set; }

        [Column("IDHistory")]
        public int IDHistory { get; set; }

        [Column("UserGeneric")]
        public string UserGeneric { get; set; }

        [Column("Datetime")]
        public string Datetime { get; set; }

        [Column("ProjectPath")]
        public string ProjectPath { get; set; }

        [Column("CategoryPath")]
        public string CategoryPath { get; set; }

        [Column("IDQuestion")]
        public int IDQuestion { get; set; }

        [Column("LabelQuestion")]
        public string LabelQuestion { get; set; }

        [Column("IDResponse")]
        public int IDResponse { get; set; }

        [Column("TypeResponse")]
        public string TypeResponse { get; set; }

        [Column("LabelResponse")]
        public string LabelResponse { get; set; }

        [Column("AnswerData")]
        public string AnswerData { get; set; }

        [Column("ContactRecipients")]
        public string ContactRecipients { get; set; }

        [Column("UserRecipients")]
        public string UserRecipients { get; set; }

    }
}
