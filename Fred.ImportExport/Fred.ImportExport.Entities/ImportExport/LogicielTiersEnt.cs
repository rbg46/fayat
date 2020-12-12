using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities
{
    [Table("FRED_LOGICIEL_TIERS")]
    public class LogicielTiersEnt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FredLogicielTiersId { get; set; }

        /// <summary>
        /// Nom du logiciel tiers
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Index("IX_UniqueNomLogicielNomServeurMandant", IsUnique = true, Order = 1)]
        public string NomLogiciel { get; set; }

        /// <summary>
        /// Nom du serveur distant hébergeant le logiciel
        /// </summary>
        [MaxLength(50)]
        [Index("IX_UniqueNomLogicielNomServeurMandant", IsUnique = true, Order = 2)]
        public string NomServeur { get; set; }

        [MaxLength(10)]
        [Index("IX_UniqueNomLogicielNomServeurMandant", IsUnique = true, Order = 3)]
        public string Mandant { get; set; }
    }
}
