using Fred.Web.Models.CI;
using Fred.Web.Shared.Models.Journal;
using Fred.Web.Shared.Models.Nature;
using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.OperationDiverse
{
    /// <summary>
    /// Représente une famille d'OD
    /// </summary>
    public class FamilleOperationDiverseModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une famille d'OD
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Obtient ou définit le Code d'une famille d'OD.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une famille d'OD.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé court d'une famille d'OD.
        /// </summary>
        public string LibelleCourt { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la societe.
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Obtient ou définit si la comptabilisation de la famille d'OD est cumulé, sinon il s'agit d'une comptabilisation ligne à ligne.
        /// </summary>
        public bool IsAccrued { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la societe
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Dertermine si l'OD doit obligatoirement avoir une commande associé à l'écriture 
        /// </summary>
        public bool MustHaveOrder { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'ordre d'une famille d'OD.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des journaux associees.
        /// </summary>
        public List<JournalFamilleODModel> AssociatedJournaux { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des natures associees.
        /// </summary>
        public List<NatureFamilleOdModel> AssociatedNatures { get; set; }
    }
}
