using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.EntityBase;
using Fred.Entities.Societe;

namespace Fred.Entities.RapportPrime
{
    /// <summary>
    /// Représente un Rapport Prime mensuel
    /// </summary>
    public class RapportPrimeEnt : Deletable
    {
        private DateTime dateRapportPrime;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Rapport Prime.
        /// </summary>
        public int RapportPrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de chantier (le mois)
        /// </summary>
        public DateTime DateRapportPrime
        {
            get
            {
                return DateTime.SpecifyKind(dateRapportPrime, DateTimeKind.Utc);
            }
            set
            {
                dateRapportPrime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Obtient ou définit l'ID de la société associée.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité societe
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes du Rapport Prime
        /// </summary>
        public List<RapportPrimeLigneEnt> ListLignes { get; set; } = new List<RapportPrimeLigneEnt>();

        /// <summary>
        ///   Vide certaines propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            AuteurCreation = null;
            AuteurModification = null;
            AuteurSuppression = null;

            if (ListLignes != null)
            {
                ListLignes.ToList().ForEach(l => l.CleanLinkedProperties());
            }
        }
    }
}
