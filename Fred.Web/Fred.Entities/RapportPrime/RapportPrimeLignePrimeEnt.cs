using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Referential;

namespace Fred.Entities.RapportPrime
{
    /// <summary>
    ///   Représente ou défini une prime d'une ligne d'un Rapport Prime
    /// </summary>
    public class RapportPrimeLignePrimeEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la prime de la ligne du Rapport Prime
        /// </summary>
        public int RapportPrimeLignePrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la prime
        /// </summary>
        public int PrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Prime
        /// </summary>
        public PrimeEnt Prime { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne du Rapport Prime
        /// </summary>
        public int RapportPrimeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne du Rapport Prime
        /// </summary>
        public RapportPrimeLigneEnt RapportPrimeLigne { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant de la prime
        /// </summary>
        public double? Montant { get; set; }

        /// <summary>
        /// Indique si la ligne est déjà envoyé a ANAEL
        /// </summary>
        public bool IsSendToAnael { get; set; }

        /// <summary>
        /// Indique si la ligne est déjà envoyé a ANAEL
        /// </summary>
        public int? UpdateUtilisateurId { get; set; }

        /// <summary>
        /// Indique si la ligne est déjà envoyé a ANAEL
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en création
        /// </summary>
        public bool IsCreated => RapportPrimeLignePrimeId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est en modification
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Vide certaines propriété
        /// </summary>
        public void CleanLinkedProperties()
        {
            RapportPrimeLigne = null;
        }
    }
}
