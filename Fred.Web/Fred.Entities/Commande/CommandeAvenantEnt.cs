using Fred.Entities.Avis;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Commande
{
    /// <summary>
    /// Représente un avenant de commande.
    /// </summary>
    public class CommandeAvenantEnt
    {
        private DateTime? dateValidation;

        private DateTime? dateCreation;

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la 'avenant.
        /// </summary>
        public int CommandeAvenantId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la commande de l'avenant.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande de l'avenant.
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de l'avenant.
        /// </summary>
        public int NumeroAvenant { get; set; }


        /// <summary>
        /// Avis attachés à l'avenant d'une commande
        /// </summary>
        public List<AvisCommandeAvenantEnt> AvisCommandeAvenant { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur qui a validé l'avenant.
        /// </summary>
        public int? AuteurValidationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur qui a validé l'avenant.
        /// </summary>
        public UtilisateurEnt AuteurValidation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de validation de l'avenant.
        /// </summary>
        public DateTime? DateValidation
        {
            get { return (dateValidation.HasValue) ? DateTime.SpecifyKind(dateValidation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateValidation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur qui a créée l'avenant.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur qui a créée l'avenant.
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de validation de l'avenant.
        /// </summary>
        public DateTime? DateCreation
        {
            get { return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'idenfiant du job hangfire.
        /// </summary>
        public string HangfireJobId { get; set; }
    }
}
