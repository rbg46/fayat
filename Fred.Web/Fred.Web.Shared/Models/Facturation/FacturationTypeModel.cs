namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// Représente un type de facturation
    /// </summary>
    public class FacturationTypeModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du type de facturation.
        /// </summary>
        public int FacturationTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de facturation.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

    }
}
