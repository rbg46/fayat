using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Groupe;

namespace Fred.Entities.Societe.Classification
{
    /// <summary>
    /// Classification des sociétes
    /// </summary>
    public class SocieteClassificationEnt
    {
        /// <summary>
        /// Obtient ou définit l'Identifiant de la classification
        /// </summary>
        /// [Key]
        public int SocieteClassificationId { get; set; }

        /// <summary>
        /// Obtient ou définit le Code de la classification
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit Libelle de la classification
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le Flag du statut de la classification
        /// </summary>
        public bool Statut { get; set; } = true;

        /// <summary>
        /// Obtient ou définit L'identication du groupe
        /// </summary>
        /// <remarks>ce n'est un foreign key</remarks>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient la concatenation du code et du libelle 
        /// </summary>
        public string CodeLibelle => Code + " " + Libelle;

        /// <summary>
        /// Obtient ou définit la liste des sociétés attachées à cette classification
        /// </summary>  
        public virtual ICollection<SocieteEnt> Societes { get; set; }

    }
}
