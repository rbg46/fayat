namespace Fred.Entities.Budget.Avancement
{
    /// <summary>
    ///   Représente un avancement
    /// </summary>
    public class AvancementEtatEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un état de avancement.
        /// </summary>
        public int AvancementEtatId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un état de avancement.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un état de avancement.
        /// </summary>
        public string Libelle { get; set; }
    }
}