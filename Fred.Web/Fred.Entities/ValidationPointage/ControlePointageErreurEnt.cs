using System;
using Fred.Entities.Personnel;

namespace Fred.Entities.ValidationPointage
{
    /// <summary>
    ///   Classe ControlePointageErreurEnt
    /// </summary>
    public class ControlePointageErreurEnt
    {
        private DateTime? dateRapport;

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'erreur de validation
        /// </summary>
        public int ControlePointageErreurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le message d'erreur
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du Controle
        /// </summary>
        public int ControlePointageId { get; set; }

        /// <summary>
        ///   Obtient ou définit le controle
        /// </summary>
        public ControlePointageEnt ControlePointage { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du personnel concerné par l'erreur
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel concerné par l'erreur
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        ///   Obtient ou définit le jour du rapport
        /// </summary>
        public DateTime? DateRapport
        {
            get { return (dateRapport.HasValue) ? DateTime.SpecifyKind(dateRapport.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateRapport = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit le code CI (ou Code Affaire)
        /// </summary>
        public string CodeCi { get; set; }
    }
}