using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities
{
    [Table("FRED_WORKFLOW_LOGICIEL_TIERS")]
    public class WorkflowLogicielTiersEnt
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RapportLigneId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(LogicielTiers))]
        public int LogicielTiersId { get; set; }

        public LogicielTiersEnt LogicielTiers { get; set; }

        public int AuteurId { get; set; }

        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 2)]
        public string FluxName { get; set; }
    }
}
