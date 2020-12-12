namespace Fred.Entities.IndemniteDeplacement
{
    /// <summary>
    /// Représente le type de calcul à effectuer pour les indemnités de déplacement.
    /// </summary>
    public class IndemniteDeplacementCalculTypeEnt
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
