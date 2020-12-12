using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Entities.ImportExport
{
    [Table("SocieteCodeImportMaterielEnt")]
    public class SocieteCodeImportMaterielEnt
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SocieteCodeImportMaterielID { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de la societe.
        /// </summary>
        [Column("Code")]
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du group.
        /// </summary>
        [Column("GroupCode")]
        public int GroupCode { get; set; }
    }
}
