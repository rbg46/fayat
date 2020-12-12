namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Resultat d'une generation de contenu de mail
    /// </summary>
    public class EmailGenerationResult
    {
        /// <summary>
        /// Contenu du mail
        /// </summary>
        public string EmailContent { get; set; }
        /// <summary>
        /// PersonnelId
        /// </summary>
        public int PersonnelId { get; set; }
    }
}
