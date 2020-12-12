using Fred.Web.Shared.Models.Rapport;

namespace Fred.Web.Shared.Models.Personnel
{
    /// <summary>
    /// Personnel summary pointage model
    /// </summary>
    public class PersonnelSummaryPointageModel : RapportHebdoEntreeSummaryModel
    {
        /// <summary>
        /// Get or Set the personnel Id
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Societe code
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Get or set the matricule
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Get or set le nom
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Get or set le Prenom
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Is in favourite team
        /// </summary>
        public bool IsInFavouriteTeam { get; set; }

        /// <summary>
        /// Get le nom et le prénom
        /// </summary>
        public string NomPrenom => string.Format("{0} {1}", Nom, Prenom);

        /// <summary>
        /// Get or set le Statut (« C » pour Cadre, « E » pour Etam, « O » pour Ouvrier)
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Get or set la fonction
        /// </summary>
        public string Fonction { get; set; }

        /// <summary>
        /// Get or set le code et libellé de l'établissement comptable
        /// </summary>
        public string EtablissementComptable { get; set; }

        /// <summary>
        /// Get or set le code et libellé de l' établissement de paie
        /// </summary>
        public string EtablissementPaie { get; set; }

    }
}
