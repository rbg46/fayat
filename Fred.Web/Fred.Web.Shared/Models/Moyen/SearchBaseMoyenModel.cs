using System;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une affectation ou d'une ligne de rapport d'un moyen
    /// </summary>
    public class SearchBaseMoyenModel
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
        /// Date début de recherche
        /// </summary>
        private DateTime? dateFrom;

        /// <summary>
        /// La date de début du filtre affectation
        /// </summary>
        public DateTime? DateFrom
        {
            get
            {
                return dateFrom.HasValue ? dateFrom.Value.Date : (DateTime?)null;
            }
            set
            {
                dateFrom = value;
            }
        }

        /// <summary>
        /// Date de fin de recherche
        /// </summary>
        private DateTime? dateTo;

        /// <summary>
        /// La date de fin du filtre affectation
        /// </summary>
        public DateTime? DateTo
        {
            get
            {
                return dateTo.HasValue ? dateTo.Value.Date : (DateTime?)null;
            }
            set
            {
                dateTo = value;
            }
        }

        /// <summary>
        /// Société
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Etablissement comptable id
        /// </summary>
        public int? EtablissementComptableId { get; set; }
    }
}
