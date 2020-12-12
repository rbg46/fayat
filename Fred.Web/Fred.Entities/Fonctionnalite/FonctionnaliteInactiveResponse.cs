namespace Fred.Entities.Fonctionnalite
{
    /// <summary>
    /// Resultat d'une requette pour savoir quelle sont les fonctionnalites desactivees
    /// </summary>
    public class FonctionnaliteInactiveResponse
    {
        /// <summary>
        /// FonctionnaliteId
        /// </summary>
        public int FonctionnaliteId { get; set; }
        /// <summary>
        /// SocieteId
        /// </summary>
        public int SocieteId { get; set; }

    }
}
