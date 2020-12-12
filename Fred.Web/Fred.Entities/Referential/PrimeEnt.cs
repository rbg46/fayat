using Fred.Entities.CI;
using Fred.Entities.EntityBase;
using Fred.Entities.Groupe;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Enumeration des types de primes possibles
    /// </summary>
    public enum ListePrimeType
    {
        /// <summary>
        /// Journalière
        /// </summary>
        PrimeTypeJournaliere = 0,
        /// <summary>
        /// Horaire
        /// </summary>
        PrimeTypeHoraire = 1,
        /// <summary>
        /// Mensuelle
        /// </summary>
        PrimeTypeMensuelle = 2
    }

    /// <summary>
    ///   Représente une prime
    /// </summary>
    public class PrimeEnt : Deletable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une prime.
        /// </summary>
        public int PrimeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une prime.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une prime.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une prime est de type horaire (1), journalière (0) ou mensuelle (2)
        /// </summary>
        public ListePrimeType PrimeType { get; set; }

        /// <summary>
        ///   Obtient ou définit le nombre d'heures max dans le cas où la prime serait de type horaire.
        /// </summary>
        public double NombreHeuresMax { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la prime est active ou non.
        /// </summary>
        public bool Actif { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ETAM est actif ou non
        /// </summary>
        public bool? IsETAM { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si Cadre est actif ou non
        /// </summary>
        public bool? IsCadre { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ouvrier est actif ou non
        /// </summary>
        public bool? IsOuvrier { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une valeur indiquant si la prime est une prime partenaire ou non.
        /// </summary>
        public bool PrimePartenaire { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la prime est une prime publique ou non.
        /// </summary>
        public bool Publique { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la société associée.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Seuil de la prime mensuelle
        /// </summary>
        public double? SeuilMensuel { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société associée.
        /// </summary>
        public List<CIPrimeEnt> CIPrimes { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'identifiant unique de la société associée.
        /// </summary>
        public bool IsLinkedToCI { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant unique du groupe
        /// </summary>
        public int? GroupeId { get; set; }

        /// <summary>
        /// Child PointageAnticipePrimes where [FRED_POINTAGE_ANTICIPE_PRIME].[PrimeId] point to this entity (FK_POINTAGE_ANTICIPE_PRIME_PRIME)
        /// </summary>
        public virtual ICollection<PointageAnticipePrimeEnt> PointageAnticipePrimes { get; set; } // FRED_POINTAGE_ANTICIPE_PRIME.FK_POINTAGE_ANTICIPE_PRIME_PRIME

        /// <summary>
        /// Child RapportLignePrimes where [FRED_RAPPORT_LIGNE_PRIME].[PrimeId] point to this entity (FK_RAPPORT_LIGNE_PRIME_PRIME)
        /// </summary>
        public virtual ICollection<RapportLignePrimeEnt> RapportLignePrimes { get; set; } // FRED_RAPPORT_LIGNE_PRIME.FK_RAPPORT_LIGNE_PRIME_PRIME

        /// <summary>
        /// Parent Societe pointed by [FRED_PRIME].([SocieteId]) (FK_FRED_PRIME_SOCIETE)
        /// </summary>
        public virtual SocieteEnt Societe { get; set; } // FK_FRED_PRIME_SOCIETE

        /// <summary>
        /// Le groupe associer au prime
        /// </summary>
        public virtual GroupeEnt Groupe { get; set; }

        /// <summary>
        /// statut du personnel cible
        /// </summary>
        public TargetPersonnel? TargetPersonnel { get; set; }

        /// <summary>
        /// Obtient ou définit si le prime est multi pointage par jour
        /// </summary>
        public bool? MultiPerDay { get; set; }

        /// <summary>
        /// Obtient ou définit si le prime est un prime astreinte
        /// </summary>
        public bool? IsPrimeAstreinte { get; set; }
    }
}
