namespace Fred.ImportExport.Models.JournauxComptable
{
    /// <summary>
    /// Représente un Journal Comptable venant d'Anael
    /// </summary>
    public class JournauxComptableAnaelModel
    {
        /// <summary>
        /// Code Societe
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Code Journal
        /// </summary>
        public string CodeJournal { get; set; }

        /// <summary>
        /// Nom Journal
        /// </summary>
        public string NomJournal { get; set; }

        /// <summary>
        /// Type Journal
        /// </summary>
        public string TypeJournal { get; set; }
    }
}
