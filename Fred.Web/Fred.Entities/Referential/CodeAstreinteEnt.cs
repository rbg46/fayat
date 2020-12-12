namespace Fred.Entities.Referential
{
    /// <summary>
    /// Represente un code Astreinte 
    /// </summary>
    public class CodeAstreinteEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du code Astreinte
        /// </summary>
        public int CodeAstreinteId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du code astreinte
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit une description du code astreinte
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtient ou définit un etat de sortie
        /// </summary>
        public bool EstSorti { get; set; }

        /// <summary>
        /// Obtient ou définit le groupeId
        /// </summary>
        public int GroupeId { get; set; }
    }
}
