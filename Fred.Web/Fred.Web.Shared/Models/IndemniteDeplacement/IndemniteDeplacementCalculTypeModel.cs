namespace Fred.Web.Shared.Models.IndemniteDeplacement
{
    /// <summary>
    /// Représente un type de calcul pour une indemnité de déplacement.
    /// </summary>
    public class IndemniteDeplacementCalculTypeModel
    {
        /// <summary>
        /// Identifiant unique du type de calcul.
        /// </summary>
        public int IndemniteDeplacementCalculTypeId { get; set; }

        /// <summary>
        /// Libellé du type de calcul.
        /// </summary>
        public string Libelle { get; set; }
    }
}
