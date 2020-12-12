using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.ImportExport
{
  [Table("Jobs")]
  public class JobEnt
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobId { get; set; }
    public DateTime DebutDate { get; set; }
    public DateTime FinDate { get; set; }
    public int Statut { get; set; }
    public int HangFireId { get; set; }
    public int FluxId { get; set; }
    [Required]
    public FluxEnt Flux { get; set; }
  }
}
