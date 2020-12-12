using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.ImportExport.Entities.ImportExport
{
    [Table("INTERFACE_TRANSFERT_DONNEE")]
    public class InterfaceTransfertDonneeEnt
    {
        /// <summary>
        ///   Obtient ou définit l'interface de transfert de données.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InterfaceTransfertDonneeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code interface.
        /// </summary>
        public string CodeInterface { get; set; }

        /// <summary>
        ///   Obtient ou définit le code organisation.
        /// </summary>
        public string CodeOrganisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le donnée type.
        /// </summary>
        public string DonneeType { get; set; }

        /// <summary>
        ///   Obtient ou définit la donnée ID.
        /// </summary>
        public string DonneeID { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création.
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit le code société du flux.
        ///   Les valeurs de [Statut] des lignes auront la signification suivante :
        ///   0	=> A traiter
        ///   1	=> Traité
        ///   2	=> Echec du traitement
        ///   3	=> Abandonné
        /// </summary>
        [DefaultValue(0)]
        public int Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de traitement.
        /// </summary>
        public DateTime? DateTraitement { get; set; }

        /// <summary>
        ///   Obtient ou définit le message.
        /// </summary>
        public string Message { get; set; }
    }
}
