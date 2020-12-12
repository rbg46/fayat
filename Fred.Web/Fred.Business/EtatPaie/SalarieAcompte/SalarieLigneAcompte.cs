namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// class pour la generation du fichier excel situation salaries pour acompte
    /// </summary>
    public class SalarieLigneAcompte
    {
        /// <summary>
        /// Obtient ou definit l'etablissement du personnel
        /// </summary>
        public string Etablissement { get; set; }

        /// <summary>
        /// Obtient ou definit le personnel
        /// </summary>
        public string Personnel { get; set; }

        /// <summary>
        /// Obtient ou definit le statut du personnel
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Obtient ou definit les nombre de jours travaille du personnel
        /// </summary>
        public string NbJoursTravailles { get; set; }

        /// <summary>
        /// Obtient ou definit les nombre d'absence du personnel
        /// </summary>
        public string NbJoursAbsence { get; set; }

        /// <summary>
        /// Obtient ou definit les nombre de jours non pointe du personnel
        /// </summary>
        public string NbJoursNonPointe { get; set; }

        /// <summary>
        /// NbHeuresNonPointe
        /// </summary>
        public string NbHeuresTravaille { get; set; }
    }
}
