using Fred.Entities.Utilisateur;
using System;

namespace Fred.Entities.EntityBase
{
    /// <summary>
    /// Represente une entité ou il y aurra un auteur et une date de création
    /// </summary>
    public abstract class Creatable : ICreatable
    {
        private DateTime dateCreation;

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de l'entité.
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi l'entité
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de l'entité.
        /// </summary>
        public DateTime DateCreation
        {
            get
            {
                return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc);
            }
            set
            {
                dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
    }
}
