using System;
using System.Collections.Generic;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une affectation ou d'un ligne de rapport d'un moyen
    /// </summary>
    public abstract class SearchBaseMoyenEnt
    {
        /// <summary>
        /// L'identifiant du CI
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// L'identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Numéro de parc
        /// </summary>
        public string NumParc { get; set; }

        /// <summary>
        /// Numéro d'immatriculation
        /// </summary>
        public string NumImmatriculation { get; set; }

        /// <summary>
        /// Booléan indique si la date de fin est prévisionnelle
        /// </summary>
        public bool? IsDateFinPredictedOutdated { get; set; }

        /// <summary>
        /// Booléan indique si le moyen est à repatrier
        /// </summary>
        public bool? IsToBringBack { get; set; }

        /// <summary>
        /// Société
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Etablissement comptable id
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        /// Type de moyen
        /// </summary>
        public string TypeMoyen { get; set; }

        /// <summary>
        /// Sous type de moyen
        /// </summary>
        public string SousTypeMoyen { get; set; }

        /// <summary>
        /// Modèle de moyen
        /// </summary>
        public string ModelMoyen { get; set; }

        /// <summary>
        /// L'identifiant du site actuel
        /// </summary>
        public int? SiteActuelId { get; set; }

        /// <summary>
        /// L'identifiant de type d'affectation
        /// </summary>
        public int? AffectationMoyenTypeId { get; set; }

        /// <summary>
        /// La date de début affectation
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// La date de fin affectation
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// La date de fin affectation jusqu' fin de journée
        /// </summary>
        public DateTime DateToEndOfDay => DateTo != null ? new DateTime(DateTo.Value.Year, DateTo.Value.Month, DateTo.Value.Day, 23, 59, 59) : DateTime.MaxValue;

        /// <summary>
        /// Liste des affectations moyen ids
        /// </summary>
        public IEnumerable<int> PersonnelListAffectationMoyenIds { get; set; } = new List<int>();
    }
}
