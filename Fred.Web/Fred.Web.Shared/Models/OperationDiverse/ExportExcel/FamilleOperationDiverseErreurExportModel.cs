namespace Fred.Web.Shared.Models.OperationDiverse.ExportExcel
{
    /// <summary>
    /// Représente l'export Excel des erreur de paramétrage d'une famille d'OD
    /// </summary>
    public class FamilleOperationDiverseErreurExportModel
    {
        /// <summary>
        /// Obtient ou définit le Type (Famille OD ou Journal).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Obtient ou définit le Code de l'élément en erreur.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'élément en erreur.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le descriptif de l'erreur constatée
        /// </summary>
        public string Erreur { get; set; }
    }
}
