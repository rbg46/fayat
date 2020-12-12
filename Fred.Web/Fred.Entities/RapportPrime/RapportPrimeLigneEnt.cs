using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.EntityBase;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.RapportPrime
{
    /// <summary>
    /// Représente ou défini une ligne d'un Rapport Prime
    /// </summary>
    public class RapportPrimeLigneEnt : Deletable
    {
        private DateTime? dateVerrou;
        private DateTime? dateValidation;

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne du Rapport Prime
        /// </summary>
        public int RapportPrimeLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du Rapport Prime auquel est rattachée la ligne
        /// </summary>
        public int RapportPrimeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité RapportPrime
        /// </summary>
        public RapportPrimeEnt RapportPrime { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du CI auquel est rattachée la ligne
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité RapportPrime
        /// </summary>
        public CIEnt Ci { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur du verrouillage
        /// </summary>
        public int? AuteurVerrouId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur du verrouillage
        /// </summary>
        public UtilisateurEnt AuteurVerrou { get; set; }

        /// <summary>
        /// Obtient ou définit la date de verrouillage
        /// </summary>
        public DateTime? DateVerrou
        {
            get
            {
                return dateVerrou.HasValue ? DateTime.SpecifyKind(dateVerrou.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateVerrou = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la validation
        /// </summary>
        public int? AuteurValidationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la validation
        /// </summary>
        public UtilisateurEnt AuteurValidation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de validation
        /// </summary>
        public DateTime? DateValidation
        {
            get
            {
                return dateValidation.HasValue ? DateTime.SpecifyKind(dateValidation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateValidation = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Obtient ou définit l'id de l'entité personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité Personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit le fait que la ligne soit validée
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des astreintes
        /// </summary>
        public List<RapportPrimeLigneAstreinteEnt> ListAstreintes { get; set; } = new List<RapportPrimeLigneAstreinteEnt>();

        /// <summary>
        /// Obtient ou définit la liste des primes
        /// </summary>
        public List<RapportPrimeLignePrimeEnt> ListPrimes { get; set; } = new List<RapportPrimeLignePrimeEnt>();

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public bool IsCreated => RapportPrimeLigneId == 0;

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la ligne est à supprimer
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Obtient ou définit si l'entité a été modifiée ou pas
        /// </summary>
        public bool IsUpdated { get; set; }

        /// <summary>
        /// Vide certaines propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            AuteurCreation = null;
            AuteurModification = null;
            AuteurSuppression = null;
            Ci = null;
            Personnel = null;
            RapportPrime = null;

            if (ListPrimes != null)
            {
                ListPrimes.ToList().ForEach(p => p.CleanLinkedProperties());
            }
            if (ListAstreintes != null)
            {
                ListAstreintes.ToList().ForEach(a => a.CleanLinkedProperties());
            }
        }

        public void Validate(int userId)
        {
            IsValidated = true;
            DateValidation = DateTime.UtcNow;
            AuteurValidationId = userId;
        }

        public void Invalidate()
        {
            IsValidated = false;
            DateValidation = null;
            AuteurValidationId = null;
        }

        public void Delete(int userId)
        {
            DateSuppression = DateTime.UtcNow;
            AuteurSuppressionId = userId;

            ListAstreintes = null;
            ListPrimes = null;
            RapportPrime = null;
        }
    }
}
