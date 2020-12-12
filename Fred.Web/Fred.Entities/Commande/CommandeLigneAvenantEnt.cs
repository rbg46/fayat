namespace Fred.Entities.Commande
{
    /// <summary>
    /// Représente unde ligne d'avenant de commande.
    /// </summary>
    public class CommandeLigneAvenantEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne d'avenant.
        /// </summary>
        public int CommandeLigneAvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'avenant correspondant.
        /// </summary>
        public int AvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'avenant correspondant.
        /// </summary>
        public CommandeAvenantEnt Avenant { get; set; }

        /// <summary>
        /// Obtient ou définit si la ligne d'avenant est une diminution ou non.
        /// </summary>
        public bool IsDiminution { get; set; }
    }
}
