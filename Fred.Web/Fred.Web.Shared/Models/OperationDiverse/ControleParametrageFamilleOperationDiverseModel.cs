namespace Fred.Web.Shared.Models.OperationDiverse
{
    public class ControleParametrageFamilleOperationDiverseModel
    {
        /// <summary>
        /// Obtient ou définit le type d'une famille d'OD
        /// </summary>
        public string TypeFamilleOperationDiverse { get; set; }

        /// <summary>
        /// Obtient ou définit le Code d'une famille d'OD.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une famille d'OD.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'erreur lors du contrôle paramétrage
        /// </summary>
        public string Erreur { get; set; }
    }
}
