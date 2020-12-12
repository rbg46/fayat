namespace Fred.Web.Models.Depense
{
    public class DepenseTypeModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant du type de dépense
        /// </summary>
        public int DepenseTypeId { get; set; }

        /// <summary>
        ///     Obtient ou définit le code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé du type de dépense
        /// </summary>
        public string Libelle { get; set; }
    }
}
