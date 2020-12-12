using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.Stair
{
    [Table("STAIR_Plan_Actions")] //Nom de table pour les Plans d'action
  public class StairPlanActionEnt
  {
    public StairPlanActionEnt(string[] attributs)
    {
      int value;
      IdEnregistrement = int.Parse(attributs[0]);
      UserGeneric = attributs[1];
      IDHistory = int.Parse(attributs[2]);
      int.TryParse(attributs[3], out value);
      IDQuestion = value;
      int.TryParse(attributs[4], out  value);
      IDResponse = value;
      ProjectPath = attributs[5];
      CategoryPath = attributs[6];
      CreatedDate = attributs[7];
      DueDate = attributs[8];
      ResolutionDate = attributs[9];
      Name = attributs[10];
      Comment = attributs[11];
      ResolutionComment = attributs[12];
      Priority = int.Parse(attributs[13]);
      References = attributs[14];
      Contacts = attributs[15];
      Users = attributs[16];
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int STAIRPlanActionId { get; set; }

    [Column("IdEnregistrement")]
    public int IdEnregistrement { get; set; }

    [Column("UserGeneric")]
    public string UserGeneric { get; set; }

    [Column("IDHistory")]
    public int IDHistory { get; set; }

    [Column("IDQuestion")]
    public int IDQuestion { get; set; }

    [Column("IDResponse")]
    public int IDResponse { get; set; }

    [Column("ProjectPath")]
    public string ProjectPath { get; set; }

    [Column("CategoryPath")]
    public string CategoryPath { get; set; }

    [Column("CreatedDate")]
    public string CreatedDate { get; set; }

    [Column("DueDate")]
    public string DueDate { get; set; }

    [Column("ResolutionDate")]
    public string ResolutionDate { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Comment")]
    public string Comment { get; set; }

    [Column("ResolutionComment")]
    public string ResolutionComment { get; set; }

    [Column("Priority")]
    public int Priority { get; set; }

    [Column("References")]
    public string References { get; set; }

    [Column("Contacts")]
    public string Contacts { get; set; }

    [Column("Users")]
    public string Users { get; set; }

   
  }
}
