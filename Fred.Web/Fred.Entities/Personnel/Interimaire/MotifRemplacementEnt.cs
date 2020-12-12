namespace Fred.Entities.Personnel.Interimaire
{
    /// <summary>
    ///   Représente une tâche.
    /// </summary>
    public class MotifRemplacementEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un motif de remplacement.
        /// </summary>
        public int MotifRemplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du motif
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit lelibellé du motif
        /// </summary>
        public string Libelle { get; set; }
    }
}
