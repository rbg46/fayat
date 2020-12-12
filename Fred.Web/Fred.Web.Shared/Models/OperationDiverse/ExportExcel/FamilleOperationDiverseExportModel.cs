namespace Fred.Web.Shared.Models.OperationDiverse.ExportExcel
{
    /// <summary>
    /// Représente l'export Excel des familles d'OD
    /// </summary>
    public class FamilleOperationDiverseExportModel
    {
        /// <summary>
        /// Obtient ou définit le code du journal.
        /// </summary>
        public string CodeJournal { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé du journal.
        /// </summary>
        public string LibelleJournal { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la famille d'OD rattaché au journal.
        /// </summary>
        public string CodeFamille { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la famille d'OD rattaché au journal.
        /// </summary>
        public string LibelleFamille { get; set; }

        /// <summary>
        /// Obtient ou définit si le numéro de commande est enregistré dans les écritures pour la famille.
        /// </summary>
        public bool IsNumCommandeSaveInCompta { get; set; }
    }
}
